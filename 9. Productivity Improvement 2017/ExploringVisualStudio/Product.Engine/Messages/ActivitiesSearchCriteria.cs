using System;

namespace Product.Engine.Repositories.Messages
{
    public class ActivitiesSearchCriteria
    {
        public string Filter { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

    }
}