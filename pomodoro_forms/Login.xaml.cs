using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();

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


            if (email == "admin@gmail.com" && password == "admin")
            {
                await DisplayAlert("Success", "Login successful!", "OK");
                await Navigation.PushModalAsync(new MainPage());

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
