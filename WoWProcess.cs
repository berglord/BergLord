using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace BotProject
{
    class WoWProcess
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        public static User32.Rect ProcessRectangle;
        public static WoWProcess Instance;
        public WoWProcess()
        {
            Instance = this;
            Process = Process.GetProcessesByName("WowClassic")[0];
            User32.GetWindowRect(Process.MainWindowHandle, ref ProcessRectangle);
            FocusProcess();
        }
        public static Process Process;
     
        public int Width
        {
            get { return ProcessRectangle.right - ProcessRectangle.left;}
        }
        public int Height
        {
            get { return ProcessRectangle.bottom - ProcessRectangle.top; }
        }

        public void GetProcess()
        {
            Process = Process.GetProcessesByName("WowClassic")[0];
            User32.GetWindowRect(Process.MainWindowHandle, ref ProcessRectangle);
        }

        public void GetProcessRect()
        {
            User32.GetWindowRect(Process.MainWindowHandle, ref ProcessRectangle);
        }

        public void FocusProcess()
        {
            SetForegroundWindow(Process.MainWindowHandle);
        }

        public class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        }
    }
}
