using System;
using System.Text;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.Core;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace Kovai.AtomicScope.XLANGHelper
{
    public class XLANGHelper
    {
        public static void ArchiveMessage(XLANGMessage Message)
        {
            try
            {
                string ActivityID = Guid.NewGuid().ToString();
                string ShareLocation = "D:\\Share\\TrackingArchive\\"; // Should be an UNC Path with desired access granted to the User Account
                string FileExtension = ".txt";
                string FullFilePath = ShareLocation + ActivityID + FileExtension;
                bool IsArchive = true;
                bool ReadFullContext = true;
                Hashtable HContext = new Hashtable();
                StringBuilder SBContext = new StringBuilder();
                string InterchangeID = "NA";

                string ServiceName = Microsoft.XLANGs.Core.Service.RootService.Name; //Get the Calling Orchestration Name

                if (IsArchive == true)
                {
                    ArchiveMsg(Message, FullFilePath);
                }

                if (ReadFullContext == true)
                {
                    HContext = GetContext(Message);
                    foreach (DictionaryEntry item in HContext)
                    {
                        SBContext.Append($"{(item.Key as XmlQName).Name},{ (item.Key as XmlQName).Namespace},{item.Value}");
                        SBContext.Append(";");

                        if ((item.Key as XmlQName).Name == "InterchangeID")
                        {
                            InterchangeID = item.Value.ToString();
                        }
                    }
                }



                SqlConnection Conn = new SqlConnection("Server=BT360DEV34\\MSSQLSERVER1;Database=B360_BAM;Integrated Security=True;");

                SqlCommand command = new SqlCommand(
                                   "INSERT INTO [dbo].[CustomTrackTbl] " +
                                   "([ActivityID] " +
                                   ",[CorrelationID] " +
                                   ",[StartTime] " +
                                   ",[EndTime] " +
                                   ",[ServiceName] " +
                                   ",[MsgContext] " +
                                   ",[StreamMsgPayload] " +
                                   ",[RawMsgPayload] " +
                                   ",[Status] " +
                                   ",[MessageArchiveLocation])" +
                                   "VALUES(@ActivityID,@CorrelationID,@StartTime,@EndTime,@ServiceName,@MsgContext,@StreamMsgPayload,@RawMsgPayload,@Status,@MessageArchiveLocation)", Conn);

                command.Parameters.Add("@ActivityID", SqlDbType.VarChar).Value = ActivityID;
                command.Parameters.Add("@CorrelationID", SqlDbType.VarChar).Value = InterchangeID;
                command.Parameters.Add("@StartTime", SqlDbType.VarChar).Value = DateTime.Now.ToString();
                command.Parameters.Add("@EndTime", SqlDbType.VarChar).Value = DateTime.Now.ToString();
                command.Parameters.Add("@ServiceName", SqlDbType.VarChar).Value = ServiceName;
                command.Parameters.Add("@MsgContext", SqlDbType.VarChar).Value = SBContext.ToString();
                command.Parameters.Add("@StreamMsgPayload", SqlDbType.Binary).Value = (Stream)Message[0].RetrieveAs(typeof(Stream));
                string RawText;
                StreamReader reader = new StreamReader((Stream)Message[0].RetrieveAs(typeof(Stream)), Encoding.UTF8);
                RawText = reader.ReadToEnd();
                command.Parameters.Add("@RawMsgPayload", SqlDbType.VarChar).Value = RawText;
                command.Parameters.Add("@Status", SqlDbType.VarChar, 255).Value = "Success";
                command.Parameters.Add("@MessageArchiveLocation", SqlDbType.VarChar).Value = ShareLocation + ActivityID + "." + FileExtension;

                Conn.Open();
                command.ExecuteNonQuery();
                Conn.Close();
                reader.Dispose();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }
            finally
            {
                Message.Dispose();

            }
        }

        public static void ArchiveMsg(XLANGMessage message, string FullFilePath)
        {
            Stream OrchStream = (Stream)message[0].RetrieveAs(typeof(Stream));
            if (OrchStream.CanSeek)
                OrchStream.Position = 0;
            long lstreamlength;
            lstreamlength = OrchStream.Length;
            if (lstreamlength > 0)
            {

                long lreadbytes = 0;
                Stream S = File.OpenWrite(FullFilePath);
                {
                    //Writing with Encoding
                    BinaryWriter bw = new BinaryWriter(S, Encoding.UTF8);

                    while (lstreamlength > 0)
                    {
                        if (lstreamlength >= 1000000)
                        {
                            lreadbytes = 1000000;
                        }
                        else
                        {
                            lreadbytes = lstreamlength;
                        }
                        lstreamlength = lstreamlength - lreadbytes;
                        byte[] ba = new byte[lreadbytes];

                        OrchStream.Read(ba, 0, ba.Length);
                        bw.Write(ba);
                    }
                }
                S.Close();
            }
        }

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
        }        

    }
}
