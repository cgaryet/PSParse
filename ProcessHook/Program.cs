﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MemoryScanner
{
    class Program
    {
        // REQUIRED CONSTS
        const int PROCESS_QUERY_INFORMATION = 0x0400;
        const int MEM_COMMIT = 0x00001000;
        const int PAGE_READWRITE = 0x04;
        const int PROCESS_WM_READ = 0x0010;

        // REQUIRED METHODS
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);

        // REQUIRED STRUCTS
        public struct MEMORY_BASIC_INFORMATION
        {
            public int BaseAddress;
            public int AllocationBase;
            public int AllocationProtect;
            public int RegionSize;
            public int State;
            public int Protect;
            public int lType;
        }

        public struct SYSTEM_INFO
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

        public static void Main()
        {
            // getting minimum & maximum address
            SYSTEM_INFO sys_info = new SYSTEM_INFO();
            GetSystemInfo(out sys_info);

            IntPtr proc_min_address = IntPtr.Zero;
            long proc_min_address_l = 0;
            long proc_max_address_l = 0x7fff0000;

            // notepad better be runnin'
            Process process = Process.GetProcessesByName("pokerstars")[0];

            // opening the process with desired access level
            IntPtr processHandle = OpenProcess(PROCESS_QUERY_INFORMATION | PROCESS_WM_READ, false, process.Id);

            //StreamWriter sw = new StreamWriter("dump.txt");

            // this will store any information we get from VirtualQueryEx()
            MEMORY_BASIC_INFORMATION mem_basic_info = new MEMORY_BASIC_INFORMATION();

            int bytesRead = 0;  // number of bytes read with ReadProcessMemory
            
            while (proc_min_address_l < proc_max_address_l)
            {
                //Console.WriteLine(proc_min_address_l);
                // 28 = sizeof(MEMORY_BASIC_INFORMATION)
                VirtualQueryEx(processHandle, proc_min_address, out mem_basic_info, 28);

                // if this memory chunk is accessible
                if (mem_basic_info.Protect == PAGE_READWRITE && mem_basic_info.State == MEM_COMMIT)
                {
                    byte[] buffer = new byte[mem_basic_info.RegionSize];

                    // read everything in the buffer above
                    ReadProcessMemory((int)processHandle, mem_basic_info.BaseAddress, buffer, mem_basic_info.RegionSize, ref bytesRead);

                    //Console.WriteLine("0x{0}", (mem_basic_info.BaseAddress).ToString("X"));

                    // then output this in the file
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < mem_basic_info.RegionSize; i++)
                    {
                        sb.Append((char)buffer[i]);
                    }
                    string memBlockString = sb.ToString();
                    Regex handMatch = new Regex("PokerStars Home Game Hand #(\\d{12}):");
                    MatchCollection Matches = handMatch.Matches(memBlockString);
                    foreach(Match match in Matches)
                    {
                        if (match.Success)
                        {
                            sb.Clear();
                            int idx = match.Index;
                            while(memBlockString[idx] != '\0')
                            {
                                sb.Append(memBlockString[idx]);
                                idx++;
                            }
                            //Console.Write(sb.ToString());     
                        }
                    }
                }

                // move to the next memory chunk
                proc_min_address_l += mem_basic_info.RegionSize;
                proc_min_address = new IntPtr(proc_min_address_l);
            }
            //sw.Close();

            Console.ReadLine();
        }
    }
}
