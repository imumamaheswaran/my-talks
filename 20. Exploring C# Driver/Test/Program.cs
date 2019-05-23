namespace Test
{
    using System;
    using System.Data.SqlClient;

    internal class Program
    {
        private static void Main()
        {
            try
            {
                var stageActivity = new StageActivity
                {
                    StageActivityId = Guid.NewGuid(),
                    MainActivityId = Guid.NewGuid(),
                    CorrelationToken = string.Empty,
                    CurrentStage = string.Empty,
                    PreviousStage = string.Empty,
                    EndTime = DateTime.Now,
                    LastModifiedOn = DateTime.Now,
                    StartTime = DateTime.Now,
                    Status = StageStatus.Failure,
                    TrackedData = ""

                };

                
                using (var con =
                    new SqlConnection("Data Source=localhost;Initial Catalog=sample;Integrated Security=True"))
                {
                    const string query = "INSERT INTO [dbo].[Tracking_StageActivity]([StageActivityId],[MainActivityId],[EndTime],[StartTime],[CurrentStage],[PreviousStage],[TrackedData],[Status],[LastModifiedOn]) Values (@StageActivityId,@MainActivityId,@EndTime,@StartTime,@CurrentStage,@PreviousStage,@TrackedData,@Status,@LastModifiedOn)";

                   // const string query = "delete * from [dbo].[Tracking_StageActivity]";

                    con.ExecuteWithRetry(query, stageActivity);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Number);
            }

            Console.Read();
        }
    }

    public class StageActivity
    {
        public Guid StageActivityId { get; set; }
        public Guid MainActivityId { get; set; }
        public string CurrentStage { get; set; }
        public string PreviousStage { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CorrelationToken { get; set; }
        public StageStatus Status { get; set; }
        public string TrackedData { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }

    public enum StageStatus
    {
        Success = 0,
        Failure = 1,
    }
}
