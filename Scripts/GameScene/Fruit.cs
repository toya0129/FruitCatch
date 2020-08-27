using System;
using System.Threading;

namespace CatchGame
{
    public class Fruit
    {
        // フルーツ情報とスコア
        private enum FruitsInfo
        {
            apple = 10,
            grape = 20,
            orange = 30,
            strawberry = 50,
            suika = 100
        }

        protected string kind;
        protected int index;

        // フルーツの状態保持
        private FruitsInfo fruit_info;
        // スコア
        protected int score;
        // 落ちるスピード
        protected float speed = 10f;
        // 現在の座標
        private DataControl.Position position_x;
        protected float position_y = 0;

        protected bool collider_trigger;

        private Thread thread;

        public Fruit(int num)
        {
            Initialized(num);

            thread = new Thread(new ThreadStart(() =>
            {
                MoveStart();
            }));
            thread.Start();
        }

        private void Initialized(int num)
        {
            collider_trigger = true;
            SetTexture(num);
            SetPosition();
        }

        // 情報の初期化
        protected virtual void SetTexture(int num)
        {
            index = num;
            kind = "fruit";
            int rand = new Random().Next(0, 100);

            if (rand >= 0 && rand < 30)
            {
                fruit_info = FruitsInfo.grape;
            }
            else if (rand >= 30 && rand < 50)
            {
                fruit_info = FruitsInfo.orange;
            }
            else if (rand >= 50 && rand < 65)
            {
                fruit_info = FruitsInfo.strawberry;
            }
            else if (rand >= 65 && rand < 70)
            {
                fruit_info = FruitsInfo.suika;
            }
            else
            {
                fruit_info = FruitsInfo.apple;
            }
            score = (int)fruit_info;
        }
        // 位置の設定
        private void SetPosition()
        {
            int rand = new Random().Next(0, 100);

            if (rand >= 0 && rand < 30)
            {
                position_x = DataControl.Position.Middle;
            }
            else if (rand >= 30 && rand < 60)
            {
                position_x = DataControl.Position.Left;
            }
            else
            {
                position_x = DataControl.Position.Right;
            }
        }

        // スレッド
        // オブジェクトの移動
        private void MoveStart()
        {
            while (position_y <= DataControl.EndPos + 500)
            {
                if(position_y >= DataControl.EndPos && position_y <= DataControl.EndPos + 50)
                {
                    if (position_x == DataControl.Player.PlayerPosition)
                    {
                        ColliderPlayer();
                        break;
                    }
                }
                Thread.Sleep(50);
                position_y += speed;
            }

            Close();
        }

        private void Close()
        {
            ObjectGenerator.Reject(index, kind);
        }

        // オブジェクトの移動を強制終了
        public void ExitMove()
        {
            position_y = DataControl.EndPos + 600;
            collider_trigger = false;
        }

        // オブジェクトの当たり判定
        protected virtual void ColliderPlayer()
        {
            if (collider_trigger)
            {
                DataControl.Player.CalcScore(score);
            }
        }

        // getter and setter
        public float PositionY
        {
            get { return position_y; }
        }
        public DataControl.Position PositionX
        {
            get { return position_x; }
        }
        public virtual string Info
        {
            get { return fruit_info.ToString(); }
        }
        public int Score
        {
            get { return score; }
        }
    }
}
