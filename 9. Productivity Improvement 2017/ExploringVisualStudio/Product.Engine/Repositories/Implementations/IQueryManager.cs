#region Using Directives
using Product.Engine.Repositories.Messages;
#endregion

namespace Product.Engine.Repositories.Implementations
{
    interface IQueryManager<T>
    {
        T GetActivitiesSearchQuery(ActivitiesSearchCriteria activitiesSearchCriteria);
    }
}
