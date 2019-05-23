namespace Kovai.AtomicScope
{ 
    using System;
    using System.ComponentModel.DataAnnotations;

    [Serializable]
    public  class ActivityInfo
    {        

        [Key]
        public string ActivityId { get; set;}

        public string EventId    { get; set; }

        [StringLength(250)]
        public string InterfaceName { get; set; }

        [StringLength(250)]
        public string MessageId { get; set; }

        [StringLength(250)]
        public string BatchId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }
        
        [StringLength(250)]
        public string CurrentStage { get; set; }

        [StringLength(250)]
        public string PreviousStage { get; set; }

        public string TrackedData { get; set; }

        public string ArchiveId { get; set; }

        public int? Status { get; set; }

        public string ExceptionId { get; set; }

        public string  ReceiverId { get; set; }

        public string SenderId { get; set; }

        public string MessageType { get; set; }

        public string  CorrelationToken { get; set; }

        public bool IsArchiveDB { get; set; }

        public bool IsArchiveFile { get; set; }

        public string ArchiverLinkFriendlyName { get; set; }

        public Customtrackdata[] customTrackData { get; set; }

        public CorrelationToken[] correlationToken { get; set; }

        public TrackedData[] trackedData { get; set; }
        
    }

    public class ContextProperty
    {
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string Value { get; set; }
    }
    [Serializable]
    public class TrackedData
    {
       
        public string displayName { get; set; }

        public string value { get; set; }
    }



}
