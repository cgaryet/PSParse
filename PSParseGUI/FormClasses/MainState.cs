using PSniffGUI.Model.Parsing;
using PSParseGUI.Helpers;
using PSParseGUI.Model.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PSParseGUI.FormClasses
{
    internal class MainState
    {
        public string HandTitleMatch { get; set; }
        public string TableTitleMatch { get; set; }
        public IProgress<string> StatusProgress { get; }
        public IProgress<string> ErrorProgress { get; }
        public bool IsAttached { get; private set; }

        public int LogoutBaseAddress { get; set; }

        public IReadOnlyCollection<Hand> Hands { get { return HandCache.AsReadOnly(); } }

        private List<Hand> HandCache;
        private List<Player> PlayerCache;

        public MainState()
        {
            this.StatusProgress = new Progress<string>();
            this.ErrorProgress = new Progress<string>();
            this.HandTitleMatch = "PokerStars Home Game";
            this.LogoutBaseAddress = -1;
            this.TableTitleMatch = "";
            HandCache = new List<Hand>();
            PlayerCache = new List<Player>();
        }

        public async Task<bool> Attach()
        {
            return await Task.Run(() => {
                this.IsAttached = HookProcess.Attach(this.StatusProgress, this.ErrorProgress);
                return this.IsAttached;
            });
        }

        public async Task<bool> Detach()
        {
            return await Task.Run(() => {
                HookProcess.Detach();
                this.IsAttached = false;
                this.LogoutBaseAddress = -1;
                return true;
            });
        }

        public async Task<(List<Hand>, List<Hand>, List<Player>)> ScrapeData()
        {
            return await Task.Run(() =>
            {
                SortedList<long, string> RawHandHistory = new SortedList<long, string>();
                HookProcess.GetHandHistoryDictionary(RawHandHistory, this.HandTitleMatch, this.TableTitleMatch);
                if (this.LogoutBaseAddress != -1)
                {
                    HookProcess.WriteTimeoutMemory(false, LogoutBaseAddress);
                }

                // loop through the hand cache, making sure all of the raw values we have match the lengths
                List<Hand> Rewrites = new List<Hand>();
                for (int Idx = HandCache.Count - 1; Idx >= 0; Idx--)
                {
                    Hand CachedHand = HandCache[Idx];
                    string NewRaw = RawHandHistory[long.Parse(CachedHand.HandNumber)];
                    if (CachedHand.Raw.Length != NewRaw.Length)
                    {
                        CachedHand = ParseHands.Parse(NewRaw);
                        HandCache[Idx] = CachedHand;
                        this.StatusProgress.Report($"{CachedHand.HandNumber} was re-evaluated for a nonmatching raw length");
                        Rewrites.Add(CachedHand);
                    }
                }
                List<string> NewRawHands = RawHandHistory.SkipWhile(c => HandCache.FirstOrDefault(d => d.HandNumber == c.Key.ToString()) != null).Select(c => c.Value).ToList();
                // parse the new hands
                List<Hand> NewHands = ParseHands.Parse(NewRawHands);
                HandCache.AddRange(NewHands);
                HandCache.Sort((h1, h2) => { return h1.Date.CompareTo(h2.Date); });
                PlayerCache.Clear();
                // parse out the transactions
                ParseHands.ParseTransactions(HandCache, PlayerCache, true);

                return (NewHands, Rewrites, PlayerCache);
            });
        }
    }
}
