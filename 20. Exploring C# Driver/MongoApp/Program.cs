namespace MongoApp
{
    using System;
    using System.Data.SqlClient;

    internal class Program
    {
       
        private static void Main()
        {
            try
            {
                using (var connection =
                    new SqlConnection("Data Source=localhost;Initial Catalog=Sample;Integrated Security=True"))
                {
                    connection.Open();
                    var query =
                        $"INSERT INTO [dbo].[Tracking_StageActivity]([StageActivityId],[MainActivityId],[EndTime],[StartTime],[CurrentStage],[PreviousStage],[TrackedData],[Status],[LastModifiedOn])VALUES({Guid.NewGuid()},{Guid.NewGuid()},{DateTime.Now:s},{DateTime.Now:s},'','','',{1},{DateTime.Now:s})";
                    using (var command = new SqlCommand("", connection))
                    {
                        var result = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.ErrorCode);
            }

            Console.Read();
        }


    }
    
}
