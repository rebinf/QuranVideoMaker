using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuranVideoMaker.Data
{
    [DebuggerDisplay("{ToString()}")]
    public struct TimeCode
    {
        public double TotalFrames { get; }

        [JsonIgnore]
        public double TotalSeconds { get; }

        public double FPS { get; }

        [JsonIgnore]
        public int Hours { get; }

        [JsonIgnore]
        public int Minutes { get; }

        [JsonIgnore]
        public int Seconds { get; }

        [JsonIgnore]
        public double Frame { get; }

        public static readonly TimeCode Zero = new TimeCode();

        [JsonConstructor]
        public TimeCode(double totalFrames, double fps)
        {
            TotalFrames = totalFrames;
            FPS = fps;

            if (totalFrames != default)
            {
                Hours = Convert.ToInt32(Math.Round(totalFrames / (3600 * fps), MidpointRounding.ToZero));
                Minutes = Convert.ToInt32(Math.Round(totalFrames % (3600 * fps) / (60 * fps), MidpointRounding.ToZero));
                Seconds = Convert.ToInt32(Math.Round(totalFrames % (3600 * fps) % (60 * fps) / fps, MidpointRounding.ToZero));
                Frame = Convert.ToInt32(Math.Round(totalFrames % (3600 * fps) % (60 * fps) % fps, MidpointRounding.ToZero));
                TotalSeconds = new TimeSpan(Hours, Minutes, Seconds).TotalSeconds;
            }
            else
            {
                TotalSeconds = default;
                Hours = 0;
                Minutes = 0;
                Seconds = 0;
                Frame = 0;
            }
        }

        public TimeCode AddFrames(int frames)
        {
            return new TimeCode(TotalFrames + frames, FPS);
        }

        public static TimeCode FromSeconds(double seconds, double fps)
        {
            var frames = seconds * fps;
            return new TimeCode(frames, fps);
        }

        public static TimeCode FromMilliseconds(double milliseconds, double fps)
        {
            var frames = (milliseconds / 1000d) * fps;
            return new TimeCode(frames, fps);
        }

        public static TimeCode FromTime(int hours, int minutes, int seconds, double fps)
        {
            var ts = new TimeSpan(hours, minutes, seconds);
            return FromSeconds(ts.TotalSeconds, fps);
        }

        public override string ToString()
        {
            return $"{Hours:00}:{Minutes:00}:{Seconds:00}:{Frame:00}";
        }

        ///<inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is TimeCode info && info.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return TotalFrames.GetHashCode() ^ FPS.GetHashCode();
        }

        public static bool operator ==(TimeCode t1, TimeCode t2)
        {
            return t1.Equals(t2);
        }

        public static bool operator !=(TimeCode t1, TimeCode t2)
        {
            return !(t1 == t2);
        }

        public static TimeCode operator +(TimeCode t1, TimeCode t2)
        {
            return new TimeCode(Convert.ToInt32(t1.TotalFrames + t2.TotalFrames), Convert.ToInt32(t1.FPS));
        }

        public static TimeCode operator -(TimeCode t1, TimeCode t2)
        {
            return new TimeCode(Convert.ToInt32(t1.TotalFrames - t2.TotalFrames), Convert.ToInt32(t1.FPS));
        }

        public static bool operator <(TimeCode t1, TimeCode t2)
        {
            return (t1.TotalFrames < t2.TotalFrames);
        }

        public static bool operator >(TimeCode t1, TimeCode t2)
        {
            return (t1.TotalFrames > t2.TotalFrames);
        }

        public static bool operator <=(TimeCode t1, TimeCode t2)
        {
            return (t1.TotalFrames <= t2.TotalFrames);
        }

        public static bool operator >=(TimeCode t1, TimeCode t2)
        {
            return (t1.TotalFrames >= t2.TotalFrames);
        }
    }
}
