using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warden;
using Warden.Core;
using Warden.Watchers;
using Warden.Watchers.MsSql;

namespace TestDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			//var testWatcher = new TestWatcher();
			//var config = WardenConfiguration
			//	.Create()
			//	.AddWatcher(testWatcher, hooks =>
			//		{
			//			hooks.OnCompleted(result => Console.WriteLine($"Completed {result.IsValid} {DateTime.Now:s}"));
			//		})
			//	.WithInterval(TimeSpan.FromSeconds(10))
			//	.Build();

			var configuration = MsSqlWatcherConfiguration
				.Create(@"Data Source=localhost;Initial Catalog=AtomicScope;Integrated Security=True")
				.WithQuery("select * from Config_BusinessProcess")
				.EnsureThat(users => users.Any(user => user.ProcessName == "Kovai.sadf"))
				.Build();

			var mssqlWatcher = MsSqlWatcher.Create("My MSSQL watcher", configuration);

			var wardenConfiguration = WardenConfiguration
				.Create()
				.AddWatcher(mssqlWatcher, hooks => { hooks.OnCompleted(result => Print(result)); })
				.Build();

			var instance = WardenInstance.Create(wardenConfiguration);

			Task.WaitAll(instance.StartAsync());

		}

		static void Print(IWardenCheckResult result)
		{
			//var resultObj = (MsSqlWatcherCheckResult) result;
			Console.WriteLine($"{result.IsValid}");
		}
	}

	public class TestWatcher : IWatcher
	{
		public TestWatcher()
		{
			Name = "Test Watcher";
		}
		public async Task<IWatcherCheckResult> ExecuteAsync()
		{
			return await Task.FromResult(new TestWatcherCheckResult(this, true, ""));
		}

		public string Name { get; }
		public string Group { get; }
	}

	public class TestWatcherCheckResult : WatcherCheckResult
	{
		public TestWatcherCheckResult(IWatcher watcher, bool isValid, string description) : base(watcher, isValid, description)
		{
		}
	}
}
