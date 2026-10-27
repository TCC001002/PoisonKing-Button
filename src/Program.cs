using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WinProfile
{
    internal static class Native
    {
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const uint LLKHF_INJECTED = 0x10;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_SCANCODE = 0x0008;
        public const uint MAPVK_VK_TO_VSC = 0;
        public const int GWL_EXSTYLE = -20;
        public const long WS_EX_TOOLWINDOW = 0x00000080L;
        public const uint GW_OWNER = 4;

        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public int type;
            public InputUnion U;
        }

        /// <summary>联合体必须与 Win32 对齐：x64 下取 MOUSEINPUT 的 32 字节，INPUT 总长 40</summary>
        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MOUSEINPUT mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HARDWAREINPUT hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT { public int Left, Top, Right, Bottom; }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint pid);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(uint access, bool inherit, int pid);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool QueryFullProcessImageName(IntPtr h, uint flags, StringBuilder name, ref uint size);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool OpenProcessToken(IntPtr process, uint access, out IntPtr token);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation(IntPtr token, int infoClass, out int info, int len, out int retLen);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr h);

        /// <summary>跨权限级别取进程名（管理员进程也能读）</summary>
        public static string QueryProcessName(int pid)
        {
            IntPtr h = OpenProcess(0x1000, false, pid); // PROCESS_QUERY_LIMITED_INFORMATION
            if (h == IntPtr.Zero) return "";
            try
            {
                StringBuilder sb = new StringBuilder(1024);
                uint size = 1024;
                if (!QueryFullProcessImageName(h, 0, sb, ref size)) return "";
                string s = sb.ToString(0, (int)size);
                int i = s.LastIndexOf('\\');
                if (i >= 0) s = s.Substring(i + 1);
                if (s.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)) s = s.Substring(0, s.Length - 4);
                return s;
            }
            finally { CloseHandle(h); }
        }

        /// <summary>目标进程是否以管理员权限运行（读不到返回 false）</summary>
        public static bool IsProcessElevated(int pid)
        {
            IntPtr h = OpenProcess(0x1000, false, pid);
            if (h == IntPtr.Zero) return false;
            try
            {
                IntPtr tok;
                if (!OpenProcessToken(h, 0x8, out tok)) return false; // TOKEN_QUERY
                try
                {
                    int elev, retLen;
                    if (!GetTokenInformation(tok, 20, out elev, 4, out retLen)) return false; // TokenElevation
                    return elev != 0;
                }
                finally { CloseHandle(tok); }
            }
            finally { CloseHandle(h); }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public static int ForegroundPid()
        {
            IntPtr h = GetForegroundWindow();
            if (h == IntPtr.Zero) return 0;
            uint pid;
            GetWindowThreadProcessId(h, out pid);
            return (int)pid;
        }

        private static bool IsExtendedKey(int vk)
        {
            // 方向键、Home/End/PgUp/PgDn/Insert/Delete、小键盘除号与回车
            switch (vk)
            {
                case 0x21: case 0x22: case 0x23: case 0x24:
                case 0x25: case 0x26: case 0x27: case 0x28:
                case 0x2D: case 0x2E:
                case 0x2F: case 0x90: case 0x6F:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>前台注入：按键进入系统输入队列，只会送给当前前台窗口</summary>
        public static bool SendKeyPress(int vk, bool scan)
        {
            INPUT[] arr = new INPUT[2];
            arr[0] = MakeInput(vk, scan, false);
            arr[1] = MakeInput(vk, scan, true);
            uint r = SendInput(2, arr, Marshal.SizeOf(typeof(INPUT)));
            return r == 2;
        }

        private static INPUT MakeInput(int vk, bool scan, bool up)
        {
            INPUT inp = new INPUT();
            inp.type = 1; // INPUT_KEYBOARD
            inp.U.ki.wVk = (ushort)(scan ? 0 : vk);
            inp.U.ki.wScan = (ushort)(scan ? MapVirtualKey((uint)vk, MAPVK_VK_TO_VSC) : 0);
            inp.U.ki.dwFlags = (scan ? KEYEVENTF_SCANCODE : 0) | (up ? KEYEVENTF_KEYUP : 0);
            inp.U.ki.time = 0;
            inp.U.ki.dwExtraInfo = (IntPtr)0x5A5A0001; // 标记本工具注入的按键
            return inp;
        }

        /// <summary>后台投递：把按键消息直接塞进目标窗口的消息队列，不论该窗口是否在前台</summary>
        public static bool PostKeyPress(IntPtr hwnd, int vk, bool scan)
        {
            if (hwnd == IntPtr.Zero || !IsWindow(hwnd)) return false;
            uint sc = MapVirtualKey((uint)vk, MAPVK_VK_TO_VSC);
            int baseLp = 1 | (((int)sc & 0xFF) << 16);
            if (IsExtendedKey(vk)) baseLp |= (1 << 24);
            int lpUp = baseLp | (1 << 30) | (1 << 31);
            bool a = PostMessage(hwnd, WM_KEYDOWN, (IntPtr)vk, (IntPtr)baseLp);
            bool b = PostMessage(hwnd, WM_KEYUP, (IntPtr)vk, (IntPtr)lpUp);
            return a && b;
        }

        /// <summary>按 PID 找到该进程的主窗口（面积最大、可见、非工具窗）</summary>
        public static IntPtr FindMainWindow(int pid)
        {
            IntPtr best = IntPtr.Zero;
            long bestArea = -1;
            EnumWindows(delegate(IntPtr h, IntPtr l)
            {
                uint p;
                GetWindowThreadProcessId(h, out p);
                if ((int)p != pid) return true;
                if (!IsWindowVisible(h)) return true;
                if (GetWindow(h, GW_OWNER) != IntPtr.Zero) return true;
                long ex = (long)GetWindowLongPtr(h, GWL_EXSTYLE);
                if ((ex & WS_EX_TOOLWINDOW) != 0) return true;
                RECT r;
                if (!GetWindowRect(h, out r)) return true;
                long area = (long)(r.Right - r.Left) * (long)(r.Bottom - r.Top);
                if (area > bestArea) { bestArea = area; best = h; }
                return true;
            }, IntPtr.Zero);
            return best;
        }
    }

    /// <summary>全局低级键盘钩子</summary>
    public class KeyHook : IDisposable
    {
        private IntPtr hookId = IntPtr.Zero;
        private Native.LowLevelKeyboardProc proc;
        private IntPtr marker = (IntPtr)0x5A5A0001;

        /// <summary>返回 true 表示吞掉该按键；v 为虚拟键码，down 表示按下</summary>
        public Func<int, bool, bool> Handler;

        public bool Install()
        {
            proc = HookCallback;
            using (Process cur = Process.GetCurrentProcess())
            using (ProcessModule mod = cur.MainModule)
            {
                hookId = Native.SetWindowsHookEx(Native.WH_KEYBOARD_LL, proc, Native.GetModuleHandle(mod.ModuleName), 0);
            }
            return hookId != IntPtr.Zero;
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                Native.KBDLLHOOKSTRUCT k = (Native.KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(Native.KBDLLHOOKSTRUCT));
                bool injected = (k.flags & (int)Native.LLKHF_INJECTED) != 0 || k.dwExtraInfo == marker;
                if (!injected && Handler != null)
                {
                    int msg = wParam.ToInt32();
                    bool down = (msg == Native.WM_KEYDOWN || msg == Native.WM_SYSKEYDOWN);
                    bool up = (msg == Native.WM_KEYUP || msg == Native.WM_SYSKEYUP);
                    if (down || up)
                    {
                        try
                        {
                            if (Handler(k.vkCode, down)) return (IntPtr)1;
                        }
                        catch { }
                    }
                }
            }
            return Native.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        public void Dispose()
        {
            if (hookId != IntPtr.Zero)
            {
                Native.UnhookWindowsHookEx(hookId);
                hookId = IntPtr.Zero;
            }
        }
    }

    /// <summary>
    /// 连发引擎（单窗口实例，支持多组绑定）。每个窗口一个独立实例，互不影响：
    /// 目标窗口不在前台时，改用窗口消息投递，继续运行。
    /// </summary>
    public class Repeater : IDisposable
    {
        private class Slot
        {
            public int vk;
            public int interval;
            public Timer timer;
        }

        private readonly object gate = new object();
        private readonly List<Slot> slots = new List<Slot>();
        private int pid;
        private int sendMode;
        private bool scan;
        private bool pauseWhenMinimized;
        private IntPtr hwnd = IntPtr.Zero;

        public bool Running { get; private set; }
        public int TargetPid { get { return pid; } }
        public long SentCount { get; private set; }
        public string LastMode { get; private set; }
        public string LastError { get; private set; }

        /// <summary>bindings：虚拟键码 → 间隔毫秒</summary>
        public void Start(int targetPid, List<KeyValuePair<int, int>> bindings, int mode, bool useScan, bool pauseMin)
        {
            lock (gate)
            {
                StopInternal();
                pid = targetPid;
                sendMode = mode;
                scan = useScan;
                pauseWhenMinimized = pauseMin;
                hwnd = Native.FindMainWindow(targetPid);
                SentCount = 0;
                LastError = null;
                LastMode = "";

                foreach (KeyValuePair<int, int> b in bindings)
                {
                    if (b.Key == 0) continue;
                    Slot s = new Slot();
                    s.vk = b.Key;
                    s.interval = b.Value < 10 ? 10 : b.Value;
                    slots.Add(s);
                }
                if (slots.Count == 0) return;

                Running = true;
                foreach (Slot s in slots)
                    s.timer = new Timer(Tick, s, s.interval, s.interval);
            }
        }

        private void Tick(object state)
        {
            Slot s = (Slot)state;
            lock (gate)
            {
                if (!Running) return;

                if (hwnd == IntPtr.Zero || !Native.IsWindow(hwnd))
                {
                    hwnd = Native.FindMainWindow(pid);
                    if (hwnd == IntPtr.Zero)
                    {
                        LastError = "窗口已关闭";
                        StopInternal();
                        return;
                    }
                }

                if (pauseWhenMinimized && Native.IsIconic(hwnd)) return;

                bool foreground = Native.ForegroundPid() == pid;
                try
                {
                    bool ok;
                    if (sendMode == 2)
                    {
                        ok = Native.PostKeyPress(hwnd, s.vk, scan);
                        LastMode = "后台投递";
                        if (!ok) { LastError = "消息投递失败（窗口拒绝或已失效）"; return; }
                    }
                    else if (sendMode == 1)
                    {
                        if (!foreground) return;      // 仅前台模式：窗口不在前台就不发
                        ok = Native.SendKeyPress(s.vk, scan);
                        LastMode = "前台注入";
                        if (!ok) { LastError = "前台注入被拒绝（可能权限不足或被安全软件拦截）"; return; }
                    }
                    else
                    {
                        if (foreground)
                        {
                            ok = Native.SendKeyPress(s.vk, scan);
                            LastMode = "前台注入";
                        }
                        else
                        {
                            ok = Native.PostKeyPress(hwnd, s.vk, scan);
                            LastMode = "后台投递";
                        }
                        if (!ok) { LastError = "发送失败（权限或窗口状态）"; return; }
                    }
                    LastError = null;
                    SentCount++;
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                }
            }
        }

        private void StopInternal()
        {
            Running = false;
            foreach (Slot s in slots)
            {
                if (s.timer != null)
                {
                    s.timer.Dispose();
                    s.timer = null;
                }
            }
            slots.Clear();
        }

        public void Stop()
        {
            lock (gate) { StopInternal(); }
        }

        public void Dispose() { Stop(); }
    }
}
