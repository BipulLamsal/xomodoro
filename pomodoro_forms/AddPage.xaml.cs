using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class AddPage : ContentPage
    {
        private DatabaseHelper _db;
        private int _currentUserId;
        private TaskModel _task;

        public AddPage() : this(null)
        {
        }
        public AddPage(TaskModel task = null) 
        {
            InitializeComponent();
            _db = new DatabaseHelper();

            _currentUserId = Preferences.Get("UserId", -1);

            if (_currentUserId == -1)
            {
                DisplayAlert("Error", "User not found! Please log in again.", "OK");
                Application.Current.MainPage = new Login();
            }

            _task = task;

            if (_task != null)
            {
                // Populate fields with task details
                TaskNameEntry.Text = _task.TaskName;
                CategoryPicker.SelectedItem = _task.Category;
                DescriptionEntry.Text = _task.Description;
                TaskDatePicker.Date = _task.TaskDate;
                StartTimePicker.Time = _task.StartTime;
                EndTimePicker.Time = _task.EndTime;
                IsDoneSwitch.IsChecked = _task.IsDone;
            }
        }

        private async void AddTaskButton_Clicked(object sender, EventArgs e)
        {
            string taskName = TaskNameEntry.Text;
            string category = CategoryPicker.SelectedItem?.ToString();
            string description = DescriptionEntry.Text;
            DateTime taskDate = TaskDatePicker.Date;
            TimeSpan startTime = StartTimePicker.Time;
            TimeSpan endTime = EndTimePicker.Time;
            bool isDone = IsDoneSwitch.IsChecked;

            if (string.IsNullOrWhiteSpace(taskName) || string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            if (endTime <= startTime)
            {
                await DisplayAlert("Error", "End time must be later than start time", "OK");
                return;
            }

            if (_task == null)
            {
                _task = new TaskModel
                {
                    TaskName = taskName,
                    Category = category,
                    Description = description,
                    TaskDate = taskDate,
                    StartTime = startTime,
                    EndTime = endTime,
                    UserId = _currentUserId,
                    IsDone = isDone
                };

                int result = _db.InsertTask(_task);

                if (result > 0)
                {
                    await DisplayAlert("Success", "Task added successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to add task", "OK");
                }
            }
            else
            {
                _task.TaskName = taskName;
                _task.Category = category;
                _task.Description = description;
                _task.TaskDate = taskDate;
                _task.StartTime = startTime;
                _task.EndTime = endTime;
                _task.IsDone = isDone;

                int result = _db.UpdateTask(_task);

                if (result > 0)
                {
                    await DisplayAlert("Success", "Task updated successfully!", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update task", "OK");
                }
            }

            await Navigation.PushAsync(new AddPage());
            Navigation.RemovePage(this);
        }
    }


}
