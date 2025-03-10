using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace pomodoro_forms
{
    public partial class AddPage : ContentPage
    {
        public AddPage()
        {
            InitializeComponent();
        }

        public static ObservableCollection<Task> TaskList = new ObservableCollection<Task>();

        private void AddTaskButton_Clicked(object sender, EventArgs e)
        {

            string taskName = TaskNameEntry.Text;
            string description = DescriptionEntry.Text;
            DateTime taskDate = TaskDatePicker.Date;
            bool isDone = IsDoneSwitch.IsToggled;

            if (string.IsNullOrWhiteSpace(taskName) || string.IsNullOrWhiteSpace(description))
            {

                DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            Task newTask = new Task
            {
                TaskName = taskName,
                Description = description,
                TaskDate = taskDate,
                IsDone = isDone
            };

            TaskList.Add(newTask);

            DisplayAlert("Task Added", "Your task has been added successfully!", "OK");
            Console.WriteLine($"Task added: {newTask.TaskName}, Total tasks in TaskList: {TaskList.Count}");

            TaskNameEntry.Text = string.Empty;
            DescriptionEntry.Text = string.Empty;
            TaskDatePicker.Date = DateTime.Now;
            IsDoneSwitch.IsToggled = false;
        }
    }
}
