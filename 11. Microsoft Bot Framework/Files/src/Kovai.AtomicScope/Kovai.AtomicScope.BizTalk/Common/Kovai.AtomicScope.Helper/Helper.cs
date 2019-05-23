using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Kovai.AtomicScope.Helper
{
    public static class PipelineHelper
    {

        public static void ArchivetoStorage(Microsoft.BizTalk.Component.Interop.IPipelineContext pc, Microsoft.BizTalk.Message.Interop.IBaseMessage inmsg, string FullFilePath, bool ReadFullContext)
        {
            StringBuilder SBContext = new StringBuilder();
            ReadOnlySeekableStream stream = new ReadOnlySeekableStream(inmsg.BodyPart.GetOriginalDataStream());
            Stream sourceStream = inmsg.BodyPart.GetOriginalDataStream();
            List<JsonContextProperty> jsonContextpropertylist = new List<JsonContextProperty>();

            if (!sourceStream.CanSeek)
            {
                ReadOnlySeekableStream seekableStream = new ReadOnlySeekableStream(sourceStream);

                inmsg.BodyPart.Data = seekableStream;

                sourceStream = inmsg.BodyPart.Data;
            }

            if (inmsg.BodyPart != null)
            {
                string json = "NA";
                VirtualStream virtualStream = new VirtualStream(sourceStream);
                //ArchiveToFileLocation(virtualStream, FullFilePath);
                //virtualStream.Seek(0, SeekOrigin.Begin);
                inmsg.BodyPart.Data = virtualStream;


                if (ReadFullContext == true)
                {
                    IBaseMessageContext ctx = inmsg.Context;
                    string name;
                    string nspace;

                    for (int loop = 0; loop < ctx.CountProperties; loop++)
                    {
                        ctx.ReadAt(loop, out name, out nspace);
                        string value = ctx.Read(name, nspace).ToString();
                        jsonContextpropertylist.Add(new JsonContextProperty()
                        {
                            Name = name,
                            NameSpace = nspace,
                            Value = value
                        });
                    }
                    json = JsonConvert.SerializeObject(jsonContextpropertylist);

                }
                string InterchangeID = inmsg.Context.Read("InterchangeID", "http://schemas.microsoft.com/BizTalk/2003/system-properties").ToString();
                string ServiceName = "NA";
                if (inmsg.Context.Read("ReceivePortID", "http://schemas.microsoft.com/BizTalk/2003/system-properties") != null)
                {
                    ServiceName = inmsg.Context.Read("ReceiveLocationName", "http://schemas.microsoft.com/BizTalk/2003/system-properties").ToString();
                }
                else if (inmsg.Context.Read("SPID", "http://schemas.microsoft.com/BizTalk/2003/system-properties") != null)
                {
                    ServiceName = inmsg.Context.Read("SPName", "http://schemas.microsoft.com/BizTalk/2003/system-properties").ToString();
                }

                string msgId = inmsg.MessageID.ToString();

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

                command.Parameters.Add("@ActivityID", SqlDbType.VarChar).Value = msgId;
                command.Parameters.Add("@CorrelationID", SqlDbType.VarChar).Value = InterchangeID;
                command.Parameters.Add("@StartTime", SqlDbType.VarChar).Value = DateTime.Now.ToString();
                command.Parameters.Add("@EndTime", SqlDbType.VarChar).Value = DateTime.Now.ToString();
                command.Parameters.Add("@ServiceName", SqlDbType.VarChar).Value = ServiceName;
                command.Parameters.Add("@MsgContext", SqlDbType.VarChar).Value = json;
                sourceStream.Position = 0;
                command.Parameters.Add("@StreamMsgPayload", SqlDbType.Binary).Value = sourceStream;
                command.Parameters.Add("@RawMsgPayload", SqlDbType.VarChar).Value = "NA";
                command.Parameters.Add("@Status", SqlDbType.VarChar, 255).Value = "Success";
                command.Parameters.Add("@MessageArchiveLocation", SqlDbType.VarChar).Value = FullFilePath;

                Conn.Open();
                command.ExecuteNonQuery();
                Conn.Close();
            }
        }
        public static void ArchiveToFileLocation(VirtualStream dataStream, string FullFilePath)
        {
            try
            {
                //string saveTo = "C:\\Projects\\Unicer.Archive\\ArchiveFolder\\teste.xml";
                // create a write stream
                FileStream FileArchiveStream = new FileStream(FullFilePath, FileMode.Create, FileAccess.Write);
                BinaryWriter FileBinaryWriter = new BinaryWriter(FileArchiveStream);

                // Read the stream and write to the archive file
                byte[] Buffer = new byte[256];
                int SizeRead;
                while ((SizeRead = dataStream.Read(Buffer, 0, 256)) != 0)
                {
                    FileBinaryWriter.Write(Buffer, 0, SizeRead);
                }

                // Flush and close the output stream
                FileBinaryWriter.Flush();
                FileBinaryWriter.Close();
            }
            catch (IOException Ex)
            {
                //System.Diagnostics.EventLog.WriteEntry(EventData.EVENT_LOG_SOURCE, "An error occured creating the archive file - the file has not been archived.\n\nThe following error was raised: " + Ex.Message, EventLogEntryType.Error);
            }

            // dataStream.Seek(0, SeekOrigin.Begin); //(ERROR IS THROW HERE) 
            // return (dataStream);
        }
    }
    public class JsonContextProperty
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string Value { get; set; }
    }

}
