using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using System.IO;
using System.Reflection;
using Microsoft.BizTalk.Streaming;
using Kovai.AtomicScope;

namespace AtomicScope.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("E12A8B9F-E678-43CD-9D5E-CFA6B17237E7")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public class MessageLogger : IComponentUI, IComponent, IPersistPropertyBag, IBaseComponent
    {       
        private System.Resources.ResourceManager resourceManager = new System.Resources.ResourceManager("AtomicScope.PipelineComponents.MessageLogger", Assembly.GetExecutingAssembly());

        #region Public Properties                
        private string _encoding;
        private string _ReceiverIdXpath;
        private string _SenderIdXpath;
        private string _interfaceName;
        private string _stageId;
        private bool _archiveMessage;
        private bool _UpdateActivity;
        private bool _StartActivity;
        
        public string Encoding 
        { 
            get{return _encoding; }
            set{_encoding = value;}
        }
        public string ReceiverIdXpath
        {
            get { return _ReceiverIdXpath; }
            set { _ReceiverIdXpath = value; }
        }
        public string SenderIdXPath
        {
            get { return _SenderIdXpath; }
            set { _SenderIdXpath = value; }
        }

        public string InterfaceName
        {
            get { return _interfaceName; }
            set { _interfaceName = value; }
        }

        public string StageId
        {
            get { return _stageId; }
            set { _stageId = value; }
        }

        public bool ArchiveMessage
        {
            get { return _archiveMessage; }
            set { _archiveMessage = value; }
        }

        public bool UpdateActivity
        {
            get { return _UpdateActivity; }
            set { _UpdateActivity  = value; }
        }

        public bool StartActivty
        {
            get { return _StartActivity; }
            set { _StartActivity = value; }
        }
        #endregion

        #region IComponentUI Members
        public IntPtr Icon
        {
            get { return new IntPtr(); }
        }

        public System.Collections.IEnumerator Validate(object projectSystem)
        {
            return null;
        }
        #endregion

        #region IComponent Members
        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage inMsg)
        {
            //ActivityLogger activityLogger = new ActivityLogger(this.InterfaceName, true, this.StageId);
            ActivityLogger1 activityLogger = new ActivityLogger1();
            string ActivityId = default(string);

            //string eventID = activityLogger.GenerateEventID();
            try
            {
                //ActivityLogger activityLogger = new ActivityLogger(this.InterfaceName,true, this.StageId);

                
                //This is when a new activity is logged.
                if (StartActivty == true)
                {
                    ActivityId= activityLogger.StartActivity(StageId, InterfaceName, null, inMsg);
                    
                    //activityLogger.StartActivity(inMsg, InterfaceName, StageId, eventID);
                  
                }

                //When archive activity is enabled. 
           //    throw new Exception("Test Pipeline Exception");

                if (UpdateActivity == true)
                {
                    activityLogger.UpdateStageActivity(inMsg,InterfaceName,StageId);
                }

                if(ArchiveMessage == true)
                {
                    activityLogger.ArchiveMessage(inMsg, ActivityId);
                }


                string trackingId = (string)inMsg.Context.Read("StageActivityId", "https://Kovai.AtomicScope");

                StringBuilder strbuilder = new StringBuilder();
                Random a = new Random();

                inMsg.Context.Write("HttpHeaders", "http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties", "x-ms-client-tracking-id: " + trackingId);
                inMsg.Context.Write("IsDynamicSend", "http://schemas.microsoft.com/BizTalk/2003/system-properties", true);

                return inMsg;
            }
            catch (Exception ex)
            {
                activityLogger.logException(ActivityId, "System Exception", "RcvPort Decode "+ex.Message);
            }            
            return inMsg; 
        }

        internal void WriteProperties(List<ContextProperty> properties, IBaseMessage inMsg)
        {
            foreach (ContextProperty property in properties)
            {
                inMsg.Context.Write(property.Name, property.NameSpace, property.Value);
            }

        }

        /// <summary>
        /// Method to populate message information
        /// </summary>
        /// <param name="pInMsg">IBaseMessage</param>
        /// <param name="logInfo">LogInfo</param>
        /// <returns></returns>
        private IBaseMessage PopulateMessageInfo(IBaseMessage pInMsg)
        {
            /********************************************
             * Finds wheather message is an HL7 message or not. 
             * If it is HL7 Message then senderid , receiver id and cotrol id are read from msh segment.
             * If message is an XML message control ID will be taken  from XPath statements configured 
             * in property bag
             * *******************************************/
            
            byte[] byteArray = new byte[pInMsg.BodyPart.Data.Length];
           

            Stream stream = null;
            if (pInMsg.BodyPart.Data.CanSeek)
            {
                stream = pInMsg.BodyPart.GetOriginalDataStream();
            }
            else
            {
                stream = new ReadOnlySeekableStream(pInMsg.BodyPart.GetOriginalDataStream());
            }
            StreamReader sr = new StreamReader(stream);
           

            //Create a memory stream which will be updated and contains output stream.
            MemoryStream memoryStream = new MemoryStream();

            string headerString = string.Empty;
            //Read the stream till a non empty line is read
            while(headerString.Trim().Length == 0) headerString = sr.ReadLine();
            

            //Once the message is processed 
            if (pInMsg.Context.Read("ParentControlID", "https://ViaPath.BizTalk.Common.Properties") == null)
            {
             // pInMsg.Context.Write("ParentControlID", "https://ViaPath.BizTalk.Common.Properties", logInfo.ControlID);               
            }


            //In receive pipeline of a receive port , interchange id wont be present.
            //In send pipeline of a receive port, interchagne id may be present but not used.
            //in send pipeline of the send port interchange id present. 
            //in receive pipelien of the send port interchange id is not present. 
            string tmpID = Guid.NewGuid().ToString();
            if (pInMsg.Context.Read("InterchangeID", "http://schemas.microsoft.com/BizTalk/2003/system-properties") != null)
            {

            }

            else     

            //Set the stream position back to 0 after msh processing
            stream.Seek(0, SeekOrigin.Begin);
                

         

            pInMsg.BodyPart.Data = stream;
            stream.Seek(0, SeekOrigin.Begin);            
           
            return pInMsg;            
        }

        /*rivate void PromoteProperties(IBaseMessage pInMsg, LogInfo logInfo)
        {         
            pInMsg.Context.Write("SenderID", "https://ViaPath.BizTalk.Common.Properties", logInfo.SenderID);
            pInMsg.Context.Write("ReceiverID", "https://ViaPath.BizTalk.Common.Properties", logInfo.ReceiverID);                     
        }*/

        #endregion
        
        #region IPersistPropertyBag Members

        public void GetClassID(out Guid classID)
        {
            classID = new System.Guid("E12A8B9F-E678-43CD-9D5E-CFA6B17237E7");
        }

        public void InitNew()
        {
        }

        public void Load(IPropertyBag pb, int errorLog)
        {
            //TODO
            object val = null;
            val = this.ReadPropertyBag(pb, "Encoding");
            if (val != null)  this._encoding = (string)val;

            val = this.ReadPropertyBag(pb, "ReceiverIdXPath");
            if(val !=null) this._ReceiverIdXpath = (string)val;

            val = ReadPropertyBag(pb,"SenderIdXPath");
            if(val!= null) this._SenderIdXpath = (string) val;

            val = ReadPropertyBag(pb, "InterfaceName");
            if (val != null) this._interfaceName  = (string)val;

            val = ReadPropertyBag(pb, "StageId");
            if (val != null) this._stageId = (string)val;

            val = ReadPropertyBag(pb, "ArchiveMessage");
            if (val != null) this._archiveMessage  = (bool)val;

            val = ReadPropertyBag(pb, "StartActivity");
            if (val != null) this._StartActivity = (bool)val;

            val = ReadPropertyBag(pb, "UpdateActivity");
            if (val != null) this._UpdateActivity = (bool)val;
        }

        public void Save(IPropertyBag pb, bool clearDirty, bool saveAllProperties)
        {
            this.WritePropertyBag(pb, "Encoding", this._encoding);
            this.WritePropertyBag(pb, "ReceiverIdXPath", this._ReceiverIdXpath);
            this.WritePropertyBag(pb, "SenderIdXPath", this._SenderIdXpath);
            this.WritePropertyBag(pb, "InterfaceName", this._interfaceName);
            this.WritePropertyBag(pb, "StageId", this._stageId);
            this.WritePropertyBag(pb, "ArchiveMessage", this._archiveMessage);
            this.WritePropertyBag(pb, "StartActivity", this._StartActivity);
            this.WritePropertyBag(pb, "UpdateActivity", this._UpdateActivity);
        }
        /// <summary>
        /// Writes property values into a property bag.
        /// </summary>
        /// <param name="pb">Property bag.</param>
        /// <param name="propName">Name of property.</param>
        /// <param name="val">Value of property.</param>
        private void WritePropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName, object val)
        {
            try
            {
                pb.Write(propName, ref val);
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
        }
        /// <summary>
        /// Reads property value from property bag
        /// </summary>
        /// <param name="pb">Property bag</param>
        /// <param name="propName">Name of property</param>
        /// <returns>Value of the property</returns>
        private object ReadPropertyBag(Microsoft.BizTalk.Component.Interop.IPropertyBag pb, string propName)
        {
            object val = null;
            try
            {
                pb.Read(propName, out val, 0);
            }
            catch (System.ArgumentException)
            {
                return val;
            }
            catch (System.Exception e)
            {
                throw new System.ApplicationException(e.Message);
            }
            return val;
        }
        #endregion  

        #region IBaseComponent members
        /// <summary>
        /// Name of the component
        /// </summary>

        public string Name
        {
            get
            {
                return resourceManager.GetString("COMPONENTNAME", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Version of the component
        /// </summary>

        public string Version
        {
            get
            {
                return resourceManager.GetString("COMPONENTVERSION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Description of the component
        /// </summary>

        public string Description
        {
            get
            {
                return resourceManager.GetString("COMPONENTDESCRIPTION", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        #endregion

    }

}
