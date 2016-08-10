using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi2Beep
{
    //struct BeepNote
    //{
    //    public int Frequency { get; }
    //    // public int Velocity { get; set; }
    //    public int Time { get; }
    //    public BeepNote(string frequency, string time)
    //    {
    //        Frequency = int.Parse(frequency);
    //        Time = int.Parse(time);
    //    }
    //}

    /// <summary>
    /// 原始 Midi 数据
    /// </summary>
    struct RawMidiCommand
    {
        /// <summary>
        /// 通道
        /// </summary>
        public int Channel { get; }

        /// <summary>
        /// 绝对时间
        /// </summary>
        public int AbsTime { get; }

        /// <summary>
        /// 音高
        /// </summary>
        public int Note { get; }

        /// <summary>
        /// 响度
        /// </summary>
        public int Velocity { get; }

        public bool IsNoteOn => Velocity > 0;

        public RawMidiCommand(string channel, string absTime, string note, string velocity)
        {
            Channel = int.Parse(channel);
            AbsTime = int.Parse(absTime);
            Note = int.Parse(note);
            Velocity = int.Parse(velocity);
        }

        public static bool operator == (RawMidiCommand lhs, RawMidiCommand rhs)
        {
            return lhs.AbsTime == rhs.AbsTime
                && lhs.Note == rhs.Note 
                && lhs.Velocity == rhs.Velocity;
        }

        public static bool operator != (RawMidiCommand lhs, RawMidiCommand rhs)
        {
            return !(lhs == rhs);
        }
    }

    struct BeepCommand : IFormattable
    {
        /// <summary>
        /// 音高
        /// </summary>
        public int? Frequency { get; }

        /// <summary>
        /// 绝对时间
        /// </summary>
        public int TimeSpan { get; }

        public BeepCommand(int note, int timeSpan)
        {
            Frequency = Utils.NoteToFrequency(note);
            TimeSpan = timeSpan;
        }

        public override string ToString()
        {
            return string.Format($"{this:CPP}");
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (format)
            {
                case "CPP":
                    if (Frequency != null)
                    {
                        return $"beep({Frequency}, {TimeSpan});";
                    }
                    else
                    {
                        return $"sleep({TimeSpan});";
                    }
                case "BAT":
                    if (Frequency != null)
                    {
                        return $"beep {Frequency} {TimeSpan}";
                    }
                    else
                    {
                        return $"beep 1 1 /s {TimeSpan}";
                    }
                default:
                    return base.ToString();
            }
        }
    }

    class MidiCommandByNoteComparer : IComparer<RawMidiCommand>
        {
            public int Compare(RawMidiCommand x, RawMidiCommand y)
            {
                return x.Note.CompareTo(y.Note);
            }
        }

}
