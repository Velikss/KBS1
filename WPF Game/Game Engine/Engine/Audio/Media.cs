using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using NAudio.Wave;

namespace GameEngine
{
    public class Media
    {
        private WaveOutEvent outputDevice;
        private MediaFoundationReader audioFile;
        private Dictionary<string, MediaPlayer> Soundtrack = new Dictionary<string, MediaPlayer>();

        public void AddMedia(string Name, string Location)
        {
            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
            }
            if (audioFile == null)
            {
                audioFile = new MediaFoundationReader(@"C:\Users\usr\Downloads\1.wav");
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }
    }
}
