using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Midi2Beep
{
    class Program
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
        }

        struct MidiCommand
        {
            public int Note { get; }

            public int AbsTime { get; }

            public MidiCommand(string note, string absTime)
            {
                Note = int.Parse(note);
                AbsTime = int.Parse(absTime);
            }
        }
        static void Main(string[] args)
        {
            //Stream input = Console.OpenStandardInput();
            string line;
            List<string> midi = new List<string>();
            while (string.IsNullOrEmpty(line = Console.ReadLine()))
            {
                midi.Add(line);
            }
            const string patten = @"\s*(\d+)\s*,\s*(\d+)\s*,\s*Note_(.{2,3})_c\s*,\s*\d+\s*,\s*(\d+)\s*,\s*(\d+)\s*";
            var midiData =
                midi
                .SelectMany(l =>
                {
                    var m = Regex.Match(l, patten);
                    return m.Success
                        ? new[]
                        {
                            new RawMidiCommand
                            (
                                channel: m.Groups[1].Value,
                                absTime: m.Groups[2].Value,
                                // 我们不需要这一行
                                // IsNoteOn = m.Groups[3].Value == "on",
                                note: m.Groups[4].Value,
                                velocity: m.Groups[5].Value
                            )
                        }
                        : new RawMidiCommand[] { };
                });
            SortedSet<MidiCommand> cmds = new SortedSet<MidiCommand>();
            foreach (var item in midiData)
            {
                //TODO
            }
        }
    }
}
