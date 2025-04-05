using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.SimpleAudioPlayer;

namespace pomodoro_forms
{
    public partial class StopFocus : ContentPage
    {
        private ISimpleAudioPlayer _audioPlayer;

        public StopFocus()
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
            await Navigation.PushAsync(new ShortBreakPage());
        }
    }
}
