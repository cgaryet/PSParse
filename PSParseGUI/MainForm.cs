using PSniffGUI.Enums;
using PSniffGUI.Model.Parsing;
using PSParseGUI.FormClasses;
using PSParseGUI.Forms;
using PSParseGUI.Helpers;
using PSParseGUI.Logic;
using PSParseGUI.Model.Export;
using PSParseGUI.Model.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSParseGUI
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
        private bool bMonitoring = false;
        
        private StatsForm NewStatsForm = new StatsForm();
        private Dictionary<string, PlayerStats> StatsCache = new Dictionary<string, PlayerStats>();

        private Dictionary<string, PlayerStats> ImportedStatsCache = new Dictionary<string, PlayerStats>();

        private MainState MainState = new MainState();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
#if (!DEBUG)
                debugModeToolStripMenuItem.Enabled = false;
#endif
            StatusLabel.Text = "Idle";
            bMonitoring = false;

            ((Progress<string>)MainState.StatusProgress).ProgressChanged += (s, message) =>
            {
                if (!StatusLabel.IsDisposed)
                {
                    StatusLabel.Text = message;
                }
            };

            ((Progress<string>)MainState.ErrorProgress).ProgressChanged += (s, message) =>
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
        }

        private async void attachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startMonitoringToolStripMenuItem.Enabled = false;
            testLogoutToolStripMenuItem.Enabled = false;
            attachToolStripMenuItem.Enabled = false;
            detachToolStripMenuItem.Enabled = false;
            if (await MainState.Attach())
            {
                attachToolStripMenuItem.Enabled = false;
                detachToolStripMenuItem.Enabled = true;
                startMonitoringToolStripMenuItem.Enabled = true;
                stopMonitoringToolStripMenuItem.Enabled = false;
                testLogoutToolStripMenuItem.Enabled = true;
            }
            else
            {
                attachToolStripMenuItem.Enabled = true;
                detachToolStripMenuItem.Enabled = false;
                startMonitoringToolStripMenuItem.Enabled = false;
                stopMonitoringToolStripMenuItem.Enabled = false;
                testLogoutToolStripMenuItem.Enabled = false;
            }
        }

        private async void detachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (await MainState.Detach() && !MainState.IsAttached)
            {
                attachToolStripMenuItem.Enabled = true;
                detachToolStripMenuItem.Enabled = false;
                startMonitoringToolStripMenuItem.Enabled = false;
                stopMonitoringToolStripMenuItem.Enabled = false;
                testLogoutToolStripMenuItem.Enabled = false;
                StatusLabel.Text = "Idle";
            }
        }

        private async void startMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MainState.LogoutBaseAddress == -1)
            {
                if(MessageBox.Show("The logout hook has not been configured yet. Continue without it?\n(Stars will disconnect after 30m of inactivity, LOSING THE HAND HISTORY)", "Continue?", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            attachToolStripMenuItem.Enabled = false;
            detachToolStripMenuItem.Enabled = false;
            startMonitoringToolStripMenuItem.Enabled = false;
            stopMonitoringToolStripMenuItem.Enabled = true;
            testLogoutToolStripMenuItem.Enabled = false;

            await MonitorLoop();

            detachToolStripMenuItem.Enabled = true;
            startMonitoringToolStripMenuItem.Enabled = true;
            stopMonitoringToolStripMenuItem.Enabled = false;
            testLogoutToolStripMenuItem.Enabled = true;
        }

        private async Task MonitorLoop()
        {
            while (!CancelTokenSource.IsCancellationRequested)
            {
                bMonitoring = true;
                StatusLabel.Text = "Pulling Data from Stars...";
                (List<Hand> Hands, List<Hand> Rewrites, List<Player> Players) = await MainState.ScrapeData();

                string UpdateText = "No new hands found";
                if (Rewrites.Count > 0)
                {
                    int SelectedIdx = HandList.SelectedIndex;
                    HandList.SelectedIndex = -1;

                    List<Hand> HandsInList = HandList.Items.Cast<Hand>().ToList();
                    foreach(Hand Rewrite in Rewrites)
                    {
                        Hand ToRewrite = HandsInList.FirstOrDefault(c => c.HandNumber == Rewrite.HandNumber);
                        if(ToRewrite != null)
                        {
                            int RemoveIdx = HandList.Items.IndexOf(ToRewrite);
                            //Console.WriteLine($"Found rewrite at idx {RemoveIdx} replacing:\n{ToRewrite.Raw} with:\n{Rewrite.Raw}");
                            HandList.Items.RemoveAt(RemoveIdx);
                            HandList.Items.Insert(RemoveIdx, Rewrite);
                        }
                    }
                    HandList.SelectedIndex = SelectedIdx;
                }
                if (Hands.Count > 0)
                {
                    Hands.ForEach(c => HandList.Items.Add(c));
                    Hand ThisHand = Hands.Last();

                    for (int x = 1; x <= 9; x++)
                    {
                        FormHelpers.GetControlByName(this, $"Live_Seat{x}").Text = "Seat Empty";
                    }
                    Players.ForEach(ParsedPlayer =>
                    {
                        HandPlayer InCurrentHand = ThisHand.Players.FirstOrDefault(c => c.Name == ParsedPlayer.Name);

                        if (InCurrentHand != null)
                        {
                            string status = $"{ParsedPlayer.Name}{Environment.NewLine}";
                            status += $"Chips: {InCurrentHand.ChipCountEnd}{Environment.NewLine}";
                            status += $"BuyIn: {ParsedPlayer.Transactions.Where(c => c.TransactionType == TransactionType.BuyIn).Select(c => c.Amount).Sum()}{Environment.NewLine}";
                            status += $"Addons: {ParsedPlayer.Transactions.Where(c => c.TransactionType == TransactionType.Addon).Select(c => c.Amount).Sum()}{Environment.NewLine}";
                            FormHelpers.GetControlByName(this, $"Live_Seat{InCurrentHand.SeatNumber}").Text = status;

                            //StatsCache[ParsedPlayer.Name] = new PlayerStats(ParsedPlayer);
                        }
                    });

                    IEnumerable<Transaction> HandTransactions = Players.SelectMany(c => c.Transactions).OrderBy(c => c.HandNumber);
                    IEnumerable<Transaction> ExistingTransactions = transactionBindingSource.List.OfType<Transaction>();

                    List<Transaction> Additions = HandTransactions.Except(ExistingTransactions).ToList();
                    List<Transaction> Subtractions = ExistingTransactions.Except(HandTransactions).ToList();

                    foreach (Transaction Add in Additions)
                    {
                        transactionBindingSource.Add(Add);
                    }
                    foreach (Transaction Sub in Subtractions)
                    {
                        transactionBindingSource.Remove(Sub);
                    }
                    UpdateText = String.Format("Added {0} Hands, Updated {1} Transactions", Hands.Count, Additions.Count);
                }
                StatusLabel.Text = string.Format("{0}. Sleeping 3s", UpdateText);
                await Task.Delay(1000);                          
                StatusLabel.Text = string.Format("{0}. Sleeping 2s", UpdateText);
                await Task.Delay(1000);                          
                StatusLabel.Text = string.Format("{0}. Sleeping 1s", UpdateText);
                await Task.Delay(1000);
            }
            bMonitoring = false;

        }

        private void stopMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopMonitoring();
        }


        private void StopMonitoring()
        {
            CancelTokenSource.Cancel();
        }

        private async void cSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() => {
                ExportCSV Export = new ExportCSV();
                Export.Export(MainState.Hands);
            });
        }

        private void HandList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (HandList.SelectedItem != null)
            {
                HandText.Clear();
                HandText.Text = ((Hand)HandList.SelectedItem).Raw.Replace("\n", Environment.NewLine);
            }
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigurationForm newConfig = new ConfigurationForm();
            newConfig.SetDefault(MainState.HandTitleMatch, MainState.TableTitleMatch);
            newConfig.FormClosing += (sender, e) =>
            {
                Tuple<string, string> Result = newConfig.GetResult();
                MainState.HandTitleMatch = Result.Item1;
                MainState.TableTitleMatch = Result.Item2;
                //if (bMonitoring)
                //{
                //    MessageBox.Show("Configuration change will reset the hand list and restart monitoring.", "Change", MessageBoxButtons.OK);
                //}
                //else
                //{
                    MessageBox.Show("Configuration change will reset the hand list.", "Change", MessageBoxButtons.OK);
                //}

                //if (bMonitoring)
                //{
                //    await Task.Run(() =>
                //    {
                //        StopMonitoring();
                //    });
                //    HandList.SelectedIndex = -1;
                //    HandList.Items.Clear();
                //    HandList.Text = String.Empty;
                //    HandText.Clear();
                //    StartMonitoring();
                //}
            };
            newConfig.ShowDialog();

        }

        private async void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopMonitoring();
            while(bMonitoring)
            {
                await Task.Delay(100);
            }
            await MainState.Detach();
            this.Close();
        }

        private void dumpHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            File.WriteAllText("handhistory.txt", string.Join("\n\n\n", MainState.Hands.Select(c => c.Raw)));
        }

        private void debugModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //NewStatsForm.Show();
        }

        private void Live_Seat_Click(object sender, EventArgs e)
        {
            // get user
            //string Name = (sender as TextBox).Text.Split('\n')[0].Trim();
            //if (StatsCache.ContainsKey(Name))
            //{
            //    PlayerStats Stats = StatsCache[Name];
            //    if (ImportedStatsCache.ContainsKey(Name))
            //    {
            //        Stats = ImportedStatsCache[Name] + Stats;
            //    }
            //    (NewStatsForm.Controls["statsTextBox"] as TextBox).Text = Stats.ToString();
            //}
        }

        private void processToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog newImportFolder = new FolderBrowserDialog();
            if(newImportFolder.ShowDialog() == DialogResult.OK)
            {
                foreach (string FileName in Directory.EnumerateFiles(newImportFolder.SelectedPath, "*.txt"))
                {
                    string FileText = File.ReadAllText(FileName);

                    List<string> Raws = new List<string>();
                    int EmptyLineCounter = 0;
                    StringBuilder rawBuilder = new StringBuilder();
                    foreach (string Line in FileText.Split('\n'))
                    {
                        if (string.IsNullOrWhiteSpace(Line))
                        {
                            EmptyLineCounter++;
                        }
                        else
                        {
                            rawBuilder.AppendLine(Line.Trim());
                            //ParseLine(NewHand, Line.Trim());
                        }
                        // save off
                        if (EmptyLineCounter == 3)
                        {
                            EmptyLineCounter = 0;
                            Raws.Add(rawBuilder.ToString());
                            rawBuilder.Clear();
                        }
                    }


                    List<Hand> Hands = ParseHands.Parse(Raws);

                    ExportCSV Export = new ExportCSV();
                    Export.Export(Hands);

                }
            }
        }

        private void testLogoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogoutTest NewLogoutTest = new LogoutTest();
            NewLogoutTest.ShowDialog();
            MainState.LogoutBaseAddress = NewLogoutTest.SuccessAddress;
        }

    }
}
