using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FastKing
{
    public class TimeCheck
    {
        Timer myTimer;
        int interval;
        int targetHour;
        int targetMinute;

        public Action<string> OnTimeStrUpdate;

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

        public TimeCheck(int second, int targetHour = 23, int targetMinute = 30)
        {
            this.interval = second * 1000;
            this.targetHour = targetHour;
            this.targetMinute = targetMinute;
            TimeUpdateCheck();

            TimerInitial();
            myTimer.Start();
        }

        private void TimerInitial()
        {
            myTimer = new Timer(interval)
            {
                AutoReset = true
            };

            myTimer.Elapsed += new ElapsedEventHandler(OnElapsed);
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            TimeUpdateCheck();
        }

        private void TimeUpdateCheck()
        {
            string timeStr = DateTime.Now.ToLongTimeString();

            int hour = Convert.ToInt32(timeStr.Split(':')[0]);
            int minute = Convert.ToInt32(timeStr.Split(':')[1]);
            int second = Convert.ToInt32(timeStr.Split(':')[2]);

            OnTimeStrUpdate?.Invoke($"{FS(hour)}:{FS(minute)}:{FS(second)}");

            bool b1 = hour >= 0 && hour < 7;
            bool b2 = hour >= targetHour && minute >= targetMinute;

            if (b1 || b2)
            {
                SetSuspendState(false, true, true);
            }
        }

        private string FS(int t)
        {
            return t < 10 ? $"0{t}" : t.ToString();
        }
    }
}
