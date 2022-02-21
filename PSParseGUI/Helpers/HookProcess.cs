using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static PSParseGUI.MainForm;

namespace PSParseGUI.Helpers
{
    internal static class HookProcess
    {
        // REQUIRED CONSTS
        private const int PROCESS_QUERY_INFORMATION = 0x0400;
        private const int MEM_COMMIT = 0x00001000;
        private const int PAGE_READWRITE = 0x04;
        private const int PROCESS_WM_READ = 0x0010;
        private const int PROCESS_WM_WRTIE = 0x0020;
        private const int PROCESS_WM_OPERATION = 0x0008;

        // REQUIRED METHODS
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);


        [DllImport("kernel32.dll")]
        private static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        // REQUIRED STRUCTS
        private struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        private struct SYSTEM_INFO
        {
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }

        private static Process Process;
        private static IntPtr ProcessHandle;
        private static string ModuleName;
        private static int ProcessID;

        public static bool Attach(IProgress<string> UpdateProgress = null, IProgress<string> ErrorProgress = null, string ProcessName = "pokerstars")
        {
            try
            {
                Process = Process.GetProcessesByName(ProcessName)[0];
                ProcessHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ | PROCESS_WM_WRTIE | PROCESS_WM_OPERATION, false, Process.Id);
                ModuleName = Process.MainModule.ModuleName;
                ProcessID = Process.Id;
                if(UpdateProgress != null)
                {
                    UpdateProgress.Report(String.Format("Successfully attached to process {0}: PID: {1}", ModuleName, ProcessID));
                }
                
                return true;
            }
            catch (Exception)
            {
                if (ErrorProgress != null)
                {
                    ErrorProgress.Report(String.Format("Couldn't find process {0}. Ensure PokerStars is running and try and attach again.", ProcessName));
                }
                return false;
            }
            
        }

        public static void TestLogoutHook(int BaseAddress)
        {
            WriteTimeoutMemory(true, BaseAddress);
        }

        public static void WriteTimeoutMemory(bool bTestLogout = false, int BaseAddress = -1)
        {
            int v = Environment.TickCount;
            if (bTestLogout)
            {
                v -= 19000000;
            }
            var val = BitConverter.GetBytes(v);
            WriteProcessMemory(ProcessHandle, new IntPtr(BaseAddress), val, 4, out _);
        }

        private static (byte[], int, int, int) GetMemory(long proc_min_address_l, long proc_max_address_l)
        {
            WriteTimeoutMemory();
            // getting minimum & maximum address
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();
            VirtualQueryEx(ProcessHandle, new IntPtr(proc_min_address_l), out mem_basic_info, 28);

            if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
            {
                byte[] buffer = new byte[mem_basic_info.RegionSize];
                int bytesRead = 0;
                // read everything in the buffer above
                ReadProcessMemory((int)ProcessHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);
                return (buffer, bytesRead, mem_basic_info.BaseAddress, mem_basic_info.RegionSize);       
            }
            return (null, -1, mem_basic_info.BaseAddress, mem_basic_info.RegionSize);
        }

        public static int GetLogoutMouseClick(int TickCount)
        {
            byte[] TickCountBytes = BitConverter.GetBytes(TickCount);
            Console.WriteLine($"[{TickCountBytes[0].ToString("x")},{TickCountBytes[1].ToString("x")},{TickCountBytes[2].ToString("x")},{TickCountBytes[3].ToString("x")}]");
            long proc_min_address_l = 0;
            long proc_max_address_l = 0x7fff0000;

            if (Process != null && !Process.HasExited)
            {
                while (proc_min_address_l < proc_max_address_l)
                {
                    if (Process.HasExited)
                    {
                        return -1;
                    }
                    (byte[] buffer, int bytesRead, int baseAddress, int regionSize) = GetMemory(proc_min_address_l, proc_max_address_l);
                    if (bytesRead != -1)
                    {
                        int maxFirstCharSlot = buffer.Length - TickCountBytes.Length + 1;
                        for (int i = 0; i < maxFirstCharSlot; i++)
                        {
                            // compare only first byte
                            if (buffer[i] != TickCountBytes[0] || proc_min_address_l < 16777216)
                            {
                                continue;
                            }

                            // found a match on first byte, now try to match rest of the pattern
                            bool bMatch = true;
                            for (int j = TickCountBytes.Length - 1; j >= 1; j--)
                            {
                                if (buffer[i + j] != TickCountBytes[j])
                                {
                                    bMatch = false;
                                    break;
                                }
                            }
                            if(bMatch)
                            {
                                return baseAddress + i;
                            }
                        }
                    }
                    proc_min_address_l += regionSize;
                }
            }
            return -1;
        }


        public static void GetHandHistoryDictionary(SortedList<long, string> InHandHistory, string InHandTitle, string InTableTitle)
        {
            Regex HandTitleRegex = new Regex($"{InHandTitle} Hand #(\\d{{12}}):");

            long proc_min_address_l = 0;
            long proc_max_address_l = 0x7fff0000;

            if (Process != null && !Process.HasExited)
            {
                while (proc_min_address_l < proc_max_address_l)
                {
                    if (Process.HasExited)
                    {
                        return;
                    }
                    (byte[] buffer, int bytesRead, int baseAddress, int regionSize) = GetMemory(proc_min_address_l, proc_max_address_l);
                    if (bytesRead != -1)
                    {
                        // then output this in the file
                        StringBuilder MemBlockSB = new StringBuilder();
                        for (int i = 0; i < regionSize; i++)
                        {
                            MemBlockSB.Append((char)buffer[i]);
                        }
                        string MemBlockValue = MemBlockSB.ToString();

                        MatchCollection Matches = HandTitleRegex.Matches(MemBlockValue);
                        foreach (Match Match in Matches)
                        {
                            if (Match.Success)
                            {
                                int StartIdx = Match.Index;
                                int BytesToRead = StartIdx;
                                long HandId = long.Parse(Match.Groups[1].Value);
                                while (MemBlockValue[BytesToRead] != '\0')
                                {
                                    BytesToRead++;
                                }
                                string Hand = MemBlockValue.Substring(StartIdx, BytesToRead - StartIdx);

                                bool bTableMatch = true;
                                if (!string.IsNullOrEmpty(InTableTitle))
                                {
                                    if (!Hand.Contains($"Table '{InTableTitle}'"))
                                    {
                                        bTableMatch = false;
                                    }
                                }

                                if (bTableMatch && Hand.Contains("*** SUMMARY ***"))
                                {
                                    if (!InHandHistory.ContainsKey(HandId))
                                    {
                                        InHandHistory.Add(HandId, Hand);
                                    }
                                }

                            }
                        }
                    }
                    // move to the next memory chunk
                    proc_min_address_l += regionSize;
                }

            }

        }

        public static void Detach()
        {
            if (ProcessHandle != IntPtr.Zero)
            {
                Process.Dispose();
                ProcessHandle = IntPtr.Zero;
                ProcessID = 0;
            }
        }
    }
}
