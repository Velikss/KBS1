using System.Collections.Generic;
using System.Windows.Media;
using NAudio.Wave;

namespace BaseEngine
{
    public class Audio
    {
        private WaveOutEvent outputDevice;

        public WaveMixerStream32 mixer = new WaveMixerStream32();

        public void Play()
        {
            if (outputDevice == null) outputDevice = new WaveOutEvent();
            var audioFile = new WaveFileReader(@"C:\Users\usr\Downloads\1.wav");
            var audioFile2 =
                new WaveFileReader(@"C:\Users\usr\Documents\GitHub\Runch\WPF Game\bin\Debug\Music\Stage 2.wav");

            LoopStream background = new LoopStream(audioFile);

            LoopStream background2 = new LoopStream(audioFile2);
            var background32 = new WaveChannel32(background);
            background32.PadWithZeroes = false;
            // set the volume of background file
            background32.Volume = 0.4f;
            var background322 = new WaveChannel32(background2);
            background322.PadWithZeroes = false;
            // set the volume of background file
            background322.Volume = 0.4f;
            //add stream into the mixer
            mixer.AddInputStream(background32);
            mixer.AddInputStream(background322);
            outputDevice.Init(mixer);

            outputDevice.Play();
        }

        public class LoopStream : WaveStream
        {
            WaveStream sourceStream;

            /// <summary>
            /// Creates a new Loop stream
            /// </summary>
            /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
            /// or else we will not loop to the start again.</param>
            public LoopStream(WaveStream sourceStream)
            {
                this.sourceStream = sourceStream;
                this.EnableLooping = true;
            }

            /// <summary>
            /// Use this to turn looping on or off
            /// </summary>
            public bool EnableLooping { get; set; }

            /// <summary>
            /// Return source stream's wave format
            /// </summary>
            public override WaveFormat WaveFormat
            {
                get { return sourceStream.WaveFormat; }
            }

            /// <summary>
            /// LoopStream simply returns
            /// </summary>
            public override long Length
            {
                get { return sourceStream.Length; }
            }

            /// <summary>
            /// LoopStream simply passes on positioning to source stream
            /// </summary>
            public override long Position
            {
                get { return sourceStream.Position; }
                set { sourceStream.Position = value; }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                int totalBytesRead = 0;

                while (totalBytesRead < count)
                {
                    int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        if (sourceStream.Position == 0 || !EnableLooping)
                        {
                            // something wrong with the source stream
                            break;
                        }

                        // loop
                        sourceStream.Position = 0;
                    }

                    totalBytesRead += bytesRead;
                }

                return totalBytesRead;
            }
        }
    }
}