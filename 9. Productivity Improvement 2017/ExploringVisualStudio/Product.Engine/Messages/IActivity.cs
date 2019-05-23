#region Using Directives
using System;
#endregion

namespace Product.Engine.Messages
{
    public interface IActivity
    {
        int Id { get; set; }
        DateTime StartTime { get; set; }
    }
}
