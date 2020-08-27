using System;
using System.Threading;
using System.Collections.Generic;

namespace CatchGame
{
    public static class ObjectGenerator
    {
        // 生成したオブジェクトを保持
        private static List<Fruit> fruits;
        private static List<Bug> bugs;
        private static List<Item> items;

        // 必要クラス
        private static Thread generate_thread;
        private static Thread interval_thread;


        // 300 ~ 2000
        private static readonly int generate_time_limit = 300;
        private static int generate_time = 2000;

        private static bool generate_trigger;

        // 生成スタート
        public static void Start()
        {
            fruits = new List<Fruit>();
            bugs = new List<Bug>();
            items = new List<Item>();

            generate_trigger = true;
            generate_time = 1500;

            generate_thread = new Thread(new ThreadStart(() =>
            {
                GenerateStart();
            }));
            interval_thread = new Thread(new ThreadStart(() =>
            {
                Interval();
            }));

            generate_thread.Start();
            interval_thread.Start();
        }

        // 生成終了
        public static void Stop()
        {
            generate_trigger = false;
            generate_thread.Join();

            AllClear();
        }

        // デッドエリアに達したオブジェクトをリストから取り出す
        public static void Reject(int index, string kind)
        {
            try
            {
                switch (kind)
                {
                    case "fruit":
                        fruits[index] = null;
                        break;
                    case "bug":
                        bugs[index] = null;
                        break;
                    case "item":
                        items[index] = null;
                        break;
                }
            }
            catch
            {

            }
        }

        // オブジェクト生成
        private static void Generator()
        {
            int rand = new Random().Next(0, 100);
            int count;

            if (rand >= 0 && rand < 40)
            {
                count = bugs.Count;
                bugs.Add(new Bug(count));
            }
            else if (rand >= 40 && rand < 50)
            {
                count = items.Count;
                items.Add(new Item(count));
            }
            else
            {
                count = fruits.Count;
                fruits.Add(new Fruit(count));
            }
        }

        // データのリセット
        private static void AllClear()
        {
            Thread del_fruits;
            Thread del_bugs;
            Thread del_items;

            del_fruits = new Thread(new ThreadStart(() =>
            {
                ClearFruits();
            }));
            del_bugs = new Thread(new ThreadStart(() =>
            {
                ClearBugs();
            }));
            del_items = new Thread(new ThreadStart(() =>
            {
                ClearItems();
            }));

            del_bugs.Start();
            del_fruits.Start();
            del_items.Start();

            del_bugs.Join();
            del_fruits.Join();
            del_items.Join();

            fruits = new List<Fruit>();
            bugs = new List<Bug>();
            items = new List<Item>();
        }


        // スレッド
        // オブジェクトの生成
        private static void GenerateStart()
        {
            while (generate_trigger)
            {
                Thread.Sleep(generate_time);
                Generator();
            }
        }
        // 生成するインターバル
        private static void Interval()
        {
            while (generate_trigger)
            {
                Thread.Sleep(2000);

                if(generate_time >= generate_time_limit)
                {
                    generate_time -= 100;
                }
            }
        }
        // 生成したオブジェクトのリセット
        private static void ClearFruits()
        {
            for (int f = 0; f < fruits.Count; f++)
            {
                if (fruits[f] != null)
                {
                    fruits[f].ExitMove();
                }
            }
        }
        private static void ClearBugs()
        {
            for (int b = 0; b < bugs.Count; b++)
            {
                if (bugs[b] != null)
                {
                    bugs[b].ExitMove();
                }
            }
        }
        private static void ClearItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] != null)
                {
                    items[i].ExitMove();
                }
            }
        }

        // getter and setter
        public static List<Fruit> Fruits
        {
            get { return fruits; }
        }
        public static List<Bug> Bugs
        {
            get { return bugs; }
        }
        public static List<Item> Items
        {
            get { return items; }
        }
    }
}
