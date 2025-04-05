using Xamarin.Forms;
using Xamarin.Essentials;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace pomodoro_forms
{
    public partial class HomePage : ContentPage
    {
        private DatabaseHelper _db;
        private ObservableCollection<TaskModel> _tasks;

        public HomePage()
        {
            InitializeComponent();
            _db = new DatabaseHelper();
            _tasks = new ObservableCollection<TaskModel>();

            SetGreeting();
            SetUsername();

            ToDoListStack.Children.Clear();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasksAsync();
            UpdateTaskCounts();
            SetUsername();
        }

        private void SetGreeting()
        {
            var hour = DateTime.Now.Hour;

            if (hour >= 5 && hour < 12)
                GreetingLabel.Text = "Good Morning! 👋";
            else if (hour >= 12 && hour < 18)
                GreetingLabel.Text = "Good Afternoon! 👋";
            else
                GreetingLabel.Text = "Good Evening! 👋";
        }

        private void SetUsername()
        {
            int userId = Preferences.Get("UserId", -1);
            if (userId != -1)
            {
                var user = _db.GetUserById(userId);
                UsernameLabel.Text = user != null ? $"Hello, {user.Nickname}" : "Hello, Guest";
            }
            else
            {
                UsernameLabel.Text = "Hello, Guest";
            }
        }


        private async Task LoadTasksAsync()
        {
            int userId = Preferences.Get("UserId", -1);
            if (userId == -1)
            {
                await DisplayAlert("Error", "User not found. Please log in again.", "OK");
                return;
            }

            ToDoListStack.Children.Clear();

            var today = DateTime.Today;
            var tasks = _db.GetTasksByUser(userId)
                           .Where(t => t.TaskDate.Date == today)
                           .ToList();

            _tasks.Clear();
            if (tasks.Count == 0)
            {
                
                var emptyStateImage = new Image
                {
                    Source = "Notasks.png", 
                    HeightRequest = 150,
                    Aspect = Aspect.AspectFit,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 20)
                };

                var emptyStateLabel = new Label
                {
                    Text = "No tasks for today",
                    FontSize = 18,
                    TextColor = Color.Gray,
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = new Thickness(0, 10)
                };

                var emptyStateStack = new StackLayout
                {
                    Children = { emptyStateImage, emptyStateLabel },
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                ToDoListStack.Children.Add(emptyStateStack);
            }
            else
            {
                foreach (var task in tasks)
                {
                    _tasks.Add(task);
                    AddTaskToUI(task);
                }
            }
        }

        private void AddTaskToUI(TaskModel task)
        {
            var checkBox = new CheckBox
            {
                IsChecked = task.IsDone,
                Color = Color.FromHex("#7768E9"),
                VerticalOptions = LayoutOptions.Center
            };

            checkBox.CheckedChanged += async (s, e) =>
            {
                task.IsDone = e.Value;
                _db.UpdateTask(task);
                UpdateTaskCounts();
                
            };



            var taskLayout = new Frame
            {
                CornerRadius = 10,
                Padding = 10,
                BackgroundColor = Color.White,
                Content = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 10,
                    Children =
                    {
                        checkBox,
                        new StackLayout
                        {
                            Orientation = StackOrientation.Vertical,
                            Children =
                            {
                                new Label
                                {
                                    Text = task.TaskName,
                                    FontSize = 16,
                                    TextColor = Color.Black,
                                    FontAttributes = FontAttributes.Bold
                                },
                                new Label
                                {
                                    Text = $"{task.StartTime:hh\\:mm} - {task.EndTime:hh\\:mm}",
                                    FontSize = 14,
                                    TextColor = Color.Gray
                                }
                            }
                        }
                    }
                }
            };

            ToDoListStack.Children.Add(taskLayout);
        }


        private void UpdateTaskCounts()
        {
            int userId = Preferences.Get("UserId", -1);
            if (userId == -1) return;

            var tasks = _db.GetTasksByUser(userId);
            var completedTasksCount = tasks.Count(t => t.IsDone);
            var incompleteTasksCount = tasks.Count(t => !t.IsDone);

            CompletedTask.Text = completedTasksCount.ToString();
            IncompletedTask.Text = incompleteTasksCount.ToString();
        }
        private async void OnProfileButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserPage());
        }

    }
}
