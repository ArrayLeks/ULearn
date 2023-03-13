using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {      
        private readonly int level;
        private readonly string name;
        private readonly List<Timer> timers;
        private bool disposedValue;
        private Stopwatch stopwatch;
        private readonly StringWriter writer;

        public Timer(StringWriter writer, string name, int level)
        {
            this.level = level;
            this.writer = writer;
            this.name = name;
            timers = new List<Timer>();
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        ~Timer()
        {
            disposedValue = false;
        }

        public Timer StartChildTimer(string name)
        {
            var newTimer = new Timer(new StringWriter(), name, level + 1);
            timers.Add(newTimer);

            return newTimer;
        }

        public static Timer Start(StringWriter writer, string name = "*")
        {
            return new Timer(writer, name, 0);
        }

        private static string FormatReportLine(string timerName, int level, long value)
        {
            var intro = new string(' ', level * 4) + timerName;
            return $"{intro,-20}: {value}\n";
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    stopwatch.Stop();
                    writer.Write(FormatReportLine(name, level, stopwatch.ElapsedMilliseconds));
                    foreach (var t in timers)
                        writer.Write(t.writer);
                    if (timers.Count != 0)
                        writer.Write(FormatReportLine("Rest", level + 1, stopwatch.ElapsedMilliseconds - Rest(this)));
                }
                disposedValue = true;
            }
        }

        public long Rest(Timer timer)
        {
            long result = 0;

            foreach (var t in timer.timers)
            {
                result += t.stopwatch.ElapsedMilliseconds;
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
