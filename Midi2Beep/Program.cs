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
        struct BeepNote
        {
            public int Frequency { get; set; }
            // public int Velocity { get; set; }
            public int Time { get; set; }
        }
        struct RawMidiCommand
        {
            public int Channel { get; set; }
            public int AbsTime { get; set;}
            public int Note { get; set; }
            public int Velocity { get; set; }
        }
        struct MidiCommand
        {
            public int Note { get; set; }
            public int AbsTime { get; set; }
        }
        static void Main(string[] args)
        {
            //Stream input = Console.OpenStandardInput();
            string line;
            List<string> midi = new List<string>();
            while (String.IsNullOrEmpty(line = Console.ReadLine()))
            {
                midi.Add(line);
            }
            const string patten = @"\s*(\d+)\s*,\s*(\d+)\s*,\s*Note_(.{2,3})_c\s*,\s*\d+\s*,\s*(\d+)\s*,\s*(\d+)\s*";
            var midiData =
                midi
                .Where(l => Regex.IsMatch(l, patten))
                .Select(l =>
                {
                    var m = Regex.Match(l, patten);
                    return new
                    {
                        Channel = m.Groups[1].Value,
                        Time = m.Groups[2].Value,
                        // IsNoteOn = m.Groups[3].Value == "on",
                        Note = m.Groups[4].Value,
                        Velocity = m.Groups[5].Value
                    };
                })
                .Select(d => new RawMidiCommand()
                {
                    AbsTime = int.Parse(d.Time),
                    Channel = int.Parse(d.Channel),
                    Velocity = int.Parse(d.Velocity),
                    Note = int.Parse(d.Note)
                });
            SortedSet<MidiCommand> cmds = new SortedSet<MidiCommand>();

        }
    }
}
