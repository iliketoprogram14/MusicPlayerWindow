using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MusicPlayerWindow
{
    public class KeyHandler
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        public LowLevelKeyboardProc _proc;
        public IntPtr _hookID = IntPtr.Zero;
        private const int MediaNext = 176;
        private const int MediaPrev = 177;
        private const int MediaPlayPause = 179;
        private MainWindow window;

        public KeyHandler() {
            _proc = HookCallback;
        }

        public void setWindow(MainWindow w) { window = w; }

        public IntPtr SetHook() {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule) {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, _proc,
                    GetModuleHandle(curModule.ModuleName), 0);
                return _hookID;
            }
        }

        public delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        public IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN) {
                int vkCode = Marshal.ReadInt32(lParam);
                EventArgs e = EventArgs.Empty;
                try {
                    switch (vkCode) {
                        case MediaNext:
                            Console.Out.WriteLine("Next pushed");
                            window.nextButton_Click(window, e);
                            break;
                        case MediaPrev:
                            Console.Out.WriteLine("Previous pushed");
                            window.prevButton_Click(this, EventArgs.Empty);
                            break;
                        case MediaPlayPause:
                            Console.Out.WriteLine("Play pushed");
                            window.playButton_Click(this, EventArgs.Empty);
                            break;
                        default:
                            break;
                    }
                }
                catch (System.NullReferenceException) {
                    Console.Out.WriteLine("Shit");
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
