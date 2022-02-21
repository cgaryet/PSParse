using PSniffGUI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSniffGUI.Model.Parsing
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    internal class HandPlayer
    {
        public HandPlayer()
        {
            Position = TablePosition.Unset;
        }
        public string Name { get; set; }
        public int ChipCountStart { get; set; }
        public int ChipCountEnd { get; set; }

        public TablePosition Position { get; set; }

        public int SeatNumber { get; set; }

        public override string ToString()
        {
            return string.Format("{0}({1}): {2} -> {3}", Name, Position, ChipCountStart, ChipCountEnd);
        }
    }
}
