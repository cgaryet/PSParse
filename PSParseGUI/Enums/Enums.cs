using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSniffGUI.Enums
{
    internal enum TablePosition
    {
        Unset = 0,
        SB = 1,
        BB = 2,
        UTG = 3,
        UTG1 = 4,
        MP1 = 5,
        MP2 = 6,
        HJ = 7,
        CO = 8,
        BTN = 9,
        BTNSB = 10
    }
    [Flags]
    public enum ActionStatType : long
    {

        None = 0,
        FlopSeen = 1,
        VPIP = 1L << 2,
        PreflopRaise = 1L << 3,
        CalledPreflopRaise = 1L << 4,
        UnopenedPreflopRaise = 1L << 5,
        ThreeBetPreflop = 1L << 6,
        FoldedTo3BetPreflop = 1L << 7,
        FourBetPreflop = 1L << 8,
        FoldedTo4BetPreflop = 1L << 9,
        Squeeze = 1L << 10,
        BlindStealAttempt = 1L << 11,
        FoldedBigBlindToStealAttempt = 1L << 12,
        BetOrRaisedFlop = 1L << 13,
        BetRasiedCalledFoldedFlop = 1L << 14,
        BetOrRaisedTurn = 1L << 15,
        BetRasiedCalledFoldedTurn = 1L << 16,
        BetOrRaisedRiver = 1L << 17,
        BetRasiedCalledFoldedRiver = 1L << 18,
        CheckRaisedFlop = 1L << 19,
        CheckRaisedTurn = 1L << 20,
        CheckRaisedRiver = 1L << 21,
        CBetFlop = 1L << 22,
        CBetTurn = 1L << 23,
        CBetRiver = 1L << 24,
        CalledCBetFlop = 1L << 25,
        RaisedCBetFlop = 1L << 26,
        FoldedToCBetFlop = 1L << 27,
        FoldedToRaiseAfterCBetFlop = 1L << 28,
        CalledCBetTurn = 1L << 29,
        RaisedCBetTurn = 1L << 30,
        FoldedToCBetTurn = 1L << 31,
        FoldedToRaiseAfterCBetTurn = 1L << 32,
        RaisedCBetRiver = 1L << 33,
        CalledCBetRiver = 1L << 34,
        FoldedToCBetRiver = 1L << 35,
        FoldedToRaiseAfterCBetRiver = 1L << 36,
        DonkBetFlop = 1L << 37,
        DonkBetTurn = 1L << 38,
        DonkBetRiver = 1L << 39,
        WentToShowdown = 1L << 40,
        WonAtShowdown = 1L << 41,
        WonWithoutShowdown = 1L << 42,
    }
    [Flags]
    public enum ActionOpportunityStatFlags
    {
        None = 0,
        UnopenedRaiseOpportunity = 1,
        ThreeBetPreflopOpportunity = 1 << 2,
        FourBetOpportunity = 1 << 3,
        SqueezeOpportunity = 1 << 4,
        BlindStealAttemptOpportunity = 1 << 5,
        FoldedBigBlindToStealAttemptOpportunity = 1 << 6,
        FoldedTo3BetPreflopOpportunity = 1 << 7,
        FoldedTo4BetPreflopOpportunity = 1 << 8,
        CheckRaisedFlopOpportunity = 1 << 9,
        CheckRaisedTurnOpportunity = 1 << 10,
        CheckRaisedRiverOpportunity = 1 << 11,
        CBetFlopOpportunity = 1 << 12,
        CBetTurnOpportunity = 1 << 13,
        CBetRiverOpportunity = 1 << 14,
        FoldedToCBetFlopOpportunity = 1 << 15,
        FoldedToCBetTurnOpportunity = 1 << 16,
        FoldedToCBetRiverOpportunity = 1 << 17,
        DonkBetFlopOpportunity = 1 << 18,
        DonkBetTurnOpportunity = 1 << 19,
        DonkBetRiverOpportunity = 1 << 20
    }

    public enum PlayerAction
    {
        PostSmallBlind = 0,
        PostBigBlind,
        BuyBlinds,
        AlteredToBuyBlinds,
        Check,
        Bet,
        Call,
        Raise,
        Fold,
        ChipsReturned,
        Showdown,
        Win,
        Flop,
        Turn,
        River
    };

    public enum TransactionType
    {
        BuyIn = 0,
        CashOut = 1,
        Addon = 2
    }
}
