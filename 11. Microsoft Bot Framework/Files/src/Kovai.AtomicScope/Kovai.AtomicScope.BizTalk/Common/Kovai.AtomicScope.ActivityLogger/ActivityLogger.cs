using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using Newtonsoft.Json;
using Microsoft.XLANGs.BaseTypes;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Microsoft.XLANGs.Core;
using System.Collections;

namespace Kovai.AtomicScope
{
    [Serializable]
    public class ActivityLogger
    {
        ActivityInfo _activityInfo;
        List<ExceptionInfo> Exceptions;
        bool _archiveMessage;
        public string GenerateEventID()
        {
            return Guid.NewGuid().ToString();
        } //BusinessLayer

        //Similar to this we will have a constructor for an orchestration
        public ActivityLogger(string interfaceName, bool archiveMessage, string eventId)
        {
            _activityInfo = new AtomicScope.ActivityInfo();
            _activityInfo.InterfaceName = interfaceName;
            _archiveMessage = archiveMessage;
            // _ibaseMessage = msg;
            _activityInfo.CurrentStage = eventId;
        }
        public ActivityLogger(IBaseMessage msg)
        {
            _activityInfo = new AtomicScope.ActivityInfo();

        }

        public void StartActivity(IBaseMessage inMsg, string interfaceName, string stageName, string eventId)
        {
            try
            {

                //LogHelper.WriteDebugLog("Initialising Activity");
                List<ContextProperty> contextProperties = new List<ContextProperty>();
                string context = ReadProperties(inMsg);

                //Read Config values from SQL
                Stage stage = new Stage();//Reading from the config file //
                stage = InitialActivity(interfaceName, stageName);

                //If tracking id is not already present create a new tracking id. 
                if (inMsg.Context.Read("ActivityId", "https://Kovai.AtomicScope.TrackingSchema.Kovai") == null)
                {
                    _activityInfo.ActivityId = Guid.NewGuid().ToString();
                    inMsg.Context.Promote("ActivityId", "https://Kovai.AtomicScope.TrackingSchema.Kovai", _activityInfo.ActivityId);
                }


                //Otherwise use the tracking id which is already present in the message context
                else
                {
                    _activityInfo.ActivityId = (string)inMsg.Context.Read("ActivityId", "https://Kovai.AtomicScope.TrackingSchema.Kovai");
                }

                //Create a new activity id on start activity method and write it to message context.
                _activityInfo.EventId = eventId;
                inMsg.Context.Promote("EventId", "https://Kovai.AtomicScope.TrackingSchema.Kovai", _activityInfo.EventId);
                _activityInfo.StartTime = DateTime.UtcNow;

                //When we start the activity the previous stage will be set to "." 
                if (inMsg.Context.Read("PreviousStage", "https://Kovai.AtomicScope.TrackingSchema.Kovai") == null)
                {
                    _activityInfo.PreviousStage = ".";
                }

                else
                {
                    _activityInfo.PreviousStage = (string)inMsg.Context.Read("CurrentStage", "https://Kovai.AtomicScope.TrackingSchema.Kovai");
                }

                if (inMsg.Context.Read("MessageType", "http://schemas.microsoft.com/BizTalk/2003/system-properties") == null)
                {
                    _activityInfo.MessageType = string.Empty;

                }
                else
                {
                    _activityInfo.MessageType = (string)inMsg.Context.Read("MessageType", "http://schemas.microsoft.com/BizTalk/2003/system-properties");
                }

                inMsg.Context.Write("PreviousStage", "https://Kovai.AtomicScope.TrackingSchema.Kovai", _activityInfo.PreviousStage);
                inMsg.Context.Write("CurrentStage", "https://Kovai.AtomicScope.TrackingSchema.Kovai", stageName);
                _activityInfo.CurrentStage = stageName;
                inMsg.Context.Write("InterfaceName", "https://Kovai.AtomicScope.TrackingSchema.Kovai", interfaceName);

                if (stage.archiveMessagePayloadDB == true)
                {
                    _activityInfo.ArchiveId = Guid.NewGuid().ToString();
                    _activityInfo.ArchiverLinkFriendlyName = stage.archiverLinkFriendlyName;
                    ArchiveMessage(inMsg, _activityInfo);
                }

                else
                { _activityInfo.ArchiveId = null; }

                _activityInfo.correlationToken = stage.correlationToken;
                getCorrelationTokens(inMsg, _activityInfo.correlationToken);

                _activityInfo.customTrackData = stage.customTrackData;
                getCustomProperties(inMsg, _activityInfo.customTrackData);
                StartActivity(_activityInfo);//DataLayerCall 
            }
            catch (Exception ex)
            {
                //LogHelper.WriteDebugLog(ex.InnerException.ToString());

                //throw;
            }

        }//Pipeline Business Layer

        public void StartActivity(XLANGMessage unConstructedXMessage, XLANGMessage constructedXMessage, string interfaceName, string stageName, string EventID)
        {

            Interface items = new Interface();
            items = readConfigOrch(interfaceName);

            foreach (var stage in items.stages)
            {
                if (stage.stageName == stageName)
                {
                    if (stage.startActivity == true)
                    {
                        //If tracking id is not already present create a new tracking id. 
                        if (constructedXMessage.GetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.StageActivityId)) == null)
                        {
                            _activityInfo.ActivityId = Guid.NewGuid().ToString();

                            unConstructedXMessage.SetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.StageActivityId), _activityInfo.ActivityId.ToString());
                        }


                        //Otherwise use the tracking id which is already present in the message context
                        else
                        {
                            _activityInfo.ActivityId = (string)constructedXMessage.GetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.StageActivityId));

                        }

                        //Create a new activity id on start activity method and write it to message context.
                        //_activityInfo.ActivityId = Guid.NewGuid().ToString();
                        _activityInfo.EventId = EventID;
                        unConstructedXMessage.SetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.StageActivityId), _activityInfo.ActivityId.ToString());
                        _activityInfo.StartTime = DateTime.UtcNow;

                        //When we start the activity the previous stage will be set to "." 
                        if (constructedXMessage.GetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.PreviousStage)) == null)
                        {
                            _activityInfo.PreviousStage = ".";
                        }
                        else
                        {
                            _activityInfo.PreviousStage = (string)constructedXMessage.GetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.CurrentStage));
                        }
                        unConstructedXMessage.SetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.PreviousStage), _activityInfo.PreviousStage);
                        unConstructedXMessage.SetPropertyValue(typeof(Kovai.AtomicScope.TrackingSchema.CurrentStage), stageName);

                        if (stage.archiveMessagePayloadDB == true)
                        {
                            _activityInfo.ArchiveId = Guid.NewGuid().ToString();
                            // ArchiveMessage(inMsg, stage, _activityInfo);
                        }
                        else
                        { _activityInfo.ArchiveId = null; }

                        _activityInfo.InterfaceName = interfaceName;

                        _activityInfo.CurrentStage = stageName;

                        _activityInfo.StartTime = DateTime.UtcNow;
                        ArchiveMessageXlang(constructedXMessage, stage, _activityInfo);
                        _activityInfo.customTrackData = stage.customTrackData;
                        getCustomPropertiesOrch(constructedXMessage, _activityInfo.customTrackData);
                        StartActivity(_activityInfo);//DataLayerCall
                    }
                }
            }

        }//Orchestration Business Layer 

        public void StartActivity(ActivityInfo activityInfo)
        {
            Guid ArchiveID = new Guid();
            if (string.IsNullOrEmpty(activityInfo.ArchiveId))
            {
                ArchiveID = default(Guid);
            }
            else
            {
                ArchiveID = Guid.Parse(activityInfo.ArchiveId);
            }
            string jsonCustomProp = JsonConvert.SerializeObject(activityInfo.customTrackData);
            SqlConnection sqlConnection = new SqlConnection("Data Source=BT360DEV34\\MSSQLSERVER1;Initial Catalog=AtomicScope;Integrated Security=SSPI");
            SqlCommand command = new SqlCommand(
                                "INSERT INTO [dbo].[AtomicScope_ActivityLog] " +
                                "([EventId] " +
                                ",[ActivityId] " +
                                ",[InterfaceName]" +
                                ",[CurrentStage]" +
                                ",[PreviousStage]" +
                                ",[ArchiveId]" +
                                  // ",[messageType] " +
                                  ",[TrackedData] " +
                                  ",[CorrelationToken] " +
                                ",[StartTime])" +
                                "VALUES(@EventId, @ActivityId, @interfaceName, @currentStage, @previousStage,@archiveId,@trackedData,@correlationToken,@startTime)", sqlConnection);

            command.Parameters.Add("@EventId", SqlDbType.VarChar).Value = activityInfo.EventId;
            command.Parameters.Add("@ActivityId", SqlDbType.VarChar).Value = activityInfo.ActivityId;
            command.Parameters.Add("@interfaceName", SqlDbType.VarChar).Value = activityInfo.InterfaceName;
            command.Parameters.Add("@currentStage", SqlDbType.VarChar).Value = activityInfo.CurrentStage;
            command.Parameters.Add("@previousStage", SqlDbType.VarChar).Value = activityInfo.PreviousStage;
            command.Parameters.Add("@archiveId", SqlDbType.UniqueIdentifier).Value = ArchiveID;
            ///command.Parameters.Add("@messageType", SqlDbType.VarChar).Value = activityInfo.MessageType;
            command.Parameters.Add("@trackedData", SqlDbType.VarChar).Value = JsonConvert.SerializeObject(activityInfo.trackedData);
            command.Parameters.Add("@correlationToken", SqlDbType.VarChar).Value = JsonConvert.SerializeObject(activityInfo.correlationToken);
            command.Parameters.Add("@startTime", SqlDbType.DateTime).Value = activityInfo.StartTime;


            sqlConnection.Open();
            command.ExecuteNonQuery();
            sqlConnection.Close();
        } //Data Layer
        public void UpdateActivity(IBaseMessage pInMsg)
        {
            ActivityInfo activityInfo = new ActivityInfo();

            if (pInMsg.Context.Read("EventId", "https://Kovai.AtomicScope.TrackingSchema.Kovai") != null)
            {
                activityInfo.EventId = (string)pInMsg.Context.Read("EventId", "https://Kovai.AtomicScope.TrackingSchema.Kovai");
            }

            if (pInMsg.Context.Read("OrderId", "https://SampleBizTalkApplication.PropertySchema") != null)
            {
                activityInfo.MessageId = (string)pInMsg.Context.Read("OrderId", "https://SampleBizTalkApplication.PropertySchema");
            }

            if (pInMsg.Context.Read("OrderId", "https://SampleBizTalkApplication.PropertySchema") != null)
            {
                activityInfo.BatchId = (string)pInMsg.Context.Read("OrderId", "https://SampleBizTalkApplication.PropertySchema");
            }

            if (pInMsg.Context.Read("MessageType", "http://schemas.microsoft.com/BizTalk/2003/system-properties") != null)
            {
                activityInfo.MessageType = (string)pInMsg.Context.Read("MessageType", "http://schemas.microsoft.com/BizTalk/2003/system-properties");
            }
            if (pInMsg.Context.Read("SenderId", "https://SampleBizTalkApplication.PropertySchema") != null)
            {
                activityInfo.SenderId = (string)pInMsg.Context.Read("SenderId", "https://SampleBizTalkApplication.PropertySchema");
            }

            if (pInMsg.Context.Read("ReceiverId", "https://SampleBizTalkApplication.PropertySchema") != null)
            {
                activityInfo.ReceiverId = (string)pInMsg.Context.Read("ReceiverId", "https://SampleBizTalkApplication.PropertySchema");
            }

            UpdateActivity(activityInfo);//DataLayer
        }//Business Layer
        public void UpdateActivity(ActivityInfo activityInfo)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source=BT360DEV34\\MSSQLSERVER1;Initial Catalog=AtomicScope;Integrated Security=SSPI");

            StringBuilder commandString = new StringBuilder("UPDATE [dbo].[AtomicScope_ActivityLog] SET");
            //TODO : change this to cleaner approach
            if (activityInfo.MessageId != null) commandString.Append(" MessageId = @messageId,");
            if (activityInfo.BatchId != null) commandString.Append(" BatchId = @batchId,");
            if (activityInfo.MessageType != null) commandString.Append(" MessageType = @messageType, ");
            if (activityInfo.SenderId != null) commandString.Append(" SenderId = @senderId, ");
            if (activityInfo.ReceiverId != null) commandString.Append(" ReceiverId = @receiverId, ");
            commandString.Append(" EndTime = @endDateTime");
            commandString.Append(" Where EventId= @EventId");


            SqlCommand command = new SqlCommand(commandString.ToString(), sqlConnection);

            command.Parameters.Add("@EventId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(activityInfo.EventId);
            if (activityInfo.MessageId != null) command.Parameters.Add("@messageId", SqlDbType.VarChar).Value = activityInfo.MessageId;
            if (activityInfo.BatchId != null) command.Parameters.Add("@batchId", SqlDbType.VarChar).Value = activityInfo.BatchId;
            if (activityInfo.MessageType != null) command.Parameters.Add("@messageType", SqlDbType.VarChar).Value = activityInfo.MessageType;
            if (activityInfo.SenderId != null) command.Parameters.Add("@senderId", SqlDbType.VarChar).Value = activityInfo.SenderId;
            if (activityInfo.ReceiverId != null) command.Parameters.Add("@receiverId", SqlDbType.VarChar).Value = activityInfo.ReceiverId;
            command.Parameters.Add("@endDateTime", SqlDbType.DateTime).Value = DateTime.UtcNow;


            sqlConnection.Open();
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }//DataLayer
        public void ArchiveMessage(IBaseMessage msg, ActivityInfo activityInfo)
        {
            if (!msg.BodyPart.Data.CanSeek)
            {
                ReadOnlySeekableStream seekableStream = new ReadOnlySeekableStream(msg.BodyPart.Data);

                msg.BodyPart.Data = seekableStream;
            }

            if (msg.BodyPart != null)
            {
                byte[] messageBody = new byte[msg.BodyPart.Data.Length];

                msg.BodyPart.Data.Read(messageBody, 0, (int)msg.BodyPart.Data.Length);
                string contextProperties = ReadProperties(msg);
                ArchiveMessage(messageBody, contextProperties, _activityInfo);
                msg.BodyPart.Data.Position = 0;
            }
        }//Business Layer
        public void ArchiveMessageXlang(XLANGMessage Xmsg, Stage stage, ActivityInfo activityInfo)
        {
            Dictionary<object, object> HContext = new Dictionary<object, object>();
            StringBuilder SBContext = new StringBuilder();
            string ServiceName = Microsoft.XLANGs.Core.Service.RootService.Name; //Get the Calling Orchestration Name

            if (stage.archiveMessagePayloadDB == true)
            {
                var DContext = GetContext(Xmsg).Cast<DictionaryEntry>().ToDictionary(d => d.Key, d => d.Value);
                var serializedObject = DContext.Select(ctx => new ContextProperty
                {
                    Name = (ctx.Key as XmlQName).Name,
                    NameSpace = (ctx.Key as XmlQName).Namespace,
                    Value = Convert.ToString(ctx.Value)
                });
                string context = JsonConvert.SerializeObject(serializedObject);
                XmlDocument xdoc = new XmlDocument();
                xdoc = (XmlDocument)Xmsg[0].RetrieveAs(typeof(XmlDocument));
                byte[] messageBody = Encoding.Default.GetBytes(xdoc.InnerXml.ToString());
                activityInfo.ArchiverLinkFriendlyName = stage.archiverLinkFriendlyName;
                ArchiveMessage(messageBody, context, activityInfo);
            }

        }//Business Layer
        public void ArchiveMessage(byte[] messageBody, string messageContext, ActivityInfo activityInfo)
        {
            //TODO: Srini move this data access to seperate layer.

            DateTime CurrentDate = DateTime.UtcNow;
            SqlConnection sqlConnection = new SqlConnection("Data Source=BT360DEV34\\MSSQLSERVER1;Initial Catalog=AtomicScope;Integrated Security=SSPI");

            SqlCommand command = new SqlCommand(
                                "INSERT INTO [dbo].[AtomicScope_Archive] " +
                                "([ArchiveId] " +
                                ",[ActivityId] " +
                                ",[LinkFriendlyName] " +
                                ",[DownloadURL] " +
                                 ",[EventId] " +
                                ",[TimeStamp] " +
                                ",[MessageBody] " +
                                ",[MessageContext]) " +
                                "VALUES(@archiveId,@ActivityId,@linkFriendlyName,@downloadURL, @EventId, @timeStamp, @messageBody, @messageContext )", sqlConnection);

            command.Parameters.Add("@archiveId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(activityInfo.ArchiveId);
            command.Parameters.Add("@ActivityId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(activityInfo.ActivityId);
            command.Parameters.Add("@linkFriendlyName", SqlDbType.VarChar).Value = activityInfo.ArchiverLinkFriendlyName;
            command.Parameters.Add("@downloadURL", SqlDbType.VarChar).Value = "http://localhost/" + Guid.NewGuid();
            command.Parameters.Add("@EventId", SqlDbType.UniqueIdentifier).Value = Guid.Parse(activityInfo.EventId);
            command.Parameters.Add("@timeStamp", SqlDbType.DateTime).Value = CurrentDate;
            command.Parameters.Add("@messageBody", SqlDbType.VarBinary).Value = messageBody;
            command.Parameters.Add("@messageContext", SqlDbType.VarChar).Value = messageContext;

            sqlConnection.Open();
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }//DataLayer

        public void logException(string stageActivityId, int exceptionType, string exceptionDetails)//Business Layer
        {
            ExceptionInfo exInfo = new ExceptionInfo();
            exInfo.StageActivityId = Guid.Parse(stageActivityId);
            exInfo.ExceptionId = Guid.NewGuid();
            exInfo.ExceptionType = exceptionType;
            exInfo.ExceptionDetails = exceptionDetails;
            exInfo.loggedDateTime = DateTime.UtcNow;
            logException(exInfo);

        } //Business Layer

        public void logException(ExceptionInfo exInfo)
        {
            SqlConnection sqlConnection = new SqlConnection("Data Source=BT360DEV34\\MSSQLSERVER1;Initial Catalog=AtomicScope;Integrated Security=SSPI");
            SqlCommand command = new SqlCommand(
                                "INSERT INTO [dbo].[AtomicScope_Exceptions] " +
                                "([EventId] " +
                                ",[ExceptionId] " +
                                ",[ExceptionType]" +
                                ",[ExceptionDetails]" +
                                ",[LoggedDateTime])" +
                                "VALUES(@EventId, @ExceptionId, @ExceptionType, @ExceptionDetails, @loggedDateTime)", sqlConnection);

            command.Parameters.Add("@EventId", SqlDbType.UniqueIdentifier).Value = exInfo.StageActivityId;
            command.Parameters.Add("@ExceptionId", SqlDbType.UniqueIdentifier).Value = exInfo.ExceptionId;
            command.Parameters.Add("@ExceptionType", SqlDbType.Int).Value = exInfo.ExceptionType;
            command.Parameters.Add("@ExceptionDetails", SqlDbType.VarChar).Value = exInfo.ExceptionDetails;
            command.Parameters.Add("@LoggedDateTime", SqlDbType.DateTime).Value = exInfo.loggedDateTime;

            sqlConnection.Open();
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        public string ReadProperties(IBaseMessage msg)
        {
            IBaseMessageContext ctx = msg.Context;
            string name;
            string nspace;
            List<ContextProperty> contextProperties = new List<AtomicScope.ContextProperty>();

            for (int i = 0; i < ctx.CountProperties; i++)
            {
                ctx.ReadAt(i, out name, out nspace);
                string value = ctx.Read(name, nspace).ToString();
                contextProperties.Add(new ContextProperty()
                {
                    Name = name,
                    NameSpace = nspace,
                    Value = value
                });
            }
            return JsonConvert.SerializeObject(contextProperties);
        }//Business Layer

        public List<ContextProperty> ReadContextProperty(IBaseMessage msg, string PropertyName, string Namespace)
        {
            IBaseMessageContext ctx = msg.Context;
            string name;
            string nspace;
            List<ContextProperty> contextProperties = new List<AtomicScope.ContextProperty>();

            for (int i = 0; i < ctx.CountProperties; i++)
            {
                ctx.ReadAt(i, out name, out nspace);
                string PropertyValue = ctx.Read(name, nspace).ToString();
                contextProperties.Add(new ContextProperty()
                {
                    Name = name,
                    NameSpace = nspace,
                    Value = PropertyValue
                });
            }
            return contextProperties;
        }
        public Stage InitialActivity(string InterfaceName, string stageID) //different data layer 
        {
            Interface items = new Interface();
            //using (StreamReader r = new StreamReader(@"D:\Vignesh\Projects\Atomic Scope\JSonFormat\AtomicScopeConfiguration.json"))
            //{
            //    string json = r.ReadToEnd();
            //    items = JsonConvert.DeserializeObject<Interface>(json);
            //}


            SqlConnection con = new SqlConnection("Data Source=BT360DEV34\\MSSQLSERVER1;Initial Catalog=AtomicScope;Integrated Security=SSPI");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.Parameters.Add("InterfaceName", SqlDbType.VarChar).Value = "OrderProcess";
            //cmd.Parameters.Add("InterfaceID", SqlDbType.VarChar).Value = "1234 - 5678 - 9101112";
            cmd.CommandText = "SELECT * FROM AtomicScope_InterfaceConfig WHERE InterfaceName = @InterfaceName";
            con.Open();
            string config = string.Empty;
            SqlDataReader dreader = cmd.ExecuteReader();
            while (dreader.Read())
            {
                config = (string)dreader["Configuration"];
            }
            con.Close();
            items = JsonConvert.DeserializeObject<Interface>(config);


            foreach (var stage in items.stages)
            {
                if (stage.stageName == stageID)
                {
                    return stage;
                }
            }
            return default(Stage);
        } //Business Layer

        public Interface readConfigOrch(string interfaceName)
        {
            Interface items = new Interface();
            SqlConnection con = new SqlConnection("Data Source=BT360DEV34\\MSSQLSERVER1;Initial Catalog=AtomicScope;Integrated Security=SSPI");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            cmd.Parameters.Add("InterfaceName", SqlDbType.VarChar).Value = interfaceName;
            //cmd.Parameters.Add("InterfaceID", SqlDbType.VarChar).Value = "1234 - 5678 - 9101112";
            cmd.CommandText = "SELECT * FROM AtomicScope_InterfaceConfig WHERE InterfaceName = @InterfaceName";
            con.Open();
            string config = string.Empty;
            SqlDataReader dreader = cmd.ExecuteReader();
            while (dreader.Read())
            {
                config = (string)dreader["Configuration"];
            }
            con.Close();
            items = JsonConvert.DeserializeObject<Interface>(config);
            return items;
        } //Business Layer

        public void getCustomProperties(IBaseMessage msg, Customtrackdata[] CustomTrackProp)
        {
            try
            {

                Stream MsgStream = msg.BodyPart.GetOriginalDataStream();

                if (!MsgStream.CanSeek)
                {
                    ReadOnlySeekableStream readStream = new ReadOnlySeekableStream(MsgStream);
                    msg.BodyPart.Data = readStream;
                    MsgStream = readStream;
                }

                XmlReaderSettings settings = new XmlReaderSettings()
                {
                    ConformanceLevel = ConformanceLevel.Document,
                    IgnoreWhitespace = true,
                    ValidationType = ValidationType.None,
                    IgnoreProcessingInstructions = true,
                    IgnoreComments = true,
                    CloseInput = false
                };
                MsgStream.Seek(0, SeekOrigin.Begin);
                XmlReader reader = XmlReader.Create(MsgStream, settings);
                string strValue = null;
                // if (!string.IsNullOrEmpty("/*[local-name()='SalesOrder' and namespace-uri()='http://SampleBizTalkApplication']/*[local-name()='AskPrice' and namespace-uri()='']"))

                if (reader.Read())
                {
                    XPathDocument xPathDoc = new XPathDocument(reader);
                    XPathNavigator xNavigator = xPathDoc.CreateNavigator();
                    foreach (var prp in CustomTrackProp[0].propertyList)
                    {
                        if (prp.isContext == false)
                        {
                            XPathNodeIterator xNodes = xNavigator.Select(prp.xPath);
                            if (xNodes.Count != 0 && xNodes.MoveNext())
                            {
                                prp.value = xNodes.Current.Value;
                            }
                        }
                        else
                        {
                            prp.value = Convert.ToString(msg.Context.Read(prp.propertyName, prp.propertyNamespace ?? string.Empty));
                        }

                    }
                    MsgStream.Seek(0, SeekOrigin.Begin);


                }
                _activityInfo.trackedData = new TrackedData[CustomTrackProp[0].propertyList.Length];
                for (int i = 0; i < CustomTrackProp[0].propertyList.Length; i++)
                {
                    //if (CustomTrackProp[0].propertyList[i].isContext == true)
                    //{
                    //    List<ContextProperty> aa = new List<ContextProperty>();
                    //    aa = JsonConvert.DeserializeObject<List<ContextProperty>>(ReadProperties(msg));
                    //    _activityInfo.trackedData[i] = aa.Where(q => q.Name == CustomTrackProp[0].propertyList[i].propertyName && q.NameSpace == CustomTrackProp[0].propertyList[i].propertyNamespace).Select(s => new TrackedData { displayName = CustomTrackProp[0].propertyList[i].displayName, value = s.Value }).FirstOrDefault();

                    //}
                    //else
                    {
                        _activityInfo.trackedData[i] = new TrackedData { displayName = CustomTrackProp[0].propertyList[i].displayName, value = CustomTrackProp[0].propertyList[i].value };
                    }
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.EventLog aa = new System.Diagnostics.EventLog();
                aa.WriteEntry(ex.Message.ToString());
            }

            // return CustomTrackProp;
        }  //Business Layer

        public void getCustomPropertiesOrch(XLANGMessage Xmsg, Customtrackdata[] CustomTrackProp)
        {
            Stream MsgStream = (Stream)Xmsg[0].RetrieveAs(typeof(Stream));
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                ConformanceLevel = ConformanceLevel.Document,
                IgnoreWhitespace = true,
                ValidationType = ValidationType.None,
                IgnoreProcessingInstructions = true,
                IgnoreComments = true,
                CloseInput = false
            };

            XmlReader reader = XmlReader.Create(MsgStream, settings);
            if (reader.Read())
            {
                XPathDocument xPathDoc = new XPathDocument(reader);
                XPathNavigator xNavigator = xPathDoc.CreateNavigator();

                foreach (var prp in CustomTrackProp[0].propertyList)
                {

                    {
                        XPathNodeIterator xNodes = xNavigator.Select(prp.xPath);
                        if (xNodes.Count != 0 && xNodes.MoveNext())
                        {
                            if (prp.isContext == false)
                            { prp.value = xNodes.Current.Value; }
                            else
                            {
                                Hashtable HContext = GetContext(Xmsg);
                                foreach (DictionaryEntry item in HContext)
                                {
                                    if ((item.Key as XmlQName).Name == prp.propertyName && (item.Key as XmlQName).Namespace == prp.propertyNamespace)
                                    {
                                        prp.value = item.Value.ToString();

                                    }
                                }
                            }
                        }

                    }

                }

                _activityInfo.trackedData = new TrackedData[CustomTrackProp[0].propertyList.Length];
                for (int i = 0; i < CustomTrackProp[0].propertyList.Length; i++)
                {
                    _activityInfo.trackedData[i] = new TrackedData { displayName = CustomTrackProp[0].propertyList[i].displayName, value = CustomTrackProp[0].propertyList[i].value };

                }
            }
        }

        public Hashtable getCustomContextPropOrch(XLANGMessage Xmsg, Customtrackdata[] customTrackProp)
        {
            return new Hashtable();
        }
        public void getCorrelationTokens(IBaseMessage msg, CorrelationToken[] correlationToken)
        {

            foreach (var prp in correlationToken)
            {
                if (prp.isContext == true)
                {
                    prp.value = msg.Context.Read(prp.propertyName, prp.propertynamespace).ToString();
                }
                else
                {
                    Stream MsgStream = msg.BodyPart.GetOriginalDataStream();

                    if (!MsgStream.CanSeek)
                    {
                        ReadOnlySeekableStream readStream = new ReadOnlySeekableStream(MsgStream);
                        msg.BodyPart.Data = readStream;
                        MsgStream = readStream;
                    }

                    XmlReaderSettings settings = new XmlReaderSettings()
                    {
                        ConformanceLevel = ConformanceLevel.Document,
                        IgnoreWhitespace = true,
                        ValidationType = ValidationType.None,
                        IgnoreProcessingInstructions = true,
                        IgnoreComments = true,
                        CloseInput = false
                    };
                    MsgStream.Seek(0, SeekOrigin.Begin);
                    XmlReader reader = XmlReader.Create(MsgStream, settings);
                    //string strValue = null;
                    // if (!string.IsNullOrEmpty(prp.xPath))
                    {
                        if (reader.Read())
                        {
                            XPathDocument xPathDoc = new XPathDocument(reader);
                            XPathNavigator xNavigator = xPathDoc.CreateNavigator();
                            //foreach (var prpo in CustomTrackProp[0].propertyList)
                            //{

                            XPathNodeIterator xNodes = xNavigator.Select(prp.xPath);
                            if (xNodes.Count != 0 && xNodes.MoveNext())
                            {
                                prp.value = xNodes.Current.Value;
                            }

                        }
                        MsgStream.Seek(0, SeekOrigin.Begin);
                    }

                }
            }
        } //Business Layer

        public static Hashtable GetContext(XLANGMessage message)
        {
            try
            {
                foreach (Segment segment in Service.RootService._segments)
                {
                    IDictionary fields = Context.FindFields(typeof(XLANGMessage), segment.ExceptionContext);

                    foreach (DictionaryEntry field in fields)
                    {
                        XMessage msg = (field.Value as XMessage);
                        if (msg == null)
                            continue;

                        if (String.Compare(msg.Name, message.Name) != 0)
                            continue;

                        return msg.GetContextProperties();
                    }
                }
            }
            catch (Exception ex /* e */)
            {
                throw new Exception(ex.InnerException.ToString());
            }

            return new Hashtable();
        }  //Business Layer

        public void SaveActivity()
        {

        } //TBD

        public void write()
        {

        } //TBD



    }
}


