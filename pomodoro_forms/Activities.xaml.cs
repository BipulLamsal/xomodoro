using Xamarin.Forms;
using Xamarin.Essentials;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace pomodoro_forms
{
    public partial class Activities : ContentPage
    {
        private DatabaseHelper _db;
        private ObservableCollection<TaskModel> _tasks;
        private int _userId;
        private DateTime _selectedDate;

        public Activities()
        {
            InitializeComponent();
            _db = new DatabaseHelper();
            _tasks = new ObservableCollection<TaskModel>();
            _selectedDate = DateTime.Today;

            ActivityDatePicker.Date = _selectedDate;

            _userId = Preferences.Get("UserId", -1);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTasksAsync().ConfigureAwait(false);
        }

     

        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            _selectedDate = e.NewDate;
            await LoadTasksAsync();
        }

        private async Task LoadTasksAsync()
        {
            if (_userId == -1)
            {
                await DisplayAlert("Error", "User not found. Please log in again.", "OK");
                return;
            }

            
            TaskCardsStack.Children.Clear();

            var tasks = _db.GetTasksByUser(_userId)
                          .Where(t => t.TaskDate.Date == _selectedDate.Date)
                          .ToList();

            _tasks.Clear();
            foreach (var task in tasks)
            {
                _tasks.Add(task);
            }

  
            if (_tasks.Count == 0)
            {
                ShowEmptyState();
                return;
            }

            // Add task cards to the UI
            foreach (var task in _tasks)
            {
                AddTaskCardToUI(task);
            }
        }

        private void ShowEmptyState()
        {
            var emptyStateImage = new Image
            {
                Source = "Notasks.png", 
                HeightRequest = 150,
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 50)
            };

            var emptyStateLabel = new Label
            {
                Text = $"No activities for {_selectedDate.ToString("MMMM dd, yyyy")}",
                FontSize = 18,
                TextColor = Color.Gray,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 10)
            };

            var emptyStateStack = new StackLayout
            {
                Children = { emptyStateImage, emptyStateLabel },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            TaskCardsStack.Children.Add(emptyStateStack);
        }

        private void AddTaskCardToUI(TaskModel task)
        {
            var checkBox = new CheckBox
            {
                IsChecked = task.IsDone,
                Color = Color.FromHex("#7768E9"),
                VerticalOptions = LayoutOptions.Center
            };

            checkBox.CheckedChanged += async (s, e) => await OnTaskCheckChanged(task, e.Value);

            var taskNameLabel = new Label
            {
                Text = task.TaskName,
                FontSize = 18,
                TextColor = Color.FromHex("#333333"),
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center
            };

            var taskHeaderGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Auto },
            new ColumnDefinition { Width = GridLength.Star }
        }
            };

            taskHeaderGrid.Children.Add(checkBox, 0, 0);
            taskHeaderGrid.Children.Add(taskNameLabel, 1, 0);

            // Task Description
            var descriptionLabel = new Label
            {
                Text = task.Description,
                FontSize = 15,
                TextColor = Color.FromHex("#666666"),
                LineBreakMode = LineBreakMode.WordWrap
            };

            // Task Date Grid
            var dateGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        },
                Margin = new Thickness(0, 5, 0, 0)
            };

            var startDateStack = new StackLayout
            {
                Children =
        {
            new Label
            {
                Text = "Start Date",
                FontSize = 13,
                TextColor = Color.FromHex("#7768E9")
            },
            new Label
            {
                Text = task.TaskDate.ToString("MMMM dd, yyyy"),
                FontSize = 14,
                TextColor = Color.FromHex("#444444")
            }
        }
            };

            dateGrid.Children.Add(startDateStack, 0, 0);

            // Task Time Grid
            var timeGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        },
                Margin = new Thickness(0, 5, 0, 0)
            };

            var startTimeStack = new StackLayout
            {
                Children =
        {
            new Label
            {
                Text = "Start Time",
                FontSize = 13,
                TextColor = Color.FromHex("#7768E9")
            },
            new Label
            {
                Text = task.StartTime.ToString(@"hh\:mm"),
                FontSize = 14,
                TextColor = Color.FromHex("#444444")
            }
        }
            };

            var endTimeStack = new StackLayout
            {
                Children =
        {
            new Label
            {
                Text = "End Time",
                FontSize = 13,
                TextColor = Color.FromHex("#7768E9")
            },
            new Label
            {
                Text = task.EndTime.ToString(@"hh\:mm"),
                FontSize = 14,
                TextColor = Color.FromHex("#444444")
            }
        }
            };

            timeGrid.Children.Add(startTimeStack, 0, 0);
            timeGrid.Children.Add(endTimeStack, 1, 0);

            var actionButtonsGrid = new Grid
            {
                ColumnDefinitions =
        {
            new ColumnDefinition { Width = GridLength.Star },
            new ColumnDefinition { Width = GridLength.Star }
        },
                ColumnSpacing = 15,
                Margin = new Thickness(0, 10, 0, 0)
            };

            var editButton = new Button
            {
                Text = "Edit",
                BackgroundColor = Color.FromHex("#7768E9"),
                TextColor = Color.White,
                FontSize = 13,
                CornerRadius = 8,
                Padding = new Thickness(10, 5),
                HeightRequest = 35
            };

            var deleteButton = new Button
            {
                Text = "Delete",
                BackgroundColor = Color.FromHex("#FF6B6B"),
                TextColor = Color.White,
                FontSize = 13,
                CornerRadius = 8,
                Padding = new Thickness(10, 5),
                HeightRequest = 35
            };

            editButton.Clicked += async (s, e) => await OnEditTaskClicked(task);
            deleteButton.Clicked += async (s, e) => await OnDeleteTaskClicked(task);

            actionButtonsGrid.Children.Add(editButton, 0, 0);
            actionButtonsGrid.Children.Add(deleteButton, 1, 0);

            var taskCardStack = new StackLayout
            {
                Spacing = 12,
                Children =
        {
            taskHeaderGrid,
            descriptionLabel,
            dateGrid,
            timeGrid,
            actionButtonsGrid
        }
            };

            var taskCard = new Frame
            {
                CornerRadius = 15,
                Padding = 15,
                BackgroundColor = Color.White,
                HasShadow = false,
                BorderColor = Color.FromHex("#E0E0E0"),
                Content = taskCardStack
            };

            if (task.IsDone)
            {
                taskNameLabel.TextDecorations = TextDecorations.Strikethrough;
                taskCard.Opacity = 0.7;
            }

            TaskCardsStack.Children.Add(taskCard);
        }


        private async Task OnTaskCheckChanged(TaskModel task, bool isChecked)
        {
            task.IsDone = isChecked;
            int result = _db.UpdateTask(task);

            if (result <= 0)
            {
                await DisplayAlert("Error", "Failed to update task status", "OK");
            }

            // Refresh the UI to show the updated task status
            await LoadTasksAsync();
        }

        private async Task OnEditTaskClicked(TaskModel task)
        {
            // Navigate to the AddPage, passing the task ID
            await Navigation.PushAsync(new AddPage(task));
        }

        private async Task OnDeleteTaskClicked(TaskModel task)
        {
            bool confirm = await DisplayAlert("Confirm Delete",
                $"Are you sure you want to delete '{task.TaskName}'?", "Yes", "No");

            if (confirm)
            {
                int result = _db.DeleteTask(task.Id);

                if (result > 0)
                {
                    await DisplayAlert("Success", "Task deleted successfully", "OK");
                    await LoadTasksAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete task", "OK");
                }
            }
        }
    }
}
