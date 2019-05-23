using System;

namespace Kovai.AtomicScope
{
   public  enum  DataUpdateEnum
    {
        All,
        MetadataOnly,
        CustomdataOnly,
        SenderId,
        ReceiverId,
        MessageId,
        BatchId,
        CorrelationToken                
    }

    public class Interface //Copy 
    {
        public string interfaceName { get; set; }
        public long interfaceId { get; set; }
        public Stage[] stages { get; set; }
    }

    public class Stage // Copy
    {
        public string stageName { get; set; }
        public string stageID { get; set; }
        public string stageArtifact { get; set; }
        public bool startActivity { get; set; }
        public bool updateActivity { get; set; }
        public bool trackContext { get; set; }
        public bool archiveMessagePayloadDB { get; set; }

        public bool archiveMessagePayloadFile { get; set; }
        public string archiveFolderName { get; set; }
        public string archiveFileFormat { get; set; }
        public string archiverLinkFriendlyName { get; set; }
        public bool archiveEncryption { get; set; }
        public Customtrackdata[] customTrackData { get; set; }
        public CorrelationToken[] correlationToken { get; set; }
    }

    [Serializable]
    public class Customtrackdata
    {
        public Propertylist[] propertyList { get; set; }
    }

    [Serializable]
    public class Propertylist
    {
        public string displayName { get; set; }
        public bool isContext { get; set; }
        public string propertyName { get; set; }
        public string propertyNamespace { get; set; }
        public string xPath { get; set; }
        public string jSONPath { get; set; }
        public string value { get; set; }
    }

    public class CorrelationToken
    {
        public bool isContext { get; set; }
        public string propertyName { get; set; }
        public string propertynamespace { get; set; }
        public string xPath { get; set; }
        public string interfaceNames { get; set; }
        public string value { get; set; }
    }

    public class interfaceRequest
    {
        public string interfaceName { get; set; }
        public string interfaceID { get; set; }
        public string stageID { get; set; }
        public string stageName { get; set; }
    }

    public class JsonContextProperty
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string Value { get; set; }
    }
}
