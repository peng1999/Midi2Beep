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
            SortedSet<RawMidiCommand> currentCmdSet = new SortedSet<RawMidiCommand>(new MidiCommandByNoteComparer());
            List<BeepCommand> cmds = new List<BeepCommand>();
            foreach (var item in midiData)
            {
                RawMidiCommand current = currentCmdSet.Max;
                if (item.isNoteOn)
                {
                    currentCmdSet.Add(item);
                }
                else
                {
                    currentCmdSet.RemoveWhere(midiCommand => midiCommand.Note == item.Note);
                }
                // 结束的时候插入到列表中。
                if (current.Note != currentCmdSet.Max.Note)
                    cmds.Add(current.ToBeepCommand());
            }
        }
    }
}
