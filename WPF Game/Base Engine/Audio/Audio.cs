using NAudio.Wave;

namespace BaseEngine
{
    public class AudioPlayer
    {
        private static WaveOutEvent outputDevice;
        private static readonly WaveMixerStream32 mixer = new WaveMixerStream32();
        private static bool Playing;
        private readonly WaveChannel32 Channel;

        private readonly LoopStream Stream;

        public AudioPlayer(string Path, float Volume, bool Repeat = true)
        {
            Stream = new LoopStream(new WaveFileReader(Path));
            Stream.Looping = Repeat;
            Channel = new WaveChannel32(Stream);
            Channel.PadWithZeroes = false;
            Channel.Volume = Volume;
        }

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

        public void Play()
        {
            mixer.AddInputStream(Channel);
        }

        public class LoopStream : WaveStream
        {
            private readonly WaveStream sourceStream;

            /// <summary>
            ///     Creates a new Loop stream
            /// </summary>
            /// <param name="sourceStream">
            ///     The stream to read from. Note: the Read method of this stream should return 0 when it reaches the end
            ///     or else we will not loop to the start again.
            /// </param>
            public LoopStream(WaveStream sourceStream)
            {
                this.sourceStream = sourceStream;
                Looping = true;
            }

            /// <summary>
            ///     Use this to turn looping on or off
            /// </summary>
            public bool Looping { get; set; }

            /// <summary>
            ///     Return source stream's wave format
            /// </summary>
            public override WaveFormat WaveFormat => sourceStream.WaveFormat;

            /// <summary>
            ///     LoopStream simply returns
            /// </summary>
            public override long Length => sourceStream.Length;

            /// <summary>
            ///     LoopStream simply passes on positioning to source stream
            /// </summary>
            public override long Position
            {
                get => sourceStream.Position;
                set => sourceStream.Position = value;
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                var totalBytesRead = 0;

                while (totalBytesRead < count)
                {
                    var bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                    if (bytesRead == 0)
                    {
                        if (sourceStream.Position == 0 || !Looping) break;

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