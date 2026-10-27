using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinProfile
{
    // ───────────────────────────── 主题 ─────────────────────────────

    /// <summary>一套界面配色</summary>
    public class ThemePalette
    {
        public string Name;
        public Color TBack;        // 窗体背景
        public Color TCard;        // 卡片背景
        public Color TCardBorder;  // 卡片描边
        public Color TBtnFill;     // 普通按钮填充
        public Color TBtnHover;    // 悬停填充
        public Color TBtnBorder;   // 按钮描边
        public Color TBtnText;     // 按钮文字
        public Color TText;        // 正文
        public Color TSub;         // 次要文字
        public Color TRun;         // 运行中高亮
        public Color TIdle;        // 停止灰
        public Color TAccFill;     // 强调按钮填充
        public Color TAccBorder;   // 强调按钮描边
        public Color TAccText;     // 强调按钮文字
        public Color TStatus;      // 状态条背景
        public Color THead;        // 分组标题
    }

    public static class Themes
    {
        public static readonly List<ThemePalette> All = Build();

        private static Color C(int rgb)
        {
            return Color.FromArgb((rgb >> 16) & 0xFF, (rgb >> 8) & 0xFF, rgb & 0xFF);
        }

        private static List<ThemePalette> Build()
        {
            List<ThemePalette> list = new List<ThemePalette>();

            // 热烈·朝霞（暖色浅底）
            ThemePalette t = new ThemePalette();
            t.Name = "热烈·朝霞";
            t.TBack = C(0xFFF4EC); t.TCard = C(0xFFFFFF); t.TCardBorder = C(0xF2D4C4);
            t.TBtnFill = C(0xFFE9DB); t.TBtnHover = C(0xFFD8C2); t.TBtnBorder = C(0xE8A787); t.TBtnText = C(0x8A3B1D);
            t.TText = C(0x5B2A16); t.TSub = C(0xA8765F); t.TRun = C(0xE0492A); t.TIdle = C(0xC0A79A);
            t.TAccFill = C(0xE0492A); t.TAccBorder = C(0xC13C21); t.TAccText = Color.White;
            t.TStatus = C(0xFFE9DB); t.THead = C(0xC13C21);
            list.Add(t);

            // 护眼·豆绿（低饱和浅绿底）
            t = new ThemePalette();
            t.Name = "护眼·豆绿";
            t.TBack = C(0xE8F1E0); t.TCard = C(0xF7FBF2); t.TCardBorder = C(0xC4D9B0);
            t.TBtnFill = C(0xE0EFD2); t.TBtnHover = C(0xCFE6BB); t.TBtnBorder = C(0x9ABE7F); t.TBtnText = C(0x33511F);
            t.TText = C(0x33511F); t.TSub = C(0x74885F); t.TRun = C(0x4C9A2A); t.TIdle = C(0xA2B392);
            t.TAccFill = C(0x578C36); t.TAccBorder = C(0x47742B); t.TAccText = Color.White;
            t.TStatus = C(0xDDEBD0); t.THead = C(0x47742B);
            list.Add(t);

            // 跟随系统（启动时读 Windows 深浅色设置）
            t = new ThemePalette();
            t.Name = "跟随系统";
            t.TBack = C(0xE9EDF2); t.TCard = Color.White; t.TCardBorder = C(0xC9D2DC);
            t.TBtnFill = C(0xE8EDF3); t.TBtnHover = C(0xD5DEE9); t.TBtnBorder = C(0x93A6BA); t.TBtnText = C(0x2B3A4A);
            t.TText = C(0x2B3A4A); t.TSub = C(0x7A8AA0); t.TRun = C(0x1E8A5F); t.TIdle = C(0x9DAAB8);
            t.TAccFill = C(0x3D6FB8); t.TAccBorder = C(0x305B96); t.TAccText = Color.White;
            t.TStatus = C(0xDCE4EE); t.THead = C(0x305B96);
            list.Add(t);

            // 黑暗·墨夜（深色低蓝光）
            t = new ThemePalette();
            t.Name = "黑暗·墨夜";
            t.TBack = C(0x14181F); t.TCard = C(0x1D232D); t.TCardBorder = C(0x2E3A4A);
            t.TBtnFill = C(0x262F3D); t.TBtnHover = C(0x333F51); t.TBtnBorder = C(0x44536A); t.TBtnText = C(0xD8E1EE);
            t.TText = C(0xD8E1EE); t.TSub = C(0x8494AB); t.TRun = C(0x3FCE93); t.TIdle = C(0x67758C);
            t.TAccFill = C(0x3D7BD9); t.TAccBorder = C(0x5488CE); t.TAccText = Color.White;
            t.TStatus = C(0x0F1218); t.THead = C(0x9FC2F2);
            list.Add(t);

            // 极客·终端（黑底荧光绿）
            t = new ThemePalette();
            t.Name = "极客·终端";
            t.TBack = C(0x0A0F0A); t.TCard = C(0x101810); t.TCardBorder = C(0x1F3A22);
            t.TBtnFill = C(0x142516); t.TBtnHover = C(0x1D371F); t.TBtnBorder = C(0x2F5A34); t.TBtnText = C(0x7CE38B);
            t.TText = C(0x7CE38B); t.TSub = C(0x4E8F58); t.TRun = C(0x39D353); t.TIdle = C(0x3A5F3E);
            t.TAccFill = C(0x238636); t.TAccBorder = C(0x2EA043); t.TAccText = C(0xEAF5EC);
            t.TStatus = C(0x060A06); t.THead = C(0x39D353);
            list.Add(t);

            return list;
        }

        /// <summary>读取 Windows 应用深浅色设置</summary>
        private static bool IsSystemDark()
        {
            try
            {
                Microsoft.Win32.RegistryKey k = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(
                    "Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize");
                if (k == null) return false;
                object v = k.GetValue("AppsUseLightTheme", 1);
                k.Close();
                return Convert.ToInt32(v) == 0;
            }
            catch { return false; }
        }

        public static ThemePalette Get(string name)
        {
            if (name == "跟随系统") return IsSystemDark() ? Get("黑暗·墨夜") : Get("护眼·豆绿");
            foreach (ThemePalette t in All)
            {
                if (t.Name == name) return t;
            }
            return All[0];
        }
    }

    // ───────────────────────────── 自绘控件 ─────────────────────────────

    /// <summary>抗锯齿圆角按钮</summary>
    public class RoundButton : Control
    {
        public Color FillCol = Color.White;
        public Color HoverCol = Color.White;
        public Color StrokeCol = Color.Gray;
        public Color LabelCol = Color.Black;

        private bool hover;

        public RoundButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Cursor = Cursors.Hand;
            Height = 30;
        }

        protected override void OnMouseEnter(EventArgs e) { hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { hover = false; Invalidate(); base.OnMouseLeave(e); }
        protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fill = !Enabled ? Color.FromArgb(150, FillCol) : (hover ? HoverCol : FillCol);
            int rad = Math.Min(8, Height / 2);
            using (GraphicsPath path = RoundPath(r, rad))
            {
                using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, path);
                using (Pen pen = new Pen(Enabled ? StrokeCol : Color.FromArgb(150, StrokeCol))) g.DrawPath(pen, path);
            }
            Color tc = !Enabled ? Color.FromArgb(150, LabelCol) : LabelCol;
            TextRenderer.DrawText(g, Text, Font, r, tc,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine);
        }

        internal static GraphicsPath RoundPath(Rectangle r, int rad)
        {
            GraphicsPath p = new GraphicsPath();
            p.AddArc(r.X, r.Y, rad * 2, rad * 2, 180, 90);
            p.AddArc(r.Right - rad * 2, r.Y, rad * 2, rad * 2, 270, 90);
            p.AddArc(r.Right - rad * 2, r.Bottom - rad * 2, rad * 2, rad * 2, 0, 90);
            p.AddArc(r.X, r.Bottom - rad * 2, rad * 2, rad * 2, 90, 90);
            p.CloseFigure();
            return p;
        }
    }

    /// <summary>顶部横幅（内嵌图片 + 渐变遮罩 + 标题）</summary>
    public class BannerPanel : Panel
    {
        public Image SkinImage;
        public ThemePalette Palette;
        public string VersionText = "v1.0";

        public BannerPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Dock = DockStyle.Top;
            Height = 104;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = ClientRectangle;
            if (SkinImage != null) g.DrawImage(SkinImage, r);
            else g.Clear(Palette != null ? Palette.TBack : SystemColors.Control);

            Color scrim = Palette != null ? Palette.TBack : Color.White;
            using (LinearGradientBrush b = new LinearGradientBrush(r,
                Color.FromArgb(215, scrim), Color.FromArgb(30, scrim), 0f))
            {
                g.FillRectangle(b, r);
            }

            if (Palette != null)
            {
                string big = "PoisonKing按键辅助";
                string small = "多窗口并发 · 后台继续运行 · " + VersionText;
                Font f1 = new Font("Microsoft YaHei UI", 15.5F, FontStyle.Bold);
                Font f2 = new Font("Microsoft YaHei UI", 9F);
                try
                {
                    TextRenderer.DrawText(g, big, f1, new Point(18, 22), Palette.THead,
                        TextFormatFlags.Left | TextFormatFlags.SingleLine);
                    TextRenderer.DrawText(g, small, f2, new Point(21, 58), Palette.TSub,
                        TextFormatFlags.Left | TextFormatFlags.SingleLine);
                }
                finally { f1.Dispose(); f2.Dispose(); }
            }
        }
    }

    /// <summary>带标题的圆角卡片容器</summary>
    public class CardPanel : Panel
    {
        public string TitleText = "";
        public Color StrokeCol = Color.Gray;
        public Color TitleCol = Color.Black;

        public CardPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Padding = new Padding(14, 36, 14, 10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = RoundButton.RoundPath(r, 10))
            {
                using (SolidBrush b = new SolidBrush(BackColor)) g.FillPath(b, path);
                using (Pen pen = new Pen(StrokeCol)) g.DrawPath(pen, path);
            }
            if (TitleText.Length > 0)
            {
                using (Font f = new Font("Microsoft YaHei UI", 9.5F, FontStyle.Bold))
                {
                    TextRenderer.DrawText(g, TitleText, f, new Point(14, 9), TitleCol,
                        TextFormatFlags.Left | TextFormatFlags.SingleLine);
                }
            }
        }
    }

    /// <summary>双缓冲列表（消除刷新闪烁）</summary>
    // ───────────────────────── 自绘列表（不依赖系统 ListView，绘制行为完全可控） ─────────────────────────

    public class SimpleList : Control
    {
        private string[] headers = new string[0];
        private int[] widths = new int[0];
        private List<string[]> rows = new List<string[]>();
        private List<object> tags = new List<object>();
        private List<Color> statFore = new List<Color>();   // 末列（运行状态）文字颜色
        private int sel = -1;
        private VScrollBar vsb = new VScrollBar();
        private HScrollBar hsb = new HScrollBar();
        private Font headFont;

        public const int RowHeight = 28;
        public const int HeaderHeight = 30;

        public Color HeaderBack = Color.White;
        public Color HeaderFore = Color.Black;
        public Color GridColor = Color.LightGray;
        public Color SelBack = Color.SteelBlue;
        public Color SelFore = Color.White;
        public Color RowFore = Color.Black;

        public event EventHandler SelectedIndexChanged;
        public event EventHandler RowActivate;
        public event Action<int> HeaderClick;      // 点击表头列号（用于排序）
        public int SortColumn = -1;                // 当前排序列（-1 = 默认顺序）
        public bool SortAsc = true;

        public SimpleList()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.Selectable, true);
            vsb.Width = 20;      // 滚动条加粗，缩到最小窗口时也容易用鼠标拖
            hsb.Height = 20;
            hsb.Dock = DockStyle.Bottom;   // 横向：贴列表底部，铺满整个宽度
            vsb.Dock = DockStyle.Right;    // 竖向：贴列表右侧，铺满整个高度
            Controls.Add(hsb);
            Controls.Add(vsb);             // 后加的先停靠，竖条占满全高
            vsb.Scroll += delegate { Invalidate(); };
            hsb.Scroll += delegate { Invalidate(); };
            headFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold);
            Cursor = Cursors.Hand;
            TabStop = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && headFont != null) headFont.Dispose();
            base.Dispose(disposing);
        }

        public void SetHeaders(string[] h, int[] w)
        {
            headers = h;
            widths = (int[])w.Clone();
            StretchLastColumn();
            UpdateScrollbars();
            Invalidate();
        }

        /// <summary>整表重建：滚动归零、取消选中</summary>
        public void SetRows(string[][] newRows, object[] newTags)
        {
            rows = new List<string[]>(newRows);
            tags = new List<object>(newTags);
            statFore = new List<Color>();
            for (int i = 0; i < rows.Count; i++) statFore.Add(Color.Empty);
            sel = -1;
            StretchLastColumn();
            UpdateScrollbars();
            Invalidate();
            if (SelectedIndexChanged != null) SelectedIndexChanged(this, EventArgs.Empty);
        }

        public int SelectedIndex
        {
            get { return sel; }
            set
            {
                if (value >= -1 && value < rows.Count && value != sel)
                {
                    sel = value;
                    Invalidate();
                    if (SelectedIndexChanged != null) SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        public object SelectedTag { get { return (sel >= 0 && sel < tags.Count) ? tags[sel] : null; } }
        public int RowCount { get { return rows.Count; } }
        public object GetTag(int i) { return (i >= 0 && i < tags.Count) ? tags[i] : null; }

        public void SetCell(int row, int col, string text, Color fore)
        {
            if (row < 0 || row >= rows.Count) return;
            string[] r = rows[row];
            if (col < r.Length) r[col] = text;
            while (statFore.Count <= row) statFore.Add(Color.Empty);
            statFore[row] = fore;
            Invalidate();
        }

        private int TotalWidth()
        {
            int t = 0;
            for (int i = 0; i < widths.Length; i++) t += widths[i];
            return t;
        }

        private void StretchLastColumn()
        {
            if (widths.Length == 0) return;
            int cw = Width - (vsb.Visible ? vsb.Width : 0);
            int used = 0;
            for (int i = 0; i < widths.Length - 1; i++) used += widths[i];
            int want = cw - used;
            if (want > 120) widths[widths.Length - 1] = want;   // 窗口够宽时末列吃掉剩余宽度，不出横向滚动条
        }

        private void UpdateScrollbars()
        {
            bool nv = rows.Count * RowHeight > Height - HeaderHeight;
            bool nh = TotalWidth() > Width;
            vsb.Visible = nv;
            hsb.Visible = nh;
            int cw = Width - (vsb.Visible ? vsb.Width : 0);
            int ch = Height - HeaderHeight - (hsb.Visible ? hsb.Height : 0);
            vsb.Minimum = 0;
            vsb.Maximum = Math.Max(0, rows.Count * RowHeight - 1);
            vsb.LargeChange = Math.Max(RowHeight, ch);
            vsb.SmallChange = RowHeight;
            int vmax = Math.Max(0, vsb.Maximum - vsb.LargeChange + 1);
            if (vsb.Value > vmax) vsb.Value = vmax;
            hsb.Minimum = 0;
            hsb.Maximum = Math.Max(0, TotalWidth() - 1);
            hsb.LargeChange = Math.Max(20, cw);
            hsb.SmallChange = 20;
            int hmax = Math.Max(0, hsb.Maximum - hsb.LargeChange + 1);
            if (hsb.Value > hmax) hsb.Value = hmax;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            StretchLastColumn();
            UpdateScrollbars();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (headFont != null) headFont.Dispose();
            headFont = new Font(Font, FontStyle.Bold);
        }

        private int ContentHeight()
        {
            return Height - HeaderHeight - (hsb.Visible ? hsb.Height : 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(BackColor);
            int cw = Width - (vsb.Visible ? vsb.Width : 0);
            int ch = ContentHeight();
            int sx = hsb.Visible ? hsb.Value : 0;
            int sy = vsb.Visible ? vsb.Value : 0;

            // 表头
            using (SolidBrush hb = new SolidBrush(HeaderBack)) g.FillRectangle(hb, 0, 0, cw, HeaderHeight);
            int x = -sx;
            for (int c = 0; c < headers.Length; c++)
            {
                string txt = headers[c];
                bool sortable = (c == SortColumn);
                int pad = 10;
                TextRenderer.DrawText(g, txt, headFont,
                    new Rectangle(x + 8, 0, Math.Max(10, widths[c] - pad - 10), HeaderHeight),
                    HeaderFore, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                if (sortable)
                {
                    // 排序列画箭头：↑升序 ↓降序
                    TextRenderer.DrawText(g, SortAsc ? "↑" : "↓", headFont,
                        new Rectangle(x + widths[c] - 20, 0, 16, HeaderHeight),
                        HeaderFore, TextFormatFlags.VerticalCenter | TextFormatFlags.Right | TextFormatFlags.NoPrefix);
                }
                x += widths[c];
            }
            using (Pen gp = new Pen(GridColor)) g.DrawLine(gp, 0, HeaderHeight - 1, cw, HeaderHeight - 1);

            // 数据行（只画可视范围）
            if (rows.Count > 0)
            {
                int first = Math.Max(0, sy / RowHeight);
                int last = Math.Min(rows.Count - 1, (sy + ch) / RowHeight);
                for (int i = first; i <= last; i++)
                {
                    int y = HeaderHeight + i * RowHeight - sy;
                    bool isSel = (i == sel);
                    if (isSel)
                        using (SolidBrush sb = new SolidBrush(SelBack)) g.FillRectangle(sb, 0, y, cw, RowHeight);
                    else if ((i & 1) == 1)
                        using (SolidBrush sb = new SolidBrush(Color.FromArgb(36, GridColor))) g.FillRectangle(sb, 0, y, cw, RowHeight);
                    x = -sx;
                    string[] r = rows[i];
                    for (int c = 0; c < headers.Length && c < r.Length; c++)
                    {
                        Color fc = isSel ? SelFore
                            : (c == headers.Length - 1 && i < statFore.Count && statFore[i] != Color.Empty) ? statFore[i]
                            : RowFore;
                        TextRenderer.DrawText(g, r[c] == null ? "" : r[c], Font,
                            new Rectangle(x + 8, y, Math.Max(10, widths[c] - 10), RowHeight),
                            fc, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
                        x += widths[c];
                    }
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            if (e.Y < HeaderHeight)
            {
                // 点表头：触发排序请求（不改变当前选中行）
                int col = ColAt(e.X + (hsb.Visible ? hsb.Value : 0));
                if (col >= 0 && HeaderClick != null) HeaderClick(col);
                return;
            }
            int row = (e.Y - HeaderHeight + (vsb.Visible ? vsb.Value : 0)) / RowHeight;
            if (row >= 0 && row < rows.Count && row != sel)
            {
                sel = row;
                Invalidate();
                if (SelectedIndexChanged != null) SelectedIndexChanged(this, EventArgs.Empty);
            }
        }

        private int ColAt(int x)
        {
            int cx = 0;
            for (int i = 0; i < widths.Length; i++)
            {
                if (x >= cx && x < cx + widths[i]) return i;
                cx += widths[i];
            }
            return -1;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (!vsb.Visible) return;
            HandledMouseEventArgs he = e as HandledMouseEventArgs;
            int vmax = Math.Max(0, vsb.Maximum - vsb.LargeChange + 1);
            int nv = vsb.Value - Math.Sign(e.Delta) * RowHeight * 3;
            nv = Math.Max(0, Math.Min(nv, vmax));
            if (nv != vsb.Value) { vsb.Value = nv; Invalidate(); }
            if (he != null) he.Handled = true;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (RowActivate != null) RowActivate(this, EventArgs.Empty);
        }
    }

    // ───────────────────────────── 主窗体 ─────────────────────────────

    public class MainForm : Form
    {
        private AppConfig cfg;
        private KeyHook hook;
        private readonly Dictionary<int, Repeater> repeaters = new Dictionary<int, Repeater>();
        private Timer watchTimer = new Timer();
        private bool hookEnabled = true;

        private bool globallyPaused = false;
        private readonly List<int> pausedPids = new List<int>();

        private ThemePalette theme;

        private SimpleList lv;
        private RoundButton btnRefresh;
        private CheckBox chkOnlyGame;
        private Label lblForeground;
        private CheckBox chkHook;
        private CheckBox chkAnywhere;
        private ComboBox gcbPause;
        private RoundButton btnGlobal;
        private ComboBox cmbTheme;

        private ComboBox cmbToggle;
        private ComboBox cmbMode;
        private CheckBox chkScan;
        private CheckBox chkEnabled;
        private CheckBox chkPauseMin;
        private RoundButton btnApply;
        private RoundButton btnToggle;
        private RoundButton btnStopAll;
        private RoundButton btnDelete;
        private Label lblStatus;
        private Label lblTip;

        private CardPanel leftCard;
        private CardPanel rightCard;
        private Panel pnlBindArea;
        private FlowLayoutPanel flw;
        private RoundButton btnAddBinding;
        private List<BindingRow> bindingRows = new List<BindingRow>();

        private Panel listHost;
        private FlowLayoutPanel strip;
        private TableLayoutPanel editorTlp;
        // private BannerPanel banner;   // 横幅已按需求移除，把空间留给窗口列表
        private Panel statusPanel;

        private const int EMERGENCY_VK = 0x7B; // F12

        private static readonly string[] ModeNames = new string[]
        {
            "自动（前台注入 / 后台投递）",
            "仅前台注入（窗口不在前台就暂停）",
            "后台消息投递（窗口不在前台也继续）"
        };

        private class BindingRow
        {
            public Panel host;
            public ComboBox key;
            public NumericUpDown interval;
        }

        public MainForm()
        {
            cfg = Store.Load();
            theme = Themes.Get(cfg.ThemeName);

            Text = "PoisonKing按键辅助 v1.0";
            try
            {
                // 窗口标题栏 / 任务栏图标：加载内嵌的 app.ico（龙争虎斗徽章）
                using (System.IO.Stream icoStream = System.Reflection.Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("kpres.icon"))
                {
                    if (icoStream != null) Icon = new Icon(icoStream);
                }
            }
            catch { }
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1180, 780);
            MinimumSize = new Size(760, 560);
            Font = new Font("Microsoft YaHei UI", 9.75F);
            AutoScaleMode = AutoScaleMode.Dpi;

            InitUi();
            ApplyTheme();
            RefreshWindows();
            InstallHook();

            watchTimer.Interval = 500;
            watchTimer.Tick += WatchTimer_Tick;
            watchTimer.Start();

            // 窗口大小变化时编辑区高度按比例伸缩，列表始终保留可见空间
            FormClosing += MainForm_FormClosing;
            Resize += delegate { FitEditor(); };
        }

        private void FitEditor()
        {
            if (editorTlp == null) return;
            int h = ClientSize.Height;
            int fixedTop = strip.Height;
            int fixedBottom = statusPanel.Height;
            int ed = (int)(h * 0.38);          // 编辑区占 38%，列表区拿到更多行
            if (ed > 400) ed = 400;
            int minList = 170;
            if (h - fixedTop - fixedBottom - ed < minList)
                ed = h - fixedTop - fixedBottom - minList;
            if (ed < 210) ed = 210;
            editorTlp.Height = ed;
        }

        /// <summary>窗口显示后：小屏 / 高 DPI 屏上不超出工作区（B 屏比 A 屏小时自动收缩）</summary>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Rectangle wa = Screen.GetWorkingArea(this);
            if (Width > wa.Width || Height > wa.Height)
                Size = new Size(Math.Min(Width, wa.Width), Math.Min(Height, wa.Height));
            FitEditor();
        }

        // ─────────────── 界面构建 ───────────────

        private void InitUi()
        {
            // 控制条（FlowLayoutPanel 自适应排列）
            strip = new FlowLayoutPanel();
            strip.Dock = DockStyle.Top;
            strip.Height = 52;
            strip.FlowDirection = FlowDirection.LeftToRight;
            strip.WrapContents = true;   // 窗口窄时自动换行，不再裁掉按钮
            strip.AutoSize = true;
            strip.MinimumSize = new Size(0, 52);
            strip.Padding = new Padding(12, 10, 12, 4);

            btnRefresh = new RoundButton();
            btnRefresh.Text = "刷新窗口";
            btnRefresh.Size = new Size(92, 30);
            btnRefresh.Click += delegate { RefreshWindows(); };

            chkOnlyGame = new CheckBox();
            chkOnlyGame.Text = "仅显示剑网3窗口";
            chkOnlyGame.Checked = cfg.ShowOnlyGame;
            chkOnlyGame.AutoSize = true;
            chkOnlyGame.Margin = new Padding(10, 8, 6, 0);
            chkOnlyGame.CheckedChanged += delegate
            {
                cfg.ShowOnlyGame = chkOnlyGame.Checked;
                Store.Save(cfg);
                RefreshWindows();
            };

            lblForeground = new Label();
            lblForeground.AutoSize = true;
            lblForeground.Text = "当前前台：—";
            lblForeground.Margin = new Padding(8, 10, 8, 0);

            chkHook = new CheckBox();
            chkHook.Text = "启用按键接管";
            chkHook.Checked = true;
            chkHook.AutoSize = true;
            chkHook.Margin = new Padding(10, 8, 6, 0);
            chkHook.CheckedChanged += delegate
            {
                hookEnabled = chkHook.Checked;
                UpdateStatus();
            };

            chkAnywhere = new CheckBox();
            chkAnywhere.Text = "热键全局生效";
            chkAnywhere.Checked = cfg.ToggleAnywhere;
            chkAnywhere.AutoSize = true;
            chkAnywhere.Margin = new Padding(10, 8, 6, 0);
            chkAnywhere.CheckedChanged += delegate
            {
                cfg.ToggleAnywhere = chkAnywhere.Checked;
                Store.Save(cfg);
            };

            Label lp = new Label();
            lp.Text = "总开关";
            lp.AutoSize = true;
            lp.Margin = new Padding(12, 11, 4, 0);

            gcbPause = new ComboBox();
            gcbPause.Width = 64;
            gcbPause.DropDownStyle = ComboBoxStyle.DropDown;
            gcbPause.Items.AddRange(new object[] { "F8", "F9", "F10", "F11" });
            gcbPause.Text = string.IsNullOrEmpty(cfg.GlobalPauseKey) ? "F8" : cfg.GlobalPauseKey;
            gcbPause.Margin = new Padding(2, 6, 6, 0);
            gcbPause.KeyDown += CaptureKey;
            gcbPause.TextChanged += delegate
            {
                cfg.GlobalPauseKey = VkMap.Normalize(gcbPause.Text);
                Store.Save(cfg);
            };

            btnGlobal = new RoundButton();
            btnGlobal.Text = "暂停 / 恢复全部";
            btnGlobal.Size = new Size(126, 30);
            btnGlobal.Margin = new Padding(4, 0, 4, 0);
            btnGlobal.Click += delegate { ToggleGlobalPause(); };

            Label ltheme = new Label();
            ltheme.Text = "主题";
            ltheme.AutoSize = true;
            ltheme.Margin = new Padding(14, 11, 4, 0);

            cmbTheme = new ComboBox();
            cmbTheme.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTheme.Width = 96;
            foreach (ThemePalette t in Themes.All) cmbTheme.Items.Add(t.Name);
            cmbTheme.SelectedItem = theme.Name;
            cmbTheme.Margin = new Padding(2, 6, 8, 0);
            cmbTheme.SelectedIndexChanged += delegate
            {
                cfg.ThemeName = Convert.ToString(cmbTheme.SelectedItem);
                theme = Themes.Get(cfg.ThemeName);
                Store.Save(cfg);
                ApplyTheme();
            };

            strip.Controls.Add(btnRefresh);
            strip.Controls.Add(chkOnlyGame);
            strip.Controls.Add(lblForeground);
            strip.Controls.Add(chkHook);
            strip.Controls.Add(chkAnywhere);
            strip.Controls.Add(lp);
            strip.Controls.Add(gcbPause);
            strip.Controls.Add(btnGlobal);
            strip.Controls.Add(ltheme);
            strip.Controls.Add(cmbTheme);
            Controls.Add(strip);

            // 窗口列表（带内衬的容器，滚动用 ListView 自带的原生滚动条）
            listHost = new Panel();
            listHost.Dock = DockStyle.Fill;
            listHost.Padding = new Padding(12, 6, 12, 6);

            lv = new SimpleList();
            lv.Dock = DockStyle.Fill;
            lv.SetHeaders(
                new string[] { "序号", "PID", "窗口标题", "进程", "开关热键", "连发绑定", "投递方式", "运行状态" },
                new int[] { 50, 60, 280, 130, 66, 180, 88, 200 });
            lv.SelectedIndexChanged += delegate { LoadSelectedToEditor(); };
            lv.RowActivate += delegate { BtnToggle_Click(null, null); };
            lv.HeaderClick += delegate(int col) { SortByColumn(col); };

            listHost.Controls.Add(lv);
            Controls.Add(listHost);

            // 底部编辑区：左右两张卡片
            editorTlp = new TableLayoutPanel();
            editorTlp.Dock = DockStyle.Bottom;
            editorTlp.Height = 296;
            editorTlp.ColumnCount = 2;
            editorTlp.RowCount = 1;
            editorTlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54));   // 两栏按比例缩放，不再裁切
            editorTlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46));
            editorTlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            editorTlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            editorTlp.Padding = new Padding(12, 2, 12, 8);

            leftCard = new CardPanel();
            leftCard.Dock = DockStyle.Fill;   // 填满所在单元格，随窗口伸缩（之前默认 200x100 导致遮盖）
            leftCard.MinimumSize = new Size(300, 240);
            leftCard.AutoScroll = true;       // 内容（含使用提示）超出时出现竖向滚动条
            leftCard.TitleText = "① 方案配置";

            Label lk1 = new Label(); lk1.Text = "开关热键"; lk1.AutoSize = true; lk1.Location = new Point(14, 42);
            cmbToggle = new ComboBox();
            cmbToggle.Location = new Point(82, 38);
            cmbToggle.Width = 96;
            cmbToggle.DropDownStyle = ComboBoxStyle.DropDown;
            cmbToggle.Items.AddRange(new object[] { "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12" });
            cmbToggle.Text = "F1";
            cmbToggle.KeyDown += CaptureKey;

            Label lk2 = new Label(); lk2.Text = "投递方式"; lk2.AutoSize = true; lk2.Location = new Point(196, 42);
            cmbMode = new ComboBox();
            cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMode.Location = new Point(256, 38);
            cmbMode.Width = 314;
            cmbMode.Items.AddRange(ModeNames);
            cmbMode.SelectedIndex = 0;

            chkEnabled = new CheckBox(); chkEnabled.Text = "启用该方案"; chkEnabled.Checked = true; chkEnabled.AutoSize = true;
            chkEnabled.Margin = new Padding(4, 4, 10, 0);
            chkScan = new CheckBox(); chkScan.Text = "以扫描码发送"; chkScan.Checked = true; chkScan.AutoSize = true;
            chkScan.Margin = new Padding(4, 4, 10, 0);
            chkPauseMin = new CheckBox(); chkPauseMin.Text = "最小化时暂停"; chkPauseMin.AutoSize = true;
            chkPauseMin.Margin = new Padding(4, 4, 10, 0);

            // 勾选项与按钮改成自动换行的流式布局，窄窗口下不再被遮盖
            FlowLayoutPanel chkRow = new FlowLayoutPanel();
            chkRow.Location = new Point(8, 74);
            chkRow.Size = new Size(566, 32);
            chkRow.FlowDirection = FlowDirection.LeftToRight;
            chkRow.WrapContents = true;
            chkRow.Controls.Add(chkEnabled);
            chkRow.Controls.Add(chkScan);
            chkRow.Controls.Add(chkPauseMin);

            FlowLayoutPanel btnRow = new FlowLayoutPanel();
            btnRow.Location = new Point(8, 108);
            btnRow.Size = new Size(566, 40);
            btnRow.FlowDirection = FlowDirection.LeftToRight;
            btnRow.WrapContents = true;

            btnApply = new RoundButton(); btnApply.Text = "保存方案"; btnApply.Size = new Size(110, 34);
            btnApply.Margin = new Padding(2, 2, 6, 2);
            btnApply.Click += BtnApply_Click;

            btnToggle = new RoundButton(); btnToggle.Text = "启动 / 停止选中窗口"; btnToggle.Size = new Size(168, 34);
            btnToggle.Margin = new Padding(2, 2, 6, 2);
            btnToggle.Click += BtnToggle_Click;

            btnStopAll = new RoundButton(); btnStopAll.Text = "全部停止"; btnStopAll.Size = new Size(100, 34);
            btnStopAll.Margin = new Padding(2, 2, 6, 2);
            btnStopAll.Click += delegate { KillAll(); };

            // 第二行按钮（全部停止 / 删除方案）
            FlowLayoutPanel btnRow2 = new FlowLayoutPanel();
            btnRow2.Location = new Point(8, 150);
            btnRow2.Size = new Size(566, 40);
            btnRow2.FlowDirection = FlowDirection.LeftToRight;

            btnDelete = new RoundButton(); btnDelete.Text = "删除方案"; btnDelete.Size = new Size(100, 34);
            btnDelete.Margin = new Padding(2, 2, 6, 2);
            btnDelete.Click += BtnDelete_Click;
            btnRow2.Controls.Add(btnStopAll); btnRow2.Controls.Add(btnDelete);
            btnRow.Controls.Add(btnApply); btnRow.Controls.Add(btnToggle);

            lblTip = new Label();
            lblTip.AutoSize = false;
            lblTip.Location = new Point(14, 196);
            lblTip.Size = new Size(560, 110);
            lblTip.ForeColor = theme.TSub;
            lblTip.Text = "使用提示：\n· 双击列表行 = 启动 / 停止该窗口\n· F12 = 全部急停；总开关 = 可恢复的暂停\n· 投递方式选“后台消息投递”时，切到别的窗口连发不会中断\n· 主题切换在顶部工具条，配置自动保存在 exe 同目录 config.json";

            leftCard.Controls.Add(lk1); leftCard.Controls.Add(cmbToggle);
            leftCard.Controls.Add(lk2); leftCard.Controls.Add(cmbMode);
            leftCard.Controls.Add(chkRow); leftCard.Controls.Add(btnRow);
            leftCard.Controls.Add(btnRow2);
            leftCard.Controls.Add(lblTip);

            // 卡片尺寸变化时按实际宽度重设各控件，不再用锚点（避免初始 200x100 基准导致的无限拉伸）
            EventHandler fitLeft = delegate
            {
                // 内宽要减掉可能出现的竖向滚动条，避免内容被滚动条遮住
                int sbw = leftCard.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth + 2 : 0;
                int w = leftCard.ClientSize.Width - 16 - sbw;
                cmbMode.Width = Math.Max(130, w - cmbMode.Left + 6);
                chkRow.Width = Math.Max(150, w);
                btnRow.Width = Math.Max(150, w);
                btnRow2.Width = Math.Max(150, w);

                // “使用提示”按文字实际高度撑开，而不是被卡片高度截断
                int tipW = Math.Max(120, w - 14);
                int tipH = TextRenderer.MeasureText(lblTip.Text, lblTip.Font,
                    new Size(tipW, int.MaxValue), TextFormatFlags.WordBreak | TextFormatFlags.NoPadding).Height + 6;
                lblTip.Size = new Size(tipW, Math.Max(60, tipH));

                // 内容总高超出可视区 → 自动出现竖向滚动条
                leftCard.AutoScrollMinSize = new Size(0, lblTip.Bottom + 14);
            };
            leftCard.Resize += fitLeft;

            rightCard = new CardPanel();
            rightCard.Dock = DockStyle.Fill;
            rightCard.MinimumSize = new Size(280, 210);
            rightCard.TitleText = "② 连发绑定（点框后按下按键）";

            btnAddBinding = new RoundButton();
            btnAddBinding.Text = "＋ 添加一组";
            btnAddBinding.Size = new Size(104, 26);
            btnAddBinding.Location = new Point(8, 6);   // 放在本模块左头
            btnAddBinding.Click += delegate { AddBindingRow("", 100); };

            Panel bindHeader = new Panel();
            bindHeader.Dock = DockStyle.Top;
            bindHeader.Height = 38;

            pnlBindArea = new Panel();
            pnlBindArea.Dock = DockStyle.Fill;
            pnlBindArea.Padding = new Padding(2);

            flw = new FlowLayoutPanel();
            flw.Dock = DockStyle.Fill;
            flw.FlowDirection = FlowDirection.TopDown;
            flw.WrapContents = false;
            flw.AutoScroll = true;
            flw.Padding = new Padding(4);
            flw.Resize += delegate { FitBindScroll(); };

            pnlBindArea.Controls.Add(flw);

            rightCard.Controls.Add(pnlBindArea);
            rightCard.Controls.Add(bindHeader);
            bindHeader.Controls.Add(btnAddBinding);

            editorTlp.Controls.Add(leftCard, 0, 0);
            editorTlp.Controls.Add(rightCard, 1, 0);
            Controls.Add(editorTlp);

            // 底部状态条
            statusPanel = new Panel();
            statusPanel.Dock = DockStyle.Bottom;
            statusPanel.Height = 32;
            statusPanel.Padding = new Padding(14, 6, 14, 0);

            lblStatus = new Label();
            lblStatus.AutoSize = false;
            lblStatus.Dock = DockStyle.Fill;
            lblStatus.Text = "";
            statusPanel.Controls.Add(lblStatus);
            Controls.Add(statusPanel);

            // ── 停靠顺序修正（关键）：WinForms 从集合末尾往前依次分配空间 ──
            // 把工具条移到集合末尾：它最先拿到顶部空间；列表 Fill 排最后，只拿剩余区域，
            // 从根上杜绠“工具条压住列表首行”的遮盖问题
            strip.SendToBack();
            // 工具条自动换行后高度变化时，重新分配下方区域
            strip.SizeChanged += delegate { FitEditor(); };
        }

        /// <summary>应用主题到所有控件</summary>
        private void ApplyTheme()
        {
            BackColor = theme.TBack;

            strip.BackColor = theme.TBack;
            lblForeground.ForeColor = theme.TSub;
            foreach (Control c in strip.Controls)
            {
                if (c is CheckBox) c.ForeColor = theme.TText;
                if (c is Label) c.ForeColor = theme.TText;
            }

            StyleButton(btnRefresh, false);
            StyleButton(btnGlobal, true);
            StyleButton(btnApply, true);
            StyleButton(btnToggle, false);
            StyleButton(btnStopAll, false);
            StyleButton(btnDelete, false);
            StyleButton(btnAddBinding, false);

            lv.BackColor = theme.TCard;
            lv.ForeColor = theme.TText;
            lv.HeaderBack = theme.TBtnFill;
            lv.HeaderFore = theme.TBtnText;
            lv.GridColor = theme.TCardBorder;
            lv.SelBack = theme.TAccFill;
            lv.SelFore = theme.TAccText;
            lv.RowFore = theme.TText;
            lv.Invalidate();
            if (listHost != null) listHost.BackColor = theme.TCard;

            leftCard.BackColor = theme.TCard;
            leftCard.StrokeCol = theme.TCardBorder;
            leftCard.TitleCol = theme.THead;
            leftCard.Invalidate();
            rightCard.BackColor = theme.TCard;
            rightCard.StrokeCol = theme.TCardBorder;
            rightCard.TitleCol = theme.THead;
            rightCard.Invalidate();

            foreach (Control c in leftCard.Controls)
            {
                if (c is FlowLayoutPanel)
                {
                    c.BackColor = theme.TCard;
                    foreach (Control k in c.Controls)
                        if (k is CheckBox) k.ForeColor = theme.TText;
                }
                if (c is CheckBox) c.ForeColor = theme.TText;
                if (c is Label && c != lblTip) c.ForeColor = theme.TText;
            }
            lblTip.ForeColor = theme.TSub;

            flw.BackColor = theme.TCard;
            pnlBindArea.BackColor = theme.TCard;
            Control bindHdr = null;
            foreach (Control c in rightCard.Controls) if (c.Height == 38 && c.Dock == DockStyle.Top) { bindHdr = c; break; }
            if (bindHdr != null) bindHdr.BackColor = theme.TCard;

            statusPanel.BackColor = theme.TStatus;
            lblStatus.ForeColor = theme.TText;

            UpdateRunColumn();
        }

        private void StyleButton(RoundButton b, bool accent)
        {
            if (accent)
            {
                b.FillCol = theme.TAccFill;
                b.HoverCol = ControlPaint.Light(theme.TAccFill);
                b.StrokeCol = theme.TAccBorder;
                b.LabelCol = theme.TAccText;
            }
            else
            {
                b.FillCol = theme.TBtnFill;
                b.HoverCol = theme.TBtnHover;
                b.StrokeCol = theme.TBtnBorder;
                b.LabelCol = theme.TBtnText;
            }
            b.Invalidate();
        }

        /// <summary>加载内嵌横幅图（exe 里自带，无需外部文件）</summary>
        private Image LoadEmbeddedSkin()
        {
            try
            {
                // 同目录存在 banner.jpg 时优先使用（方便换肤）
                string ext = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "banner.jpg");
                if (File.Exists(ext)) return Image.FromFile(ext);

                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream s = asm.GetManifestResourceStream("kpres.banner"))
                {
                    if (s != null)
                    {
                        using (Bitmap tmp = new Bitmap(s))
                        {
                            return new Bitmap(tmp);
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        // ─────────────── 钩子与热键逻辑（与 v3 一致） ───────────────

        private void InstallHook()
        {
            hook = new KeyHook();
            hook.Handler = OnKey;
            if (!hook.Install())
            {
                MessageBox.Show("键盘钩子安装失败，可能被安全软件拦截。请尝试以管理员身份运行。",
                    "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool OnKey(int vk, bool down)
        {
            if (!hookEnabled) return false;

            if (vk == EMERGENCY_VK && down)
            {
                if (RunningCount() > 0)
                {
                    KillAll();
                    return true;
                }
                return false;
            }

            if (!down) return false;

            int gvk = VkMap.Parse(cfg.GlobalPauseKey);
            if (gvk != 0 && vk == gvk)
            {
                ToggleGlobalPause();
                return true;
            }

            int fg = Native.ForegroundPid();

            Profile fgProfile = FindProfile(fg);
            if (fgProfile != null && fgProfile.Enabled)
            {
                int toggleVk = VkMap.Parse(fgProfile.ToggleKey);
                if (toggleVk != 0 && vk == toggleVk)
                {
                    ToggleProfile(fgProfile);
                    return true;
                }
            }

            if (!cfg.ToggleAnywhere) return false;

            Profile cand = null;
            int hits = 0;
            foreach (Profile p in cfg.Profiles)
            {
                if (!p.Enabled) continue;
                int v = VkMap.Parse(p.ToggleKey);
                if (v != 0 && v == vk) { cand = p; hits++; }
            }
            if (hits == 1 && cand != null)
            {
                ToggleProfile(cand);
                return true;
            }
            return false;
        }

        private void ToggleGlobalPause()
        {
            if (!globallyPaused)
            {
                if (RunningCount() == 0)
                {
                    UpdateStatusSafe("当前没有正在运行的连发，无需暂停");
                    return;
                }
                pausedPids.Clear();
                foreach (KeyValuePair<int, Repeater> kv in repeaters)
                {
                    if (kv.Value != null && kv.Value.Running) pausedPids.Add(kv.Key);
                }
                foreach (int pid in pausedPids) StopPid(pid);
                globallyPaused = true;
                UpdateStatusSafe("⏸ 已全局暂停 " + pausedPids.Count + " 个窗口的连发，再按 " +
                    (string.IsNullOrEmpty(cfg.GlobalPauseKey) ? "总开关" : cfg.GlobalPauseKey) + " 恢复");
            }
            else
            {
                int restored = 0;
                foreach (int pid in pausedPids)
                {
                    Profile p = FindProfile(pid);
                    if (p != null && p.Enabled)
                    {
                        StartProfile(p);
                        restored++;
                    }
                }
                globallyPaused = false;
                pausedPids.Clear();
                UpdateStatusSafe("▶ 已恢复 " + restored + " 个连发");
            }
        }

        private void KillAll()
        {
            foreach (KeyValuePair<int, Repeater> kv in repeaters)
            {
                if (kv.Value != null) kv.Value.Stop();
            }
            globallyPaused = false;
            pausedPids.Clear();
            UpdateStatusSafe("■ 已全部停止（F12 急停）");
        }

        private void StartProfile(Profile p)
        {
            List<KeyBinding> binds = p.EffectiveBindings();
            if (binds.Count == 0)
            {
                UpdateStatusSafe("没有有效的连发绑定：" + SafeTitle(p));
                return;
            }

            IntPtr hwnd = Native.FindMainWindow(p.Pid);
            if (hwnd == IntPtr.Zero)
            {
                UpdateStatusSafe("找不到该窗口，可能已关闭。请点“刷新窗口”。");
                return;
            }

            List<KeyValuePair<int, int>> list = new List<KeyValuePair<int, int>>();
            foreach (KeyBinding b in binds)
                list.Add(new KeyValuePair<int, int>(VkMap.Parse(b.Key), b.IntervalMs));

            GetRepeater(p.Pid).Start(p.Pid, list, p.SendMode, p.UseScanCode, p.PauseWhenMinimized);
            string warn = "";
            try
            {
                if (Native.IsProcessElevated(p.Pid) && !IsSelfElevated())
                    warn = "   ⚠ 目标是管理员权限，请右键 PoisonKing按键辅助.exe 以管理员身份运行，否则按键无法送达";
            }
            catch { }
            UpdateStatusSafe("▶ 已启动：" + SafeTitle(p) + "  →  " + p.BindingsText() + "（" + ShortMode(p.SendMode) + "）" + warn);
        }

        private static bool IsSelfElevated()
        {
            System.Security.Principal.WindowsPrincipal pr = new System.Security.Principal.WindowsPrincipal(
                System.Security.Principal.WindowsIdentity.GetCurrent());
            return pr.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }

        private void ToggleProfile(Profile p)
        {
            if (IsRunning(p.Pid))
            {
                StopPid(p.Pid);
                UpdateStatusSafe("■ 已停止：" + SafeTitle(p));
                return;
            }
            StartProfile(p);
        }

        private static int Clamp(int mode)
        {
            if (mode < 0 || mode > 2) return 0;
            return mode;
        }

        private Repeater GetRepeater(int pid)
        {
            Repeater r;
            if (!repeaters.TryGetValue(pid, out r) || r == null)
            {
                r = new Repeater();
                repeaters[pid] = r;
            }
            return r;
        }

        private bool IsRunning(int pid)
        {
            Repeater r;
            return repeaters.TryGetValue(pid, out r) && r != null && r.Running;
        }

        private int RunningCount()
        {
            int n = 0;
            foreach (KeyValuePair<int, Repeater> kv in repeaters)
            {
                if (kv.Value != null && kv.Value.Running) n++;
            }
            return n;
        }

        private void StopPid(int pid)
        {
            Repeater r;
            if (repeaters.TryGetValue(pid, out r) && r != null) r.Stop();
        }

        private Profile FindProfile(int pid)
        {
            if (pid <= 0) return null;
            string actual = null;
            foreach (Profile p in cfg.Profiles)
            {
                if (p.Pid != pid) continue;
                // 防止 PID 复用：重启后旧 PID 可能属于完全无关的进程，
                // 校验当前进程名与方案保存时的进程名一致才生效，失配视为方案过期。
                if (string.IsNullOrEmpty(p.ProcessName)) return p;
                if (actual == null) actual = Native.QueryProcessName(pid);
                if (string.IsNullOrEmpty(actual)) return p; // 查不到进程名时不阻断
                if (string.Equals(actual, p.ProcessName, StringComparison.OrdinalIgnoreCase)) return p;
                return null; // PID 已被其他进程占用，旧方案不生效
            }
            return null;
        }

        private string SafeTitle(Profile p)
        {
            string t = p.WindowTitle == null ? "" : p.WindowTitle;
            if (t.Length > 20) return t.Substring(0, 20) + "…";
            return t;
        }

        private void WatchTimer_Tick(object sender, EventArgs e)
        {
            int fg = Native.ForegroundPid();
            string title = GetTitleByPid(fg);
            if (fg == Process.GetCurrentProcess().Id)
                lblForeground.Text = "当前前台：本工具";
            else
            {
                if (title != null && title.Length > 18) title = title.Substring(0, 18) + "…";
                lblForeground.Text = "当前前台：PID " + fg + (string.IsNullOrEmpty(title) ? "" : "  " + title);
            }

            List<int> dead = new List<int>();
            foreach (KeyValuePair<int, Repeater> kv in repeaters)
            {
                if (!kv.Value.Running && FindProfile(kv.Key) == null) dead.Add(kv.Key);
            }
            foreach (int pid in dead) repeaters.Remove(pid);

            UpdateRunColumn();
        }

        private void UpdateRunColumn()
        {
            for (int i = 0; i < lv.RowCount; i++)
            {
                WinInfo w = lv.GetTag(i) as WinInfo;
                if (w == null) continue;

                Repeater r;
                if (repeaters.TryGetValue(w.Pid, out r) && r != null && r.Running)
                {
                    string mode = string.IsNullOrEmpty(r.LastMode) ? "等待首帧" : r.LastMode;
                    lv.SetCell(i, 7, "▶ 运行中（" + mode + "）· " + r.SentCount, theme.TRun);
                }
                else
                {
                    string err = (r == null) ? null : r.LastError;
                    lv.SetCell(i, 7, string.IsNullOrEmpty(err) ? "已停止" : "已停止（" + err + "）", theme.TIdle);
                }
            }
        }

        /// <summary>当前列表里的窗口（用于排序重绘）</summary>
        private List<WinInfo> currentWindows = new List<WinInfo>();

        /// <summary>点表头排序：序号列 = 恢复默认（游戏优先）；PID / 窗口标题 / 进程 可切升序降序</summary>
        private void SortByColumn(int col)
        {
            if (col < 1 || col > 3)
            {
                if (col == 0)
                {
                    // 点“序号”列 = 恢复默认顺序
                    lv.SortColumn = -1;
                    currentWindows.Sort(delegate(WinInfo a, WinInfo b) { return GameRank(b).CompareTo(GameRank(a)); });
                    RebuildRows();
                }
                return;
            }
            bool asc = (lv.SortColumn == col) ? !lv.SortAsc : true;
            lv.SortColumn = col;
            lv.SortAsc = asc;
            currentWindows.Sort(delegate(WinInfo a, WinInfo b)
            {
                int c = 0;
                if (col == 1) c = a.Pid.CompareTo(b.Pid);
                else if (col == 2) c = string.Compare(a.Title == null ? "" : a.Title, b.Title == null ? "" : b.Title, StringComparison.CurrentCulture);
                else c = string.Compare(a.ProcessName == null ? "" : a.ProcessName, b.ProcessName == null ? "" : b.ProcessName, StringComparison.CurrentCultureIgnoreCase);
                if (c == 0) c = a.Pid.CompareTo(b.Pid);
                return asc ? c : -c;
            });
            RebuildRows();
            UpdateStatusSafe("已按「" + (col == 1 ? "PID" : col == 2 ? "窗口标题" : "进程") + "」" + (asc ? "升序" : "降序") + "排列，点「序号」恢复默认。");
        }

        /// <summary>用 currentWindows 重建列表行（保留原选中 PID）</summary>
        private void RebuildRows()
        {
            int keepPid = 0;
            WinInfo selw = lv.SelectedTag as WinInfo;
            if (selw != null) keepPid = selw.Pid;

            int n = currentWindows.Count;
            string[][] rows = new string[n][];
            object[] tags = new object[n];
            for (int i = 0; i < n; i++)
            {
                WinInfo w = currentWindows[i];
                Profile p = FindProfile(w.Pid);
                if (p != null)
                {
                    rows[i] = new string[] {
                        (i + 1).ToString(), w.Pid.ToString(), w.Title, w.ProcessName,
                        p.ToggleKey, p.BindingsText(), ShortMode(p.SendMode), "已停止" };
                }
                else
                {
                    rows[i] = new string[] {
                        (i + 1).ToString(), w.Pid.ToString(), w.Title, w.ProcessName,
                        "—", "—", "—", "未配置" };
                }
                tags[i] = w;
            }
            lv.SetRows(rows, tags);
            if (keepPid != 0)
            {
                for (int i = 0; i < n; i++)
                {
                    if (currentWindows[i].Pid == keepPid) { lv.SelectedIndex = i; break; }
                }
            }
            UpdateRunColumn();
        }

        private void RefreshWindows()
        {
            currentWindows = EnumGameWindows(cfg.ShowOnlyGame);
            // 默认：游戏窗口排最前，避免被其它软件挤到下面看不到
            currentWindows.Sort(delegate(WinInfo a, WinInfo b) { return GameRank(b).CompareTo(GameRank(a)); });
            try
            {
                lv.SortColumn = -1;   // 刷新后回到默认顺序
                RebuildRows();
            }
            catch (Exception ex)
            {
                UpdateStatusSafe("刷新列表出错：" + ex.Message);
                return;
            }
            // 状态栏用最直白的话：窗口总数 + 操作指引
            UpdateStatusSafe("共 " + currentWindows.Count + " 个窗口，剑网3 已排在最前。点「刷新窗口」重新扫描，点表头可排序。");
        }

        private static string Trunc(string s, int n)
        {
            if (string.IsNullOrEmpty(s)) return "";
            s = s.Replace("\r", " ").Replace("\n", " ");
            return s.Length <= n ? s : s.Substring(0, n) + "…";
        }

        private static string ShortMode(int mode)
        {
            switch (Clamp(mode))
            {
                case 1: return "仅前台";
                case 2: return "后台投递";
                default: return "自动";
            }
        }

        private void LoadSelectedToEditor()
        {
            WinInfo w = lv.SelectedTag as WinInfo;
            if (w == null) return;
            Profile p = FindProfile(w.Pid);

            ClearBindingRows();
            if (p != null)
            {
                cmbToggle.Text = p.ToggleKey;
                cmbMode.SelectedIndex = Clamp(p.SendMode);
                chkScan.Checked = p.UseScanCode;
                chkEnabled.Checked = p.Enabled;
                chkPauseMin.Checked = p.PauseWhenMinimized;

                List<KeyBinding> binds = p.EffectiveBindings();
                if (binds.Count == 0) AddBindingRow("1", 100);
                else foreach (KeyBinding b in binds) AddBindingRow(b.Key, b.IntervalMs);
            }
            else
            {
                cmbToggle.Text = "F1";
                cmbMode.SelectedIndex = 0;
                chkScan.Checked = true;
                chkEnabled.Checked = true;
                chkPauseMin.Checked = false;
                AddBindingRow("1", 100);
            }
        }

        private void AddBindingRow(string keyName, int interval)
        {
            BindingRow row = new BindingRow();
            row.host = new Panel();
            row.host.Width = 264;   // 键位 + 间隔 + 删除紧凑一行
            row.host.Height = 36;
            row.host.Margin = new Padding(4, 3, 4, 3);
            row.host.BackColor = theme.TCard;

            row.key = new ComboBox();
            row.key.Location = new Point(2, 5);
            row.key.Width = 104;
            row.key.DropDownStyle = ComboBoxStyle.DropDown;
            row.key.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6", "7", "8", "SPACE", "TAB", "Q", "E" });
            row.key.Text = keyName;
            row.key.KeyDown += CaptureKey;

            row.interval = new NumericUpDown();
            row.interval.Location = new Point(116, 7);
            row.interval.Width = 74;
            row.interval.Minimum = 10;
            row.interval.Maximum = 5000;
            row.interval.Increment = 10;
            row.interval.Value = Math.Max(10, Math.Min(5000, interval));

            Label ms = new Label();
            ms.Text = "ms";
            ms.AutoSize = true;
            ms.Location = new Point(196, 11);
            ms.ForeColor = theme.TSub;

            RoundButton del = new RoundButton();
            del.Text = "✕";
            del.Location = new Point(226, 4);   // 紧挨时间选择
            del.Size = new Size(32, 27);
            del.Click += delegate
            {
                flw.Controls.Remove(row.host);
                bindingRows.Remove(row);
            };
            StyleButton(del, false);

            row.host.Controls.Add(row.key);
            row.host.Controls.Add(row.interval);
            row.host.Controls.Add(ms);
            row.host.Controls.Add(del);

            bindingRows.Add(row);
            flw.Controls.Add(row.host);
            FitBindScroll();
        }

        /// <summary>连发绑定区：内容超出时竖向滚动，并让每行宽度跟随卡片</summary>
        private void FitBindScroll()
        {
            if (flw == null) return;
            int sbw = flw.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth + 4 : 0;
            int rowW = Math.Max(250, flw.ClientSize.Width - 14 - sbw);
            int need = 8;
            foreach (Control c in flw.Controls)
            {
                c.Width = rowW;
                need += c.Height + c.Margin.Top + c.Margin.Bottom;
            }
            if (need > flw.ClientSize.Height) flw.AutoScrollMinSize = new Size(0, need);
            else flw.AutoScrollMinSize = new Size(0, 0);
        }

        private void ClearBindingRows()
        {
            flw.Controls.Clear();
            bindingRows.Clear();
            FitBindScroll();
        }

        /// <summary>点一下输入框再按下按键，自动填入键名（F1–F12 不能靠手打）</summary>
        private void CaptureKey(object sender, KeyEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null) return;

            if (e.KeyCode == Keys.Tab || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Escape ||
                e.KeyCode == Keys.Back || e.KeyCode == Keys.ShiftKey ||
                e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.Menu) return;

            string name = VkMap.ToName(e.KeyValue);
            if (name.StartsWith("0x")) return;

            cb.Text = name;
            if (cb.Text.Length > 0) cb.SelectionStart = cb.Text.Length;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            WinInfo w = lv.SelectedTag as WinInfo;
            if (w == null)
            {
                MessageBox.Show("请先在列表中选择一个窗口。", "提示");
                return;
            }

            string toggleName = VkMap.Normalize(cmbToggle.Text);
            if (VkMap.Parse(toggleName) == 0)
            {
                MessageBox.Show("开关热键无法识别。\n\n用法：点一下“开关热键”输入框，然后直接按下你要用的键（例如 F1）。", "提示");
                return;
            }

            int gvk = VkMap.Parse(cfg.GlobalPauseKey);
            if (gvk != 0 && VkMap.Parse(toggleName) == gvk)
            {
                MessageBox.Show("开关热键不能与全局总开关（" + cfg.GlobalPauseKey + "）相同，请换一个键。", "提示");
                return;
            }

            List<KeyBinding> binds = new List<KeyBinding>();
            foreach (BindingRow r in bindingRows)
            {
                string kn = VkMap.Normalize(r.key.Text);
                if (kn.Length == 0) continue;
                if (VkMap.Parse(kn) == 0)
                {
                    MessageBox.Show("有一组连发按键无法识别：" + r.key.Text + "\n\n点框后直接按键即可填入。", "提示");
                    return;
                }
                binds.Add(new KeyBinding(kn, (int)r.interval.Value));
            }
            if (binds.Count == 0)
            {
                MessageBox.Show("至少需要一组连发绑定（按键 + 间隔）。", "提示");
                return;
            }

            Profile p = FindProfile(w.Pid);
            if (p == null)
            {
                p = new Profile();
                p.Pid = w.Pid;
                cfg.Profiles.Add(p);
            }
            p.WindowTitle = w.Title;
            p.ProcessName = w.ProcessName;
            p.ToggleKey = toggleName;
            p.Bindings = binds;
            p.SendKey = binds[0].Key;
            p.IntervalMs = binds[0].IntervalMs;
            p.UseScanCode = chkScan.Checked;
            p.SendMode = Clamp(cmbMode.SelectedIndex);
            p.PauseWhenMinimized = chkPauseMin.Checked;
            p.Enabled = chkEnabled.Checked;

            if (IsRunning(w.Pid))
            {
                StopPid(w.Pid);
                if (p.Enabled) StartProfile(p);
            }

            Store.Save(cfg);
            RefreshWindows();
            UpdateStatusSafe("✔ 已保存：" + SafeTitle(p) + "  热键 " + p.ToggleKey + "  →  " + p.BindingsText());
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            WinInfo w = lv.SelectedTag as WinInfo;
            if (w == null)
            {
                MessageBox.Show("请先在列表中选择一个窗口。", "提示");
                return;
            }
            Profile p = FindProfile(w.Pid);
            if (p == null)
            {
                MessageBox.Show("该窗口还没有按键方案，请先填好参数点“保存方案”。", "提示");
                return;
            }
            ToggleProfile(p);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            WinInfo w = lv.SelectedTag as WinInfo;
            if (w == null) return;
            for (int i = cfg.Profiles.Count - 1; i >= 0; i--)
            {
                if (cfg.Profiles[i].Pid == w.Pid) cfg.Profiles.RemoveAt(i);
            }
            StopPid(w.Pid);
            repeaters.Remove(w.Pid);
            Store.Save(cfg);
            RefreshWindows();
            UpdateStatusSafe("已删除该窗口的按键方案");
        }

        private void UpdateStatus()
        {
            string pause = string.IsNullOrEmpty(cfg.GlobalPauseKey) ? "（未设置）" : cfg.GlobalPauseKey;
            string cfgNote = Store.UsingAppData
                ? "；配置保存于 %APPDATA%\\PoisonKing（程序目录不可写，已自动切换）"
                : "";
            UpdateStatusSafe("已配置 " + cfg.Profiles.Count + " 个方案，运行中 " + RunningCount() +
                " 个；总开关 " + pause + "（按=全停并记住，再按=恢复）；F12 急停；双击列表行=启停。" +
                (hookEnabled ? "" : "（按键接管当前关闭）") + cfgNote);
        }

        private void UpdateStatusSafe(string text)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.BeginInvoke((MethodInvoker)delegate { lblStatus.Text = text; });
            }
            else
            {
                lblStatus.Text = text;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (KeyValuePair<int, Repeater> kv in repeaters)
            {
                if (kv.Value != null) kv.Value.Stop();
            }
            if (hook != null) hook.Dispose();
        }

        #region 窗口枚举

        public class WinInfo
        {
            public int Pid;
            public string Title;
            public string ProcessName;
        }

        private static List<WinInfo> EnumGameWindows(bool onlyGame)
        {
            List<WinInfo> result = new List<WinInfo>();
            HashSet<int> seen = new HashSet<int>();
            int self = Process.GetCurrentProcess().Id;

            Native.EnumWindows(delegate(IntPtr hWnd, IntPtr lParam)
            {
                if (!Native.IsWindowVisible(hWnd)) return true;
                StringBuilder sb = new StringBuilder(512);
                Native.GetWindowText(hWnd, sb, sb.Capacity);
                string title = sb.ToString();
                if (string.IsNullOrEmpty(title)) return true;

                uint pid;
                Native.GetWindowThreadProcessId(hWnd, out pid);
                if ((int)pid == self) return true;
                if (seen.Contains((int)pid)) return true;

                string pname = "";
                try { pname = Process.GetProcessById((int)pid).ProcessName; }
                catch { }
                if (string.IsNullOrEmpty(pname)) pname = Native.QueryProcessName((int)pid);   // 提权进程兑底
                if (string.IsNullOrEmpty(pname)) pname = "(受限)";   // 不再丢弃窗口

                if (onlyGame)
                {
                    bool isGame = pname.IndexOf("JX3", StringComparison.OrdinalIgnoreCase) >= 0
                        || title.IndexOf("剑网", StringComparison.Ordinal) >= 0
                        || title.IndexOf("剑侠", StringComparison.Ordinal) >= 0
                        || title.IndexOf("JX3", StringComparison.OrdinalIgnoreCase) >= 0;
                    if (!isGame) return true;
                }

                WinInfo w = new WinInfo();
                w.Pid = (int)pid;
                w.Title = title;
                w.ProcessName = pname;
                result.Add(w);
                seen.Add((int)pid);
                return true;
            }, IntPtr.Zero);

            return result;
        }

        /// <summary>游戏窗口排序权重（1 = 剑网类，排最前）</summary>
        private static int GameRank(WinInfo w)
        {
            if (w == null) return 0;
            if (!string.IsNullOrEmpty(w.ProcessName) && w.ProcessName.IndexOf("JX3", StringComparison.OrdinalIgnoreCase) >= 0) return 1;
            if (!string.IsNullOrEmpty(w.Title) && (w.Title.IndexOf("剑网", StringComparison.Ordinal) >= 0 || w.Title.IndexOf("剑侠", StringComparison.Ordinal) >= 0)) return 1;
            return 0;
        }

        private static string GetTitleByPid(int pid)
        {
            string found = "";
            Native.EnumWindows(delegate(IntPtr hWnd, IntPtr lParam)
            {
                uint p;
                Native.GetWindowThreadProcessId(hWnd, out p);
                if ((int)p != pid) return true;
                if (!Native.IsWindowVisible(hWnd)) return true;
                StringBuilder sb = new StringBuilder(256);
                Native.GetWindowText(hWnd, sb, sb.Capacity);
                if (sb.Length > 0)
                {
                    found = sb.ToString();
                    return false;
                }
                return true;
            }, IntPtr.Zero);
            return found;
        }

        #endregion
    }

    internal static class ProgramEntry
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
