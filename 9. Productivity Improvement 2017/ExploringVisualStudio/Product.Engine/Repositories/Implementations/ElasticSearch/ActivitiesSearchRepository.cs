#region Using Directives
using System;
using System.Linq;
using System.Collections.Generic;
using Product.Engine.Messages;
using Product.Engine.Repositories.Messages;
#endregion

namespace Product.Engine.Repositories.Implementations.ElasticSearch
{

    public class Repo
    {
        public int Name { get; set; }
    }



    public class ActivitiesSearchRepository : IActivitySearchRepository
    {
        public int Limit { get; set; }

        public ActivitiesSearchRepository()
        {
           
        }

        public List<IActivity> GetActivitiesReport(ActivitiesSearchCriteria criteria)
        {
            List<IActivity> activities = GetMainActivities().ToList<IActivity>();

            return activities;
        }


        private List<MainActivity> GetMainActivities()
        {
            return new List<MainActivity>()
            {
                new MainActivity{
                    Id=1,
                    StartTime=DateTime.Now.AddSeconds(10)
                },
                new MainActivity{
                    Id=2,
                    StartTime=DateTime.Now.AddSeconds(10)
                },
                new MainActivity{
                    Id=3,
                    StartTime=DateTime.Now.AddSeconds(10)
                },
                new MainActivity{
                    Id=4,
                    StartTime=DateTime.Now.AddSeconds(10)
                },
                new MainActivity{
                    Id=5,
                    StartTime=DateTime.Now.AddSeconds(10)
                }
            };
        }
    }
}
