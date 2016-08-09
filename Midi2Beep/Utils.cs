﻿using System;
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
                return (int)((440 / 32) * (Math.Pow(2, ((note - 9) / 12))));
            }
            else
            {
                return -1;
            }
        }
    }
}
