using Plugin.SimpleAudioPlayer;
using System;
using System.Threading;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class LongBreakPage : ContentPage
    {
        private int _secondsRemaining;
        private bool _isRunning = false;
        private bool _isPaused = false;
        private System.Threading.Timer _timer;
        private ISimpleAudioPlayer _audioPlayer;
        private int WorkDuration = SettingsManager.LongBreak * 60;

        public LongBreakPage()
        {
            InitializeComponent();
            _audioPlayer = CrossSimpleAudioPlayer.Current;
            bool loaded = _audioPlayer.Load("tick.wav");
            _audioPlayer.Loop = true;

            _secondsRemaining = WorkDuration;
            UpdateTimerDisplay();
        }

        private async void onFocusButtonClicked(object sender, EventArgs e)
        {

            if (SettingsManager.Sound)
            {
                _audioPlayer.Stop();
            }

            await Navigation.PushAsync(new MainPage());
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
            if (_secondsRemaining > 0)
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

            await Navigation.PushAsync(new StopLongBreak());
        }
    }
}
