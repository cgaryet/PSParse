using CsvHelper;
using CsvHelper.Configuration;
using PSniffGUI.Enums;
using PSniffGUI.Model.Parsing;
using PSParseGUI.Helpers;
using PSParseGUI.Model.Export;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PSniffGUI.Model.Parsing.HandAction;

namespace PSParseGUI.Logic
{
    internal class ExportCSV
    {
        private List<Player> PlayerList = new List<Player>();

        public ExportCSV()
        {
        }

        public void Export(IReadOnlyCollection<Hand> Hands, string ExportFileName = "output.csv")
        {
            bool WriteStats = false;

            ParseHands.ParseTransactions(Hands.ToList(), PlayerList, false);

            if (WriteStats)
            {
                using (StreamWriter Stream = File.CreateText("actionStats.txt"))
                {
                    foreach (Hand Hand in Hands)
                    {
                        Stream.WriteLine("Hand: " + Hand.HandNumber);
                        foreach (HandAction Action in Hand.Action)
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
                        List<HandStats> AllStats = Player.HandsPlayed.Select(c => c.Stats).ToList();

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

                        //PlayersInDatabase.Add(new PlayerInDatabase
                        //{
                        //    Name = Player.Name,
                        //    HandsPlayed = Player.HandsPlayed.Count,
                        //    FlopsSeen = FlopsSeen,
                        //    PreflopRaises = PreflopRaises,
                        //    VPIP = VPIPs
                        //});

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
                            Writer.WriteField(Action.TransactionType + " - Hand " + Action.HandNumber);
                            Writer.WriteField(Action.Amount);
                            Writer.NextRecord();
                        });
                        Writer.NextRecord();

                    });
                }
            }

            if (WriteStats)
            {
                //using (LiteDatabase LiteDB = new LiteDatabase("lite.db"))
                //{
                //    ILiteCollection<PlayerInDatabase> PlayerCollection = LiteDB.GetCollection<PlayerInDatabase>("players");
                //    List<string> PlayerNames = PlayersInDatabase.Select(c => c.Name).ToList();
                //    List<PlayerInDatabase> ExistingInDB = PlayerCollection.Query().Where(c => PlayerNames.Contains(c.Name)).ToList();

                //    foreach (PlayerInDatabase Player in PlayersInDatabase)
                //    {
                //        PlayerInDatabase ExistingPlayer = ExistingInDB.FirstOrDefault(c => c.Name == Player.Name);
                //        if (ExistingPlayer != null)
                //        {
                //            ExistingPlayer.FlopsSeen += Player.FlopsSeen;
                //            ExistingPlayer.HandsPlayed += Player.HandsPlayed;
                //            ExistingPlayer.PreflopRaises += Player.PreflopRaises;
                //            ExistingPlayer.VPIP += Player.VPIP;
                //            PlayerCollection.Update(ExistingPlayer);
                //        }
                //        else
                //        {
                //            PlayerCollection.Insert(Player);
                //        }
                //        PlayerCollection.EnsureIndex(c => c.Name);
                //    }
                //}
            }


        }
        
    }
}
