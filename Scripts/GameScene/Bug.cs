using System;

namespace CatchGame
{
    public class Bug : Fruit
    {
        // 虫の情報とスコア
        private enum BugsInfo
        {
            bug = -30,
            spider = -40,
            bee = -80,
            dummy_apple = -60
        }

        // 虫の状態
        private BugsInfo bugs_info;

        public Bug(int num) : base(num)
        {
            
        }

        protected override void SetTexture(int num)
        {
            index = num;
            kind = "bug";
            int rand = new Random().Next(0, 100);

            if (rand >= 0 && rand < 30)
            {
                bugs_info = BugsInfo.spider;
            }
            else if (rand >= 30 && rand < 50)
            {
                bugs_info = BugsInfo.bee;
            }
            else if (rand >= 50 && rand < 70)
            {
                bugs_info = BugsInfo.dummy_apple;
            }
            else
            {
                bugs_info = BugsInfo.bug;
            }

            score = (int)bugs_info;
        }

        protected override void ColliderPlayer()
        {
            base.ColliderPlayer();
            DataControl.Player.DamageHp(1);
        }

        public override string Info
        {
            get { return bugs_info.ToString(); }
        }
    }
}
