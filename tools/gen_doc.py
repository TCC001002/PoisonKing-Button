"""生成 使用说明.docx（不依赖 python-docx，直接拼 OOXML）"""
import zipfile, os, re

TITLE = "PoisonKing按键辅助 v1.0"
SUB = "使用说明书 · 作者：柒韵-龙争虎斗 · 2026-10"

# (level, text)  level: 0=大标题 1=一级标题 2=二级标题 3=正文 4=小字注释 5=项目符号
DOC = [
    (0, TITLE),
    (4, SUB),

    (1, "重要提示（使用前必读）"),
    (3, "本工具会自动产生按键操作，属于自动化辅助工具。任何网络游戏的服务条款通常都禁止此类行为，使用本工具可能违反游戏用户协议，并导致角色被警告、临时封禁甚至永久封禁——该风险由使用者自行承担，作者不承担任何责任。请知悉并接受后再使用。"),
    (3, "请只从作者公布的官方渠道下载本软件，并核对 SHA256 校验值（见文末「文件完整性校验」章节），谨防被他人重打包捆绑恶意代码后冒名分发。"),

    (1, "一、这个软件是什么"),
    (3, "「PoisonKing按键辅助」是一款 Windows 平台的多窗口按键工具：可以为每个窗口单独指定一套按键方案，让多个窗口同时按各自的节奏自动连发，切到别的窗口、甚至把游戏窗口拖到后面，连发也不会中断。"),
    (3, "它不修改游戏文件、不读取游戏内存、不注入 DLL，只向目标窗口发送标准的键盘按键消息。程序为单文件绿色版，不写注册表、不装服务，拷到别的电脑双击即用。"),

    (1, "二、开发目的"),
    (3, "做这个工具，起因很具体：剑网3 双开时，两个角色都要挂机做日常，手动切窗口按键效率极低，而且一切窗口连发就断。市面上的小工具要么只支持单窗口，要么必须一直保持窗口在前台，用起来别扭。"),
    (3, "于是定了三个目标："),
    (5, "每个窗口一套独立方案：不同账号、不同窗口，热键互不冲突；"),
    (5, "后台继续运行：切走、最小化也不断，这是最核心的诉求；"),
    (5, "拿来就能用：单文件、免安装、配置随身带，换电脑不用重新折腾。"),

    (1, "三、开发经历"),
    (2, "1. 从「能不能改别人的软件」开始"),
    (3, "最初的想法是给现成的按键小工具加功能，后来判断不如自己写一个：别人的程序结构不透明，加功能等于重造，而且加密后维护成本更高。于是决定用 C# 从零实现。"),
    (2, "2. 第一版：能按，但只对前台窗口有效"),
    (3, "用全局键盘钩子（WH_KEYBOARD_LL）捕获热键，再用 SendInput 模拟按键。很快发现问题：SendInput 只能往当前激活窗口发送，一旦切窗口，连发就打到别的程序上去了。"),
    (2, "3. 关键突破：后台消息投递"),
    (3, "改用 PostMessage 直接向目标窗口句柄投递按键消息，并用「按进程 ID 区分窗口」的方式管理多套方案。双开实测通过：两个窗口不同热键、同一个连发键，互不干扰，切到别的窗口连发照旧。"),
    (3, "这一步也踩了不少坑：INPUT 结构体在 64 位下大小必须是 40 字节；部分窗口一个消息不够，需要按下、抬起成对投递；游戏以管理员权限启动时，工具也必须提权，否则系统直接丢弃消息。"),
    (2, "4. 界面反复重做"),
    (3, "功能通了之后，界面改了七八轮：最初控件互相遮盖、缩小窗口文字被裁、换到小屏界面超大、列表行画不出来……最后把系统列表控件整个换成自绘控件（自己画表头、行、滚动条），才彻底稳定。中间也补齐了五套配色、DPI 自适应、窗口尺寸记忆。"),
    (2, "5. 发布与收尾"),
    (3, "为了让别人也能直接用：内置 JSON 解析去掉额外依赖、把图片资源内嵌进 exe、对代码做符号混淆、打包成一键生成的压缩包。版本号定为 1.0，代表功能闭环、可以日常使用。"),

    (1, "四、核心功能"),
    (5, "多窗口并发：每个窗口一套方案，各自热键、各自连发；"),
    (5, "多组连发：一个方案可配多组按键，每组独立间隔（10–5000ms）；"),
    (5, "后台投递：切窗口、最小化都不中断（可选自动 / 仅前台 / 后台三种模式）；"),
    (5, "全局急停：F12 一键全停，总开关热键可暂停/恢复；"),
    (5, "自绘窗口列表：序号、PID、标题、进程一目了然，点表头可排序；"),
    (5, "五套配色：热烈·朝霞 / 护眼·豆绿 / 跟随系统 / 黑暗·墨夜 / 极客·终端；"),
    (5, "绿色单文件：不写注册表，配置存在 exe 同目录 config.json。"),

    (1, "五、第一次使用（三步上手）"),
    (2, "第 1 步：打开程序"),
    (3, "先开游戏、登录角色，窗口保持可见（不要最小化到托盘），再双击 PoisonKing按键辅助.exe。若出现「Windows 已保护你的电脑」，点「更多信息 → 仍要运行」。若游戏是以管理员身份启动的，本工具也要右键「以管理员身份运行」，否则按键送不进去。"),
    (2, "第 2 步：选窗口、配方案"),
    (3, "点左上角「刷新窗口」，列表按「游戏窗口优先」排列，剑网3 会排在最前面。单击选中一个窗口，然后在「① 方案配置」里设置开关热键（点输入框后直接按键，F1–F12 都支持）、投递方式（推荐「自动」或「后台投递」），点「保存方案」。"),
    (2, "第 3 步：加连发键、启动"),
    (3, "在「② 连发绑定」点「＋ 添加一组」，选按键、填间隔毫秒数，可加多组；再点一次「保存方案」。最后双击列表该行（或点「启动 / 停止选中窗口」），运行状态列会变成「▶ 运行中」。对第二个窗口重复以上步骤，用不同的开关热键即可。"),

    (1, "六、几个要点"),
    (5, "投递方式：自动（智能判断）/ 仅前台（切走就停）/ 后台投递（双开必备）；"),
    (5, "以扫描码发送：部分游戏只认扫描码，部分只认虚拟键码，遇到不识别就切换试试；"),
    (5, "最小化时暂停：勾上后窗口最小化时停止连发，省资源；"),
    (5, "行列操作：点表头 PID / 窗口标题 / 进程排序，再点切换升降序，点「序号」恢复默认；"),
    (5, "滚动：列表与卡片内容超出时都可用鼠标拖动滚动条。"),

    (1, "七、能用在其他程序上吗"),
    (3, "可以。本工具不限定游戏：列表里出现的任何可见窗口（记事本、聊天窗、办公软件、其他游戏）都能选中并配置按键方案。需要注意两点："),
    (5, "目标程序若以管理员权限运行，本工具也要以管理员身份运行；"),
    (5, "部分带反外挂系统的网游会拦截后台按键消息，那种情况只能用「仅前台」模式。"),
    (3, "换句话说，它是一个通用的「多窗口后台连发器」，剑网3 只是最初的使用场景。"),

    (1, "八、文件说明"),
    (5, "PoisonKing按键辅助.exe —— 主程序（单文件，无需安装运行库）；"),
    (5, "config.json —— 配置（方案、热键、主题），自动生成；"),
    (5, "使用说明.docx —— 本文档；"),
    (5, "README_RELEASE.txt —— 简短说明。"),
    (3, "换电脑：整个文件夹拷过去即可（Win10/11 自带 .NET Framework 4.8）。请放在有写权限的目录；若放在 C:\\Program Files 等只读目录，配置会自动改存到 %APPDATA%\\PoisonKing，状态栏会有提示。"),

    (1, "九、常见问题"),
    (2, "列表里没有剑网3？"),
    (3, "确认游戏窗口没有最小化；勾掉「仅显示剑网3窗口」看总数是否合理；若游戏以管理员启动，请以管理员身份运行本工具后重新刷新。"),
    (2, "启动了但按热键没反应？"),
    (3, "先确认点过「保存方案」；检查状态栏是否提示「目标是管理员权限」；试一次「后台投递」模式；确认「热键全局生效」已勾选。"),
    (2, "切到别的窗口连发就停？"),
    (3, "投递方式改成「自动」或「后台投递」。「仅前台」模式切走就会停，这是设计如此。"),
    (2, "游戏里按键变成动作而不是技能？"),
    (3, "试试切换「以扫描码发送」开关。"),
    (2, "配置保存失败？"),
    (3, "v1.0 起已自动处理：程序目录没有写权限（例如 C:\\Program Files）时，配置自动改存到 %APPDATA%\\PoisonKing\\config.json，状态栏会有提示，两个位置都能被程序识别。"),

    (1, "十、免责声明"),
    (3, "0. 封号风险提示：本软件属于自动化按键工具，使用它可能违反游戏厂商用户协议，可能导致账号被警告、封号（含永久封禁）等处罚，该风险由使用者自行承担，作者不承担任何责任。"),
    (3, "1. 本软件为个人自用工具，按「现状」提供，作者不对使用效果、稳定性作出任何明示或暗示的保证。"),
    (3, "2. 本软件不读写游戏内存、不修改游戏文件、不注入进程、不提供任何自动打怪、透视、加速等作弊功能；但它确实会自动产生按键操作，属于自动化工具。"),
    (3, "3. 任何网络游戏的服务条款通常禁止使用第三方自动化工具。使用本软件可能导致账号被警告、封禁或其他处罚，风险由使用者自行承担，作者不承担任何责任。"),
    (3, "4. 因使用本软件造成的账号损失、数据丢失、设备故障、时间与经济损失，作者一律不承担赔偿责任。"),
    (3, "5. 请勿将本软件用于任何违反当地法律法规、侵犯他人权益的场景。下载、安装、运行即视为已阅读并同意本声明。"),
    (3, "6. 本软件仅供学习交流与技术研究，请在下载后 24 小时内删除的约定同样适用于本软件副本。"),

    (1, "十一、关于作者"),
    (5, "作　者：柒韵-龙争虎斗"),
    (5, "服务器：剑网3 · 电信区 · 龙争虎斗"),
    (5, "版　本：v1.0（2026-10）"),
    (3, "作者信息出现在三处，可据此确认来源：软件标题栏；exe 文件属性（右键 → 属性 → 详细信息）；本文档末尾。二次分发请保留作者信息。"),
    (2, "开发方式说明"),
    (3, "本软件的代码、界面与本文档，均由作者提出需求、判断方案、逐轮验收，在人工智能（AI）辅助下共同完成：AI 负责编码实现、排错与文档初稿，作者负责需求定义、方案取舍与实机验证。"),
    (4, "本文档由人工指导、AI 辅助开发整理。"),
]

# 文件完整性校验章节：SHA256 由 release.bat 在每次构建后通过环境变量注入
PK_SHA = (os.environ.get("PK_SHA256") or "").strip()
DOC += [
    (1, "十二、文件完整性校验（SHA256）"),
    (3, "为防止有人把本软件重新打包、捆绑木马后冒名分发，官方发布时会公布主程序的 SHA256 校验值。校验方法：在解压目录打开命令提示符（cmd）或 PowerShell，执行："),
    (3, "certutil -hashfile \"PoisonKing按键辅助.exe\" SHA256"),
    (3, "把输出的值与下方（以及压缩包内 SHA256.txt、官方发布帖）逐一比对，完全一致才是官方原版；不一致请立即删除，不要运行。"),
]
if PK_SHA:
    DOC.append((3, "本次发布（v1.0）官方 SHA256 校验值："))
    DOC.append((3, PK_SHA))
else:
    DOC.append((3, "（本次生成文档时未提供校验值，请以压缩包内 SHA256.txt 与官方发布帖公布的值为准。）"))


def esc(s):
    s = s.replace('&', '&amp;').replace('<', '&lt;').replace('>', '&gt;')
    return s


def para(level, text):
    style = {0: 'Title', 1: 'Heading1', 2: 'Heading2', 3: 'Body', 4: 'Small', 5: 'Bullet'}[level]
    ppr = '<w:pPr><w:pStyle w:val="%s"/></w:pPr>' % style
    return '<w:p>%s<w:r><w:t xml:space="preserve">%s</w:t></w:r></w:p>' % (ppr, esc(text))


body = ''.join(para(l, t) for l, t in DOC)

document = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
<w:body>%s<w:sectPr><w:pgSz w:w="11906" w:h="16838"/><w:pgMar w:top="1440" w:right="1247" w:bottom="1440" w:left="1247" w:header="851" w:footer="992" w:gutter="0"/></w:sectPr></w:body>
</w:document>''' % body

styles = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<w:styles xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
<w:docDefaults>
  <w:rPrDefault><w:rPr><w:rFonts w:ascii="Microsoft YaHei" w:hAnsi="Microsoft YaHei" w:eastAsia="Microsoft YaHei" w:cs="Microsoft YaHei"/><w:sz w:val="21"/><w:szCs w:val="21"/></w:rPr></w:rPrDefault>
  <w:pPrDefault><w:pPr><w:spacing w:line="300" w:lineRule="auto" w:after="80"/></w:pPr></w:pPrDefault>
</w:docDefaults>
<w:style w:type="paragraph" w:default="1" w:styleId="Body"><w:name w:val="Body"/><w:pPr><w:spacing w:after="90" w:line="300" w:lineRule="auto"/></w:pPr><w:rPr><w:sz w:val="21"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Title"><w:name w:val="Title"/><w:pPr><w:spacing w:before="120" w:after="120"/><w:jc w:val="center"/></w:pPr><w:rPr><w:b/><w:sz w:val="40"/><w:color w:val="1F3864"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Heading1"><w:name w:val="Heading 1"/><w:pPr><w:spacing w:before="260" w:after="110"/><w:outlineLvl w:val="0"/></w:pPr><w:rPr><w:b/><w:sz w:val="28"/><w:color w:val="1F3864"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Heading2"><w:name w:val="Heading 2"/><w:pPr><w:spacing w:before="160" w:after="70"/><w:outlineLvl w:val="1"/></w:pPr><w:rPr><w:b/><w:sz w:val="23"/><w:color w:val="2E5496"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Small"><w:name w:val="Small"/><w:pPr><w:jc w:val="center"/><w:spacing w:after="240"/></w:pPr><w:rPr><w:sz w:val="18"/><w:color w:val="808080"/></w:rPr></w:style>
<w:style w:type="paragraph" w:styleId="Bullet"><w:name w:val="Bullet"/><w:pPr><w:ind w:left="420" w:hanging="240"/><w:spacing w:after="70"/></w:pPr></w:style>
</w:styles>'''

ct = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
<Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
<Default Extension="xml" ContentType="application/xml"/>
<Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
<Override PartName="/word/styles.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.styles+xml"/>
<Override PartName="/docProps/core.xml" ContentType="application/vnd.openxmlformats-package.core-properties+xml"/>
<Override PartName="/docProps/app.xml" ContentType="application/vnd.openxmlformats-officedocument.extended-properties+xml"/>
</Types>'''

rels = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
<Relationship Id="rId2" Type="http://schemas.openxmlformats.org/package/2006/relationships/metadata/core-properties" Target="docProps/core.xml"/>
<Relationship Id="rId3" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/extended-properties" Target="docProps/app.xml"/>
</Relationships>'''

doc_rels = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
<Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles" Target="styles.xml"/>
</Relationships>'''

core = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<cp:coreProperties xmlns:cp="http://schemas.openxmlformats.org/package/2006/metadata/core-properties" xmlns:dc="http://purl.org/dc/elements/1.1/" xmlns:dcterms="http://purl.org/dc/terms/" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
<dc:title>%s</dc:title><dc:creator>柒韵-龙争虎斗</dc:creator><cp:lastModifiedBy>柒韵-龙争虎斗</cp:lastModifiedBy>
<cp:keywords>按键辅助 连发 多窗口</cp:keywords><dc:description>使用说明书（人工指导、AI 辅助编写）</dc:description>
</cp:coreProperties>''' % esc(TITLE)

app = '''<?xml version="1.0" encoding="UTF-8" standalone="yes"?>
<Properties xmlns="http://schemas.openxmlformats.org/officeDocument/2006/extended-properties"><Application>Microsoft Office Word</Application><Company>柒韵-龙争虎斗</Company></Properties>'''

out = os.path.join('docs', '使用说明.docx')
os.makedirs('docs', exist_ok=True)
if os.path.exists(out):
    os.remove(out)
with zipfile.ZipFile(out, 'w', zipfile.ZIP_DEFLATED) as z:
    z.writestr('[Content_Types].xml', ct)
    z.writestr('_rels/.rels', rels)
    z.writestr('word/document.xml', document)
    z.writestr('word/styles.xml', styles)
    z.writestr('word/_rels/document.xml.rels', doc_rels)
    z.writestr('docProps/core.xml', core)
    z.writestr('docProps/app.xml', app)
print('written', out, os.path.getsize(out), 'bytes;', len(DOC), 'paragraphs')
