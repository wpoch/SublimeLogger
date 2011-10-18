using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SublimeLogger
{
    public class LoggerImpl : Logger
    {
        private Stopwatch _stopwatch;

        #region Overrides of Logger

        public override void Initialize(IEventSource eventSource)
        {
            eventSource.BuildStarted += BuildStarted;
            eventSource.BuildFinished += BuildFinished;
            eventSource.ErrorRaised += ErrorRaised;
        }

        void BuildFinished(object sender, BuildFinishedEventArgs e)
        {
            _stopwatch.Stop();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Compilation done in {0}ms.", _stopwatch.ElapsedMilliseconds);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            if (e.Succeeded)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Compilation SUCCEEDED.");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Compilation FAILED!.");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        void BuildStarted(object sender, BuildStartedEventArgs e)
        {
            _stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Initializing the compilation.");
            Console.WriteLine();
            Console.WriteLine();
        }

        void ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            var path = Path.GetDirectoryName(e.ProjectFile);
            var fullPath = Path.Combine(path, e.File);
            Console.WriteLine(" *{0}({1},{2}): {3}.", fullPath, e.LineNumber, e.ColumnNumber, e.Message);
        }

        #endregion
    }
}
