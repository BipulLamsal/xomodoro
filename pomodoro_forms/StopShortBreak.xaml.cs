using Plugin.SimpleAudioPlayer;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace pomodoro_forms
{
    public partial class StopShortBreak : ContentPage
    {
        private ISimpleAudioPlayer _audioPlayer;

        public StopShortBreak()
        {
            InitializeComponent();

            _audioPlayer = CrossSimpleAudioPlayer.Current;
            bool loaded = _audioPlayer.Load("audio.mp3");

            if (loaded)
            {
                _audioPlayer.Loop = true;
                _audioPlayer.Play();
            }

        }

        private async void NextScreen(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LongBreakPage());
            
        }
    }
}

