using System;
using System.Collections.ObjectModel;
using System.Linq;  // For LINQ methods
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class HomePage : ContentPage
    {
        // Static Task List with predefined data
        public ObservableCollection<Task> TaskList { get; set; }
        public ObservableCollection<Task> FilteredTaskList { get; set; }

        // Static Activity Log
        public ObservableCollection<ActivityLog> FilteredActivityLog { get; set; }

        public HomePage()
        {
            InitializeComponent();

            // Initialize task list with more past tasks, some done, some pending
            TaskList = new ObservableCollection<Task>
            {
                new Task { TaskName = "Swimming", TaskDate = DateTime.Today.AddDays(-1), Description = "Swim 30 laps", IsDone = false },
                new Task { TaskName = "Coding", TaskDate = DateTime.Today.AddDays(-2), Description = "Code Pomodoro app", IsDone = true },
                new Task { TaskName = "Reading", TaskDate = DateTime.Today.AddDays(-3), Description = "Read 'Clean Code'", IsDone = false },
                new Task { TaskName = "Exercise", TaskDate = DateTime.Today, Description = "Full-body workout", IsDone = false },
                new Task { TaskName = "Study Math", TaskDate = DateTime.Today.AddDays(-1), Description = "Math homework", IsDone = true },
                new Task { TaskName = "Yoga", TaskDate = DateTime.Today.AddDays(-2), Description = "Yoga practice", IsDone = false },
                new Task { TaskName = "Portfolio", TaskDate = DateTime.Today.AddDays(-3), Description = "Update portfolio", IsDone = false },
                new Task { TaskName = "Grocery Shopping", TaskDate = DateTime.Today.AddDays(-4), Description = "Buy groceries", IsDone = false },
                new Task { TaskName = "Gym", TaskDate = DateTime.Today.AddDays(-4), Description = "Leg day", IsDone = true },
                new Task { TaskName = "Project Meeting", TaskDate = DateTime.Today.AddDays(-5), Description = "Discuss project", IsDone = false },
                new Task { TaskName = "Laundry", TaskDate = DateTime.Today.AddDays(-5), Description = "Do laundry", IsDone = true },
                new Task { TaskName = "Blog Writing", TaskDate = DateTime.Today.AddDays(-1), Description = "Write blog post", IsDone = false },
                new Task { TaskName = "Gardening", TaskDate = DateTime.Today.AddDays(-3), Description = "Water plants", IsDone = true },
                new Task { TaskName = "Call Friend", TaskDate = DateTime.Today.AddDays(-2), Description = "Catch up call", IsDone = false },
                new Task { TaskName = "Art Practice", TaskDate = DateTime.Today.AddDays(-4), Description = "Sketching", IsDone = false }
            };

            // Initialize filtered task list with only pending tasks from 0 to -3 days
            FilteredTaskList = new ObservableCollection<Task>(
                TaskList.Where(t => !t.IsDone && t.TaskDate >= DateTime.Today.AddDays(-3) && t.TaskDate <= DateTime.Today)
            );

            // Initialize activity log with tasks from 0 to -3 days (both done and pending)
            FilteredActivityLog = new ObservableCollection<ActivityLog>(
                TaskList.Where(t => t.TaskDate >= DateTime.Today.AddDays(-3) && t.TaskDate <= DateTime.Today)
                        .Select(t => new ActivityLog
                        {
                            Activity = $"{t.TaskName} - {t.Description} - Status: {(t.IsDone ? "Completed" : "Pending")}",
                            Date = t.TaskDate
                        })
            );

            // Set the binding context to the page's view model
            BindingContext = this;
        }

        // Handle DatePicker selection and filter tasks based on selected date
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (e.NewDate != null)
            {
                var selectedDate = e.NewDate.Date;
                FilteredActivityLog.Clear();
                FilteredTaskList.Clear();

                // Filter tasks for the selected date that are within -3 to 0 days
                var filteredTasks = TaskList.Where(t => t.TaskDate.Date == selectedDate && t.TaskDate >= DateTime.Today.AddDays(-3) && t.TaskDate <= DateTime.Today);

                foreach (var task in filteredTasks)
                {
                    string status = task.IsDone ? "Completed" : "Pending";
                    FilteredActivityLog.Add(new ActivityLog
                    {
                        Activity = $"{task.TaskName} - {task.Description} - Status: {status}",
                        Date = task.TaskDate
                    });

                    // Add to filtered list if not done
                    if (!task.IsDone)
                        FilteredTaskList.Add(task);
                }
            }
        }

        // Command to mark task as done
        private Command MarkDoneCommand => new Command((task) =>
        {
            var selectedTask = task as Task;
            if (selectedTask != null)
            {
                selectedTask.IsDone = true;
                FilteredTaskList.Remove(selectedTask);  // Remove if done
            }
        });

        // Command to delete a task
        private Command DeleteCommand => new Command((task) =>
        {
            var selectedTask = task as Task;
            if (selectedTask != null)
            {
                TaskList.Remove(selectedTask);
                FilteredTaskList.Remove(selectedTask);
            }
        });
    }

    // Task Model
    public class Task
    {
        public string TaskName { get; set; }
        public DateTime TaskDate { get; set; }
        public string Description { get; set; }
        public bool IsDone { get; set; }
    }

    // Activity Log Model
    public class ActivityLog
    {
        public string Activity { get; set; }
        public DateTime Date { get; set; }
    }
}
