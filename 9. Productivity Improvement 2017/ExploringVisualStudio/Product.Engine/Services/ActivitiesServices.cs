#region Using Directives
using Product.Engine.Messages;
using Product.Engine.Repositories;
using Product.Engine.Repositories.Messages;
using System;
using System.Collections.Generic;
#endregion

namespace Product.Engine.Services
{
    public class ActivitiesServices
    {
        private readonly IActivitySearchRepository _activitySearchRepository;

        public ActivitiesServices()
        {
            RepositoryManager manager = new RepositoryManager();
            
        }

        public ActivitiesServices(IActivitySearchRepository activitySearchRepository)
        {
            _activitySearchRepository = activitySearchRepository;
        }

        public List<IActivity> GetActivities(string filter, DateTime startTime, DateTime? endTime = null)
        {
            if (string.IsNullOrEmpty(filter))
            {
                var a = 10;
                var b = 0;
                var c = a / b;
               // throw new ArgumentException(nameof(filter), "Invalid filter string.");
            }
                //throw new ArgumentException(nameof(filter), "Invalid filter string.");

            if (startTime == null)
                throw new ArgumentException(nameof(startTime), "Criteria must have a start time");

            var criteria = new ActivitiesSearchCriteria
            {
                Filter = filter,
                StartTime = startTime,
                EndTime = endTime
            };

            var activities = _activitySearchRepository.GetActivitiesReport(criteria);

            if (activities == null)
                throw new ApplicationException("Internal processing error.");

            return activities;
        }
    }
}
