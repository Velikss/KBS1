using NAudio.Wave;

namespace BaseEngine
{
    public class AudioPlayer
    {
        private static WaveOutEvent outputDevice;
        private static WaveMixerStream32 mixer = new WaveMixerStream32();
        private static bool Playing;

        public static void Initialize()
        {
            if (outputDevice == null) outputDevice = new WaveOutEvent();
            outputDevice.Init(mixer);
        }

        public static void PlayStop()
        {
            if (!Playing)
            {
                outputDevice.Play();
                Playing = true;
            }
            else
            {
                outputDevice.Stop();
                Playing = false;
            }
        }

        private readonly LoopStream Stream;
        private readonly WaveChannel32 Channel;

        public AudioPlayer(LoopStream Stream, float Volume, bool Repeat = true)
        {
            this.Stream = Stream;
            this.Stream.Looping = Repeat;
            Channel = new WaveChannel32(this.Stream);
            Channel.PadWithZeroes = false;
            Channel.Volume = Volume;
        }

        public void Play()
        {
            mixer.AddInputStream(Channel);
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
                this.Looping = true;
            }

            /// <summary>
            /// Use this to turn looping on or off
            /// </summary>
            public bool Looping { get; set; }

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
                        if (sourceStream.Position == 0 || !Looping)
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