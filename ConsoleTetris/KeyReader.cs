using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTetris
{
    class KeyReader
    {
        private static Thread inputThread;
        private static AutoResetEvent getInput, gotInput;
        private static ConsoleKey input;

        static KeyReader()
        {
            getInput = new AutoResetEvent(false);
            gotInput = new AutoResetEvent(false);
            inputThread = new Thread(Reader)
            {
                IsBackground = true
            };
            inputThread.Start();
        }

        private static void Reader()
        {
            while (true)
            {
                getInput.WaitOne();
                input = Console.ReadKey().Key;
                gotInput.Set();
            }
        }

        public static bool TryReadKey(out ConsoleKey key, int timeOutMillisecs = Timeout.Infinite)
        {
            getInput.Set();
            bool success = gotInput.WaitOne(timeOutMillisecs);
            if (success)
                key = input;
            else
                key = ConsoleKey.DownArrow;
            return success;
        }
    }
}
