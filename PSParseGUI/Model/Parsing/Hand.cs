using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSniffGUI.Model.Parsing
{
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    internal class Hand : IEquatable<Hand>
    {
        public Hand(string Raw)
        {
            Players = new List<HandPlayer>();
            Dealer = new HandPlayer { Name = "Dealer", ChipCountStart = 0, ChipCountEnd = 0 };
            Winners = new List<HandPlayer>();
            Action = new List<HandAction>();
            Stats = null;
            PreflopParsed = false;
            this.Raw = Raw;
        }
        public string HandNumber { get; set; }

        [Browsable(false)]
        public DateTime Date { get; set; }

        [Browsable(false)]
        public int BigBlindAmount { get; set; }
        
        [Browsable(false)]
        public int SmallBlindAmount { get; set; }
        
        [Browsable(false)]
        public int PurchasedHandAmount { get; set; }

        [Browsable(false)] 
        public int Rake { get; set; }

        [Browsable(false)] 
        public int TotalPot { get; set; }
        
        [Browsable(false)] 
        public HandPlayer Dealer { get; set; }

        [Browsable(false)]
        public List<HandPlayer> Players { get; set; }

        [Browsable(false)]
        public List<HandPlayer> Winners { get; set; }

        [Browsable(false)]
        public List<HandAction> Action { get; set; }

        [Browsable(false)]
        public HandStats Stats { get; set; }

        [Browsable(false)]
        public bool PreflopParsed { get; set; }

        [Browsable(false)]
        public string Raw { get; set; }

        public bool Equals(Hand Second)
        {
            if (Second is null)
                return false;

            return this.HandNumber == Second.HandNumber;
        }

        public override bool Equals(object obj) => Equals(obj as Hand);
        public override int GetHashCode() => (HandNumber).GetHashCode();

        public override string ToString()
        {
            return string.Format("{0} - {1}: Pot: {2} | Rake: {3} - Winner(s) {4}", HandNumber, Date, TotalPot, Rake, Winners.Count > 0 ? string.Join(",", Winners.Select(c => c.Name)) : "");
        }

    }
}
