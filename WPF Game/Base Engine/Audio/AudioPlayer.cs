using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Media;

namespace WPF_Game.Base_Engine.Audio
{
    public class AudioPlayer
    {
        public static Dictionary<string, AudioPlayer> Soundtrack = new Dictionary<string, AudioPlayer>(
        );

        public MediaPlayer player;
        public bool running;
        public bool repeat;

        public static void Load(string Name, string Path, bool repeat)
        {
            var ap = new AudioPlayer();
            ap.player = new MediaPlayer();
            ap.player.Open(new Uri(Path));
            ap.player.MediaEnded += delegate
            {
                if (!repeat)
                    ap.running = false;
                else
                {
                    ap.player.Position = TimeSpan.Zero;
                    ap.player.Play();
                }
            };
            ap.repeat = repeat;
            Soundtrack.Add(Name, ap);
        }

        public static void Play(string Name)
        {
            Soundtrack.First(o => o.Key == Name).Value.player.Play();
            Soundtrack.First(o => o.Key == Name).Value.running = true;
        }
    }
}