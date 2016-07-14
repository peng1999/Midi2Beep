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

        public bool isNoteOn => Velocity == 0;

        public RawMidiCommand(string channel, string absTime, string note, string velocity)
        {
            Channel = int.Parse(channel);
            AbsTime = int.Parse(absTime);
            Note = int.Parse(note);
            Velocity = int.Parse(velocity);
        }

        public BeepCommand ToBeepCommand()
        {
            return new BeepCommand();
        }
    }

    struct BeepCommand
    {
        /// <summary>
        /// 音高
        /// </summary>
        public int Note { get; }

        /// <summary>
        /// 绝对时间
        /// </summary>
        public int AbsTime { get; }

        public BeepCommand(string note, string absTime)
        {
            Note = int.Parse(note);
            AbsTime = int.Parse(absTime);
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
