# -*- coding: utf-8 -*-
"""
obfuscate.py — 符号重命名混淆器（发布用）
读取当前目录的 Program.cs / ProfileStore.cs / MainForm.cs，
把所有用户自定义的 类型/方法/属性/字段 名替换为无意义短名（z0, z1, ...），
输出到 build/src/。字符串字面量、注释、JSON 键名一律不动，
因此 config.json 兼容性和 UI 文案不受影响。
"""
import io
import os
import re
import sys

SRC_DIR = "src"
SRC_FILES = ["Program.cs", "ProfileStore.cs", "MainForm.cs"]

# 不参与重命名的保留名（入口点 / 接口映射 / 框架同名成员）
EXCLUDE = {
    "Main",          # 入口点
    "Dispose",       # IDisposable 映射
    "Enabled",       # Control.Enabled 同名
    "Start", "Stop", # WinForms Timer 同名方法，避免误伤
    "OnPaint", "OnMouseEnter", "OnMouseLeave", "OnEnabledChanged",  # 框架虚方法
    "OnMouseDown", "OnMouseUp", "OnMouseMove", "OnMouseWheel",      # 框架虚方法
    "OnResize", "OnFontChanged", "OnDoubleClick", "OnClick",        # 框架虚方法
    "SelectedIndex", "SelectedIndexChanged",                        # ComboBox 等框架成员同名
    "Key",           # KeyValuePair.Key 等框架成员同名
    "Tick",          # Timer.Tick 事件同名
    "ProcessName",   # Process.ProcessName 同名
}

# 参与重命名的用户符号（与源码逐一对应，漏一个只是名字保留，多一个会编译失败）
NAMES = """
WinProfile
MainForm BufferedListView RoundButton BannerPanel CardPanel ThemePalette Themes BindingRow WinInfo
ProgramEntry Native KeyHook Repeater KeyBinding Profile AppConfig Store MiniJson VkMap
InputUnion MOUSEINPUT HARDWAREINPUT KEYBDINPUT KBDLLHOOKSTRUCT RECT
EnumWindowsProc LowLevelKeyboardProc Slot
TBack TCard TCardBorder TBtnFill TBtnHover TBtnBorder TBtnText TText TSub TRun TIdle
TAccFill TAccBorder TAccText TStatus THead
ConfigPath Load Save ReadProfile
Parse SkipWs ParseValue Match ParseObj ParseArr ParseStr ParseNum AsStr AsInt AsBool Esc
Build Normalize ToName Map
EffectiveBindings BindingsText Key IntervalMs Pid WindowTitle ProcessName
ToggleKey SendKey UseScanCode SendMode PauseWhenMinimized Bindings
Profiles ShowOnlyGame EmergencyStopVk ToggleAnywhere GlobalPauseKey ThemeName
Install HookCallback Handler
Tick StopInternal Running TargetPid SentCount LastMode LastError
ForegroundPid IsExtendedKey SendKeyPress MakeInput PostKeyPress FindMainWindow
WH_KEYBOARD_LL WM_KEYDOWN WM_KEYUP WM_SYSKEYDOWN WM_SYSKEYUP LLKHF_INJECTED
KEYEVENTF_KEYUP KEYEVENTF_SCANCODE MAPVK_VK_TO_VSC GWL_EXSTYLE WS_EX_TOOLWINDOW GW_OWNER
InitUi ApplyTheme StyleButton LoadEmbeddedSkin InstallHook OnKey ToggleGlobalPause KillAll
ToggleProfile Clamp GetRepeater IsRunning RunningCount StopPid FindProfile SafeTitle
WatchTimer_Tick UpdateRunColumn RefreshWindows ShortMode LoadSelectedToEditor
AddBindingRow ClearBindingRows CaptureKey BtnApply_Click BtnToggle_Click BtnDelete_Click
UpdateStatus UpdateStatusSafe EnumGameWindows GetTitleByPid MainForm_FormClosing
EMERGENCY_VK ModeNames
cfg hook repeaters watchTimer hookEnabled globallyPaused pausedPids
lv btnRefresh chkOnlyGame lblForeground chkHook chkAnywhere gcbPause btnGlobal cmbTheme
cmbToggle cmbMode chkScan chkEnabled chkPauseMin btnApply btnToggle btnStopAll btnDelete
lblStatus lblTip leftCard rightCard pnlBindArea flw btnAddBinding bindingRows
strip banner statusPanel theme
FillCol HoverCol StrokeCol LabelCol hover RoundPath
SkinImage Palette VersionText TitleText TitleCol
hookId proc marker
slots gate pid sendMode scan pauseWhenMinimized hwnd vk timer
host key interval
Title
All Get C
""".split()


def tokenize_and_rename(text, mapping):
    out = []
    i = 0
    n = len(text)
    mode = None  # None=code 'str' 'chr' 'line' 'block'
    ident_re = re.compile(r'[A-Za-z_][A-Za-z0-9_]*')
    while i < n:
        c = text[i]
        if mode is None:
            m = ident_re.match(text, i)
            if m:
                word = m.group(0)
                out.append(mapping.get(word, word))
                i = m.end()
                continue
            if c == '"':
                mode = 'str'
            elif c == "'":
                mode = 'chr'
            elif text.startswith('//', i):
                mode = 'line'
            elif text.startswith('/*', i):
                mode = 'block'
            out.append(c)
            i += 1
        elif mode == 'str':
            if c == '\\':
                out.append(text[i:i+2]); i += 2; continue
            if c == '"':
                mode = None
            out.append(c); i += 1
        elif mode == 'chr':
            if c == '\\':
                out.append(text[i:i+2]); i += 2; continue
            if c == "'":
                mode = None
            out.append(c); i += 1
        elif mode == 'line':
            if c == '\n':
                mode = None
            out.append(c); i += 1
        elif mode == 'block':
            if text.startswith('*/', i):
                out.append('*/'); i += 2; mode = None; continue
            out.append(c); i += 1
    return ''.join(out)


def main():
    here = os.path.dirname(os.path.abspath(__file__))
    names = [w for w in NAMES if w not in EXCLUDE]
    names = sorted(set(names))
    # 生成短名 z0..zn，避免与源码现有标识符冲突
    all_text = ''
    for f in SRC_FILES:
        p = os.path.join(here, SRC_DIR, f)
        if not os.path.exists(p):
            print('missing source:', f)
            return 1
        all_text += io.open(p, encoding='utf-8-sig').read()
    existing = set(re.findall(r'\b[A-Za-z_][A-Za-z0-9_]*\b',
                 tokenize_code_only(all_text)))
    mapping = {}
    idx = 0
    for w in names:
        while True:
            cand = 'z' + str(idx)
            idx += 1
            if cand not in existing:
                break
        mapping[w] = cand

    outdir = os.path.join(here, 'build', 'src')
    if not os.path.exists(outdir):
        os.makedirs(outdir)

    total = 0
    for f in SRC_FILES:
        src = io.open(os.path.join(here, SRC_DIR, f), encoding='utf-8-sig').read()
        out = tokenize_and_rename(src, mapping)
        # 验证：旧符号在代码区（非字符串）中应不再出现
        residual = [w for w in mapping if has_word_outside_strings(out, w)]
        if residual:
            print('FAIL: identifiers not renamed in', f, residual[:8])
            return 1
        # 写出 UTF-8 BOM + CRLF
        out = out.replace('\r\n', '\n').replace('\n', '\r\n')
        io.open(os.path.join(outdir, f), 'w', encoding='utf-8-sig', newline='').write(out)
        total += 1
        print('obfuscated ->', os.path.join('build', 'src', f))

    # 括号配对自检
    for f in SRC_FILES:
        s = io.open(os.path.join(outdir, f), encoding='utf-8-sig').read()
        code = tokenize_code_only(s)
        if code.count('{') != code.count('}'):
            print('FAIL: brace mismatch in', f)
            return 1
    print('[OK] obfuscated', total, 'files,', len(mapping), 'symbols renamed ->', outdir)
    return 0


def tokenize_code_only(text):
    """去掉字符串/字符/注释后的纯代码文本（用于标识符统计与括号检查）"""
    out = []
    i = 0
    n = len(text)
    mode = None
    while i < n:
        c = text[i]
        if mode is None:
            if c == '"':
                mode = 'str'; i += 1; out.append('""'); continue
            if c == "'":
                mode = 'chr'; i += 1; out.append("''"); continue
            if text.startswith('//', i):
                j = text.find('\n', i)
                i = n if j < 0 else j
                continue
            if text.startswith('/*', i):
                j = text.find('*/', i)
                i = n if j < 0 else j + 2
                continue
            out.append(c); i += 1
        elif mode == 'str':
            if c == '\\':
                i += 2; continue
            if c == '"':
                mode = None
            i += 1
        elif mode == 'chr':
            if c == '\\':
                i += 2; continue
            if c == "'":
                mode = None
            i += 1
    return ''.join(out)


def has_word_outside_strings(text, word):
    code = tokenize_code_only(text)
    return re.search(r'\b' + re.escape(word) + r'\b', code) is not None


if __name__ == '__main__':
    sys.exit(main())
