using PSniffGUI.Enums;
using PSParseGUI.Model.Export;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSParseGUI.Model.Parsing
{
    internal class Transaction : IEquatable<Transaction>
    {
        public Transaction(Player InPlayer, TransactionType InTransactionType, int InHandNumber, string InStarsHandNumber, int InAmount)
        {
            this.PlayerRef = InPlayer;
            this.TransactionType = InTransactionType;
            this.HandNumber = InHandNumber;
            this.StarsHandNumber = InStarsHandNumber;
            this.Amount = InAmount;
        }


        public string Player { get { return PlayerRef.Name; } }
        public TransactionType TransactionType { get; }
        public int HandNumber { get; }
        public string StarsHandNumber { get; }
        public int Amount { get; }

        [Browsable(false)]
        public Player PlayerRef { get; }

        public bool Equals(Transaction Second)
        {
            if (Second is null)
                return false;

            return this.Player == Second.Player && this.TransactionType == Second.TransactionType && this.StarsHandNumber == Second.StarsHandNumber && this.Amount == Second.Amount;
        }

        public override bool Equals(object obj) => Equals(obj as Transaction);
        public override int GetHashCode() => (Player, TransactionType, StarsHandNumber, Amount).GetHashCode();
    }
}
