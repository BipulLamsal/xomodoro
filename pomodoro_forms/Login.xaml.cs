using System;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class Login : ContentPage
    {
        private DatabaseHelper _db;

        public Login()
        {
            InitializeComponent();
            _db = new DatabaseHelper(); 
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            var user = _db.GetUserByEmailAndPassword(email, password);

            if (user != null)
            {

                Preferences.Set("UserId", user.Id);

                await DisplayAlert("Success", "Login successful!", "OK");
                Application.Current.MainPage = new AppShell(); 
            }
            else
            {
                await DisplayAlert("Error", "Invalid email or password.", "OK");
            }
        }

        private async void OnRegisterLabelTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Register());
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }
    }
}
