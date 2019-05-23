using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Warden;
using Warden.Core;
using Warden.Watchers;
using Warden.Watchers.MsSql;
using static System.Console;
namespace Demo
{
	class Program
	{
		static void Main(string[] args)
		{
			//var testWatcher = new TestWatcher();
			//var configuration = WardenConfiguration
			//	.Create()
			//	.AddWatcher(testWatcher, hooks =>
			//	{
			//		hooks.OnCompleted(result => Console.WriteLine($"Completed {DateTime.Now:s}"));

			//	})
			//	.WithInterval(TimeSpan.FromSeconds(10))
			//	.Build();
			//var warden = WardenInstance.Create(configuration);


			var mssqlconfiguration = MsSqlWatcherConfiguration
				.Create(@"Data Source=localhost;Initial Catalog=AtomicScope;Integrated Security=True")
				.WithQuery("select * from Config_BusinessProcess", new Dictionary<string, object> { ["id"] = 1 })
				.EnsureThat(users => users.Any(user => user.ProcessName == "Kovai.Sales"))
				.Build();
			var mssqlWatcher = MsSqlWatcher.Create("My MSSQL watcher", mssqlconfiguration);

			var configuration = WardenConfiguration
				.Create()
				.AddWatcher(mssqlWatcher, hooks => { hooks.OnCompleted(result => PrintResult(result)); })
				.Build();

			var warden = WardenInstance.Create(configuration);
			Task.WaitAll(warden.StartAsync());

			Read();

		}

		static void PrintResult(IWardenCheckResult result)
		{
			Console.WriteLine($"{result.WatcherCheckResult.WatcherName} is {result.IsValid}");
		}
	}

	


	public class TestWatcher : IWatcher
	{
		public TestWatcher()
		{
			Name = "Test";
		}
		public async Task<IWatcherCheckResult> ExecuteAsync()
		{


			return await Task.FromResult(new TestCheckResult(this, true, ""));
		}

		public string Name { get; }
		public string Group { get; }
	}


	public class TestCheckResult : WatcherCheckResult
	{
		public TestCheckResult(IWatcher watcher, bool isValid, string description) : base(watcher, isValid, description)
		{
		}
	}
}
