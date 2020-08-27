using System.Threading;

namespace CatchGame
{
    public class Timer
    {
        // セットした時間
        private int time;
        // 現在の時間
        private int now;
        private Thread thread;

        // セットした時間になった判定
        private bool timer_end;

        public Timer()
        {

        }

        // タイマースタート
        public void Start(string kind, int time_sec)
        {
            time = time_sec;
            timer_end = false;

            switch (kind)
            {
                case "up":
                    thread = new Thread(new ThreadStart(() =>
                    {
                        now = 0;
                        CountUpStart();
                    }));
                    break;
                case "down":
                    thread = new Thread(new ThreadStart(() =>
                    {
                        now = time;
                        CountDownStart();
                    }));
                    break;
                case "infinit":
                    thread = new Thread(new ThreadStart(() =>
                    {
                        now = 0;
                        InfinitStart();
                    }));
                    break;
            }

            thread.Start();
        }

        // タイマーの強制終了
        public void ExitTimer()
        {
            timer_end = true;
            thread.Join();
        }

        // カウントアップタイマー
        private void CountUpStart()
        {
            while(!timer_end)
            {
                if (now > time)
                {
                    timer_end = true;
                }
                else
                {
                    Thread.Sleep(1000);
                    now++;
                }
            }
        }
        // カウントダウンタイマー
        private void CountDownStart()
        {
            while (!timer_end)
            {
                if (now < 0)
                {
                    timer_end = true;
                }
                else
                {
                    Thread.Sleep(1000);
                    now--;
                }
            }
        }
        // 制限のないカウントアップタイマー
        private void InfinitStart()
        {
            while (!timer_end)
            {
                Thread.Sleep(1000);
                now++;
            }
        }

        // getter and setter
        public int NowTime
        {
            get { return now; }
        }
        public bool TimerEnd
        {
            get { return timer_end; }
        }

    }
}
