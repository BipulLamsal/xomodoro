using System;
using System.Threading;
using Xamarin.Forms;
using Plugin.SimpleAudioPlayer;

namespace pomodoro_forms
{
    public partial class MainPage : ContentPage
    {
        private int _secondsRemaining;
        private bool _isRunning = false;
        private bool _isPaused = false;
        private System.Threading.Timer _timer;
        private ISimpleAudioPlayer _audioPlayer;
        private int WorkDuration = SettingsManager.FocusTime * 60;

        public MainPage()
        {
            InitializeComponent();

            _audioPlayer = CrossSimpleAudioPlayer.Current;
            bool loaded = _audioPlayer.Load("tick.wav");
            _audioPlayer.Loop = true;


            MessagingCenter.Subscribe<Settings>(this, "SettingsUpdated", (sender) =>
            {
                WorkDuration = SettingsManager.FocusTime * 60;
                _secondsRemaining = WorkDuration;
                UpdateTimerDisplay();
            });

            _secondsRemaining = WorkDuration;
            UpdateTimerDisplay();
        }

        private async void OnShortBreakButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ShortBreakPage());
        }

        private async void OnSettingsIconTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Settings());
        }

        private void OnStartPauseResumeClicked(object sender, EventArgs e)
        {
            if (!_isRunning)
            {
                _isRunning = true;

                
                if (SettingsManager.Sound)
                {
                    _audioPlayer.Play();
                }

                ShortBreakButton.IsEnabled = false;
                ShortBreakButton.Opacity = 0;
                _timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
                UpdatePauseResumeIcon();
            }
            else if (_isPaused)
            {
                _isPaused = false;


                if (SettingsManager.Sound)
                {
                    _audioPlayer.Play();
                }

                ShortBreakButton.IsEnabled = false;
                ShortBreakButton.Opacity = 0;
                _timer.Change(0, 1000);
                UpdatePauseResumeIcon();
            }
            else
            {
                _isPaused = true;

                if (SettingsManager.Sound)
                {
                    _audioPlayer.Pause();
                }

                ShortBreakButton.IsEnabled = true;
                ShortBreakButton.Opacity = 1;
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
                UpdatePauseResumeIcon();
            }
        }

        private void UpdatePauseResumeIcon()
        {
            if (_isPaused)
            {
                StartPauseButton.Text = "\uf04b"; 
            }
            else
            {
                StartPauseButton.Text = "\uf04c"; 
            }
        }

        private void OnRestartClicked(object sender, EventArgs e)
        {
            _secondsRemaining = WorkDuration;
            UpdateTimerDisplay();
        }

        private void TimerCallback(object state)
        {
            if (_secondsRemaining > 1)
            {
                _secondsRemaining--;
                Device.BeginInvokeOnMainThread(UpdateTimerDisplay);
            }
            else
            {
                _timer?.Change(Timeout.Infinite, Timeout.Infinite);

                
                if (SettingsManager.Sound)
                {
                    _audioPlayer.Stop();
                }

                Device.BeginInvokeOnMainThread(HandleTimerEnd);
            }
        }

        private void UpdateTimerDisplay()
        {
            int minutes = _secondsRemaining / 60;
            int seconds = _secondsRemaining % 60;
            minuteLabel.Text = minutes.ToString("D2");
            secondLabel.Text = seconds.ToString("D2");
        }

        private async void HandleTimerEnd()
        {
            
            if (SettingsManager.Sound)
            {
                _audioPlayer.Stop();
            }

            await Navigation.PushModalAsync(new StopFocus());
        }
    }
}
