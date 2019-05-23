#region Using Directives
using Product.Engine.Messages;
using Product.Engine.Repositories.Messages;
using System.Collections.Generic;
#endregion

namespace Product.Engine.Repositories
{
    public interface IActivitySearchRepository
    {
        int Limit { get; set; }
        List<IActivity> GetActivitiesReport(ActivitiesSearchCriteria criteria);
    }
}
