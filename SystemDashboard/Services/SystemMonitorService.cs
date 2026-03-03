using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
namespace SystemDashboard.Services
{
    public class SystemMonitorService
    {
        private DateTime _lastCheckTime;
        private TimeSpan _lastTotalProcessorTime;
        private readonly int _processorCount = Environment.ProcessorCount;
        public SystemMonitorService()
        {
            _lastCheckTime = DateTime.UtcNow;
            _lastTotalProcessorTime = GetTotalProcessorTime();
        }

        public double GetCpuUsage()
        {
            var currentTime = DateTime.UtcNow;
            var currentTotalProcessorTime = GetTotalProcessorTime();

            var cpuUsedMs = (currentTotalProcessorTime - _lastTotalProcessorTime).TotalMilliseconds;
            var totalMsPassed = (currentTime - _lastCheckTime).TotalMilliseconds;

            _lastCheckTime = currentTime;
            _lastTotalProcessorTime = currentTotalProcessorTime;

            if (totalMsPassed == 0)
                return 0;

            var cpuUsageTotal = cpuUsedMs / (_processorCount * totalMsPassed) * 100;

            return Math.Round(cpuUsageTotal, 1);
        }

        private TimeSpan GetTotalProcessorTime()
        {
            long totalTicks = 0;

            foreach (var process in Process.GetProcesses())
            {
                try
                {
                    totalTicks += process.TotalProcessorTime.Ticks;
                }
                catch
                {
                    // Some system processes deny access — ignore them
                }
                finally
                {
                    process.Dispose();
                }
            }

            return TimeSpan.FromTicks(totalTicks);
        }

        public double GetRamUsage()
        {
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            GlobalMemoryStatusEx(memStatus);

            double usedMemory = memStatus.ullTotalPhys - memStatus.ullAvailPhys;
            double usagePercent = (usedMemory / memStatus.ullTotalPhys) * 100;

            return Math.Round(usagePercent, 1);
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MEMORYSTATUSEX
        {
            public uint dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        public int Property
        {
            get => default;
            set
            {
            }
        }
    }
}