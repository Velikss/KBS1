using System.Collections.Generic;
using System.Windows.Media;
using NAudio.Wave;
// ReSharper disable UnusedMember.Local

namespace GameEngine
{
    public class Media
    {
        private MediaFoundationReader audioFile;
        private WaveOutEvent outputDevice;
        private Dictionary<string, MediaPlayer> Soundtrack = new Dictionary<string, MediaPlayer>();

        public void AddMedia(string Name, string Location)
        {
            if (outputDevice == null) outputDevice = new WaveOutEvent();
            if (audioFile == null)
            {
                audioFile = new MediaFoundationReader(@"C:\Users\usr\Downloads\1.wav");
                outputDevice.Init(audioFile);
            }

            outputDevice.Play();
        }
    }
}