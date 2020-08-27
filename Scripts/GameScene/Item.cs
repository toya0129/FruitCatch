namespace CatchGame
{
    public class Item : Fruit
    {
        // アイテム情報
        private enum ItemsInfo
        {
            crystal = 3
        }

        // アイテムの状態保持
        private ItemsInfo items_info;
        // アイテムの影響持続時間
        private int ability_time = 5;

        public Item(int num) : base(num)
        {

        }

        protected override void SetTexture(int num)
        {
            index = num;
            kind = "item";

            items_info = ItemsInfo.crystal;
            score = (int)items_info;
        }

        protected override void ColliderPlayer()
        {
            if (collider_trigger)
            {
                DataControl.Player.SetItemEffect(items_info.ToString(), score, ability_time);
            }
        }

        public override string Info
        {
            get { return items_info.ToString(); }
        }
    }
}
