using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Midi2Beep
{
    class Utils
    {
        public static int NoteToFrequency(int note)
        {
            if (note != 0)
            {
                return (int)((440D / 32D) * (Math.Pow(2, ((double)note - 9D) / 12D)));
            }
            else
            {
                return -1;
            }
        }
    }
}
