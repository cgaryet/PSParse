using EventHook;
using PSParseGUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSParseGUI.Forms
{
    public partial class LogoutTest : Form
    {
        private CancellationTokenSource CancelTokenSource;
        private List<int> TestAddresses = new List<int>();
        public int SuccessAddress { get; private set; }

        public LogoutTest()
        {
            InitializeComponent();
            CancelTokenSource = new CancellationTokenSource();
            SuccessAddress = -1;
        }

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                string Result = Buff.ToString();
                //Console.WriteLine($"WindowName: {Result}");
                return Result;
            }
            return null;
        }

        private void LogoutTest_Load(object sender, EventArgs e)
        {
            StatusText.AppendText($"To configure the logout hook, click the mouse in the PokerStars lobby, then move the mouse around.{Environment.NewLine}This text box will populate with many true/false results as the mouse is moved.{Environment.NewLine}Once a large number of similar True results are found (that are NOT ffffff), press \"Test Logout\" to verify.{Environment.NewLine}");
            Task.Run(() => {
                using (var eventHookFactory = new EventHookFactory())
                {
                    var mouseWatcher = eventHookFactory.GetMouseWatcher();
                    bool bSearching = false;
                    mouseWatcher.OnMouseInput += (s, e) =>
                    {
                        if(GetActiveWindowTitle() == "PokerStars Lobby")
                        {
                            if (e.Message == EventHook.Hooks.MouseMessages.WM_MOUSEMOVE && !bSearching)
                            {
                                bSearching = true;
                                int TickCount = Environment.TickCount;
                                if (StatusText.InvokeRequired)
                                {
                                    StatusText.Invoke((MethodInvoker)async delegate {
                                        StatusText.AppendText($"Searching for {Environment.TickCount}...");
                                        int TestAddress = await Task.Run(() => { return HookProcess.GetLogoutMouseClick(TickCount); });
                                        TestAddresses.Add(TestAddress);
                                        bSearching = false;
                                        StatusText.AppendText($"Result: {TestAddress.ToString("x")}{Environment.NewLine}");
                                    });
                                }
                                else
                                {
                                    //StatusText.AppendText(Result);
                                }
                            }
                        }
                       
                    };
                    mouseWatcher.Start();

                    while (!CancelTokenSource.IsCancellationRequested)
                    {
                        Thread.Sleep(1000);
                    }
                    mouseWatcher.Stop();
                }
            });
        }

        private async void CloseTestButton_Click(object sender, EventArgs e)
        {
            StatusText.AppendText($"Shutting down mouse event handers...");
            TestLogoutButton.Enabled = false;
            CloseTestButton.Enabled = false;
            CancelTokenSource.Cancel();
            await Task.Delay(1000);
            this.Close();
        }

        private void TestLogoutButton_Click(object sender, EventArgs e)
        {
            int BestGuess = -1;
            bool BestGuessSetByTextbox = false;
            if(!string.IsNullOrEmpty(ManualAddressInput.Text))
            {
                try
                {
                    BestGuess = Convert.ToInt32($"{(ManualAddressInput.Text.StartsWith("0x") ? ManualAddressInput.Text : $"0x{ManualAddressInput.Text}")}", 16);
                    BestGuessSetByTextbox = true;
                }
                catch
                {
                    ManualAddressInput.Text = "Invalid conversion from hex address, using best guess instead.";
                    BestGuessSetByTextbox = false;
                }
                
            }
            if (!BestGuessSetByTextbox)
            {
                BestGuess = TestAddresses.Where(c => c != -1).GroupBy(c => c).OrderByDescending(c => c.Count()).Select(c => c.Key).First();
            }
            
            if(MessageBox.Show($"Pressing Yes will attempt to issue a logout to Stars using base address {BestGuess.ToString("x")}. Use this to test to ENSURE YOU WILL STAY LOGGED IN once the hand monitor is started.\nSend now? (Make sure you are currenty logged in)", "Logout Test", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                HookProcess.TestLogoutHook(BestGuess);
                if (MessageBox.Show("Were you logged out? Clicking Yes will set the result and close this dialog. Select No to try again.", "Success?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SuccessAddress = BestGuess;
                    CloseTestButton_Click(null, null);
                }
                if(!BestGuessSetByTextbox)
                {
                    ManualAddressInput.Clear();
                }
            }
        }
    }
}
