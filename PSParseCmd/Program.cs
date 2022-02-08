using CsvHelper;
using CsvHelper.Configuration;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PSParseCmd.PlayerActionTaken;

namespace PSParseCmd
{
    public class Player
    {
        public Player()
        {
            HandsPlayed = new List<Hand>();
            HandsWon = new List<Hand>();
            Transactions = new List<Tuple<string, int, int>>();
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
        public List<Tuple<string, int, int>> Transactions { get; set; }
    }
    public enum TablePosition
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
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class PlayerInHand
    {
        public PlayerInHand()
        {
            Position = TablePosition.Unset;
        }
        public string Name { get; set; }
        public int ChipCountStart { get; set; }
        public int ChipCountEnd { get; set; }

        public TablePosition Position { get; set; }


        public override string ToString()
        {
            return string.Format("{0}({1}): {2} -> {3}", Name, Position, ChipCountStart, ChipCountEnd);
        }

    }

    public class PlayerInDatabase
    {
        public string Name { get; set; }
        public int HandsPlayed { get; set; }
        public int VPIP { get; set; }
        public int PreflopRaises { get; set; }
        public int FlopsSeen { get; set; }
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
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class PlayerActionTaken
    {
        public PlayerActionTaken()
        {
            FinalChipCommit = true;
            ActionStatFlags = ActionStatType.None;
            OpportunityStatFlags = ActionOpportunityStatFlags.None;
        }
        public enum PlayerAction
        {
            PostSmallBlind = 0,
            PostBigBlind,
            BuyBlinds,
            AlteredToSBBuyBlinds,
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
        public PlayerInHand Player { get; set; }
        public Hand Hand { get; set; }
        public PlayerAction Action { get; set; }
        public int Amount { get; set; }
        public bool FinalChipCommit { get; set; }
        public ActionStatType ActionStatFlags { get; set; }
        public ActionOpportunityStatFlags OpportunityStatFlags { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1} {{{2}}} -- {{{3}}}", Player.Name, Action, ActionStatFlags.ToString(), OpportunityStatFlags.ToString());
        }
    }
    public class HandStatistics
    {
        public HandStatistics()
        {
            // calculated stuff
            // CheckRaises
            // Donk Bets

            FlopSeen = new HashSet<PlayerInHand>();
            VPIP = new HashSet<PlayerInHand>();
            PreflopRaise = new HashSet<PlayerInHand>();
            CalledPreflopRaise = new HashSet<PlayerInHand>();
            UnopenedPreflopRaise = new HashSet<PlayerInHand>();
            UnopenedRaiseOpportunity = new HashSet<PlayerInHand>();
            ThreeBetPreflop = new HashSet<PlayerInHand>();
            ThreeBetOpportunity = new HashSet<PlayerInHand>();
            FoldedTo3BetPreflop = new HashSet<PlayerInHand>();
            FoldedTo3BetPreflopOpportunity = new HashSet<PlayerInHand>();
            FourBetPreflop = new HashSet<PlayerInHand>();
            FourBetPreflopOpportunity = new HashSet<PlayerInHand>();
            FoldedTo4BetPreflop = new HashSet<PlayerInHand>();
            FoldedTo4BetPreflopOpportunity = new HashSet<PlayerInHand>();
            Squeeze = new HashSet<PlayerInHand>();
            SqueezeOpportunity = new HashSet<PlayerInHand>();
            BlindStealAttempt = new HashSet<PlayerInHand>();
            BlindStealAttemptOpportunity = new HashSet<PlayerInHand>();
            FoldedBigBlindToStealAttempt = new HashSet<PlayerInHand>();
            FoldedBigBlindToStealAttemptOpportunity = new HashSet<PlayerInHand>();

            // postflop stuff
            BetOrRaisedFlop = new HashSet<PlayerInHand>();
            BetRasiedCalledFoldedFlop = new HashSet<PlayerInHand>();
            BetOrRaisedTurn = new HashSet<PlayerInHand>();
            BetRasiedCalledFoldedTurn = new HashSet<PlayerInHand>();
            BetOrRaisedRiver = new HashSet<PlayerInHand>();
            BetRasiedCalledFoldedRiver = new HashSet<PlayerInHand>();
            CheckRaisedFlop = new HashSet<PlayerInHand>();
            CheckRaisedFlopOpportunity = new HashSet<PlayerInHand>();
            CheckRaisedTurn = new HashSet<PlayerInHand>();
            CheckRaisedTurnOpportunity = new HashSet<PlayerInHand>();
            CheckRaisedRiver = new HashSet<PlayerInHand>();
            CheckRaisedRiverOpportunity = new HashSet<PlayerInHand>();
            CBetFlop = new HashSet<PlayerInHand>();
            CBetFlopOpportunity = new HashSet<PlayerInHand>();
            CBetTurn = new HashSet<PlayerInHand>();
            CBetTurnOpportunity = new HashSet<PlayerInHand>();
            CBetRiver = new HashSet<PlayerInHand>();
            CBetRiverOpportunity = new HashSet<PlayerInHand>();
            CalledCBetFlop = new HashSet<PlayerInHand>();
            RaisedCBetFlop = new HashSet<PlayerInHand>();
            FoldedToCBetFlop = new HashSet<PlayerInHand>();
            FoldedToCBetFlopOpportunity = new HashSet<PlayerInHand>();
            FoldedToRaiseAfterCBetFlop = new HashSet<PlayerInHand>();
            CalledCBetTurn = new HashSet<PlayerInHand>();
            RaisedCBetTurn = new HashSet<PlayerInHand>();
            FoldedToCBetTurn = new HashSet<PlayerInHand>();
            FoldedToCBetTurnOpportunity = new HashSet<PlayerInHand>();
            FoldedToRaiseAfterCBetTurn = new HashSet<PlayerInHand>();
            RaisedCBetRiver = new HashSet<PlayerInHand>();
            CalledCBetRiver = new HashSet<PlayerInHand>();
            FoldedToCBetRiver = new HashSet<PlayerInHand>();
            FoldedToCBetRiverOpportunity = new HashSet<PlayerInHand>();
            FoldedToRaiseAfterCBetRiver = new HashSet<PlayerInHand>();
            DonkBetFlop = new HashSet<PlayerInHand>();
            DonkBetFlopOpportunity = new HashSet<PlayerInHand>();
            DonkBetTurn = new HashSet<PlayerInHand>();
            DonkBetTurnOpportunity = new HashSet<PlayerInHand>();
            DonkBetRiver = new HashSet<PlayerInHand>();
            DonkBetRiverOpportunity = new HashSet<PlayerInHand>();
            WentToShowdown = new HashSet<PlayerInHand>();
            WonAtShowdown = new HashSet<PlayerInHand>();
            WonWithoutShowdown = new HashSet<PlayerInHand>();
        }
        public HashSet<PlayerInHand> FlopSeen { get; set; }
        public HashSet<PlayerInHand> VPIP { get; set; }
        public HashSet<PlayerInHand> PreflopRaise { get; set; }
        public HashSet<PlayerInHand> CalledPreflopRaise { get; set; }
        public HashSet<PlayerInHand> UnopenedPreflopRaise { get; set; }
        public HashSet<PlayerInHand> UnopenedRaiseOpportunity { get; set; }
        public HashSet<PlayerInHand> ThreeBetPreflop { get; set; }
        public HashSet<PlayerInHand> ThreeBetOpportunity { get; set; }
        public HashSet<PlayerInHand> FoldedTo3BetPreflop { get; set; }
        public HashSet<PlayerInHand> FoldedTo3BetPreflopOpportunity { get; set; }
        public HashSet<PlayerInHand> FourBetPreflop { get; set; }
        public HashSet<PlayerInHand> FourBetPreflopOpportunity { get; set; }
        public HashSet<PlayerInHand> FoldedTo4BetPreflop { get; set; }
        public HashSet<PlayerInHand> FoldedTo4BetPreflopOpportunity { get; set; }
        public HashSet<PlayerInHand> Squeeze { get; set; }
        public HashSet<PlayerInHand> SqueezeOpportunity { get; set; }
        public HashSet<PlayerInHand> BlindStealAttempt { get; set; }
        public HashSet<PlayerInHand> BlindStealAttemptOpportunity { get; set; }
        public HashSet<PlayerInHand> FoldedBigBlindToStealAttempt { get; set; }
        public HashSet<PlayerInHand> FoldedBigBlindToStealAttemptOpportunity { get; set; }
        public HashSet<PlayerInHand> BetOrRaisedFlop { get; set; }
        public HashSet<PlayerInHand> BetRasiedCalledFoldedFlop { get; set; }
        public HashSet<PlayerInHand> BetOrRaisedTurn { get; set; }
        public HashSet<PlayerInHand> BetRasiedCalledFoldedTurn { get; set; }
        public HashSet<PlayerInHand> BetOrRaisedRiver { get; set; }
        public HashSet<PlayerInHand> BetRasiedCalledFoldedRiver { get; set; }
        public HashSet<PlayerInHand> CheckRaisedFlop { get; set; }
        public HashSet<PlayerInHand> CheckRaisedFlopOpportunity { get; set; }
        public HashSet<PlayerInHand> CheckRaisedTurn { get; set; }
        public HashSet<PlayerInHand> CheckRaisedTurnOpportunity { get; set; }
        public HashSet<PlayerInHand> CheckRaisedRiver { get; set; }
        public HashSet<PlayerInHand> CheckRaisedRiverOpportunity { get; set; }
        public HashSet<PlayerInHand> CBetFlop { get; set; }
        public HashSet<PlayerInHand> CBetFlopOpportunity { get; set; }
        public HashSet<PlayerInHand> RaisedCBetFlop { get; set; }
        public HashSet<PlayerInHand> CalledCBetFlop { get; set; }
        public HashSet<PlayerInHand> FoldedToCBetFlop { get; set; }
        public HashSet<PlayerInHand> FoldedToCBetFlopOpportunity { get; set; }
        public HashSet<PlayerInHand> FoldedToRaiseAfterCBetFlop { get; set; }
        public HashSet<PlayerInHand> CBetTurn { get; set; }
        public HashSet<PlayerInHand> CBetTurnOpportunity { get; set; }
        public HashSet<PlayerInHand> RaisedCBetTurn { get; set; }
        public HashSet<PlayerInHand> CalledCBetTurn { get; set; }
        public HashSet<PlayerInHand> FoldedToCBetTurn { get; set; }
        public HashSet<PlayerInHand> FoldedToCBetTurnOpportunity { get; set; }
        public HashSet<PlayerInHand> FoldedToRaiseAfterCBetTurn { get; set; }
        public HashSet<PlayerInHand> CBetRiver { get; set; }
        public HashSet<PlayerInHand> CBetRiverOpportunity { get; set; }
        public HashSet<PlayerInHand> RaisedCBetRiver { get; set; }
        public HashSet<PlayerInHand> CalledCBetRiver { get; set; }
        public HashSet<PlayerInHand> FoldedToCBetRiver { get; set; }
        public HashSet<PlayerInHand> FoldedToCBetRiverOpportunity { get; set; }
        public HashSet<PlayerInHand> FoldedToRaiseAfterCBetRiver { get; set; }
        public HashSet<PlayerInHand> DonkBetFlop { get; set; }
        public HashSet<PlayerInHand> DonkBetFlopOpportunity { get; set; }
        public HashSet<PlayerInHand> DonkBetTurn { get; set; }
        public HashSet<PlayerInHand> DonkBetTurnOpportunity { get; set; }
        public HashSet<PlayerInHand> DonkBetRiver { get; set; }
        public HashSet<PlayerInHand> DonkBetRiverOpportunity { get; set; }
        public HashSet<PlayerInHand> WentToShowdown { get; set; }
        public HashSet<PlayerInHand> WonAtShowdown { get; set; }
        public HashSet<PlayerInHand> WonWithoutShowdown { get; set; }

    }
    [System.Diagnostics.DebuggerDisplay("{ToString()}")]
    public class Hand
    {
        public Hand()
        {
            Players = new List<PlayerInHand>();
            Dealer = new PlayerInHand { Name = "Dealer", ChipCountStart = 0, ChipCountEnd = 0 };
            Winners = new List<PlayerInHand>();
            Action = new List<PlayerActionTaken>();
            Stats = null;
            PreflopParsed = false;
        }
        public string HandNumber { get; set; }
        public DateTime Date { get; set; }

        public int BigBlindAmount { get; set; }
        public int SmallBlindAmount { get; set; }
        public int PurchasedHandAmount { get; set; }
        public int Rake;
        public int TotalPot;
        public PlayerInHand Dealer { get; set; }
        public List<PlayerInHand> Players { get; set; }
        public List<PlayerInHand> Winners { get; set; }
        public List<PlayerActionTaken> Action { get; set; }
        public HandStatistics Stats { get; set; }
        public bool PreflopParsed { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}: {2} | {3}", HandNumber, Date, TotalPot, Rake);
        }

    }

    class Program
    {
        static List<Hand> Hands = new List<Hand>();
        static List<Player> PlayerList = new List<Player>();
        static List<PlayerInDatabase> PlayersInDatabase = new List<PlayerInDatabase>();

        static string HandNumberRegex = "PokerStars.+#(\\d+).+- (\\S+ \\S+)";
        static string SeatRegex = "Seat \\d: (.+) \\((\\d+) in chips\\)";
        static string BlindRegex = "(.+): posts (small|big|small & big) blind[s]? (\\d+)";
        static string FoldRegex = "(.+): (folds|checks)";
        static string BetOrCallRegex = "(.+): (bets|calls) (\\d+)";
        static string RaiseRegex = "(.+): raises (\\d+) to (\\d+)";
        static string UncalledBetRegex = "Uncalled bet \\((\\d+)\\) returned to (.+)";
        static string CollectionRegex = "(.+) collected (\\d+) from(?: side| main)? pot";
        static string TotalPotAndRake = "Total pot (\\d+)(?: Main pot \\d+\\. Side pot \\d+\\.)? \\| Rake (\\d+)";
        static string StreetRegex = "\\*\\*\\* (FLOP|TURN|RIVER) \\*\\*\\* .+";
        static string WentToShowdownRegex = "(.+): (shows|mucks)";


        static void Main(string[] args)
        {
            string FileText = File.ReadAllText(args[0]);
            bool WriteStats = false;

            int EmptyLineCounter = 0;
            Hand NewHand = new Hand();
            // parse each line of text in the log
            foreach (string Line in FileText.Split('\n'))
            {
                if (string.IsNullOrWhiteSpace(Line))
                {
                    EmptyLineCounter++;
                }
                else
                {
                    ParseLine(NewHand, Line.Trim());
                }
                // save off
                if (EmptyLineCounter == 3)
                {
                    EmptyLineCounter = 0;
                    Hands.Add(NewHand);
                    NewHand = new Hand();
                }
            }
            // loop through each hand and figure out the game flow
            for (int HandIdx = 0; HandIdx < Hands.Count; HandIdx++)
            {
                Hand PreviousHand = HandIdx == 0 ? null : Hands[HandIdx - 1];
                Hand CurrentHand = Hands[HandIdx];
                Hand NextHand = HandIdx == Hands.Count - 1 ? null : Hands[HandIdx + 1];

                AssignActionAttributes(CurrentHand);

                CurrentHand.Players.ForEach(Player => {
                    // 1. figure out all the players in this hand, add them if they dont exist yet.
                    Player ExistingPlayer = PlayerList.FirstOrDefault(Existing => Existing.Name == Player.Name);
                    if (ExistingPlayer == null)
                    {
                        ExistingPlayer = new Player() { Name = Player.Name };
                        PlayerList.Add(ExistingPlayer);
                    }

                    // 2. add this hand to this player's hands played
                    ExistingPlayer.HandsPlayed.Add(CurrentHand);

                    // 3. if this player won this hand, add that
                    if (CurrentHand.Winners.Contains(Player))
                    {
                        ExistingPlayer.HandsWon.Add(CurrentHand);
                        ExistingPlayer.TotalRake += CurrentHand.Rake / CurrentHand.Winners.Count;
                    }

                    // 4. update chipcounts. if the player wasn't in the previous hand, mark this towards their total in count and add a tuple start
                    if (PreviousHand == null || PreviousHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1)
                    {
                        // if the previous began and ended tuple has an end with an identical chip count to this start, we sat out and rejoined, so dont add that
                        bool bIdenticalChipStack = false;
                        if (ExistingPlayer.Transactions.Count > 0)
                        {
                            Tuple<string, int, int> Previous = ExistingPlayer.Transactions[ExistingPlayer.Transactions.Count - 1];
                            if (Previous.Item1 == "Cash-Out" && Previous.Item3 == Player.ChipCountStart)
                            {
                                bIdenticalChipStack = true;
                                // also remove "profit" and the tuple entirely
                                ExistingPlayer.TotalChipCountOut -= Previous.Item3;
                                ExistingPlayer.Transactions.Remove(Previous);
                            }
                        }
                        if (!bIdenticalChipStack)
                        {
                            ExistingPlayer.TotalChipCountIn += Player.ChipCountStart;
                            ExistingPlayer.Transactions.Add(new Tuple<string, int, int>("Buy-In", HandIdx, Player.ChipCountStart));
                        }
                    }
                    // if this player isn't in the NEXT hand, mark the end of the current tuple and set the total chip out count to the end of this hand
                    else if (NextHand == null || NextHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1)
                    {
                        ExistingPlayer.TotalChipCountOut += Player.ChipCountEnd;
                        ExistingPlayer.Transactions.Add(new Tuple<string, int, int>("Cash-Out", HandIdx, Player.ChipCountEnd));
                    }
                    else
                    {
                        int PrevChipCountEnd = PreviousHand.Players.First(Prev => Prev.Name == Player.Name).ChipCountEnd;
                        // if this player's end chip count from the previous hand doesnt match the start of the next hand, they had an addon, so add the difference to the total in count
                        if (PrevChipCountEnd != Player.ChipCountStart)
                        {
                            int Amount = Player.ChipCountStart - PrevChipCountEnd;
                            ExistingPlayer.Transactions.Add(new Tuple<string, int, int>("Addon", HandIdx, Amount));
                            ExistingPlayer.TotalChipCountIn += Amount;
                        }
                    }
                });
            }

            // calculate rakeback and house cut 
            PlayerList.ForEach(Player => {
                Player.RakeBack = (int)(Player.TotalRake * .9);
                Player.HouseRake = (int)(Player.TotalRake * .1);
                Player.TotalWithRakeback = Player.TotalChipCountOut + Player.RakeBack;
                Player.Result = Player.TotalChipCountOut - Player.TotalChipCountIn + Player.RakeBack;
            });

            if (WriteStats)
            {
                using (StreamWriter Stream = File.CreateText("actionStats.txt"))
                {
                    foreach (Hand Hand in Hands)
                    {
                        Stream.WriteLine("Hand: " + Hand.HandNumber);
                        foreach (PlayerActionTaken Action in Hand.Action)
                        {
                            Stream.WriteLine(Action.ToString());
                        }
                        Stream.WriteLine();
                        Stream.WriteLine();
                        Stream.WriteLine();
                    }
                }
            }

            using (StreamWriter Stream = File.CreateText("output.csv"))
            {
                using (CsvWriter Writer = new CsvWriter(Stream, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    NewLine = Environment.NewLine,
                }))
                {

                    // overall stats
                    Writer.WriteField("Player");
                    Writer.WriteField("Chips In");
                    Writer.WriteField("Chips Out");
                    Writer.WriteField("Total Rake");
                    Writer.WriteField("RakeBack");
                    Writer.WriteField("House Rake");
                    Writer.WriteField("Total");
                    Writer.WriteField("Result");
                    Writer.NextRecord();
                    PlayerList.ForEach(Player =>
                    {
                        Writer.WriteField(Player.Name);
                        Writer.WriteField(Player.TotalChipCountIn);
                        Writer.WriteField(Player.TotalChipCountOut);
                        Writer.WriteField(Player.TotalRake);
                        Writer.WriteField(Player.RakeBack);
                        Writer.WriteField(Player.HouseRake);
                        Writer.WriteField(Player.TotalWithRakeback);
                        Writer.WriteField(Player.Result);
                        Writer.NextRecord();
                    });

                    Writer.WriteField("Totals");
                    Writer.WriteField(PlayerList.Select(c => c.TotalChipCountIn).Sum());
                    Writer.WriteField(PlayerList.Select(c => c.TotalChipCountOut).Sum());
                    Writer.WriteField(PlayerList.Select(c => c.TotalRake).Sum());
                    Writer.WriteField(PlayerList.Select(c => c.RakeBack).Sum());
                    Writer.WriteField(PlayerList.Select(c => c.HouseRake).Sum());
                    Writer.WriteField(PlayerList.Select(c => c.TotalWithRakeback).Sum());
                    Writer.WriteField(PlayerList.Select(c => c.Result).Sum());

                    Writer.NextRecord();
                    Writer.NextRecord();
                    // player individual stats
                    PlayerList.ForEach(Player =>
                    {
                        Writer.WriteField(Player.Name);
                        Writer.NextRecord();

                        // get all actions in every hand this player was in
                        List<HandStatistics> AllStats = Player.HandsPlayed.Select(c => c.Stats).ToList();

                        int HandsPlayed = Player.HandsPlayed.Count;

                        int FlopsSeen = AllStats.Where(c => c.FlopSeen.Any(d => d.Name == Player.Name)).Count();
                        int VPIPs = AllStats.Where(c => c.VPIP.Any(d => d.Name == Player.Name)).Count();
                        int PreflopRaises = AllStats.Where(c => c.PreflopRaise.Any(d => d.Name == Player.Name)).Count();
                        int CalledPreflopRaises = AllStats.Where(c => c.CalledPreflopRaise.Any(d => d.Name == Player.Name)).Count();
                        int UnopenedPreflopRaises = AllStats.Where(c => c.UnopenedPreflopRaise.Any(d => d.Name == Player.Name)).Count();
                        int UnopenedRaiseOpportunities = AllStats.Where(c => c.UnopenedRaiseOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int ThreeBetPreflops = AllStats.Where(c => c.ThreeBetPreflop.Any(d => d.Name == Player.Name)).Count();
                        int ThreeBetPreflopOpportunities = AllStats.Where(c => c.ThreeBetOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FoldedTo3BetPreflops = AllStats.Where(c => c.FoldedTo3BetPreflop.Any(d => d.Name == Player.Name)).Count();
                        int FoldedTo3BetPreflopOpportunities = AllStats.Where(c => c.FoldedTo3BetPreflopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FourBetPreflops = AllStats.Where(c => c.FourBetPreflop.Any(d => d.Name == Player.Name)).Count();
                        int FourBetPreflopOpportunities = AllStats.Where(c => c.FourBetPreflopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FoldedTo4BetPreflops = AllStats.Where(c => c.FoldedTo4BetPreflop.Any(d => d.Name == Player.Name)).Count();
                        int FoldedTo4BetPreflopOpportunities = AllStats.Where(c => c.FoldedTo4BetPreflopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int Squeezes = AllStats.Where(c => c.Squeeze.Any(d => d.Name == Player.Name)).Count();
                        int SqueezeOpportunities = AllStats.Where(c => c.SqueezeOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int BlindStealAttempts = AllStats.Where(c => c.BlindStealAttempt.Any(d => d.Name == Player.Name)).Count();
                        int BlindStealAttemptOpportunities = AllStats.Where(c => c.BlindStealAttemptOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FoldedBigBlindToStealAttempts = AllStats.Where(c => c.FoldedBigBlindToStealAttempt.Any(d => d.Name == Player.Name)).Count();
                        int FoldedBigBlindToStealAttemptOpportunities = AllStats.Where(c => c.FoldedBigBlindToStealAttemptOpportunity.Any(d => d.Name == Player.Name)).Count();

                        int BetOrRaisedFlops = AllStats.Where(c => c.BetOrRaisedFlop.Any(d => d.Name == Player.Name)).Count();
                        int BetRasiedCalledFoldedFlops = AllStats.Where(c => c.BetRasiedCalledFoldedFlop.Any(d => d.Name == Player.Name)).Count();
                        int BetOrRaisedTurns = AllStats.Where(c => c.BetOrRaisedTurn.Any(d => d.Name == Player.Name)).Count();
                        int BetRasiedCalledFoldedTurns = AllStats.Where(c => c.BetRasiedCalledFoldedTurn.Any(d => d.Name == Player.Name)).Count();
                        int BetOrRaisedRivers = AllStats.Where(c => c.BetOrRaisedRiver.Any(d => d.Name == Player.Name)).Count();
                        int BetRasiedCalledFoldedRivers = AllStats.Where(c => c.BetRasiedCalledFoldedRiver.Any(d => d.Name == Player.Name)).Count();
                        int CheckRaisedFlops = AllStats.Where(c => c.CheckRaisedFlop.Any(d => d.Name == Player.Name)).Count();
                        int CheckRaisedFlopOpportunities = AllStats.Where(c => c.CheckRaisedFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int CheckRaisedTurns = AllStats.Where(c => c.CheckRaisedTurn.Any(d => d.Name == Player.Name)).Count();
                        int CheckRaisedTurnOpportunities = AllStats.Where(c => c.CheckRaisedTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int CheckRaisedRivers = AllStats.Where(c => c.CheckRaisedRiver.Any(d => d.Name == Player.Name)).Count();
                        int CheckRaisedRiverOpportunities = AllStats.Where(c => c.CheckRaisedRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int CBetFlops = AllStats.Where(c => c.CBetFlop.Any(d => d.Name == Player.Name)).Count();
                        int CBetFlopOpportunities = AllStats.Where(c => c.CBetFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FoldedToCBetFlops = AllStats.Where(c => c.FoldedToCBetFlop.Any(d => d.Name == Player.Name)).Count();
                        int FoldedToCBetFlopOpportunities = AllStats.Where(c => c.FoldedToCBetFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int CBetTurns = AllStats.Where(c => c.CBetTurn.Any(d => d.Name == Player.Name)).Count();
                        int CBetTurnOpportunities = AllStats.Where(c => c.CBetTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FoldedToCBetTurns = AllStats.Where(c => c.FoldedToCBetTurn.Any(d => d.Name == Player.Name)).Count();
                        int FoldedToCBetTurnOpportunities = AllStats.Where(c => c.FoldedToCBetTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int CBetRivers = AllStats.Where(c => c.CBetRiver.Any(d => d.Name == Player.Name)).Count();
                        int CBetRiverOpportunities = AllStats.Where(c => c.CBetRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int FoldedToCBetRivers = AllStats.Where(c => c.FoldedToCBetRiver.Any(d => d.Name == Player.Name)).Count();
                        int FoldedToCBetRiverOpportunities = AllStats.Where(c => c.FoldedToCBetRiverOpportunity.Any(d => d.Name == Player.Name)).Count();

                        //int CalledCBetFlops = AllStats.Where(c => c.CalledCBetFlop.Any(d => d.Name == Player.Name)).Count();
                        //int RaisedCBetFlops = AllStats.Where(c => c.RaisedCBetFlop.Any(d => d.Name == Player.Name)).Count();
                        //int FoldedToRaiseAfterCBetFlops = AllStats.Where(c => c.FoldedToRaiseAfterCBetFlop.Any(d => d.Name == Player.Name)).Count();
                        //int CalledCBetTurns = AllStats.Where(c => c.CalledCBetTurn.Any(d => d.Name == Player.Name)).Count();
                        //int RaisedCBetTurns = AllStats.Where(c => c.RaisedCBetTurn.Any(d => d.Name == Player.Name)).Count();
                        //int FoldedToRaiseAfterCBetTurns = AllStats.Where(c => c.FoldedToRaiseAfterCBetTurn.Any(d => d.Name == Player.Name)).Count();
                        //int CalledCBetRivers = AllStats.Where(c => c.CalledCBetRiver.Any(d => d.Name == Plavyer.Name)).Count();
                        //int RaisedCBetRivers = AllStats.Where(c => c.RaisedCBetRiver.Any(d => d.Name == Player.Name)).Count();
                        //int FoldedToRaiseAfterCBetRivers = AllStats.Where(c => c.FoldedToRaiseAfterCBetRiver.Any(d => d.Name == Player.Name)).Count();

                        int DonkBetFlops = AllStats.Where(c => c.DonkBetFlop.Any(d => d.Name == Player.Name)).Count();
                        int DonkBetFlopOpportunities = AllStats.Where(c => c.DonkBetFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int DonkBetTurns = AllStats.Where(c => c.DonkBetTurn.Any(d => d.Name == Player.Name)).Count();
                        int DonkBetTurnOpportunities = AllStats.Where(c => c.DonkBetTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int DonkBetRivers = AllStats.Where(c => c.DonkBetRiver.Any(d => d.Name == Player.Name)).Count();
                        int DonkBetRiverOpportunities = AllStats.Where(c => c.DonkBetRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
                        int WentToShowdowns = AllStats.Where(c => c.WentToShowdown.Any(d => d.Name == Player.Name)).Count();
                        int WonAtShowdowns = AllStats.Where(c => c.WonAtShowdown.Any(d => d.Name == Player.Name)).Count();
                        int WonWithoutShowdowns = AllStats.Where(c => c.WonWithoutShowdown.Any(d => d.Name == Player.Name)).Count();

                        PlayersInDatabase.Add(new PlayerInDatabase
                        {
                            Name = Player.Name,
                            HandsPlayed = Player.HandsPlayed.Count,
                            FlopsSeen = FlopsSeen,
                            PreflopRaises = PreflopRaises,
                            VPIP = VPIPs
                        });

                        if (WriteStats)
                        {
                            Writer.WriteField("Hands Played");
                            Writer.WriteField(Player.HandsPlayed.Count);
                            Writer.NextRecord();

                            Writer.WriteField("Flops Seen");
                            Writer.WriteField((FlopsSeen * 100f / HandsPlayed).ToString("F1") + "%");
                            Writer.NextRecord();

                            Writer.WriteField("VPIP");
                            Writer.WriteField((VPIPs * 100f / HandsPlayed).ToString("F1") + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Preflop Raises");
                            Writer.WriteField((PreflopRaises * 100f / HandsPlayed).ToString("F1") + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Unopened Preflop Raises");
                            Writer.WriteField((UnopenedPreflopRaises == 0 ? "0" : (UnopenedPreflopRaises * 100f / UnopenedRaiseOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("ThreeBets");
                            Writer.WriteField((ThreeBetPreflops == 0 ? "0" : (ThreeBetPreflops * 100f / ThreeBetPreflopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Fold to Three-Bets");
                            Writer.WriteField((FoldedTo3BetPreflops == 0 ? "0" : (FoldedTo3BetPreflops * 100f / FoldedTo3BetPreflopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Four+Bets");
                            Writer.WriteField((FourBetPreflops == 0 ? "0" : (FourBetPreflops * 100f / FourBetPreflopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Fold to Four+Bets");
                            Writer.WriteField((FoldedTo4BetPreflops == 0 ? "0" : (FoldedTo4BetPreflops * 100f / FoldedTo4BetPreflopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Squeezes");
                            Writer.WriteField((Squeezes == 0 ? "0" : (Squeezes * 100f / SqueezeOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Blind Steal Attempts");
                            Writer.WriteField((BlindStealAttempts == 0 ? "0" : (BlindStealAttempts * 100f / BlindStealAttemptOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Fold BB To Steal Attempt");
                            Writer.WriteField((FoldedBigBlindToStealAttempts == 0 ? "0" : (FoldedBigBlindToStealAttempts * 100f / FoldedBigBlindToStealAttemptOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Aggression Frequency");
                            Writer.WriteField(((BetOrRaisedFlops + BetOrRaisedTurns + BetOrRaisedRivers) == 0 ? "0" :
                                               ((BetOrRaisedFlops + BetOrRaisedTurns + BetOrRaisedRivers) * 100f / (BetRasiedCalledFoldedFlops + BetRasiedCalledFoldedTurns + BetRasiedCalledFoldedRivers)).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("CBet Flop");
                            Writer.WriteField((CBetFlops == 0 ? "0" : (CBetFlops * 100f / CBetFlopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Folded to CBet Flop");
                            Writer.WriteField((FoldedToCBetFlops == 0 ? "0" : (FoldedToCBetFlops * 100f / FoldedToCBetFlopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("CBet Turn");
                            Writer.WriteField((CBetTurns == 0 ? "0" : (CBetTurns * 100f / CBetTurnOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Folded to CBet Turn");
                            Writer.WriteField((FoldedToCBetTurns == 0 ? "0" : (FoldedToCBetTurns * 100f / FoldedToCBetTurnOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("CBet River");
                            Writer.WriteField((CBetRivers == 0 ? "0" : (CBetRivers * 100f / CBetRiverOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Folded to CBet River");
                            Writer.WriteField((FoldedToCBetRivers == 0 ? "0" : (FoldedToCBetRivers * 100f / FoldedToCBetRiverOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Check Raises (Total)");
                            Writer.WriteField(((CheckRaisedFlops + CheckRaisedTurns + CheckRaisedRivers) == 0 ? "0" :
                                              ((CheckRaisedFlops + CheckRaisedTurns + CheckRaisedRivers) * 100f / (CheckRaisedFlopOpportunities + CheckRaisedTurnOpportunities + CheckRaisedRiverOpportunities)).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Check Raises (Flop)");
                            Writer.WriteField((CheckRaisedFlops == 0 ? "0" : (CheckRaisedFlops * 100f / CheckRaisedFlopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Check Raises (Turn)");
                            Writer.WriteField((CheckRaisedTurns == 0 ? "0" : (CheckRaisedTurns * 100f / CheckRaisedTurnOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Check Raises (River)");
                            Writer.WriteField((CheckRaisedRivers == 0 ? "0" : (CheckRaisedRivers * 100f / CheckRaisedRiverOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Donk Bets (Total)");
                            Writer.WriteField(((DonkBetFlops + DonkBetTurns + DonkBetRivers) == 0 ? "0" :
                                               ((DonkBetFlops + DonkBetTurns + DonkBetRivers) * 100f / (DonkBetFlopOpportunities + DonkBetTurnOpportunities + DonkBetRiverOpportunities)).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Donk Bets (Flop)");
                            Writer.WriteField((DonkBetFlops == 0 ? "0" : (DonkBetFlops * 100f / DonkBetFlopOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Donk Bets (Turn)");
                            Writer.WriteField((DonkBetTurns == 0 ? "0" : (DonkBetTurns * 100f / DonkBetTurnOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();

                            Writer.WriteField("Donk Bets (River)");
                            Writer.WriteField((DonkBetRivers == 0 ? "0" : (DonkBetRivers * 100f / DonkBetRiverOpportunities).ToString("F1")) + "%");
                            Writer.NextRecord();
                        }

                        Writer.NextRecord();
                        Writer.WriteField("Transactions");
                        Writer.NextRecord();

                        Player.Transactions.ForEach(Action =>
                        {
                            Writer.WriteField(Action.Item1 + " - Hand " + Action.Item2);
                            Writer.WriteField(Action.Item3);
                            Writer.NextRecord();
                        });
                        Writer.NextRecord();

                    });
                }
            }

            if (WriteStats)
            {
                using (LiteDatabase LiteDB = new LiteDatabase("lite.db"))
                {
                    ILiteCollection<PlayerInDatabase> PlayerCollection = LiteDB.GetCollection<PlayerInDatabase>("players");
                    List<string> PlayerNames = PlayersInDatabase.Select(c => c.Name).ToList();
                    List<PlayerInDatabase> ExistingInDB = PlayerCollection.Query().Where(c => PlayerNames.Contains(c.Name)).ToList();

                    foreach (PlayerInDatabase Player in PlayersInDatabase)
                    {
                        PlayerInDatabase ExistingPlayer = ExistingInDB.FirstOrDefault(c => c.Name == Player.Name);
                        if (ExistingPlayer != null)
                        {
                            ExistingPlayer.FlopsSeen += Player.FlopsSeen;
                            ExistingPlayer.HandsPlayed += Player.HandsPlayed;
                            ExistingPlayer.PreflopRaises += Player.PreflopRaises;
                            ExistingPlayer.VPIP += Player.VPIP;
                            PlayerCollection.Update(ExistingPlayer);
                        }
                        else
                        {
                            PlayerCollection.Insert(Player);
                        }
                        PlayerCollection.EnsureIndex(c => c.Name);
                    }
                }
            }


        }
        private static bool FindCheckForCheckRaise(PlayerActionTaken RaiseAction, int StreetIdx)
        {
            if (RaiseAction.Action != PlayerAction.Raise)
            {
                throw new Exception("Tried to find a checkraise without issuing a raise action!");
            }
            int RaiseIdx = RaiseAction.Hand.Action.IndexOf(RaiseAction);
            for (int Idx = StreetIdx; Idx < RaiseIdx; Idx++)
            {
                if (RaiseAction.Hand.Action[Idx].Player == RaiseAction.Player && RaiseAction.Hand.Action[Idx].Action == PlayerAction.Check)
                {
                    return true;
                }
            }
            return false;
        }
        private static void AssignActionAttributes(Hand Hand)
        {
            HandStatistics Stats = new HandStatistics();
            // loop through the hand's complete action and assign as we go
            // 0 - preflop, 1 - flop, 2 - turn, 3 - river, 4 - showdown
            int HandState = 0;

            PlayerInHand InitialRaiser = null;
            PlayerInHand ThreeBettor = null;
            PlayerInHand FourBettor = null;
            PlayerInHand LastRaiserPreflop = null;
            int FlopIdx = -1;
            int TurnIdx = -1;
            int RiverIdx = -1;
            for (int ActionIdx = 0; ActionIdx < Hand.Action.Count; ActionIdx++)
            {
                PlayerActionTaken Action = Hand.Action[ActionIdx];
                if (Action.Action == PlayerAction.Flop || Action.Action == PlayerAction.Turn || Action.Action == PlayerAction.River)
                {
                    if (Action.Action == PlayerAction.Flop)
                    {
                        FlopIdx = ActionIdx;
                    }
                    if (Action.Action == PlayerAction.Turn)
                    {
                        TurnIdx = ActionIdx;
                    }
                    if (Action.Action == PlayerAction.River)
                    {
                        RiverIdx = ActionIdx;
                    }
                    HandState++;
                    continue;
                }

                // preflop stats
                if (HandState == 0)
                {
                    if (Action.Action == PlayerAction.Call)
                    {
                        if (Stats.VPIP.Count == 0)
                        {
                            Stats.UnopenedRaiseOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.UnopenedRaiseOpportunity;
                            if (Action.Player.Position == TablePosition.BTN || Action.Player.Position == TablePosition.SB)
                            {
                                Stats.BlindStealAttemptOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.BlindStealAttemptOpportunity;
                            }
                        }

                        Stats.VPIP.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.VPIP;

                        if (InitialRaiser != null)
                        {
                            if (Stats.CalledPreflopRaise.Count == 0 && Stats.Squeeze.Count == 0 && Stats.ThreeBetPreflop.Count == 0)
                            {
                                Stats.ThreeBetOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.ThreeBetPreflopOpportunity;
                            }
                            else if (Stats.CalledPreflopRaise.Count == 0 && Stats.Squeeze.Count == 0 && Stats.ThreeBetPreflop.Count == 1 && Stats.FourBetPreflop.Count == 0)
                            {
                                if (InitialRaiser == Action.Player)
                                {
                                    Stats.FourBetPreflopOpportunity.Add(Action.Player);
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FourBetOpportunity;
                                }
                                Stats.FoldedTo3BetPreflopOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedTo3BetPreflopOpportunity;
                            }
                            else if (Stats.CalledPreflopRaise.Count > 0)
                            {
                                Stats.SqueezeOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.SqueezeOpportunity;
                            }

                            if (Action.Player.Position == TablePosition.BB && Stats.BlindStealAttempt.Count == 1 && Stats.CalledPreflopRaise.Count == 0 && Stats.BlindStealAttempt.FirstOrDefault() != Action.Player)
                            {
                                Stats.FoldedBigBlindToStealAttemptOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedBigBlindToStealAttemptOpportunity;
                            }

                            Stats.CalledPreflopRaise.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.CalledPreflopRaise;
                        }
                    }
                    else if (Action.Action == PlayerAction.Raise)
                    {
                        // due to logic flow we have to assume all bets are being considered in backwards order
                        if (Stats.ThreeBetPreflop.Count >= 1 && InitialRaiser == Action.Player)
                        {
                            // this stat is actually "four or more" bet
                            Stats.FoldedTo3BetPreflopOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedTo3BetPreflopOpportunity;

                            Stats.FourBetPreflop.Add(Action.Player);
                            Stats.FourBetPreflopOpportunity.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.FourBetPreflop;
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FourBetOpportunity;
                            // this may overwrite if we have a 5+bet, but since this is only used for fold-to calculations, it doesnt matter who it is.
                            FourBettor = Action.Player;
                        }
                        // if there is exactly one other preflop raise
                        if (Stats.PreflopRaise.Count == 1)
                        {
                            // if we have any callers, this is a squeeze
                            if (Stats.CalledPreflopRaise.Count > 0)
                            {
                                Stats.Squeeze.Add(Action.Player);
                                Stats.SqueezeOpportunity.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.Squeeze;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.SqueezeOpportunity;
                            }
                            // otherwise its a 3b
                            else
                            {
                                if (Action.Player.Position == TablePosition.BB && Stats.BlindStealAttempt.Count == 1 && Stats.CalledPreflopRaise.Count == 0 && Stats.BlindStealAttempt.FirstOrDefault() != Action.Player)
                                {
                                    Stats.FoldedBigBlindToStealAttemptOpportunity.Add(Action.Player);
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedBigBlindToStealAttemptOpportunity;
                                }
                                Stats.ThreeBetPreflop.Add(Action.Player);
                                Stats.ThreeBetOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.ThreeBetPreflopOpportunity;
                                Action.ActionStatFlags |= ActionStatType.ThreeBetPreflop;
                                ThreeBettor = Action.Player;
                            }
                        }

                        Stats.PreflopRaise.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.PreflopRaise;

                        // if this is the first vpip action (i.e not a check or a fold), this is an unopened raise
                        if (Stats.VPIP.Count == 0)
                        {
                            InitialRaiser = Action.Player;
                            Stats.UnopenedPreflopRaise.Add(Action.Player);
                            Stats.UnopenedRaiseOpportunity.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.UnopenedPreflopRaise;
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.UnopenedRaiseOpportunity;
                            // if we're the button or the sb, this a blind steal chance
                            if ((Action.Player.Position == TablePosition.BTN || Action.Player.Position == TablePosition.SB || Action.Player.Position == TablePosition.BTNSB) && Stats.BlindStealAttempt.Count == 0)
                            {
                                Stats.BlindStealAttempt.Add(Action.Player);
                                Stats.BlindStealAttemptOpportunity.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.BlindStealAttempt;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.BlindStealAttemptOpportunity;
                            }
                        }

                        Stats.VPIP.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.VPIP;
                        LastRaiserPreflop = Action.Player;
                    }
                    else if (Action.Action == PlayerAction.Fold)
                    {
                        if (Stats.VPIP.Count == 0)
                        {
                            Stats.UnopenedRaiseOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.UnopenedRaiseOpportunity;
                            if (Action.Player.Position == TablePosition.BTN || Action.Player.Position == TablePosition.SB)
                            {
                                Stats.BlindStealAttemptOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.BlindStealAttemptOpportunity;
                            }
                        }
                        if (Action.Player == InitialRaiser)
                        {
                            if (Stats.FourBetPreflop.Count == 1)
                            {
                                Stats.FoldedTo4BetPreflop.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.FoldedTo4BetPreflop;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedTo4BetPreflopOpportunity;
                            }
                            else if (Stats.ThreeBetPreflop.Count == 1)
                            {
                                Stats.FoldedTo3BetPreflop.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.FoldedTo3BetPreflop;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedTo3BetPreflopOpportunity;
                                if (Stats.CalledPreflopRaise.Count == 0 && Stats.FourBetPreflop.Count == 0)
                                {
                                    Stats.FourBetPreflopOpportunity.Add(Action.Player);
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FourBetOpportunity;
                                }
                            }
                        }
                        if (Action.Player.Position == TablePosition.BB && Stats.BlindStealAttempt.Count == 1 && Stats.CalledPreflopRaise.Count == 0 && Stats.BlindStealAttempt.FirstOrDefault() != Action.Player)
                        {
                            Stats.FoldedBigBlindToStealAttempt.Add(Action.Player);
                            Stats.FoldedBigBlindToStealAttemptOpportunity.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.FoldedBigBlindToStealAttempt;
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedBigBlindToStealAttemptOpportunity;

                        }
                        if (InitialRaiser != null && Stats.Squeeze.Count == 0)
                        {
                            if (Stats.CalledPreflopRaise.Count == 0 && Stats.ThreeBetPreflop.Count == 0)
                            {
                                Stats.ThreeBetOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.ThreeBetPreflopOpportunity;
                            }
                            else if (Stats.CalledPreflopRaise.Count > 0)
                            {
                                Stats.SqueezeOpportunity.Add(Action.Player);
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.SqueezeOpportunity;
                            }
                        }
                    }
                    else if (Action.Action == PlayerAction.Check)
                    {
                        if (Stats.VPIP.Count == 0)
                        {
                            Stats.UnopenedRaiseOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.UnopenedRaiseOpportunity;

                        }
                        if (InitialRaiser != null && Stats.Squeeze.Count == 0 && Stats.ThreeBetPreflop.Count == 0)
                        {
                            Stats.ThreeBetOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.ThreeBetPreflopOpportunity;
                        }
                        else if (InitialRaiser != null && Stats.Squeeze.Count == 0 && Stats.ThreeBetPreflop.Count == 1 && Stats.FourBetPreflop.Count == 0)
                        {
                            Stats.FourBetPreflopOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FourBetOpportunity;
                        }
                    }
                }
                else if (HandState == 1)
                {
                    Stats.FlopSeen.Add(Action.Player);
                    Action.ActionStatFlags |= ActionStatType.FlopSeen;
                    // bb vpip specifically
                    if ((Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Call || Action.Action == PlayerAction.Raise) && (Action.Player.Position == TablePosition.BB || Action.Hand.Action.Count(c => c.Action == PlayerAction.BuyBlinds && c.Player == Action.Player) == 1))
                    {
                        Stats.VPIP.Add(Action.Player);
                        if (!Action.Hand.Action.Any(c => c.Player == Action.Player && c.ActionStatFlags.HasFlag(ActionStatType.VPIP)))
                        {
                            Action.ActionStatFlags |= ActionStatType.VPIP;
                        }
                    }
                    // did any action besides fold...
                    if (Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Call || Action.Action == PlayerAction.Raise || Action.Action == PlayerAction.Fold)
                    {
                        Stats.BetRasiedCalledFoldedFlop.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.BetRasiedCalledFoldedFlop;
                        if (Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Raise)
                        {
                            if (Action.Action == PlayerAction.Bet)
                            {
                                if (LastRaiserPreflop == Action.Player && Stats.BetOrRaisedFlop.Count == 0)
                                {
                                    Stats.CBetFlop.Add(Action.Player);
                                    Stats.CBetFlopOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.CBetFlop;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CBetFlopOpportunity;
                                }
                                else if (Stats.CalledPreflopRaise.Contains(Action.Player) && Stats.PreflopRaise.All(c => c.Position > Action.Player.Position))
                                {
                                    Stats.DonkBetFlop.Add(Action.Player);
                                    Stats.DonkBetFlopOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.DonkBetFlop;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.DonkBetFlopOpportunity;
                                }

                            }
                            // if this is a check raise
                            else if (Action.Action == PlayerAction.Raise)
                            {
                                if (FindCheckForCheckRaise(Action, FlopIdx))
                                {
                                    Stats.CheckRaisedFlop.Add(Action.Player);
                                    Stats.CheckRaisedFlopOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.CheckRaisedFlop;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CheckRaisedFlopOpportunity;
                                }

                                if (Stats.CBetFlop.Count != 0)
                                {
                                    Stats.RaisedCBetFlop.Add(Action.Player);
                                    Stats.FoldedToCBetFlopOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.RaisedCBetFlop;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetFlopOpportunity;
                                }
                            }
                            Stats.BetOrRaisedFlop.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.BetOrRaisedFlop;
                        }
                        else if (Action.Action == PlayerAction.Call)
                        {
                            if (Stats.CBetFlop.Count != 0)
                            {
                                Stats.CalledCBetFlop.Add(Action.Player);
                                Stats.FoldedToCBetFlopOpportunity.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.CalledCBetFlop;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetFlopOpportunity;
                            }
                        }
                        else if (Action.Action == PlayerAction.Fold)
                        {
                            if (Stats.CBetFlop.Count == 1)
                            {
                                if (Stats.RaisedCBetFlop.Count != 0 && Stats.CBetFlop.First() == Action.Player)
                                {
                                    Stats.FoldedToRaiseAfterCBetFlop.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.FoldedToRaiseAfterCBetFlop;
                                }
                                else
                                {
                                    Stats.FoldedToCBetFlop.Add(Action.Player);
                                    Stats.FoldedToCBetFlopOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.FoldedToCBetFlop;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetFlopOpportunity;
                                }
                            }
                        }
                    }
                    else if (Action.Action == PlayerAction.Check)
                    {
                        Stats.CheckRaisedFlopOpportunity.Add(Action.Player);
                        Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CheckRaisedFlopOpportunity;
                        if (LastRaiserPreflop == Action.Player)
                        {
                            Stats.CBetFlopOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CBetFlopOpportunity;
                        }
                        else if (Stats.CalledPreflopRaise.Contains(Action.Player) && Stats.PreflopRaise.All(c => c.Position > Action.Player.Position))
                        {
                            Stats.DonkBetFlopOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.DonkBetFlopOpportunity;
                        }
                    }
                    else if (Action.Action == PlayerAction.Win)
                    {
                        Stats.WonWithoutShowdown.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.WonWithoutShowdown;
                    }
                }
                else if (HandState == 2)
                {
                    // bb vpip specifically
                    if ((Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Call || Action.Action == PlayerAction.Raise) && (Action.Player.Position == TablePosition.BB || Action.Hand.Action.Count(c => c.Action == PlayerAction.BuyBlinds && c.Player == Action.Player) == 1))
                    {
                        Stats.VPIP.Add(Action.Player);
                        if (!Action.Hand.Action.Any(c => c.Player == Action.Player && c.ActionStatFlags.HasFlag(ActionStatType.VPIP)))
                        {
                            Action.ActionStatFlags |= ActionStatType.VPIP;
                        }
                    }
                    if (Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Call || Action.Action == PlayerAction.Raise || Action.Action == PlayerAction.Fold)
                    {
                        Stats.BetRasiedCalledFoldedTurn.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.BetRasiedCalledFoldedTurn;
                        if (Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Raise)
                        {
                            if (Action.Action == PlayerAction.Bet)
                            {
                                if (LastRaiserPreflop == Action.Player && Stats.CBetFlop.Contains(Action.Player) && Stats.BetOrRaisedTurn.Count == 0)
                                {
                                    Stats.CBetTurn.Add(Action.Player);
                                    Stats.CBetTurnOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.CBetTurn;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CBetTurnOpportunity;
                                }
                                else if (Stats.CalledPreflopRaise.Contains(Action.Player) && Stats.CalledCBetFlop.Contains(Action.Player) && Action.Player.Position < Stats.CBetFlop.First().Position)
                                {
                                    Stats.DonkBetTurn.Add(Action.Player);
                                    Stats.DonkBetTurnOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.DonkBetTurn;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.DonkBetTurnOpportunity;
                                }
                            }
                            // if this is a check raise
                            else if (Action.Action == PlayerAction.Raise)
                            {
                                if (FindCheckForCheckRaise(Action, TurnIdx))
                                {
                                    Stats.CheckRaisedTurn.Add(Action.Player);
                                    Stats.CheckRaisedTurnOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.CheckRaisedTurn;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CheckRaisedTurnOpportunity;
                                }

                                if (Stats.CBetTurn.Count == 1 && Action.Player != Stats.CBetTurn.First())
                                {
                                    Stats.RaisedCBetTurn.Add(Action.Player);
                                    Stats.FoldedToCBetTurnOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.RaisedCBetTurn;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetTurnOpportunity;
                                }
                            }
                            Stats.BetOrRaisedTurn.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.BetOrRaisedTurn;
                        }
                        else if (Action.Action == PlayerAction.Call)
                        {
                            if (Stats.CBetTurn.Count == 1 && Stats.RaisedCBetTurn.Count == 0)
                            {
                                Stats.CalledCBetTurn.Add(Action.Player);
                                Stats.FoldedToCBetTurnOpportunity.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.CalledCBetTurn;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetTurnOpportunity;
                            }
                        }
                        else if (Action.Action == PlayerAction.Fold)
                        {
                            if (Stats.CBetTurn.Count == 1)
                            {
                                if (Stats.RaisedCBetTurn.Count != 0 && Stats.CBetTurn.First() == Action.Player)
                                {
                                    Stats.FoldedToRaiseAfterCBetTurn.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.FoldedToRaiseAfterCBetTurn;
                                }
                                else
                                {
                                    Stats.FoldedToCBetTurn.Add(Action.Player);
                                    Stats.FoldedToCBetTurnOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.FoldedToCBetTurn;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetTurnOpportunity;
                                }
                            }
                        }
                    }
                    else if (Action.Action == PlayerAction.Check)
                    {
                        Stats.CheckRaisedTurnOpportunity.Add(Action.Player);
                        Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CheckRaisedTurnOpportunity;
                        if (LastRaiserPreflop == Action.Player && Stats.CBetFlop.Contains(Action.Player))
                        {
                            Stats.CBetTurnOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CBetTurnOpportunity;
                        }
                        else if (Stats.CalledPreflopRaise.Contains(Action.Player) && Stats.CalledCBetFlop.Contains(Action.Player) && Action.Player.Position < Stats.CBetFlop.First().Position)
                        {
                            Stats.DonkBetTurnOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.DonkBetTurnOpportunity;
                        }
                    }
                    else if (Action.Action == PlayerAction.Win)
                    {
                        Stats.WonWithoutShowdown.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.WonWithoutShowdown;
                    }
                }
                else if (HandState == 3)
                {
                    // bb vpip specifically
                    if ((Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Call || Action.Action == PlayerAction.Raise) && (Action.Player.Position == TablePosition.BB || Action.Hand.Action.Count(c => c.Action == PlayerAction.BuyBlinds && c.Player == Action.Player) == 1))
                    {
                        Stats.VPIP.Add(Action.Player);
                        if (!Action.Hand.Action.Any(c => c.Player == Action.Player && c.ActionStatFlags.HasFlag(ActionStatType.VPIP)))
                        {
                            Action.ActionStatFlags |= ActionStatType.VPIP;
                        }
                    }
                    if (Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Call || Action.Action == PlayerAction.Raise || Action.Action == PlayerAction.Fold)
                    {
                        Stats.BetRasiedCalledFoldedRiver.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.BetRasiedCalledFoldedRiver;
                        if (Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Raise)
                        {
                            if (Action.Action == PlayerAction.Bet)
                            {
                                if (LastRaiserPreflop == Action.Player && Stats.CBetTurn.Contains(Action.Player) && Stats.BetOrRaisedRiver.Count == 0)
                                {
                                    Stats.CBetRiver.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.CBetRiver;
                                }
                                else if (Stats.CalledPreflopRaise.Contains(Action.Player) && Stats.CalledCBetFlop.Contains(Action.Player) && Stats.CalledCBetTurn.Contains(Action.Player) && Action.Player.Position < Stats.CBetTurn.First().Position)
                                {
                                    Stats.DonkBetRiver.Add(Action.Player);
                                    Stats.DonkBetRiverOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.DonkBetRiver;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.DonkBetRiverOpportunity;
                                }
                            }
                            // if this is a check raise
                            else if (Action.Action == PlayerAction.Raise)
                            {
                                if (FindCheckForCheckRaise(Action, RiverIdx))
                                {
                                    Stats.CheckRaisedRiver.Add(Action.Player);
                                    Stats.CheckRaisedRiverOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.CheckRaisedRiver;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CheckRaisedRiverOpportunity;
                                }

                                if (Stats.CBetRiver.Count == 1 && Stats.CBetRiver.First() != Action.Player)
                                {
                                    Stats.RaisedCBetRiver.Add(Action.Player);
                                    Stats.FoldedToCBetRiverOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.RaisedCBetRiver;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetRiverOpportunity;
                                }
                            }
                            Stats.BetOrRaisedRiver.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.BetOrRaisedRiver;
                        }
                        else if (Action.Action == PlayerAction.Call)
                        {
                            if (Stats.CBetRiver.Count == 1 && Stats.RaisedCBetRiver.Count == 0)
                            {
                                Stats.CalledCBetRiver.Add(Action.Player);
                                Stats.FoldedToCBetRiverOpportunity.Add(Action.Player);
                                Action.ActionStatFlags |= ActionStatType.CalledCBetRiver;
                                Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetRiverOpportunity;
                            }
                        }
                        else if (Action.Action == PlayerAction.Fold)
                        {
                            if (Stats.CBetRiver.Count == 1)
                            {
                                if (Stats.RaisedCBetRiver.Count != 0 && Stats.CBetRiver.First() == Action.Player)
                                {
                                    Stats.FoldedToRaiseAfterCBetRiver.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.FoldedToRaiseAfterCBetRiver;
                                }
                                else
                                {
                                    Stats.FoldedToCBetRiver.Add(Action.Player);
                                    Stats.FoldedToCBetRiverOpportunity.Add(Action.Player);
                                    Action.ActionStatFlags |= ActionStatType.FoldedToCBetRiver;
                                    Action.OpportunityStatFlags |= ActionOpportunityStatFlags.FoldedToCBetRiverOpportunity;
                                }
                            }
                        }
                    }
                    else if (Action.Action == PlayerAction.Check)
                    {
                        Stats.CheckRaisedRiverOpportunity.Add(Action.Player);
                        Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CheckRaisedRiverOpportunity;
                        if (LastRaiserPreflop == Action.Player && Stats.CBetTurn.Contains(Action.Player))
                        {
                            Stats.CBetRiverOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.CBetRiverOpportunity;
                        }
                        else if (Stats.CalledPreflopRaise.Contains(Action.Player) && Stats.CalledCBetFlop.Contains(Action.Player) && Stats.CalledCBetTurn.Contains(Action.Player) && Action.Player.Position < Stats.CBetTurn.First().Position)
                        {
                            Stats.DonkBetRiverOpportunity.Add(Action.Player);
                            Action.OpportunityStatFlags |= ActionOpportunityStatFlags.DonkBetRiverOpportunity;
                        }
                    }
                    else if (Action.Action == PlayerAction.Showdown)
                    {
                        Stats.WentToShowdown.Add(Action.Player);
                        Action.ActionStatFlags |= ActionStatType.WentToShowdown;
                    }
                    else if (Action.Action == PlayerAction.Win)
                    {
                        if (Stats.WentToShowdown.Contains(Action.Player))
                        {
                            Stats.WonAtShowdown.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.WonAtShowdown;
                        }
                        else
                        {
                            Stats.WonWithoutShowdown.Add(Action.Player);
                            Action.ActionStatFlags |= ActionStatType.WonWithoutShowdown;
                        }
                    }
                }
            };
            Hand.Stats = Stats;
        }
        private static void NegatePreviousChipCommit(Hand Hand, PlayerInHand Player)
        {
            // find the next previous bet action to call
            for (int ActionIdx = Hand.Action.Count - 1; ActionIdx >= 0; ActionIdx--)
            {
                PlayerActionTaken Action = Hand.Action[ActionIdx];
                if (Action.Player == Hand.Dealer)
                {
                    break;
                }
                if (Action.Player == Player)
                {
                    if (Action.Action == PlayerAction.PostSmallBlind || Action.Action == PlayerAction.PostBigBlind ||
                        Action.Action == PlayerAction.Bet || Action.Action == PlayerAction.Raise || Action.Action == PlayerAction.Call)
                    {
                        Action.FinalChipCommit = false;
                        break;
                    }
                    // if this raise was from a player bought blind, we'll have an extra small blind left over, so just alter this action to be a commiting small blind
                    else if (Action.Action == PlayerAction.BuyBlinds)
                    {
                        Action.Action = PlayerAction.AlteredToSBBuyBlinds;
                        Action.Amount = Hand.SmallBlindAmount;
                        Action.FinalChipCommit = true;
                        break;
                    }
                }
            }
        }
        static List<TablePosition> FillPositions(Hand Hand)
        {
            List<TablePosition> RetVal = new List<TablePosition>() { TablePosition.SB, TablePosition.BB };
            // table positions are decided really stupidly, apparently.
            switch (Hand.Players.Count)
            {
                case 2:
                    RetVal[0] = TablePosition.BTNSB;
                    break;
                case 3:
                    RetVal.Add(TablePosition.BTN);
                    break;
                case 4:
                    RetVal.Add(TablePosition.UTG);
                    RetVal.Add(TablePosition.BTN);
                    break;
                case 5:
                    RetVal.Add(TablePosition.UTG);
                    RetVal.Add(TablePosition.CO);
                    RetVal.Add(TablePosition.BTN);
                    break;
                case 6:
                    RetVal.Add(TablePosition.UTG);
                    RetVal.Add(TablePosition.HJ);
                    RetVal.Add(TablePosition.CO);
                    RetVal.Add(TablePosition.BTN);
                    break;
                case 7:
                    RetVal.Add(TablePosition.UTG);
                    RetVal.Add(TablePosition.UTG1);
                    RetVal.Add(TablePosition.HJ);
                    RetVal.Add(TablePosition.CO);
                    RetVal.Add(TablePosition.BTN);
                    break;
                case 8:
                    RetVal.Add(TablePosition.UTG);
                    RetVal.Add(TablePosition.UTG1);
                    RetVal.Add(TablePosition.MP1);
                    RetVal.Add(TablePosition.HJ);
                    RetVal.Add(TablePosition.CO);
                    RetVal.Add(TablePosition.BTN);
                    break;
                case 9:
                    RetVal.Add(TablePosition.UTG);
                    RetVal.Add(TablePosition.UTG1);
                    RetVal.Add(TablePosition.MP1);
                    RetVal.Add(TablePosition.MP2);
                    RetVal.Add(TablePosition.HJ);
                    RetVal.Add(TablePosition.CO);
                    RetVal.Add(TablePosition.BTN);
                    break;
            }
            return RetVal;
        }
        static void ParseLine(Hand Hand, string InBlock)
        {
            Match LineMatch;

            // hand number line
            LineMatch = Regex.Match(InBlock, HandNumberRegex);
            if (LineMatch.Success)
            {
                Hand.HandNumber = LineMatch.Groups[1].Value;
                Hand.Date = DateTime.Parse(LineMatch.Groups[2].Value);
                return;
            }

            // post flop line
            LineMatch = Regex.Match(InBlock, StreetRegex);
            if (LineMatch.Success)
            {
                string Street = LineMatch.Groups[1].Value;
                if (Street == "FLOP")
                {
                    Hand.Action.Add(new PlayerActionTaken() { Hand = Hand, Player = Hand.Dealer, Amount = 0, Action = PlayerAction.Flop });
                    Hand.PreflopParsed = true;
                }
                else if (Street == "TURN")
                {
                    Hand.Action.Add(new PlayerActionTaken() { Hand = Hand, Player = Hand.Dealer, Amount = 0, Action = PlayerAction.Turn });
                }
                else if (Street == "RIVER")
                {
                    Hand.Action.Add(new PlayerActionTaken() { Hand = Hand, Player = Hand.Dealer, Amount = 0, Action = PlayerAction.River });
                }

                return;
            }

            // seat numbers and starting stacks line
            LineMatch = Regex.Match(InBlock, SeatRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                int PlayerChips = int.Parse(LineMatch.Groups[2].Value);

                Hand.Players.Add(new PlayerInHand { Name = PlayerName, ChipCountStart = PlayerChips, ChipCountEnd = PlayerChips });
                return;
            }

            // blinds regexes
            LineMatch = Regex.Match(InBlock, BlindRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                string BlindPosition = LineMatch.Groups[2].Value;
                int BlindValue = int.Parse(LineMatch.Groups[3].Value);

                PlayerInHand ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);
                if (BlindPosition == "small")
                {
                    Hand.SmallBlindAmount = BlindValue;
                    Hand.Action.Add(new PlayerActionTaken { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.PostSmallBlind, Amount = BlindValue });

                    // once the small blind line is posted, we know all the players and positions at the table
                    int PlayerIdx = Hand.Players.IndexOf(ExistingPlayer);

                    // figure out how many positions we actually have to fill. we'll always have a sb/bb
                    List<TablePosition> PositionsToFill = FillPositions(Hand);
                    int CurrentPositionIdx = 0;
                    while (true)
                    {
                        // if we get back to the current player, break out of the loop
                        if (PlayerIdx == Hand.Players.IndexOf(ExistingPlayer) && ExistingPlayer.Position != TablePosition.Unset)
                        {
                            break;
                        }
                        // otherwise start assigning positions based on sb being count 1, button being count 9
                        Hand.Players[PlayerIdx].Position = PositionsToFill[CurrentPositionIdx];
                        CurrentPositionIdx++;
                        PlayerIdx++;
                        if (PlayerIdx == Hand.Players.Count)
                        {
                            PlayerIdx = 0;
                        }

                    }
                }
                else if (BlindPosition == "big")
                {
                    Hand.BigBlindAmount = BlindValue;
                    Hand.Action.Add(new PlayerActionTaken { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.PostBigBlind, Amount = BlindValue });
                }
                else if (BlindPosition == "small & big")
                {
                    Hand.PurchasedHandAmount = BlindValue;
                    Hand.Action.Add(new PlayerActionTaken { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.BuyBlinds, Amount = BlindValue });
                }
                return;
            }

            // bet or call
            LineMatch = Regex.Match(InBlock, BetOrCallRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                string BetOrCall = LineMatch.Groups[2].Value;
                int BetOrCallValue = int.Parse(LineMatch.Groups[3].Value);

                PlayerInHand ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                PlayerAction Action = BetOrCall == "bets" ? PlayerAction.Bet : PlayerAction.Call;

                PlayerActionTaken ActionTaken = new PlayerActionTaken
                {
                    Hand = Hand,
                    Player = ExistingPlayer,
                    Action = Action,
                    Amount = BetOrCallValue
                };

                bool PlayerHasPreviousVPIPAction = Hand.Action.Exists(c => c.Player == ExistingPlayer && c.ActionStatFlags.HasFlag(ActionStatType.VPIP));

                
                Hand.Action.Add(ActionTaken);

                return;
            }

            // raise
            LineMatch = Regex.Match(InBlock, RaiseRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                int RaiseValue = int.Parse(LineMatch.Groups[2].Value);
                int TotalPot = int.Parse(LineMatch.Groups[3].Value);

                PlayerInHand ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                NegatePreviousChipCommit(Hand, ExistingPlayer);

                PlayerActionTaken ActionTaken = new PlayerActionTaken
                {
                    Hand = Hand,
                    Player = ExistingPlayer,
                    Action = PlayerAction.Raise,
                    Amount = TotalPot
                };

                Hand.Action.Add(ActionTaken);
                return;
            }

            // fold/check match
            LineMatch = Regex.Match(InBlock, FoldRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                string ActionValue = LineMatch.Groups[2].Value;

                PlayerInHand ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);
                PlayerAction Action = ActionValue == "checks" ? PlayerAction.Check : PlayerAction.Fold;

                PlayerActionTaken ActionTaken = new PlayerActionTaken { Hand = Hand, Player = ExistingPlayer, Action = Action, Amount = 0 };
                
                Hand.Action.Add(ActionTaken);

                return;
            }

            // uncalled bet value
            LineMatch = Regex.Match(InBlock, UncalledBetRegex);
            if (LineMatch.Success)
            {
                int UncalledBetValue = int.Parse(LineMatch.Groups[1].Value);
                string PlayerName = LineMatch.Groups[2].Value;

                Hand.Action.Add(new PlayerActionTaken { Hand = Hand, Player = Hand.Players.FirstOrDefault(p => p.Name == PlayerName), Action = PlayerAction.ChipsReturned, Amount = UncalledBetValue });

                return;
            }

            // showdown
            LineMatch = Regex.Match(InBlock, WentToShowdownRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                string ShowOrMuck = LineMatch.Groups[2].Value;

                PlayerInHand ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                Hand.Action.Add(new PlayerActionTaken { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.Showdown, Amount = 0 /*ActionStatFlags = ActionStatType.WentToShowdown*/ });

                return;
            }


            // winner
            LineMatch = Regex.Match(InBlock, CollectionRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                int CollectionValue = int.Parse(LineMatch.Groups[2].Value);

                PlayerInHand ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                PlayerActionTaken ActionTaken = new PlayerActionTaken
                {
                    Hand = Hand,
                    Player = ExistingPlayer,
                    Action = PlayerAction.Win,
                    Amount = CollectionValue
                };

                Hand.Action.Add(ActionTaken);

                if (!Hand.Winners.Contains(ExistingPlayer))
                {
                    Hand.Winners.Add(ExistingPlayer);
                }

                return;
            }

            // total pot and rake
            LineMatch = Regex.Match(InBlock, TotalPotAndRake);
            if (LineMatch.Success)
            {
                int TotalPot = int.Parse(LineMatch.Groups[1].Value);
                int Rake = int.Parse(LineMatch.Groups[2].Value);

                Hand.Rake = Rake;
                Hand.TotalPot = TotalPot;

                Hand.Players.ForEach(Player => {
                    int TotalChipsOut = 0;
                    int TotalChipsIn = 0;
                    foreach (PlayerActionTaken Action in Hand.Action.Where(c => c.Player == Player))
                    {
                        if (Action.Action == PlayerAction.ChipsReturned || Action.Action == PlayerAction.Win)
                        {
                            TotalChipsIn += Action.Amount;
                        }
                        else
                        {
                            if (Action.FinalChipCommit)
                            {
                                TotalChipsOut += Action.Amount;
                            }
                        }
                    }
                    Player.ChipCountEnd = Player.ChipCountStart - TotalChipsOut + TotalChipsIn;
                });

                return;
            }

        }
    }
}
