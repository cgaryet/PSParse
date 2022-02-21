using PSniffGUI.Model.Parsing;
using PSParseGUI.Model.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSParseGUI.Model.Parsing
{
    internal class PlayerStats
    {
        public Player PlayerRef { get; private set; }
        public string Name { get; private set; }
        public int HandsPlayed { get; private set; }
        public int FlopsSeen { get; private set; }
        public int VPIPs { get; private set; }
        public int PreflopRaises { get; private set; }
        public int CalledPreflopRaises { get; private set; }
        public int UnopenedPreflopRaises { get; private set; }
        public int UnopenedRaiseOpportunities { get; private set; }
        public int ThreeBetPreflops { get; private set; }
        public int ThreeBetPreflopOpportunities { get; private set; }
        public int FoldedTo3BetPreflops { get; private set; }
        public int FoldedTo3BetPreflopOpportunities { get; private set; }
        public int FourBetPreflops { get; private set; }
        public int FourBetPreflopOpportunities { get; private set; }
        public int FoldedTo4BetPreflops { get; private set; }
        public int FoldedTo4BetPreflopOpportunities { get; private set; }
        public int Squeezes { get; private set; }
        public int SqueezeOpportunities { get; private set; }
        public int BlindStealAttempts { get; private set; }
        public int BlindStealAttemptOpportunities { get; private set; }
        public int FoldedBigBlindToStealAttempts { get; private set; }
        public int FoldedBigBlindToStealAttemptOpportunities { get; private set; }
        public int BetOrRaisedFlops { get; private set; }
        public int BetRasiedCalledFoldedFlops { get; private set; }
        public int BetOrRaisedTurns { get; private set; }
        public int BetRasiedCalledFoldedTurns { get; private set; }
        public int BetOrRaisedRivers { get; private set; }
        public int BetRasiedCalledFoldedRivers { get; private set; }
        public int CheckRaisedFlops { get; private set; }
        public int CheckRaisedFlopOpportunities { get; private set; }
        public int CheckRaisedTurns { get; private set; }
        public int CheckRaisedTurnOpportunities { get; private set; }
        public int CheckRaisedRivers { get; private set; }
        public int CheckRaisedRiverOpportunities { get; private set; }
        public int CBetFlops { get; private set; }
        public int CBetFlopOpportunities { get; private set; }
        public int FoldedToCBetFlops { get; private set; }
        public int FoldedToCBetFlopOpportunities { get; private set; }
        public int CBetTurns { get; private set; }
        public int CBetTurnOpportunities { get; private set; }
        public int FoldedToCBetTurns { get; private set; }
        public int FoldedToCBetTurnOpportunities { get; private set; }
        public int CBetRivers { get; private set; }
        public int CBetRiverOpportunities { get; private set; }
        public int FoldedToCBetRivers { get; private set; }
        public int FoldedToCBetRiverOpportunities { get; private set; }
        public int DonkBetFlops { get; private set; }
        public int DonkBetFlopOpportunities { get; private set; }
        public int DonkBetTurns { get; private set; }
        public int DonkBetTurnOpportunities { get; private set; }
        public int DonkBetRivers { get; private set; }
        public int DonkBetRiverOpportunities { get; private set; }
        public int WentToShowdowns { get; private set; }
        public int WonAtShowdowns { get; private set; }
        public int WonWithoutShowdowns { get; private set; }

        public PlayerStats(Player Player)
        {
            this.PlayerRef = Player;
            this.Name = Player.Name;
            List<HandStats> AllStats = Player.HandsPlayed.Select(c => c.Stats).ToList();

            this.HandsPlayed = Player.HandsPlayed.Count;
            this.FlopsSeen = AllStats.Where(c => c.FlopSeen.Any(d => d.Name == Player.Name)).Count();
            this.VPIPs = AllStats.Where(c => c.VPIP.Any(d => d.Name == Player.Name)).Count();
            this.PreflopRaises = AllStats.Where(c => c.PreflopRaise.Any(d => d.Name == Player.Name)).Count();
            this.CalledPreflopRaises = AllStats.Where(c => c.CalledPreflopRaise.Any(d => d.Name == Player.Name)).Count();
            this.UnopenedPreflopRaises = AllStats.Where(c => c.UnopenedPreflopRaise.Any(d => d.Name == Player.Name)).Count();
            this.UnopenedRaiseOpportunities = AllStats.Where(c => c.UnopenedRaiseOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.ThreeBetPreflops = AllStats.Where(c => c.ThreeBetPreflop.Any(d => d.Name == Player.Name)).Count();
            this.ThreeBetPreflopOpportunities = AllStats.Where(c => c.ThreeBetOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FoldedTo3BetPreflops = AllStats.Where(c => c.FoldedTo3BetPreflop.Any(d => d.Name == Player.Name)).Count();
            this.FoldedTo3BetPreflopOpportunities = AllStats.Where(c => c.FoldedTo3BetPreflopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FourBetPreflops = AllStats.Where(c => c.FourBetPreflop.Any(d => d.Name == Player.Name)).Count();
            this.FourBetPreflopOpportunities = AllStats.Where(c => c.FourBetPreflopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FoldedTo4BetPreflops = AllStats.Where(c => c.FoldedTo4BetPreflop.Any(d => d.Name == Player.Name)).Count();
            this.FoldedTo4BetPreflopOpportunities = AllStats.Where(c => c.FoldedTo4BetPreflopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.Squeezes = AllStats.Where(c => c.Squeeze.Any(d => d.Name == Player.Name)).Count();
            this.SqueezeOpportunities = AllStats.Where(c => c.SqueezeOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.BlindStealAttempts = AllStats.Where(c => c.BlindStealAttempt.Any(d => d.Name == Player.Name)).Count();
            this.BlindStealAttemptOpportunities = AllStats.Where(c => c.BlindStealAttemptOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FoldedBigBlindToStealAttempts = AllStats.Where(c => c.FoldedBigBlindToStealAttempt.Any(d => d.Name == Player.Name)).Count();
            this.FoldedBigBlindToStealAttemptOpportunities = AllStats.Where(c => c.FoldedBigBlindToStealAttemptOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.BetOrRaisedFlops = AllStats.Where(c => c.BetOrRaisedFlop.Any(d => d.Name == Player.Name)).Count();
            this.BetRasiedCalledFoldedFlops = AllStats.Where(c => c.BetRasiedCalledFoldedFlop.Any(d => d.Name == Player.Name)).Count();
            this.BetOrRaisedTurns = AllStats.Where(c => c.BetOrRaisedTurn.Any(d => d.Name == Player.Name)).Count();
            this.BetRasiedCalledFoldedTurns = AllStats.Where(c => c.BetRasiedCalledFoldedTurn.Any(d => d.Name == Player.Name)).Count();
            this.BetOrRaisedRivers = AllStats.Where(c => c.BetOrRaisedRiver.Any(d => d.Name == Player.Name)).Count();
            this.BetRasiedCalledFoldedRivers = AllStats.Where(c => c.BetRasiedCalledFoldedRiver.Any(d => d.Name == Player.Name)).Count();
            this.CheckRaisedFlops = AllStats.Where(c => c.CheckRaisedFlop.Any(d => d.Name == Player.Name)).Count();
            this.CheckRaisedFlopOpportunities = AllStats.Where(c => c.CheckRaisedFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.CheckRaisedTurns = AllStats.Where(c => c.CheckRaisedTurn.Any(d => d.Name == Player.Name)).Count();
            this.CheckRaisedTurnOpportunities = AllStats.Where(c => c.CheckRaisedTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.CheckRaisedRivers = AllStats.Where(c => c.CheckRaisedRiver.Any(d => d.Name == Player.Name)).Count();
            this.CheckRaisedRiverOpportunities = AllStats.Where(c => c.CheckRaisedRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.CBetFlops = AllStats.Where(c => c.CBetFlop.Any(d => d.Name == Player.Name)).Count();
            this.CBetFlopOpportunities = AllStats.Where(c => c.CBetFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FoldedToCBetFlops = AllStats.Where(c => c.FoldedToCBetFlop.Any(d => d.Name == Player.Name)).Count();
            this.FoldedToCBetFlopOpportunities = AllStats.Where(c => c.FoldedToCBetFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.CBetTurns = AllStats.Where(c => c.CBetTurn.Any(d => d.Name == Player.Name)).Count();
            this.CBetTurnOpportunities = AllStats.Where(c => c.CBetTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FoldedToCBetTurns = AllStats.Where(c => c.FoldedToCBetTurn.Any(d => d.Name == Player.Name)).Count();
            this.FoldedToCBetTurnOpportunities = AllStats.Where(c => c.FoldedToCBetTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.CBetRivers = AllStats.Where(c => c.CBetRiver.Any(d => d.Name == Player.Name)).Count();
            this.CBetRiverOpportunities = AllStats.Where(c => c.CBetRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.FoldedToCBetRivers = AllStats.Where(c => c.FoldedToCBetRiver.Any(d => d.Name == Player.Name)).Count();
            this.FoldedToCBetRiverOpportunities = AllStats.Where(c => c.FoldedToCBetRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.DonkBetFlops = AllStats.Where(c => c.DonkBetFlop.Any(d => d.Name == Player.Name)).Count();
            this.DonkBetFlopOpportunities = AllStats.Where(c => c.DonkBetFlopOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.DonkBetTurns = AllStats.Where(c => c.DonkBetTurn.Any(d => d.Name == Player.Name)).Count();
            this.DonkBetTurnOpportunities = AllStats.Where(c => c.DonkBetTurnOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.DonkBetRivers = AllStats.Where(c => c.DonkBetRiver.Any(d => d.Name == Player.Name)).Count();
            this.DonkBetRiverOpportunities = AllStats.Where(c => c.DonkBetRiverOpportunity.Any(d => d.Name == Player.Name)).Count();
            this.WentToShowdowns = AllStats.Where(c => c.WentToShowdown.Any(d => d.Name == Player.Name)).Count();
            this.WonAtShowdowns = AllStats.Where(c => c.WonAtShowdown.Any(d => d.Name == Player.Name)).Count();
            this.WonWithoutShowdowns = AllStats.Where(c => c.WonWithoutShowdown.Any(d => d.Name == Player.Name)).Count();
        }
        

        public static PlayerStats operator+(PlayerStats a, PlayerStats b)
        {
            if(a.Name != b.Name)
            {
                throw new Exception("Trying to add stats of different players together!");
            }
            PlayerStats Added = new PlayerStats(b.PlayerRef);
            Added.HandsPlayed += a.HandsPlayed;
            Added.FlopsSeen += a.FlopsSeen;
            Added.VPIPs += a.VPIPs;
            Added.PreflopRaises += a.PreflopRaises;
            Added.CalledPreflopRaises += a.CalledPreflopRaises;
            Added.UnopenedPreflopRaises += a.UnopenedPreflopRaises;
            Added.UnopenedRaiseOpportunities += a.UnopenedRaiseOpportunities;
            Added.ThreeBetPreflops += a.ThreeBetPreflops;
            Added.ThreeBetPreflopOpportunities += a.ThreeBetPreflopOpportunities;
            Added.FoldedTo3BetPreflops += a.FoldedTo3BetPreflops;
            Added.FoldedTo3BetPreflopOpportunities += a.FoldedTo3BetPreflopOpportunities;
            Added.FourBetPreflops += a.FourBetPreflops;
            Added.FourBetPreflopOpportunities += a.FourBetPreflopOpportunities;
            Added.FoldedTo4BetPreflops += a.FoldedTo4BetPreflops;
            Added.FoldedTo4BetPreflopOpportunities += a.FoldedTo4BetPreflopOpportunities;
            Added.Squeezes += a.Squeezes;
            Added.SqueezeOpportunities += a.SqueezeOpportunities;
            Added.BlindStealAttempts += a.BlindStealAttempts;
            Added.BlindStealAttemptOpportunities += a.BlindStealAttemptOpportunities;
            Added.FoldedBigBlindToStealAttempts += a.FoldedBigBlindToStealAttempts;
            Added.FoldedBigBlindToStealAttemptOpportunities += a.FoldedBigBlindToStealAttemptOpportunities;
            Added.BetOrRaisedFlops += a.BetOrRaisedFlops;
            Added.BetRasiedCalledFoldedFlops += a.BetRasiedCalledFoldedFlops;
            Added.BetOrRaisedTurns += a.BetOrRaisedTurns;
            Added.BetRasiedCalledFoldedTurns += a.BetRasiedCalledFoldedTurns;
            Added.BetOrRaisedRivers += a.BetOrRaisedRivers;
            Added.BetRasiedCalledFoldedRivers += a.BetRasiedCalledFoldedRivers;
            Added.CheckRaisedFlops += a.CheckRaisedFlops;
            Added.CheckRaisedFlopOpportunities += a.CheckRaisedFlopOpportunities;
            Added.CheckRaisedTurns += a.CheckRaisedTurns;
            Added.CheckRaisedTurnOpportunities += a.CheckRaisedTurnOpportunities;
            Added.CheckRaisedRivers += a.CheckRaisedRivers;
            Added.CheckRaisedRiverOpportunities += a.CheckRaisedRiverOpportunities;
            Added.CBetFlops += a.CBetFlops;
            Added.CBetFlopOpportunities += a.CBetFlopOpportunities;
            Added.FoldedToCBetFlops += a.FoldedToCBetFlops;
            Added.FoldedToCBetFlopOpportunities += a.FoldedToCBetFlopOpportunities;
            Added.CBetTurns += a.CBetTurns;
            Added.CBetTurnOpportunities += a.CBetTurnOpportunities;
            Added.FoldedToCBetTurns += a.FoldedToCBetTurns;
            Added.FoldedToCBetTurnOpportunities += a.FoldedToCBetTurnOpportunities;
            Added.CBetRivers += a.CBetRivers;
            Added.CBetRiverOpportunities += a.CBetRiverOpportunities;
            Added.FoldedToCBetRivers += a.FoldedToCBetRivers;
            Added.FoldedToCBetRiverOpportunities += a.FoldedToCBetRiverOpportunities;
            Added.DonkBetFlops += a.DonkBetFlops;
            Added.DonkBetFlopOpportunities += a.DonkBetFlopOpportunities;
            Added.DonkBetTurns += a.DonkBetTurns;
            Added.DonkBetTurnOpportunities += a.DonkBetTurnOpportunities;
            Added.DonkBetRivers += a.DonkBetRivers;
            Added.DonkBetRiverOpportunities += a.DonkBetRiverOpportunities;
            Added.WentToShowdowns += a.WentToShowdowns;
            Added.WonAtShowdowns += a.WonAtShowdowns;
            Added.WonWithoutShowdowns += a.WonWithoutShowdowns;
            return Added;

        }

        public override string ToString()
        {
            StringBuilder StatsBuilder = new StringBuilder();
            StatsBuilder.AppendLine(this.Name);

            StatsBuilder.AppendFormat("Hands Played: {0}\r\n", this.HandsPlayed);
            if (HandsPlayed != 0)
            {
                StatsBuilder.AppendFormat("Flops Seen: {0}%\r\n", (FlopsSeen * 100f / HandsPlayed).ToString("F1"));
                StatsBuilder.AppendFormat("VPIP: {0}%\r\n", (VPIPs * 100f / HandsPlayed).ToString("F1"));
                StatsBuilder.AppendFormat("Preflop Raises: {0}%\r\n", (PreflopRaises * 100f / HandsPlayed).ToString("F1"));
                StatsBuilder.AppendFormat("Unopened Preflop Raises: {0}%\r\n", (UnopenedPreflopRaises == 0 ? "0" : (UnopenedPreflopRaises * 100f / UnopenedRaiseOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("ThreeBets: {0}%\r\n", (ThreeBetPreflops == 0 ? "0" : (ThreeBetPreflops * 100f / ThreeBetPreflopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Fold to Three-Bets: {0}%\r\n", (FoldedTo3BetPreflops == 0 ? "0" : (FoldedTo3BetPreflops * 100f / FoldedTo3BetPreflopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Four+Bets: {0}%\r\n", (FourBetPreflops == 0 ? "0" : (FourBetPreflops * 100f / FourBetPreflopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Fold to Four+Bets: {0}%\r\n", (FoldedTo4BetPreflops == 0 ? "0" : (FoldedTo4BetPreflops * 100f / FoldedTo4BetPreflopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Squeezes: {0}%\r\n", (Squeezes == 0 ? "0" : (Squeezes * 100f / SqueezeOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Blind Steal Attempts: {0}%\r\n", (BlindStealAttempts == 0 ? "0" : (BlindStealAttempts * 100f / BlindStealAttemptOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Fold BB To Steal Attempt: {0}%\r\n", (FoldedBigBlindToStealAttempts == 0 ? "0" : (FoldedBigBlindToStealAttempts * 100f / FoldedBigBlindToStealAttemptOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Aggression Frequency: {0}%\r\n", ((BetOrRaisedFlops + BetOrRaisedTurns + BetOrRaisedRivers) == 0 ? "0" :
                                                                        ((BetOrRaisedFlops + BetOrRaisedTurns + BetOrRaisedRivers) * 100f / (BetRasiedCalledFoldedFlops + BetRasiedCalledFoldedTurns + BetRasiedCalledFoldedRivers)).ToString("F1")));
                StatsBuilder.AppendFormat("CBet Flop: {0}%\r\n", (CBetFlops == 0 ? "0" : (CBetFlops * 100f / CBetFlopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Folded to CBet Flop: {0}%\r\n", (FoldedToCBetFlops == 0 ? "0" : (FoldedToCBetFlops * 100f / FoldedToCBetFlopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("CBet Turn: {0}%\r\n", (CBetTurns == 0 ? "0" : (CBetTurns * 100f / CBetTurnOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Folded to CBet Turn: {0}%\r\n", (FoldedToCBetTurns == 0 ? "0" : (FoldedToCBetTurns * 100f / FoldedToCBetTurnOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("CBet River: {0}%\r\n", (CBetRivers == 0 ? "0" : (CBetRivers * 100f / CBetRiverOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Folded to CBet River: {0}%\r\n", (FoldedToCBetRivers == 0 ? "0" : (FoldedToCBetRivers * 100f / FoldedToCBetRiverOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Check Raises (Total): {0}%\r\n", ((CheckRaisedFlops + CheckRaisedTurns + CheckRaisedRivers) == 0 ? "0" :
                                                                        ((CheckRaisedFlops + CheckRaisedTurns + CheckRaisedRivers) * 100f / (CheckRaisedFlopOpportunities + CheckRaisedTurnOpportunities + CheckRaisedRiverOpportunities)).ToString("F1")));
                StatsBuilder.AppendFormat("Check Raises (Flop): {0}%\r\n", (CheckRaisedFlops == 0 ? "0" : (CheckRaisedFlops * 100f / CheckRaisedFlopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Check Raises (Turn): {0}%\r\n", (CheckRaisedTurns == 0 ? "0" : (CheckRaisedTurns * 100f / CheckRaisedTurnOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Check Raises (River): {0}%\r\n", (CheckRaisedRivers == 0 ? "0" : (CheckRaisedRivers * 100f / CheckRaisedRiverOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Donk Bets (Total): {0}%\r\n", (DonkBetFlops + DonkBetTurns + DonkBetRivers) == 0 ? "0" :
                                                                     ((DonkBetFlops + DonkBetTurns + DonkBetRivers) * 100f / (DonkBetFlopOpportunities + DonkBetTurnOpportunities + DonkBetRiverOpportunities)).ToString("F1"));
                StatsBuilder.AppendFormat("Donk Bets (Flop): {0}%\r\n", (DonkBetFlops == 0 ? "0" : (DonkBetFlops * 100f / DonkBetFlopOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Donk Bets (Turn): {0}%\r\n", (DonkBetTurns == 0 ? "0" : (DonkBetTurns * 100f / DonkBetTurnOpportunities).ToString("F1")));
                StatsBuilder.AppendFormat("Donk Bets (River): {0}%\r\n", (DonkBetRivers == 0 ? "0" : (DonkBetRivers * 100f / DonkBetRiverOpportunities).ToString("F1")));
            }
            return StatsBuilder.ToString();

        }
    }
}
