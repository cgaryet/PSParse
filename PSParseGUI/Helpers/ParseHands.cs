using PSniffGUI.Enums;
using PSniffGUI.Model.Parsing;
using PSParseGUI.Model.Export;
using PSParseGUI.Model.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSParseGUI.Helpers
{
    internal static class ParseHands
    {
        private const string HandNumberRegex = "PokerStars.+#(\\d+):.+- (\\S+ \\S+)";
        private const string SeatRegex = "Seat (\\d): (.+) \\((\\d+) in chips\\)";
        private const string BlindRegex = "(.+): posts (small|big|small & big) blind[s]? (\\d+)";
        private const string FoldRegex = "(.+): (folds|checks)";
        private const string BetOrCallRegex = "(.+): (bets|calls) (\\d+)";
        private const string RaiseRegex = "(.+): raises (\\d+) to (\\d+)";
        private const string UncalledBetRegex = "Uncalled bet \\((\\d+)\\) returned to (.+)";
        private const string CollectionRegex = "(.+) collected (\\d+) from(?: side| main)? pot(?:-\\d)?";
        private const string TotalPotAndRake = "Total pot (\\d+)(?: Main pot \\d+\\. (?:Side pot(?:-\\d)? \\d+\\. )*)? ?\\| Rake (\\d+)";
        private const string StreetRegex = "\\*\\*\\* (FLOP|TURN|RIVER) \\*\\*\\* .+";
        private const string WentToShowdownRegex = "(.+): (shows|mucks)";

        private static List<TablePosition> FillPositions(Hand Hand)
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

        private static bool FindCheckForCheckRaise(HandAction RaiseAction, int StreetIdx)
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


        private static void NegatePreviousChipCommit(Hand Hand, HandPlayer Player)
        {
            // find the next previous bet action to call
            for (int ActionIdx = Hand.Action.Count - 1; ActionIdx >= 0; ActionIdx--)
            {
                HandAction Action = Hand.Action[ActionIdx];
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
                        //break;
                    }
                    // if this raise was from a player bought blind, we'll have an extra small blind left over, so just alter this action to be a commiting small blind
                    else if (Action.Action == PlayerAction.BuyBlinds)
                    {
                        Action.Action = PlayerAction.AlteredToBuyBlinds;
                        if (Action.Amount == Hand.SmallBlindAmount + Hand.BigBlindAmount || Action.Amount == Hand.SmallBlindAmount)
                        {
                            Action.Amount = Hand.SmallBlindAmount;
                            Action.FinalChipCommit = true;
                        }
                        else if (Action.Amount == Hand.BigBlindAmount)
                        {
                            Action.Amount = 0;
                            Action.FinalChipCommit = true;
                        }
                        //break;
                    }
                }
            }
        }

        public static void ParseTransactions(List<Hand> Hands, List<Player> PlayerList, bool LiveView)
        {
            // loop through each hand and figure out the game flow
            for (int HandIdx = 0; HandIdx < Hands.Count; HandIdx++)
            {
                Hand PreviousHand = HandIdx == 0 ? null : Hands[HandIdx - 1];
                Hand CurrentHand = Hands[HandIdx];
                Hand NextHand = HandIdx == Hands.Count - 1 ? null : Hands[HandIdx + 1];

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
                    if (PreviousHand == null || PreviousHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1 ||
                        NextHand == null || NextHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1)
                    {
                        if (PreviousHand == null || PreviousHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1)
                        {
                            if (ExistingPlayer.Transactions.Count > 0)
                            {
                                Transaction Previous = ExistingPlayer.Transactions[ExistingPlayer.Transactions.Count - 1];
                                if (Previous.TransactionType == TransactionType.CashOut)
                                {
                                    // if the previous amount was higher, we left and bought back in for less
                                    if(Previous.Amount <= Player.ChipCountStart)
                                    {
                                        if (Previous.Amount < Player.ChipCountStart)
                                        {
                                            int AddonAmount = Player.ChipCountStart - Previous.Amount;
                                            ExistingPlayer.TotalChipCountIn += AddonAmount;
                                            ExistingPlayer.Transactions.Add(new Transaction(ExistingPlayer, TransactionType.Addon, HandIdx, CurrentHand.HandNumber, AddonAmount));
                                        }
                                        ExistingPlayer.TotalChipCountOut -= Previous.Amount;
                                        ExistingPlayer.Transactions.Remove(Previous);
                                    }
                                    else
                                    {
                                        ExistingPlayer.TotalChipCountIn += Player.ChipCountStart;
                                        ExistingPlayer.Transactions.Add(new Transaction(ExistingPlayer, TransactionType.BuyIn, HandIdx, CurrentHand.HandNumber, Player.ChipCountStart));
                                    }
                                }
                            }
                            else
                            {
                                ExistingPlayer.TotalChipCountIn += Player.ChipCountStart;
                                ExistingPlayer.Transactions.Add(new Transaction(ExistingPlayer, TransactionType.BuyIn, HandIdx, CurrentHand.HandNumber, Player.ChipCountStart));
                            }
                        }
                        // if this player isn't in the NEXT hand, mark the end of the current tuple and set the total chip out count to the end of this hand
                        if (NextHand == null || NextHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1)
                        {
                            bool bAdd = false;
                            if (!LiveView || (NextHand != null && NextHand.Players.FindIndex(Prev => Prev.Name == Player.Name) == -1))
                            {
                                bAdd = true;
                            }
                            if (bAdd)
                            {
                                ExistingPlayer.TotalChipCountOut += Player.ChipCountEnd;
                                ExistingPlayer.Transactions.Add(new Transaction(ExistingPlayer, TransactionType.CashOut, HandIdx, CurrentHand.HandNumber, Player.ChipCountEnd));
                            }
                        }
                    }
                    else
                    {
                        int PrevChipCountEnd = PreviousHand.Players.First(Prev => Prev.Name == Player.Name).ChipCountEnd;
                        // if this player's end chip count from the previous hand doesnt match the start of the next hand, they had an addon, so add the difference to the total in count
                        if (PrevChipCountEnd != Player.ChipCountStart)
                        {
                            int Amount = Player.ChipCountStart - PrevChipCountEnd;
                            ExistingPlayer.Transactions.Add(new Transaction(ExistingPlayer, TransactionType.Addon, HandIdx, CurrentHand.HandNumber, Amount));
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
        }

        //public static Tuple<List<Hand>, List<Player>> Parse(string FileName)
        //{
        //    List<Hand> Hands = new List<Hand>();
        //    List<Player> PlayerList = new List<Player>();

        //    int EmptyLineCounter = 0;
        //    int HandIdx = 0;
        //    Hand NewHand = new Hand();

        //    string FileText = File.ReadAllText(FileName);
        //    // parse each line of text in the log
        //    foreach (string Line in FileText.Split('\n'))
        //    {
        //        if (string.IsNullOrWhiteSpace(Line))
        //        {
        //            EmptyLineCounter++;
        //        }
        //        else
        //        {
        //            ParseLine(NewHand, Line.Trim());
        //        }
        //        // save off
        //        if (EmptyLineCounter == 3)
        //        {
        //            EmptyLineCounter = 0;
        //            AssignActionAttributes(NewHand);
        //            Hands.Add(NewHand);
        //            HandIdx++;
        //            NewHand = new Hand(HandIdx);
        //        }
        //    }

        //    ParseTransactions(Hands, PlayerList, false);

        //    return new Tuple<List<Hand>, List<Player>>(Hands, PlayerList);
        //}

        public static Hand Parse(string Hand)
        {
            Hand ToReturn = new Hand(Hand);
            foreach (string Line in Hand.Split('\n'))
            {
                ParseLine(ToReturn, Line.Trim());
            }
            AssignActionAttributes(ToReturn);
            return ToReturn;
        }

        public static List<Hand> Parse(IEnumerable<string> HandInput)
        {
            List<Hand> Hands = new List<Hand>();

            // parse each line of text in the log
            foreach (string Hand in HandInput)
            {
                Hands.Add(Parse(Hand));
            }

            return Hands;
        }

        private static void ParseLine(Hand Hand, string InBlock)
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
                    Hand.Action.Add(new HandAction() { Hand = Hand, Player = Hand.Dealer, Amount = 0, Action = PlayerAction.Flop });
                    Hand.PreflopParsed = true;
                }
                else if (Street == "TURN")
                {
                    Hand.Action.Add(new HandAction() { Hand = Hand, Player = Hand.Dealer, Amount = 0, Action = PlayerAction.Turn });
                }
                else if (Street == "RIVER")
                {
                    Hand.Action.Add(new HandAction() { Hand = Hand, Player = Hand.Dealer, Amount = 0, Action = PlayerAction.River });
                }

                return;
            }

            // seat numbers and starting stacks line
            LineMatch = Regex.Match(InBlock, SeatRegex);
            if (LineMatch.Success)
            {
                int SeatNumber = int.Parse(LineMatch.Groups[1].Value);
                string PlayerName = LineMatch.Groups[2].Value;
                int PlayerChips = int.Parse(LineMatch.Groups[3].Value);

                Hand.Players.Add(new HandPlayer { SeatNumber = SeatNumber, Name = PlayerName, ChipCountStart = PlayerChips, ChipCountEnd = PlayerChips });
                return;
            }

            // blinds regexes
            LineMatch = Regex.Match(InBlock, BlindRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                string BlindPosition = LineMatch.Groups[2].Value;
                int BlindValue = int.Parse(LineMatch.Groups[3].Value);

                HandPlayer ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);
                if (BlindPosition == "small")
                {
                    if(Hand.SmallBlindAmount == 0)
                    {
                        Hand.SmallBlindAmount = BlindValue;
                        Hand.Action.Add(new HandAction { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.PostSmallBlind, Amount = BlindValue });

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
                    else
                    {
                        Hand.PurchasedHandAmount = BlindValue;
                        Hand.Action.Add(new HandAction { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.BuyBlinds, Amount = BlindValue });
                    }
                }
                else if (BlindPosition == "big")
                {
                    if(Hand.BigBlindAmount == 0)
                    {
                        Hand.BigBlindAmount = BlindValue;
                        Hand.Action.Add(new HandAction { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.PostBigBlind, Amount = BlindValue });
                    }
                    else
                    {
                        Hand.PurchasedHandAmount = BlindValue;
                        Hand.Action.Add(new HandAction { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.BuyBlinds, Amount = BlindValue });
                    }
                }
                else if (BlindPosition == "small & big")
                {
                    Hand.PurchasedHandAmount = BlindValue;
                    Hand.Action.Add(new HandAction { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.BuyBlinds, Amount = BlindValue });
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

                HandPlayer ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                PlayerAction Action = BetOrCall == "bets" ? PlayerAction.Bet : PlayerAction.Call;

                HandAction ActionTaken = new HandAction
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

                HandPlayer ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                NegatePreviousChipCommit(Hand, ExistingPlayer);

                HandAction ActionTaken = new HandAction
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

                HandPlayer ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);
                PlayerAction Action = ActionValue == "checks" ? PlayerAction.Check : PlayerAction.Fold;

                HandAction ActionTaken = new HandAction { Hand = Hand, Player = ExistingPlayer, Action = Action, Amount = 0 };

                Hand.Action.Add(ActionTaken);

                return;
            }

            // uncalled bet value
            LineMatch = Regex.Match(InBlock, UncalledBetRegex);
            if (LineMatch.Success)
            {
                int UncalledBetValue = int.Parse(LineMatch.Groups[1].Value);
                string PlayerName = LineMatch.Groups[2].Value;

                Hand.Action.Add(new HandAction { Hand = Hand, Player = Hand.Players.FirstOrDefault(p => p.Name == PlayerName), Action = PlayerAction.ChipsReturned, Amount = UncalledBetValue });

                return;
            }

            // showdown
            LineMatch = Regex.Match(InBlock, WentToShowdownRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                string ShowOrMuck = LineMatch.Groups[2].Value;

                HandPlayer ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                Hand.Action.Add(new HandAction { Hand = Hand, Player = ExistingPlayer, Action = PlayerAction.Showdown, Amount = 0 /*ActionStatFlags = ActionStatType.WentToShowdown*/ });

                return;
            }


            // winner
            LineMatch = Regex.Match(InBlock, CollectionRegex);
            if (LineMatch.Success)
            {
                string PlayerName = LineMatch.Groups[1].Value;
                int CollectionValue = int.Parse(LineMatch.Groups[2].Value);

                HandPlayer ExistingPlayer = Hand.Players.FirstOrDefault(p => p.Name == PlayerName);

                HandAction ActionTaken = new HandAction
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
                    foreach (HandAction Action in Hand.Action.Where(c => c.Player == Player))
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

        private static void AssignActionAttributes(Hand Hand)
        {
            HandStats Stats = new HandStats();
            // loop through the hand's complete action and assign as we go
            // 0 - preflop, 1 - flop, 2 - turn, 3 - river, 4 - showdown
            int HandState = 0;

            HandPlayer InitialRaiser = null;
            HandPlayer ThreeBettor = null;
            HandPlayer FourBettor = null;
            HandPlayer LastRaiserPreflop = null;
            int FlopIdx = -1;
            int TurnIdx = -1;
            int RiverIdx = -1;
            for (int ActionIdx = 0; ActionIdx < Hand.Action.Count; ActionIdx++)
            {
                HandAction Action = Hand.Action[ActionIdx];
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
    }
}
