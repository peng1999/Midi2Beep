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
        static void Main(string[] args)
        {
            //Stream input = Console.OpenStandardInput();
            // bool extractChannel = false;
            IEnumerable<int> channels = null;
            if (args.Length > 0)
            {
                channels = args[0].Split(',').Select(str => int.Parse(str));
            }
            string line;
            List<string> midi = new List<string>();
            while (!string.IsNullOrEmpty(line = Console.ReadLine()))
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
            if (channels != null)
            {
                midiData = midiData.Where(cmd => channels.Contains(cmd.Channel));
            }
            SortedSet<RawMidiCommand> currentCmdSet = new SortedSet<RawMidiCommand>(new MidiCommandByNoteComparer());
            currentCmdSet.Add(new RawMidiCommand());
            List<BeepCommand> cmds = new List<BeepCommand>();
            foreach (var current in midiData)
            {
                RawMidiCommand prev = currentCmdSet.Max;
                // 维护 currentCmdSet 性质
                if (current.IsNoteOn)
                {
                    currentCmdSet.Add(current);
                }
                else
                {
                    currentCmdSet.RemoveWhere(midiCommand => midiCommand.Note == current.Note);
                }
                // 结束的时候插入到列表中。
                // On -> [Off]
                // Off || [On]
                if (prev.Note != currentCmdSet.Max.Note)
                {
                    cmds.Add(prev.ToBeepCommand(current.AbsTime));
                }
            }

            //List<BeepCommand> cmds = 

            string format = "BAT";
            if (args.Length > 1)
            {
                format = args[1];
            }
            foreach (var cmd in cmds)
            {
                Console.WriteLine(string.Format($"{{0:{format}}}", cmd));
            }
        }
    }
}
