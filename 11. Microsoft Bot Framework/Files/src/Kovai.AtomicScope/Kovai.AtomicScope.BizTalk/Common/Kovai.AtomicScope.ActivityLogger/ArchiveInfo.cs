namespace Kovai.AtomicScope
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ArchiveInfo
    {
        [Key]
        public Guid ArchiveId { get; set; }

        public Guid? EnvironmentId { get; set; }

        public byte[] MessageBody { get; set; }

        public string MessageContext { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}


