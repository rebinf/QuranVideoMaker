using System.Text.Json.Serialization;

namespace QuranVideoMaker.Data
{
    /// <summary>
    /// represents a time code.
    /// </summary>
    public struct TimeCode
    {
        /// <summary>
        /// total frames 
        /// </summary>
        public double TotalFrames { get; }

        /// <summary>
        /// frames per second
        /// </summary>
        public double FPS { get; }

        /// <summary>
        /// total seconds
        /// </summary>
        [JsonIgnore]
        public double TotalSeconds { get; }

        /// <summary>
        /// hours component of timestamp
        /// </summary>
        [JsonIgnore]
        public int Hours { get; }

        /// <summary>
        /// minutes component of timestamp
        /// </summary>
        [JsonIgnore]
        public int Minutes { get; }

        /// <summary>
        /// seconds component of timestamp
        /// </summary>
        [JsonIgnore]
        public int Seconds { get; }

        /// <summary>
        /// frame component of timestamp
        /// </summary>
        [JsonIgnore]
        public double Frame { get; }

        /// <summary>
        /// Zero time code
        /// </summary>
        [JsonIgnore]
        public static readonly TimeCode Zero = new TimeCode();

        /// <summary>
        /// create new instance of TimeCode with default values.
        /// </summary>
        public TimeCode()
        {
            TotalFrames = 0;
            FPS = 0;
            TotalSeconds = 0;
            Hours = 0;
            Minutes = 0;
            Seconds = 0;
            Frame = 0;
        }

        /// <summary>
        /// create new instance of TimeCode with specific values for total frames and frames per second
        /// </summary>
        /// <param name="totalFrames">total frames</param>
        /// <param name="fps">frames per second</param>
        [JsonConstructor]
        public TimeCode(double totalFrames, double fps) : this()
        {
            if (fps > 0)
            {
                FPS = fps;
            }

            if (totalFrames > 0 && fps > 0)
            {
                Hours = Convert.ToInt32(Math.Round(totalFrames / (3600 * fps), MidpointRounding.ToZero));
                Minutes = Convert.ToInt32(Math.Round(totalFrames % (3600 * fps) / (60 * fps), MidpointRounding.ToZero));
                Seconds = Convert.ToInt32(Math.Round(totalFrames % (3600 * fps) % (60 * fps) / fps, MidpointRounding.ToZero));
                Frame = Convert.ToInt32(Math.Round(totalFrames % (3600 * fps) % (60 * fps) % fps, MidpointRounding.ToZero));
                TotalSeconds = new TimeSpan(Hours, Minutes, Seconds).TotalSeconds;
                TotalFrames = totalFrames;
                FPS = fps;
            }
        }

        /// <summary>
        /// adds a number of frames to the TimeCode instance.
        /// </summary>
        /// <param name="frames">frame count to add to the instance</param>
        /// <returns>TimeCode instance modified with the specified frames</returns>
        public TimeCode AddFrames(int frames)
        {
            return new TimeCode(TotalFrames + frames, FPS);
        }

        /// <summary>
        /// adds a number of seconds to the TimeCode instance.
        /// </summary>
        /// <param name="seconds">seconds to add to the instance</param>
        /// <returns>TimeCode instance modified with the specified seconds</returns>
        public TimeCode AddSeconds(int seconds)
        {
            return new TimeCode(TotalFrames + seconds * FPS, FPS);
        }

        /// <summary>
        /// creates a TimeCode instance from seconds and FPS values.
        /// </summary>
        /// <param name="seconds">time span in seconds</param>
        /// <param name="fps">frames per second</param>
        /// <returns>TimeCode instance representing the time span in seconds at the specified frames per second</returns>
        public static TimeCode FromSeconds(double seconds, double fps)
        {
            return new TimeCode(seconds * fps, fps);
        }

        /// <summary>
        /// creates a TimeCode instance from System.TimeSpan and FPS values.
        /// </summary>
        /// <param name="timeSpan">time span to convert</param>
        /// <param name="fps">frames per second</param>
        /// <returns>TimeCode instance representing the time span at the specified frames per second</returns>
        public static TimeCode FromTimeSpan(TimeSpan timeSpan, double fps)
        {
            return new TimeCode(timeSpan.TotalSeconds * fps, fps);
        }

        /// <summary>
        /// create a TimeCode instance from TimeCode components.
        /// </summary>
        /// <param name="hours">hours component of the timestamp</param>
        /// <param name="minutes">minutes component of the timestamp</param>
        /// <param name="seconds">seconds component of the timestamp</param>
        /// <param name="fps">frames per second</param>
        /// <returns>TimeCode instance from specified components</returns>
        public static TimeCode FromTime(int hours, int minutes, int seconds, double fps)
        {
            return new TimeCode(new TimeSpan(hours, minutes, seconds).TotalSeconds * fps, fps);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        public static bool operator ==(TimeCode a, TimeCode b)
        {
            return a.TotalFrames == b.TotalFrames && a.FPS == b.FPS;
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        public static bool operator !=(TimeCode a, TimeCode b)
        {
            return a.TotalFrames != b.TotalFrames || a.FPS != b.FPS;
        }

        /// <summary>
        /// Addition operator
        /// </summary>
        public static TimeCode operator +(TimeCode a, TimeCode b)
        {
            return new TimeCode(a.TotalFrames + b.TotalFrames, a.FPS);
        }

        /// <summary>
        /// Subtraction operator
        /// </summary>
        public static TimeCode operator -(TimeCode a, TimeCode b)
        {
            return new TimeCode(a.TotalFrames - b.TotalFrames, a.FPS);
        }

        /// <summary>
        /// Greater-than operator
        /// </summary>
        public static bool operator >(TimeCode a, TimeCode b)
        {
            return a.TotalFrames > b.TotalFrames;
        }

        /// <summary>
        /// Less-than operator
        /// </summary>
        public static bool operator <(TimeCode a, TimeCode b)
        {
            return a.TotalFrames < b.TotalFrames;
        }

        /// <summary>
        /// Greater-than-or-equal operator
        /// </summary>
        public static bool operator >=(TimeCode a, TimeCode b)
        {
            return a.TotalFrames >= b.TotalFrames;
        }

        /// <summary>
        /// Less-than-or-equal operator
        /// </summary>
        public static bool operator <=(TimeCode a, TimeCode b)
        {
            return a.TotalFrames <= b.TotalFrames;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is TimeCode code && code.TotalFrames == TotalFrames && code.FPS == FPS;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Hours:00}:{Minutes:00}:{Seconds:00}:{Frame:00}";
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (TotalFrames + FPS).GetHashCode();
        }

        internal TimeSpan ToTimeSpan()
        {
            return new TimeSpan(0, Hours, Minutes, Seconds, Convert.ToInt32((Frame * 1000d) / FPS));
        }
    }
}
