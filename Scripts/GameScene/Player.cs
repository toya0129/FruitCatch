using System;
using System.Threading;
using Microsoft.Xna.Framework.Input;

namespace CatchGame
{
    public class Player
    {
        // プレイヤーの位置
        private DataControl.Position player_position;

        // 必要なクラス
        private Timer timer_score;
        private Thread item_thread;

        // スコア
        private int score;
        private int time_score;
        private int final_score;

        // HP情報
        private int hp;
        private int max_hp = 2;

        // プレイヤーの状態異常
        private int magnification;
        private string ability_kind;

        public Player()
        {
            Initialized();

            timer_score = new Timer();
            timer_score.Start("infinit", 0);
        }

        private void Initialized()
        {
            player_position = DataControl.Position.Middle;
            hp = max_hp;
            magnification = 1;
            final_score = 0;
            score = 0;
            ability_kind = null;
        }

        // プレイヤーの移動
        public void PlayerMove(Keys key)
        {
            switch (player_position)
            {
                case DataControl.Position.Left:
                    if (key == Keys.Right)
                    {
                        player_position = DataControl.Position.Middle;
                    }
                    break;
                case DataControl.Position.Middle:
                    if (key == Keys.Right)
                    {
                        player_position = DataControl.Position.Right;
                    }
                    else if (key == Keys.Left)
                    {
                        player_position = DataControl.Position.Left;
                    }
                    break;
                case DataControl.Position.Right:
                    if (key == Keys.Left)
                    {
                        player_position = DataControl.Position.Middle;
                    }
                    break;
            }
        }

        // プレイヤーの状態をリセット
        public void StatusReset()
        {
            magnification = 1;
            ability_kind = null;
        }

        // スコアの追加
        public void CalcScore(int num)
        {
            score += num * magnification;
        }

        // HP回復
        public void RecoverHp(int recover)
        {
            if(hp < max_hp)
            {
                this.hp += recover;
            }
        }
        // HP減少
        public void DamageHp(int damage)
        {
            if (hp >= 0)
            {
                this.hp -= damage;
            }

            CheckHp();
        }
        // アイテムの効果セット
        public void SetItemEffect(string kind, int mag, int t)
        {
            if (ability_kind == null)
            {
                magnification = mag;
                ability_kind = kind;

                item_thread = new Thread(new ThreadStart(() =>
                {
                    CheckItemTimer(t);
                }));
                item_thread.Start();
            }
        }

        // HPの判定
        private void CheckHp()
        {
            if(hp == 0)
            {
                timer_score.ExitTimer();
                time_score = timer_score.NowTime;

                final_score = score * time_score;

                Game1.GameEnd();
            }
        }
        // スレッド
        // アイテムの時間判定
        private void CheckItemTimer(int t)
        {
            int sec = t * 1000;
            Thread.Sleep(sec);
            StatusReset();
        }

        // getter and setter
        public DataControl.Position PlayerPosition
        {
            get { return player_position; }
        }
        public int HP
        {
            get { return hp; }
        }
        public int Score
        {
            get { return score; }
        }
        public int TimeScore
        {
            get { return time_score; }
        }
        public int FinalScore
        {
            get { return final_score; }
        }
        public int Magnification
        {
            get { return magnification; }
            set { magnification = value; }
        }
        public string AbilityKind
        {
            get { return ability_kind; }
        }

    }
}
