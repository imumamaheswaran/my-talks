using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Engine.Repositories;
using RepositoryImplementations = Product.Engine.Repositories.Implementations;
using Product.Engine.Services;

namespace Product.Engine.Tests
{
    [TestClass]
    public class RepositoryTests
    {
        public object RepositoryManager { get; private set; }

        [TestMethod]
        public void TestRepositoryManagerSql()
        {
            var repositoryManager = new RepositoryManager();
            
            var repository = repositoryManager.GetActivitySearchRepository(RepositoryType.Sql);

            Assert.IsInstanceOfType(repository, typeof(RepositoryImplementations.Sql.ActivitiesSearchRepository));
        }

        [TestMethod]
        public void TestRespositoryManagerElasticSearch()
        {
            var repositoryManager = new RepositoryManager();
            
            var repository = repositoryManager.GetActivitySearchRepository(RepositoryType.ElasticSearch);

            Assert.IsInstanceOfType(repository, typeof(RepositoryImplementations.ElasticSearch.ActivitiesSearchRepository));
        }

        [TestMethod]
        public void TestGetActivities()
        {
            var activitiesServices = new ActivitiesServices();

            var activities = activitiesServices.GetActivities(string.Empty,DateTime.Now.AddSeconds(-10));

            Assert.IsNotNull(activities);

        }
    }
}
