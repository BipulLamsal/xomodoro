using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();

        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            string nickname = NicknameEntry.Text?.Trim();
            string email = EmailEntry.Text?.Trim();
            string password = PasswordEntry.Text?.Trim();

            if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please fill all fields.", "OK");
                return;
            }

            if (!IsValidEmail(email))
            {
                await DisplayAlert("Error", "Invalid email format.", "OK");
                return;
            }

            if (password.Length < 6)
            {
                await DisplayAlert("Error", "Password must be at least 6 characters.", "OK");
                return;
            }

            var db = new DatabaseHelper();

            
            if (db.DoesUserExist(email))
            {
                await DisplayAlert("Error", "This email is already registered.", "OK");
                return;
            }

           
            var user = new User
            {
                Nickname = nickname,
                Email = email,
                Password = password
            };

            var result = db.InsertUser(user);

            if (result > 0)
            {
                await DisplayAlert("Success", "Registration successful!", "OK");
                await Navigation.PushModalAsync(new Login());
            }
            else
            {
                await DisplayAlert("Error", "Registration failed. Try again.", "OK");
            }
        }

        private async void OnLoginLabelTapped(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

    }
}
