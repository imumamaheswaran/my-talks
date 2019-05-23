namespace Kovai.AtomicScope
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class ExceptionInfo
    {
        [Key]
        public Guid ExceptionId { get; set; }

        public Guid? EnvironmentId { get; set; }

        public int? ExceptionType { get; set; }

        public string ExceptionDetails { get; set; }

        public Guid? StageActivityId { get; set; }

        public DateTime loggedDateTime { get; set; }
    }
}
