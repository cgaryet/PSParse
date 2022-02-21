using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSniffGUI.Model.Parsing
{
    internal class HandStats
    {
        public HandStats()
        {

            // calculated stuff
            // CheckRaises
            // Donk Bets

            FlopSeen = new HashSet<HandPlayer>();
            VPIP = new HashSet<HandPlayer>();
            PreflopRaise = new HashSet<HandPlayer>();
            CalledPreflopRaise = new HashSet<HandPlayer>();
            UnopenedPreflopRaise = new HashSet<HandPlayer>();
            UnopenedRaiseOpportunity = new HashSet<HandPlayer>();
            ThreeBetPreflop = new HashSet<HandPlayer>();
            ThreeBetOpportunity = new HashSet<HandPlayer>();
            FoldedTo3BetPreflop = new HashSet<HandPlayer>();
            FoldedTo3BetPreflopOpportunity = new HashSet<HandPlayer>();
            FourBetPreflop = new HashSet<HandPlayer>();
            FourBetPreflopOpportunity = new HashSet<HandPlayer>();
            FoldedTo4BetPreflop = new HashSet<HandPlayer>();
            FoldedTo4BetPreflopOpportunity = new HashSet<HandPlayer>();
            Squeeze = new HashSet<HandPlayer>();
            SqueezeOpportunity = new HashSet<HandPlayer>();
            BlindStealAttempt = new HashSet<HandPlayer>();
            BlindStealAttemptOpportunity = new HashSet<HandPlayer>();
            FoldedBigBlindToStealAttempt = new HashSet<HandPlayer>();
            FoldedBigBlindToStealAttemptOpportunity = new HashSet<HandPlayer>();

            // postflop stuff
            BetOrRaisedFlop = new HashSet<HandPlayer>();
            BetRasiedCalledFoldedFlop = new HashSet<HandPlayer>();
            BetOrRaisedTurn = new HashSet<HandPlayer>();
            BetRasiedCalledFoldedTurn = new HashSet<HandPlayer>();
            BetOrRaisedRiver = new HashSet<HandPlayer>();
            BetRasiedCalledFoldedRiver = new HashSet<HandPlayer>();
            CheckRaisedFlop = new HashSet<HandPlayer>();
            CheckRaisedFlopOpportunity = new HashSet<HandPlayer>();
            CheckRaisedTurn = new HashSet<HandPlayer>();
            CheckRaisedTurnOpportunity = new HashSet<HandPlayer>();
            CheckRaisedRiver = new HashSet<HandPlayer>();
            CheckRaisedRiverOpportunity = new HashSet<HandPlayer>();
            CBetFlop = new HashSet<HandPlayer>();
            CBetFlopOpportunity = new HashSet<HandPlayer>();
            CBetTurn = new HashSet<HandPlayer>();
            CBetTurnOpportunity = new HashSet<HandPlayer>();
            CBetRiver = new HashSet<HandPlayer>();
            CBetRiverOpportunity = new HashSet<HandPlayer>();
            CalledCBetFlop = new HashSet<HandPlayer>();
            RaisedCBetFlop = new HashSet<HandPlayer>();
            FoldedToCBetFlop = new HashSet<HandPlayer>();
            FoldedToCBetFlopOpportunity = new HashSet<HandPlayer>();
            FoldedToRaiseAfterCBetFlop = new HashSet<HandPlayer>();
            CalledCBetTurn = new HashSet<HandPlayer>();
            RaisedCBetTurn = new HashSet<HandPlayer>();
            FoldedToCBetTurn = new HashSet<HandPlayer>();
            FoldedToCBetTurnOpportunity = new HashSet<HandPlayer>();
            FoldedToRaiseAfterCBetTurn = new HashSet<HandPlayer>();
            RaisedCBetRiver = new HashSet<HandPlayer>();
            CalledCBetRiver = new HashSet<HandPlayer>();
            FoldedToCBetRiver = new HashSet<HandPlayer>();
            FoldedToCBetRiverOpportunity = new HashSet<HandPlayer>();
            FoldedToRaiseAfterCBetRiver = new HashSet<HandPlayer>();
            DonkBetFlop = new HashSet<HandPlayer>();
            DonkBetFlopOpportunity = new HashSet<HandPlayer>();
            DonkBetTurn = new HashSet<HandPlayer>();
            DonkBetTurnOpportunity = new HashSet<HandPlayer>();
            DonkBetRiver = new HashSet<HandPlayer>();
            DonkBetRiverOpportunity = new HashSet<HandPlayer>();
            WentToShowdown = new HashSet<HandPlayer>();
            WonAtShowdown = new HashSet<HandPlayer>();
            WonWithoutShowdown = new HashSet<HandPlayer>();
        }
        public HashSet<HandPlayer> FlopSeen { get; set; }
        public HashSet<HandPlayer> VPIP { get; set; }
        public HashSet<HandPlayer> PreflopRaise { get; set; }
        public HashSet<HandPlayer> CalledPreflopRaise { get; set; }
        public HashSet<HandPlayer> UnopenedPreflopRaise { get; set; }
        public HashSet<HandPlayer> UnopenedRaiseOpportunity { get; set; }
        public HashSet<HandPlayer> ThreeBetPreflop { get; set; }
        public HashSet<HandPlayer> ThreeBetOpportunity { get; set; }
        public HashSet<HandPlayer> FoldedTo3BetPreflop { get; set; }
        public HashSet<HandPlayer> FoldedTo3BetPreflopOpportunity { get; set; }
        public HashSet<HandPlayer> FourBetPreflop { get; set; }
        public HashSet<HandPlayer> FourBetPreflopOpportunity { get; set; }
        public HashSet<HandPlayer> FoldedTo4BetPreflop { get; set; }
        public HashSet<HandPlayer> FoldedTo4BetPreflopOpportunity { get; set; }
        public HashSet<HandPlayer> Squeeze { get; set; }
        public HashSet<HandPlayer> SqueezeOpportunity { get; set; }
        public HashSet<HandPlayer> BlindStealAttempt { get; set; }
        public HashSet<HandPlayer> BlindStealAttemptOpportunity { get; set; }
        public HashSet<HandPlayer> FoldedBigBlindToStealAttempt { get; set; }
        public HashSet<HandPlayer> FoldedBigBlindToStealAttemptOpportunity { get; set; }
        public HashSet<HandPlayer> BetOrRaisedFlop { get; set; }
        public HashSet<HandPlayer> BetRasiedCalledFoldedFlop { get; set; }
        public HashSet<HandPlayer> BetOrRaisedTurn { get; set; }
        public HashSet<HandPlayer> BetRasiedCalledFoldedTurn { get; set; }
        public HashSet<HandPlayer> BetOrRaisedRiver { get; set; }
        public HashSet<HandPlayer> BetRasiedCalledFoldedRiver { get; set; }
        public HashSet<HandPlayer> CheckRaisedFlop { get; set; }
        public HashSet<HandPlayer> CheckRaisedFlopOpportunity { get; set; }
        public HashSet<HandPlayer> CheckRaisedTurn { get; set; }
        public HashSet<HandPlayer> CheckRaisedTurnOpportunity { get; set; }
        public HashSet<HandPlayer> CheckRaisedRiver { get; set; }
        public HashSet<HandPlayer> CheckRaisedRiverOpportunity { get; set; }
        public HashSet<HandPlayer> CBetFlop { get; set; }
        public HashSet<HandPlayer> CBetFlopOpportunity { get; set; }
        public HashSet<HandPlayer> RaisedCBetFlop { get; set; }
        public HashSet<HandPlayer> CalledCBetFlop { get; set; }
        public HashSet<HandPlayer> FoldedToCBetFlop { get; set; }
        public HashSet<HandPlayer> FoldedToCBetFlopOpportunity { get; set; }
        public HashSet<HandPlayer> FoldedToRaiseAfterCBetFlop { get; set; }
        public HashSet<HandPlayer> CBetTurn { get; set; }
        public HashSet<HandPlayer> CBetTurnOpportunity { get; set; }
        public HashSet<HandPlayer> RaisedCBetTurn { get; set; }
        public HashSet<HandPlayer> CalledCBetTurn { get; set; }
        public HashSet<HandPlayer> FoldedToCBetTurn { get; set; }
        public HashSet<HandPlayer> FoldedToCBetTurnOpportunity { get; set; }
        public HashSet<HandPlayer> FoldedToRaiseAfterCBetTurn { get; set; }
        public HashSet<HandPlayer> CBetRiver { get; set; }
        public HashSet<HandPlayer> CBetRiverOpportunity { get; set; }
        public HashSet<HandPlayer> RaisedCBetRiver { get; set; }
        public HashSet<HandPlayer> CalledCBetRiver { get; set; }
        public HashSet<HandPlayer> FoldedToCBetRiver { get; set; }
        public HashSet<HandPlayer> FoldedToCBetRiverOpportunity { get; set; }
        public HashSet<HandPlayer> FoldedToRaiseAfterCBetRiver { get; set; }
        public HashSet<HandPlayer> DonkBetFlop { get; set; }
        public HashSet<HandPlayer> DonkBetFlopOpportunity { get; set; }
        public HashSet<HandPlayer> DonkBetTurn { get; set; }
        public HashSet<HandPlayer> DonkBetTurnOpportunity { get; set; }
        public HashSet<HandPlayer> DonkBetRiver { get; set; }
        public HashSet<HandPlayer> DonkBetRiverOpportunity { get; set; }
        public HashSet<HandPlayer> WentToShowdown { get; set; }
        public HashSet<HandPlayer> WonAtShowdown { get; set; }
        public HashSet<HandPlayer> WonWithoutShowdown { get; set; }
    }
}
