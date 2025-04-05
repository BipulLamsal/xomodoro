using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class UserPage : ContentPage
    {
        private DatabaseHelper _db;
        private int _currentUserId;

        public UserPage()
        {
            InitializeComponent();
            _db = new DatabaseHelper();
            _currentUserId = Preferences.Get("UserId", -1);

            if (_currentUserId == -1)
            {
                DisplayAlert("Error", "User not found! Please log in again.", "OK");
                Application.Current.MainPage = new Login();
            }
            else
            {
                LoadUserData();
            }
        }

        private void LoadUserData()
        {
            var user = _db.GetUserById(_currentUserId);
            if (user != null)
            {
                NicknameEntry.Text = user.Nickname;
                UsernameEntry.Text = user.Email;
                CurrentPasswordEntry.Text = user.Password;
                NewPasswordEntry.Text = user.Password;
            }
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            var nickname = NicknameEntry.Text;
            var username = UsernameEntry.Text;
            var currentPassword = CurrentPasswordEntry.Text;
            var newPassword = NewPasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(nickname) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                await DisplayAlert("Error", "All fields are required", "OK");
                return;
            }

            var user = _db.GetUserById(_currentUserId);
            if (user == null || user.Password != currentPassword)
            {
                await DisplayAlert("Error", "Current password is incorrect", "OK");
                return;
            }

            user.Nickname = nickname;
            user.Email = username;
            user.Password = newPassword;

            int result = _db.UpdateUser(user);

            if (result > 0)
            {
                await DisplayAlert("Success", "User updated successfully!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to update user", "OK");
            }
        }

        private void LogoutButton_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("UserId");
            Application.Current.MainPage = new Login();
        }
    }
}
