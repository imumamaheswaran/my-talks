namespace Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using Dapper;

    /// <summary>
    /// Source
    /// https://docs.microsoft.com/en-us/sql/connect/ado-net/step-4-connect-resiliently-to-sql-with-ado-net
    /// </summary>
    public static class DapperExtension
    {

        private static readonly List<int> TransientErrorNumbers =
            new List<int> { 4060, 40197, 40501, 40613,
                49918, 49919, 49920, 11001,547 };

        /**
         * 547 - SQL ERROR CODE
         * The %ls statement conflicted with the %ls constraint "%.*ls". The conflict occurred in database "%.*ls", table "%.*ls"%ls%.*ls%ls.
         * 547 error code is actually not a transient error 
         * *
         */

        public static int ExecuteWithRetry(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return Retry(() => cnn.Execute(sql, param));
        }

        public static IEnumerable<T> Query<T>(this IDbConnection cnn, string sql, object param = null,
            IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null,
            CommandType? commandType = null)
        {
            return Retry(() => cnn.Query<T>(sql, param));
        }

        private static T Retry<T>(Func<T> func, int totalNumberOfTimesToTry = 4, int retryIntervalSeconds = 10)
        {
            var result = default(T);
            for (var tries = 1; tries <= totalNumberOfTimesToTry; tries++)
            {
                try
                {
                    if (tries > 1)
                    {
                        Console.WriteLine("Transient error encountered. Will begin attempt number {0} of {1} max...",
                            tries, totalNumberOfTimesToTry
                        );
                        Thread.Sleep(1000 * retryIntervalSeconds);
                        retryIntervalSeconds = Convert.ToInt32(retryIntervalSeconds * 1.5);
                    }

                    result = func();
                    break;
                }
                catch (SqlException e)
                {
                    if (TransientErrorNumbers.Contains(e.Number))
                    {
                        continue;
                    }

                    throw;

                }
            }

            return result;
        }


    }


    public sealed class MyClass
    {
        private MyClass() { }
        private static readonly object Lock = new object();
        private static MyClass _instance;
        public static MyClass Instance
        {
            get
            {
                lock (Lock)
                {
                    return _instance ?? (_instance = new MyClass());
                }

            }
        }

        public void Do() { }
    }

    public class User
    {
        public void Do()
        {
            MyClass.Instance.Do();
        }
    }
}
