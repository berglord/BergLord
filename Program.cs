using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BotProject
{
    static class Program
    {
        public static Thread CaptureThread;
        public static Thread ImageReadThread;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DisplayForm());
        }

        public static void StartThread(Thread thread)
        {
            thread.Start();
        }
    }
}
