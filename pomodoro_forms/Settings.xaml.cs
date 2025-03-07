using System;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            FocusTimeEntry.Text = SettingsManager.FocusTime.ToString();
            ShortBreakEntry.Text = SettingsManager.ShortBreak.ToString();
            LongBreakEntry.Text = SettingsManager.LongBreak.ToString();
            NotificationsToggle.IsToggled = SettingsManager.Notifications;
            SoundToggle.IsToggled = SettingsManager.Sound;
            AutoResumeToggle.IsToggled = SettingsManager.AutoResume;
        }

        private void OnFocusTimeEntryChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry != null)
            {
                if (string.IsNullOrEmpty(entry.Text)) return;

                if (!int.TryParse(entry.Text, out int result))
                {
                    entry.Text = e.OldTextValue;
                    return;
                }

                if (result < 0 || result > 60)
                {
                    entry.Text = e.OldTextValue;
                }
            }
        }

        private async void OnSaveSettings(object sender, EventArgs e)
        {
            SettingsManager.FocusTime = int.TryParse(FocusTimeEntry.Text, out var focusTime) ? focusTime : SettingsManager.FocusTimeDefault;
            SettingsManager.ShortBreak = int.TryParse(ShortBreakEntry.Text, out var shortBreak) ? shortBreak : SettingsManager.ShortBreakDefault;
            SettingsManager.LongBreak = int.TryParse(LongBreakEntry.Text, out var longBreak) ? longBreak : SettingsManager.LongBreakDefault;
            SettingsManager.AutoResume = AutoResumeToggle.IsToggled;
            SettingsManager.Sound = SoundToggle.IsToggled;
            SettingsManager.Notifications = NotificationsToggle.IsToggled;

            MessagingCenter.Send(this, "SettingsUpdated");
            await Navigation.PopModalAsync();
        }

        private async void OnCloseSettings(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
