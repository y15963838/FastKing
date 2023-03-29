using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FastKing
{
    public static class CMDHelper
    {
        static Process p;

        static CMDHelper()
        {
            p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;    //不使用shell启动
            p.StartInfo.RedirectStandardInput = true;//喊cmd接受标准输入
            p.StartInfo.RedirectStandardOutput = false;//不想听cmd讲话所以不要他输出
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;//不显示窗口
        }

        private static void Common(string cmd)
        {
            p.Start();
            p.StandardInput.WriteLine(cmd + "&exit");
            p.StandardInput.AutoFlush = true;
            p.WaitForExit();
            p.Close();
        }

        public static void OepnUrl(string url)
        {
            Common("start " + url);
        }

        public static void OpenFile(string path)
        {
            //path = path[1..];  //不知道为啥首个字符是废的？

            Common("start " + path);
        }

        public static void OpenLocalFolder(string path)
        {
            //path = path[1..];

            Common($"explorer /select,{path}");
        }

        public static void ResolveMessage(string msg)
        {
            if (!msg.Contains("_"))
            {
                return;
            }
            var head = msg.Split('_')[0];
            var content = msg.Split('_')[1];

            if (head == "OpenLocalFolder")
            {
                OpenLocalFolder(content);
            }
            else if (head == "OpenLocalFile")
            {
                OpenFile(content);
            }
        }

        public static void Sleep(int second = 10)
        {
            ToDelay(() => { TimeCheck.SetSuspendState(false, true, true); }, second);
        }

        public static void ShutDown(int second = 10)
        {
            Process.Start("shutdown.exe", $" -s -t {second}");
        }

        static void ToDelay(Action action, int second)
        {
            Task.Run(() => {
                int start = Environment.TickCount;
                while (Math.Abs(Environment.TickCount - start) < second * 1000) //毫秒
                {
                    //do nothing
                }
                action?.Invoke();
            });
        }
    }
}
