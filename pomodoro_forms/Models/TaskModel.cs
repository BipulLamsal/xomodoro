using SQLite;
using System;

namespace pomodoro_forms
{
    public class TaskModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string TaskName { get; set; }
        public string Category { get; set; }  
        public string Description { get; set; }
        public DateTime TaskDate { get; set; }
        public TimeSpan StartTime { get; set; }  
        public TimeSpan EndTime { get; set; }    
        public bool IsDone { get; set; }

        public int UserId { get; set; }
    }
}
