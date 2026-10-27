using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace WinProfile
{
    /// <summary>一组连发绑定：按键 + 独立间隔</summary>
    public class KeyBinding
    {
        public string Key { get; set; }
        public int IntervalMs { get; set; }

        public KeyBinding() { }
        public KeyBinding(string key, int interval) { Key = key; IntervalMs = interval; }
    }

    /// <summary>单个窗口的按键方案</summary>
    public class Profile
    {
        public int Pid { get; set; }
        public string WindowTitle { get; set; }
        public string ProcessName { get; set; }
        public bool Enabled { get; set; }
        public string ToggleKey { get; set; }

        public string SendKey { get; set; }
        public int IntervalMs { get; set; }

        public bool UseScanCode { get; set; }
        public int SendMode { get; set; }
        public bool PauseWhenMinimized { get; set; }
        public List<KeyBinding> Bindings { get; set; }

        public Profile()
        {
            Enabled = true;
            ProcessName = "JX3ClientX3DX64";
            ToggleKey = "F1";
            SendKey = "";
            IntervalMs = 100;
            UseScanCode = true;
            SendMode = 0;
            PauseWhenMinimized = false;
            WindowTitle = "";
            Bindings = new List<KeyBinding>();
        }

        /// <summary>取有效绑定；旧配置（只有 SendKey）自动迁移</summary>
        public List<KeyBinding> EffectiveBindings()
        {
            List<KeyBinding> list = new List<KeyBinding>();
            if (Bindings != null)
            {
                foreach (KeyBinding b in Bindings)
                {
                    if (b == null || string.IsNullOrEmpty(b.Key)) continue;
                    if (VkMap.Parse(b.Key) == 0) continue;
                    int ms = b.IntervalMs < 10 ? 10 : b.IntervalMs;
                    list.Add(new KeyBinding(VkMap.Normalize(b.Key), ms));
                }
            }
            if (list.Count == 0 && !string.IsNullOrEmpty(SendKey) && VkMap.Parse(SendKey) != 0)
            {
                int ms = IntervalMs >= 10 ? IntervalMs : 100;
                list.Add(new KeyBinding(VkMap.Normalize(SendKey), ms));
            }
            return list;
        }

        public string BindingsText()
        {
            List<KeyBinding> list = EffectiveBindings();
            if (list.Count == 0) return "—";
            List<string> parts = new List<string>();
            foreach (KeyBinding b in list) parts.Add(b.Key + "@" + b.IntervalMs);
            string s = string.Join(", ", parts.ToArray());
            if (s.Length > 46) s = s.Substring(0, 44) + "…";
            return s;
        }
    }

    public class AppConfig
    {
        public List<Profile> Profiles { get; set; }
        public bool ShowOnlyGame { get; set; }
        public int EmergencyStopVk { get; set; }
        public bool ToggleAnywhere { get; set; }
        public string GlobalPauseKey { get; set; }
        public string ThemeName { get; set; }

        public AppConfig()
        {
            Profiles = new List<Profile>();
            ShowOnlyGame = true;
            EmergencyStopVk = 0x7B;
            ToggleAnywhere = true;
            GlobalPauseKey = "F8";
            ThemeName = "水墨蓝白";
        }
    }

    /// <summary>
    /// 内置轻量 JSON 引擎（读写本程序配置专用），不依赖 System.Web.Extensions，
    /// 保证 exe 拷贝到任何装有 .NET Framework 4.x 的机器都能直接运行。
    /// JSON 键名以字符串字面量固定，与符号重命名无关，配置文件格式向前兼容。
    /// </summary>
    internal static class MiniJson
    {
        public static object Parse(string text)
        {
            if (text == null) return null;
            int pos = 0;
            return ParseValue(text, ref pos);
        }

        private static void SkipWs(string s, ref int i)
        {
            while (i < s.Length)
            {
                char c = s[i];
                if (c == ' ' || c == '\t' || c == '\r' || c == '\n') i++;
                else break;
            }
        }

        private static object ParseValue(string s, ref int i)
        {
            SkipWs(s, ref i);
            if (i >= s.Length) return null;
            char c = s[i];
            if (c == '{') return ParseObj(s, ref i);
            if (c == '[') return ParseArr(s, ref i);
            if (c == '"') return ParseStr(s, ref i);
            if (Match(s, i, "true")) { i += 4; return true; }
            if (Match(s, i, "false")) { i += 5; return false; }
            if (Match(s, i, "null")) { i += 4; return null; }
            return ParseNum(s, ref i);
        }

        private static bool Match(string s, int i, string word)
        {
            if (i + word.Length > s.Length) return false;
            return string.CompareOrdinal(s, i, word, 0, word.Length) == 0;
        }

        private static Dictionary<string, object> ParseObj(string s, ref int i)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            i++; // {
            SkipWs(s, ref i);
            if (i < s.Length && s[i] == '}') { i++; return d; }
            while (i < s.Length)
            {
                SkipWs(s, ref i);
                string key = ParseStr(s, ref i);
                SkipWs(s, ref i);
                if (i < s.Length && s[i] == ':') i++;
                object val = ParseValue(s, ref i);
                d[key] = val;
                SkipWs(s, ref i);
                if (i < s.Length && s[i] == ',') { i++; continue; }
                if (i < s.Length && s[i] == '}') { i++; break; }
                break;
            }
            return d;
        }

        private static List<object> ParseArr(string s, ref int i)
        {
            List<object> list = new List<object>();
            i++; // [
            SkipWs(s, ref i);
            if (i < s.Length && s[i] == ']') { i++; return list; }
            while (i < s.Length)
            {
                object val = ParseValue(s, ref i);
                list.Add(val);
                SkipWs(s, ref i);
                if (i < s.Length && s[i] == ',') { i++; continue; }
                if (i < s.Length && s[i] == ']') { i++; break; }
                break;
            }
            return list;
        }

        private static string ParseStr(string s, ref int i)
        {
            StringBuilder sb = new StringBuilder();
            i++; // "
            while (i < s.Length)
            {
                char c = s[i];
                if (c == '"') { i++; break; }
                if (c == '\\' && i + 1 < s.Length)
                {
                    i++;
                    char e = s[i];
                    switch (e)
                    {
                        case '"': sb.Append('"'); i++; break;
                        case '\\': sb.Append('\\'); i++; break;
                        case '/': sb.Append('/'); i++; break;
                        case 'b': sb.Append('\b'); i++; break;
                        case 'f': sb.Append('\f'); i++; break;
                        case 'n': sb.Append('\n'); i++; break;
                        case 'r': sb.Append('\r'); i++; break;
                        case 't': sb.Append('\t'); i++; break;
                        case 'u':
                            if (i + 4 < s.Length)
                            {
                                int code;
                                if (int.TryParse(s.Substring(i + 1, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out code))
                                {
                                    sb.Append((char)code);
                                    i += 5;
                                }
                                else { sb.Append(e); i++; }
                            }
                            else { sb.Append(e); i++; }
                            break;
                        default: sb.Append(e); i++; break;
                    }
                }
                else
                {
                    sb.Append(c);
                    i++;
                }
            }
            return sb.ToString();
        }

        private static object ParseNum(string s, ref int i)
        {
            int start = i;
            while (i < s.Length)
            {
                char c = s[i];
                if ((c >= '0' && c <= '9') || c == '-' || c == '+' || c == '.' || c == 'e' || c == 'E') i++;
                else break;
            }
            double d;
            if (double.TryParse(s.Substring(start, i - start), NumberStyles.Float, CultureInfo.InvariantCulture, out d))
                return d;
            return null;
        }

        public static string AsStr(object o, string def)
        {
            string s = o as string;
            return s == null ? def : s;
        }

        public static int AsInt(object o, int def)
        {
            if (o is double) return (int)Convert.ToDouble(o);
            if (o is bool) return (bool)o ? 1 : 0;
            return def;
        }

        public static bool AsBool(object o, bool def)
        {
            if (o is bool) return (bool)o;
            if (o is double) return Convert.ToDouble(o) != 0;
            return def;
        }

        /// <summary>序列化时转义字符串</summary>
        public static string Esc(string s)
        {
            if (s == null) return "";
            StringBuilder sb = new StringBuilder(s.Length + 8);
            foreach (char c in s)
            {
                switch (c)
                {
                    case '"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    default:
                        if (c < ' ') sb.Append("\\u").Append(((int)c).ToString("x4"));
                        else sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }
    }

    public static class Store
    {
        private static string resolvedPath;
        private static bool resolved;
        private static bool usingAppData;

        /// <summary>实际使用的配置文件路径；程序目录不可写时回退到 %APPDATA%</summary>
        public static string ConfigPath
        {
            get { ResolvePath(); return resolvedPath; }
        }

        /// <summary>配置是否回退保存在 %APPDATA%（程序目录不可写）</summary>
        public static bool UsingAppData
        {
            get { ResolvePath(); return usingAppData; }
        }

        private static string ExeConfigPath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json"); }
        }

        private static string AppDataConfigPath
        {
            get
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "PoisonKing");
                try { Directory.CreateDirectory(dir); } catch { }
                return Path.Combine(dir, "config.json");
            }
        }

        /// <summary>目录是否可写（探测法：写删一个临时文件）</summary>
        private static bool CanWriteTo(string dir)
        {
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir)) return false;
            try
            {
                string probe = Path.Combine(dir, ".wp_write_test.tmp");
                File.WriteAllText(probe, "ok");
                File.Delete(probe);
                return true;
            }
            catch { return false; }
        }

        private static void ResolvePath()
        {
            if (resolved) return;
            resolved = true;
            string exePath = ExeConfigPath;
            // 已有旧配置的目录优先沿用，避免"搬家"让用户以为配置丢失
            if (File.Exists(exePath) || File.Exists(exePath + ".bak") ||
                CanWriteTo(Path.GetDirectoryName(exePath)))
            {
                resolvedPath = exePath;
                usingAppData = false;
                return;
            }
            resolvedPath = AppDataConfigPath;
            usingAppData = true;
        }

        public static AppConfig Load()
        {
            // 依次尝试：主路径 → 主路径.bak → 程序目录历史配置 → 默认
            AppConfig c = TryLoad(ConfigPath);
            if (c != null) return c;
            AppConfig b = TryLoad(ConfigPath + ".bak");
            if (b != null) return b;
            if (usingAppData)
            {
                // 程序目录曾有旧配置但当前写不进去时，只读沿用历史配置
                AppConfig o = TryLoad(ExeConfigPath);
                if (o == null) o = TryLoad(ExeConfigPath + ".bak");
                if (o != null) return o;
            }
            return new AppConfig();
        }

        private static AppConfig TryLoad(string path)
        {
            try
            {
                if (!File.Exists(path)) return null;
                string text = File.ReadAllText(path, Encoding.UTF8);
                object root = MiniJson.Parse(text);
                AppConfig cfg = new AppConfig();
                Dictionary<string, object> d = root as Dictionary<string, object>;
                if (d == null) return null;

                object v;
                if (d.TryGetValue("ShowOnlyGame", out v)) cfg.ShowOnlyGame = MiniJson.AsBool(v, cfg.ShowOnlyGame);
                if (d.TryGetValue("EmergencyStopVk", out v)) cfg.EmergencyStopVk = MiniJson.AsInt(v, cfg.EmergencyStopVk);
                if (d.TryGetValue("ToggleAnywhere", out v)) cfg.ToggleAnywhere = MiniJson.AsBool(v, cfg.ToggleAnywhere);
                if (d.TryGetValue("GlobalPauseKey", out v)) cfg.GlobalPauseKey = MiniJson.AsStr(v, cfg.GlobalPauseKey);
                if (d.TryGetValue("ThemeName", out v)) cfg.ThemeName = MiniJson.AsStr(v, cfg.ThemeName);

                object pl;
                if (d.TryGetValue("Profiles", out pl))
                {
                    List<object> arr = pl as List<object>;
                    if (arr != null)
                    {
                        foreach (object o in arr)
                        {
                            Profile p = ReadProfile(o as Dictionary<string, object>);
                            if (p != null) cfg.Profiles.Add(p);
                        }
                    }
                }
                return cfg;
            }
            catch
            {
                return null;
            }
        }

        private static Profile ReadProfile(Dictionary<string, object> d)
        {
            if (d == null) return null;
            Profile p = new Profile();
            object v;
            if (d.TryGetValue("Pid", out v)) p.Pid = MiniJson.AsInt(v, 0);
            if (d.TryGetValue("WindowTitle", out v)) p.WindowTitle = MiniJson.AsStr(v, "");
            if (d.TryGetValue("ProcessName", out v)) p.ProcessName = MiniJson.AsStr(v, "JX3ClientX3DX64");
            if (d.TryGetValue("Enabled", out v)) p.Enabled = MiniJson.AsBool(v, true);
            if (d.TryGetValue("ToggleKey", out v)) p.ToggleKey = MiniJson.AsStr(v, "F1");
            if (d.TryGetValue("SendKey", out v)) p.SendKey = MiniJson.AsStr(v, "");
            if (d.TryGetValue("IntervalMs", out v)) p.IntervalMs = MiniJson.AsInt(v, 100);
            if (d.TryGetValue("UseScanCode", out v)) p.UseScanCode = MiniJson.AsBool(v, true);
            if (d.TryGetValue("SendMode", out v)) p.SendMode = MiniJson.AsInt(v, 0);
            if (d.TryGetValue("PauseWhenMinimized", out v)) p.PauseWhenMinimized = MiniJson.AsBool(v, false);

            object bl;
            if (d.TryGetValue("Bindings", out bl))
            {
                List<object> arr = bl as List<object>;
                if (arr != null)
                {
                    foreach (object o in arr)
                    {
                        Dictionary<string, object> bd = o as Dictionary<string, object>;
                        if (bd == null) continue;
                        KeyBinding b = new KeyBinding();
                        object kv;
                        if (bd.TryGetValue("Key", out kv)) b.Key = MiniJson.AsStr(kv, "");
                        if (bd.TryGetValue("IntervalMs", out kv)) b.IntervalMs = MiniJson.AsInt(kv, 100);
                        p.Bindings.Add(b);
                    }
                }
            }
            return p;
        }

        public static void Save(AppConfig cfg)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("{\"Profiles\":[");
                bool first = true;
                foreach (Profile p in cfg.Profiles)
                {
                    if (!first) sb.Append(',');
                    first = false;
                    sb.Append("{\"Pid\":").Append(p.Pid);
                    sb.Append(",\"WindowTitle\":\"").Append(MiniJson.Esc(p.WindowTitle)).Append('"');
                    sb.Append(",\"ProcessName\":\"").Append(MiniJson.Esc(p.ProcessName)).Append('"');
                    sb.Append(",\"Enabled\":").Append(p.Enabled ? "true" : "false");
                    sb.Append(",\"ToggleKey\":\"").Append(MiniJson.Esc(p.ToggleKey)).Append('"');
                    sb.Append(",\"SendKey\":\"").Append(MiniJson.Esc(p.SendKey)).Append('"');
                    sb.Append(",\"IntervalMs\":").Append(p.IntervalMs);
                    sb.Append(",\"UseScanCode\":").Append(p.UseScanCode ? "true" : "false");
                    sb.Append(",\"SendMode\":").Append(p.SendMode);
                    sb.Append(",\"PauseWhenMinimized\":").Append(p.PauseWhenMinimized ? "true" : "false");
                    sb.Append(",\"Bindings\":[");
                    bool fb = true;
                    if (p.Bindings != null)
                    {
                        foreach (KeyBinding b in p.Bindings)
                        {
                            if (!fb) sb.Append(',');
                            fb = false;
                            sb.Append("{\"Key\":\"").Append(MiniJson.Esc(b.Key)).Append("\",\"IntervalMs\":").Append(b.IntervalMs).Append('}');
                        }
                    }
                    sb.Append("]}");
                }
                sb.Append("]");
                sb.Append(",\"ShowOnlyGame\":").Append(cfg.ShowOnlyGame ? "true" : "false");
                sb.Append(",\"EmergencyStopVk\":").Append(cfg.EmergencyStopVk);
                sb.Append(",\"ToggleAnywhere\":").Append(cfg.ToggleAnywhere ? "true" : "false");
                sb.Append(",\"GlobalPauseKey\":\"").Append(MiniJson.Esc(cfg.GlobalPauseKey)).Append('"');
                sb.Append(",\"ThemeName\":\"").Append(MiniJson.Esc(cfg.ThemeName)).Append('"');
                sb.Append('}');
                // 原子写：先写临时文件，再整体替换，避免崩溃/断电留下半截配置
                string tmp = ConfigPath + ".tmp";
                File.WriteAllText(tmp, sb.ToString(), Encoding.UTF8);
                if (File.Exists(ConfigPath))
                {
                    string bak = ConfigPath + ".bak";
                    File.Copy(ConfigPath, bak, true);
                    File.Replace(tmp, ConfigPath, null);
                }
                else
                {
                    File.Move(tmp, ConfigPath);
                }
            }
            catch
            {
                // 配置写不进去（如只读目录）不影响运行
            }
        }
    }

    /// <summary>按键名 → 虚拟键码</summary>
    public static class VkMap
    {
        private static readonly Dictionary<string, int> Map = Build();

        private static Dictionary<string, int> Build()
        {
            Dictionary<string, int> m = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 1; i <= 24; i++) m["F" + i] = 0x6F + i;
            for (char c = 'A'; c <= 'Z'; c++) m[c.ToString()] = (int)c;
            for (char c = '0'; c <= '9'; c++) m[c.ToString()] = (int)c;
            m["SPACE"] = 0x20;
            m["TAB"] = 0x09;
            m["ENTER"] = 0x0D;
            m["ESC"] = 0x1B;
            m["HOME"] = 0x24;
            m["END"] = 0x23;
            m["PGUP"] = 0x21;
            m["PGDN"] = 0x22;
            m["INSERT"] = 0x2D;
            m["DELETE"] = 0x2E;
            m["BACKSPACE"] = 0x08;
            m["UP"] = 0x26;
            m["DOWN"] = 0x28;
            m["LEFT"] = 0x25;
            m["RIGHT"] = 0x27;
            m["`"] = 0xC0;
            m["-"] = 0xBD;
            m["="] = 0xBB;
            m["["] = 0xDB;
            m["]"] = 0xDD;
            m[";"] = 0xBA;
            m["'"] = 0xDE;
            m[","] = 0xBC;
            m["."] = 0xBE;
            m["/"] = 0xBF;
            m["\\"] = 0xDC;
            return m;
        }

        public static int Parse(string name)
        {
            if (string.IsNullOrEmpty(name)) return 0;
            string s = Normalize(name);
            if (s.Length == 0) return 0;

            int v;
            if (Map.TryGetValue(s, out v)) return v;

            if (s.StartsWith("0X") && s.Length > 2)
            {
                int hex;
                if (int.TryParse(s.Substring(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out hex))
                    return hex;
            }
            return 0;
        }

        public static string Normalize(string raw)
        {
            if (raw == null) return "";
            StringBuilder sb = new StringBuilder(raw.Length);
            foreach (char c in raw)
            {
                if (c == ' ' || c == '\t' || c == '\u3000') continue;
                char ch = c;
                if (c >= '\uFF01' && c <= '\uFF5E') ch = (char)(c - 0xFEE0);
                sb.Append(char.ToUpperInvariant(ch));
            }
            return sb.ToString();
        }

        public static string ToName(int vk)
        {
            foreach (KeyValuePair<string, int> kv in Map)
            {
                if (kv.Value == vk) return kv.Key;
            }
            return "0x" + vk.ToString("X2");
        }
    }
}
