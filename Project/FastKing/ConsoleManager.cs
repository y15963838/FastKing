using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;

namespace FastKing
{
    public enum Mode { fy, lt, tp, by, zh, bk, tb, cl, bi, op }
    public enum SystemOrder { nothing, sleep, shutdown}

    public class ConsoleManager
    {
        public Mode mode;
        public SystemOrder systemOrder;
        public Action<string, Color> OnDebug;
        public string lastStr = null;
        public string transMode = "zh";  //默认翻译为中文

        private string basePath;
        private string quickPath;
        private Dictionary<string, string> quickDic = new Dictionary<string, string>();

        private OpenAI openAI;
        private OpenAI2 openAI2;
        private bool isFirstChat = true;

        public ConsoleManager()
        {
            basePath = Directory.GetCurrentDirectory();
            quickPath = basePath + @"\quick";
            openAI = new OpenAI();
            openAI2 = new OpenAI2();

            mode = Mode.fy;  //默认模式

            if (!Directory.Exists(quickPath))
            {
                Directory.CreateDirectory(quickPath);
            } 
        }

        public void ShowMode()
        {
            WriteLineInColor($"当前模式：【{GetCN(mode)}】" + "\n" + 
                "(温馨提示：输入'help'以获取帮助)"
                , Color.Green);
        }

        public void PLL(string str)
        {
            str = ApplyStr(str);

            if (!string.IsNullOrEmpty(str))
            {
                lastStr = str;

                //特定模式下需要替换空格符
                if (mode != Mode.fy && mode != Mode.op)
                {
                    if(str.Contains(" ")) str = str.Replace(" ", "-");
                }

                //特定模式下输入数字，自动替换为对应字符串
                if (mode == Mode.op)
                {
                    if (str == "h") //op帮助指令。显示所有的打开方式
                    {
                        var _str = "所有快捷方式一览：" + "\n";
                        foreach (var item in quickDic)
                        {
                            string value = item.Value.Contains(quickPath) ? item.Value.Replace(quickPath, "quick目录") : item.Value;
                            _str += $"{item.Key}  ———  {value}" + "\n";
                        }
                        WriteLineInColor(_str, Color.Yellow);
                        return;
                    }

                    if (quickDic.ContainsKey(str)) str = quickDic[str];
                }

                switch (mode)
                {
                    case Mode.fy:
                        WriteLineInColor(Trans.GetResult(str, transMode), Color.White);
                        break;
                    case Mode.lt:
                        GetChatGptResponse(str);
                        break;
                    case Mode.tp:
                        GetDalleResponse(str);
                        break;
                    case Mode.by:
                        CMDHelper.OepnUrl($"https://cn.bing.com/search?q={str}");
                        break;
                    case Mode.zh:
                        CMDHelper.OepnUrl($"https://www.zhihu.com/search?q={str}");
                        break;
                    case Mode.bk:
                        CMDHelper.OepnUrl($"https://baike.baidu.com/item/{str}");
                        break;
                    case Mode.tb:
                        CMDHelper.OepnUrl($"https://tieba.baidu.com/f/search/res?qw={str}");
                        break;
                    case Mode.cl:
                        CMDHelper.OepnUrl($"http://clg88.org/search?word={str}");
                        break;
                    case Mode.bi:
                        CMDHelper.OepnUrl($"http://www.bidiii.com/search/{str}");
                        break;
                    case Mode.op:
                        CMDHelper.OpenFile(str);
                        break;
                    default:
                        break;
                }
            }
        }

        private async void GetChatGptResponse(string str)
        {
            try
            {
                if (str.EndsWith("-n"))
                {
                    isFirstChat = true;
                    str.Remove(str.Length-2, 2);
                }
                
                string str1 = "获取回应中，请等待。。。";
                string str2 = $"【当前对话：{(isFirstChat ? "新话题" : "上下文")}】";
                string str3 = "【提示：以-n结尾开启新话题】";
                WriteLineInColor(str1 + "\n" + str2 + "\n" + str3, Color.Yellow);

                
                var result = await openAI.ChatAdvance(str, isFirstChat);
                isFirstChat = false;

                if (result[0] == '\n' && result[1] == '\n')
                {
                    result = result.Remove(0, 2);  //去除
                }
                WriteLineInColor(result, Color.White);
            }
            catch (Exception e)
            {
                WriteLineInColor("错误：" + e.Message, Color.Red);
            }
        }

        private async void GetDalleResponse(string str)
        {
            try
            {
                WriteLineInColor("获取回应中，请等待。。。", Color.Yellow);

                var result = await openAI.DALLE(str);

                result.ForEach(r =>
                {
                    System.Diagnostics.Process.Start(r);
                });
            }
            catch (Exception e)
            {
                WriteLineInColor("错误：" + e.Message, Color.Red);
            }
        }


        public string ApplyStr(string str)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            
            //优先级1：判断是否为help
            if (str.ToLower() == "hp" || str.ToLower() == "help")
            {
                WriteLineInColor("输入以下字符切换不同的模式:" + "\n" +
                    "fy —— 有道翻译" + "\n" +
                    "bd —— 百度搜索" + "\n" +
                    "by —— 必应搜索" + "\n" +
                    "bk —— 百度百科" + "\n" +
                    "zh —— 知乎搜索" + "\n" +
                    "tb —— 贴吧搜索" + "\n" +
                    "lt —— AI聊天模式" + "\n" +
                    "tp —— AI作图模式" + "\n" +
                    "cl —— 磁力狗" + "\n" +
                    "bi —— 哔嘀影视" + "\n" +
                    "op —— 打开本地快捷方式" + "\n" +
                    "可输入以上 字符+空格+内容 进行快速搜索, 例如： bd 如何暴富" + "\n"
                    , Color.Yellow);
                return null;
            }


            //优先级2：判断是否为睡眠、关机指令等系统命令
            if (str.StartsWith("sleep")) systemOrder = SystemOrder.sleep;
            else if (str.StartsWith("shutdown")) systemOrder = SystemOrder.shutdown;
            else systemOrder = SystemOrder.nothing;

            if (systemOrder != SystemOrder.nothing)
            {
                try
                {
                    int delaySecond = 10;
                    if (str.Contains(" ")) //包含空格，说明后面接自定义时间
                    {
                        delaySecond = Convert.ToInt32(str.Replace($"{systemOrder} ", ""));
                    }
                    WriteLineInColor($"将在{delaySecond}秒后执行{systemOrder}", Color.Yellow);

                    switch (systemOrder)
                    {
                        case SystemOrder.sleep:
                            CMDHelper.Sleep(delaySecond);
                            break;
                        case SystemOrder.shutdown:
                            CMDHelper.ShutDown(delaySecond);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    WriteLineInColor("输入不正确:" + e.Message, Color.Red);
                }

                return null;
            }


            //判断是否为mode关键词
            foreach (Mode mode in Enum.GetValues(typeof(Mode)))
            {
                if (str == mode.ToString())
                {
                    WriteLineInColor($"已切换到“{GetCN(mode)}”模式", Color.Yellow);
                    this.mode = mode;

                    //输入恰好为关键字时，进入的特殊逻辑
                    if (mode == Mode.op)
                    {
                        UpdateQuickdic();
                        
                        DirectoryInfo directoryInfo = new DirectoryInfo(quickPath);
                        var files = directoryInfo.GetFiles();
                        
                        if (files.Length == 0)
                        {
                            WriteLineInColor($"目录：'{quickPath}' 中不存在任何快捷方式！", Color.Red);
                        }
                        else
                        {
                            var _str = "输入数字打开对应快捷方式：" + "\n";

                            for (int i = 0; i < files.Length; i++)
                            {
                                _str += $"{i}  ————  {files[i].Name.Replace(".lnk", "")}" + "\n";
                            }

                            WriteLineInColor(_str, Color.Yellow);
                        }
                    }

                    return null;
                }
                else if (str.StartsWith(mode.ToString() + " "))
                {
                    this.mode = mode;

                    if (mode == Mode.op)
                    {
                        UpdateQuickdic();
                    }

                    //返回去除关键词+空格的字符串
                    return str.Remove(0, mode.ToString().Length + 1);
                }
            }

            //返回原字段
            return str;
        }

        public string GetCN(Mode mode)
        {
            switch (mode)
            {
                case Mode.fy:
                    return "百度翻译";
                case Mode.lt:
                    return "AI聊天模式";
                case Mode.tp:
                    return "AI作图模式";
                case Mode.by:
                    return "Bing搜索";
                case Mode.bk:
                    return "百度百科";
                case Mode.zh:
                    return "知乎搜索";
                case Mode.tb:
                    return "贴吧搜索";
                case Mode.cl:
                    return "磁力狗搜索";
                case Mode.bi:
                    return "哔嘀影视";
                case Mode.op:
                    return "快捷Open";
                default:
                    return "无效模式";
            }
        }

        public void WriteLineInColor(string str, Color color)
        {
            OnDebug?.Invoke(str, color);
        }

        //切换到op模式后调用
        private void UpdateQuickdic()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(quickPath);
            var files = directoryInfo.GetFiles();
            
            if (files.Length == 0)
            {
                quickDic.Clear();
                UpdateQuickdic_Default();
                return;
            }

            UpdateQuickdic_Default();
            for (int i = 0; i < files.Length; i++)
            {
                quickDic[i.ToString()] = files[i].FullName;
            }
        }

        //默认op应用/文件夹在此方法写死
        private void UpdateQuickdic_Default()
        {
            //别忘了Form1:OnActivate加上InputBox.Focus();

            quickDic["shit"] = "shell:RecycleBinFolder";
            quickDic["op"] = quickPath;  //op打开quick文件夹
        }
    }
}