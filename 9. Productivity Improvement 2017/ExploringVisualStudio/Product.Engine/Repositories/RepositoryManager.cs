#region Using Directives
using System;
#endregion

namespace Product.Engine.Repositories
{
    public class RepositoryManager
    {
        public IActivitySearchRepository GetActivitySearchRepository(RepositoryType type)
        {
            switch (type)
            {
                case RepositoryType.Sql:
                    return new Implementations.Sql.ActivitiesSearchRepository();
                case RepositoryType.ElasticSearch:
                    return new Implementations.ElasticSearch.ActivitiesSearchRepository();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
}
