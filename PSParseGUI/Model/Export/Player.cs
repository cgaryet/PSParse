using PSniffGUI.Model.Parsing;
using PSParseGUI.Model.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSParseGUI.Model.Export
{
    internal class Player
    {
        public Player()
        {
            HandsPlayed = new List<Hand>();
            HandsWon = new List<Hand>();
            Transactions = new List<Transaction>();
        }
        public string Name { get; set; }
        public int TotalRake { get; set; }
        public int RakeBack { get; set; }
        public int HouseRake { get; set; }
        public int TotalChipCountIn { get; set; }
        public int TotalChipCountOut { get; set; }
        public int TotalWithRakeback { get; set; }
        public int Result { get; set; }
        public List<Hand> HandsPlayed { get; set; }
        public List<Hand> HandsWon { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
