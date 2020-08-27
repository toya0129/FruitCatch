using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CatchGame
{
    public class Game1 : Game
    {
        // 必要クラス
        private static InputKeyboard inputKeyboard;
        private static Timer timer;

        // 画像データ : プレイヤー
        private Texture2D player_texture;
        // 画像データ : HP
        private Texture2D player_hp_texture;
        // 画像データ : フルーツ
        private Texture2D apple_texture;
        private Texture2D grape_texture;
        private Texture2D orange_texture;
        private Texture2D strawberry_texture;
        private Texture2D suika_texture;
        // 画像データ : 虫
        private Texture2D bug_texture;
        private Texture2D spider_texture;
        private Texture2D bee_texture;
        private Texture2D dummy_apple_texture;
        //画像データ : アイテム
        private Texture2D item_crystal_texture;
        // 画像データ : 文字
        private Texture2D title_texture;
        private Texture2D title_text_texture;
        private Texture2D result_texture;
        private Texture2D result_text_texture;
        private Texture2D score_texture;
        private Texture2D time_texture;
        private Texture2D[] number_texture = new Texture2D[10];
        private Texture2D minus_texture;
        private Texture2D sec_texture;
        private Texture2D score_unit_texture;
        private Texture2D next_navi_texture;
        // 画像データ : 説明画面
        private Texture2D[] introduction_texture = new Texture2D[4];

        // キーボードの状態
        private Keys now_input_key;
        private static bool space_lock;

        // 説明画面の切り替え
        private static int introduction_count = 0;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // ゲームシーンの開始
        public static void GameStart()
        {
            // コンストラクタ
            DataControl.Player = new Player();

            ObjectGenerator.Start();
        }
        // ゲームシーンの終了
        public static void GameEnd()
        {
            ObjectGenerator.Stop();
            DataControl.NextGameState();
        }
        // データのリセット
        public static void Clear()
        {
            timer = null;
            space_lock = false;
            introduction_count = 0;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // ウィンドウサイズ
            _graphics.PreferredBackBufferHeight = DataControl.WindowHeight;
            _graphics.PreferredBackBufferWidth = DataControl.WindowWidth;

            // マウスの表示有無
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // コンストラクタ
            inputKeyboard = new InputKeyboard();

            Clear();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // 文字のロード（fontが使用できなかったため）
            title_texture = Content.Load<Texture2D>("images/text/title");
            title_text_texture = Content.Load<Texture2D>("images/text/title_text");
            result_texture = Content.Load<Texture2D>("images/text/result");
            result_text_texture = Content.Load<Texture2D>("images/text/result_text");
            score_texture = Content.Load<Texture2D>("images/text/score");
            time_texture = Content.Load<Texture2D>("images/text/time");
            score_unit_texture = Content.Load<Texture2D>("images/text/score_unit");
            sec_texture = Content.Load<Texture2D>("images/text/sec");
            next_navi_texture = Content.Load<Texture2D>("images/text/next_navi");

            // 数字画像のロード
            for (int i = 0; i < number_texture.Length; i++)
            {
                string s = "images/text/" + i.ToString();
                number_texture[i] = Content.Load<Texture2D>(s);
            }
            minus_texture = Content.Load<Texture2D>("images/text/minus");

            // 説明画面のロード
            for(int i = 1; i <= introduction_texture.Length; i++)
            {
                string s = "images/introduction/introduction_" + i.ToString();
                introduction_texture[i - 1] = Content.Load<Texture2D>(s);
            }

            // プレイヤー画像のロード
            player_texture = Content.Load<Texture2D>("images/players/dog");
            // プレイヤーHP画像のロード
            player_hp_texture = Content.Load<Texture2D>("images/mark_heart");
            // フルーツ画像のロード
            apple_texture = Content.Load<Texture2D>("images/fruits/apple");
            grape_texture = Content.Load<Texture2D>("images/fruits/grape");
            orange_texture = Content.Load<Texture2D>("images/fruits/orange");
            strawberry_texture = Content.Load<Texture2D>("images/fruits/strawberry");
            suika_texture = Content.Load<Texture2D>("images/fruits/suika");
            //　虫画像のロード
            bug_texture = Content.Load<Texture2D>("images/bugs/bug");
            spider_texture = Content.Load<Texture2D>("images/bugs/bug_spider");
            bee_texture = Content.Load<Texture2D>("images/bugs/bug_bee");
            dummy_apple_texture = Content.Load<Texture2D>("images/bugs/dummy_apple");
            // アイテム画像のロード
            item_crystal_texture = Content.Load<Texture2D>("images/items/item_crystal");
        }

        protected override void Update(GameTime gameTime)
        {
            now_input_key = inputKeyboard.ActiveKeyControl();

            // 各シーンにおける入力キーの制御
            switch (DataControl.NowGameState)
            {
                case DataControl.GameState.Title:
                    if (now_input_key == Keys.Space)
                    {
                        DataControl.NextGameState();
                    }
                    break;
                case DataControl.GameState.Introduction:
                    if (now_input_key == Keys.Space && !space_lock)
                    {
                        if(introduction_count < introduction_texture.Length - 1)
                        {
                            introduction_count++;
                        }
                        else
                        {
                            space_lock = true;
                            timer = new Timer();
                            timer.Start("down", 3);
                        }
                    }

                    if (timer != null && timer.TimerEnd)
                    {
                        DataControl.NextGameState();
                        timer = null;
                    }

                    break;
                case DataControl.GameState.Game:
                    DataControl.Player.PlayerMove(now_input_key);

                    if (now_input_key == Keys.Space)
                    {
                        Console.WriteLine("スペーーーーース");
                        GameEnd();
                    }

                    break;
                case DataControl.GameState.Result:
                    if (now_input_key == Keys.Space)
                    {
                        DataControl.NextGameState();
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.PaleGreen);

            _spriteBatch.Begin();
            switch (DataControl.NowGameState)
            {
                case DataControl.GameState.Title:
                    // タイトル
                    _spriteBatch.Draw(title_texture, new Vector2(0, 150), null, Color.White, 0.0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0.0f);
                    // 説明
                    _spriteBatch.Draw(title_text_texture, new Vector2(0, DataControl.EndPos), null, Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);
                    break;
                case DataControl.GameState.Introduction:
                    if (!space_lock)
                    {
                        _spriteBatch.Draw(introduction_texture[introduction_count], new Vector2(0, 0), null, Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);
                        if (introduction_count == introduction_texture.Length - 1)
                        {
                            _spriteBatch.Draw(title_text_texture, new Vector2(0, DataControl.EndPos), null, Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);
                        }
                        else
                        {
                            _spriteBatch.Draw(next_navi_texture, new Vector2(100, DataControl.EndPos), null, Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);
                        }
                    }
                    else
                    {
                        DrawNumberLeft(timer.NowTime, new Vector2(250, 300), null);
                    }
                    break;
                case DataControl.GameState.Game:
                    // プレイヤーの描画
                    _spriteBatch.Draw(player_texture, new Vector2((int)DataControl.Player.PlayerPosition, DataControl.EndPos), null, Color.White, 0.0f, Vector2.Zero, 0.25f, SpriteEffects.None, 0.0f);
                    // フルーツの描画
                    for (int f_i = 0; f_i < ObjectGenerator.Fruits.Count; f_i++)
                    {
                        Fruit f = ObjectGenerator.Fruits[f_i];
                        if (f != null)
                        {
                            switch (f.Info)
                            {
                                case "apple":
                                    _spriteBatch.Draw(apple_texture, new Vector2((int)f.PositionX + 30, f.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                    break;
                                case "grape":
                                    _spriteBatch.Draw(grape_texture, new Vector2((int)f.PositionX + 30, f.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                    break;
                                case "orange":
                                    _spriteBatch.Draw(orange_texture, new Vector2((int)f.PositionX + 30, f.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.3f, SpriteEffects.None, 0.0f);
                                    break;
                                case "strawberry":
                                    _spriteBatch.Draw(strawberry_texture, new Vector2((int)f.PositionX + 30, f.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);
                                    break;
                                case "suika":
                                    _spriteBatch.Draw(suika_texture, new Vector2((int)f.PositionX, f.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0.0f);
                                    break;
                            }
                        }
                    }
                    // 虫の描画
                    for (int b_i = 0; b_i < ObjectGenerator.Bugs.Count; b_i++)
                    {
                        Bug b = ObjectGenerator.Bugs[b_i];
                        if (b != null)
                        {
                            switch (b.Info)
                            {
                                case "bug":
                                    _spriteBatch.Draw(bug_texture, new Vector2((int)b.PositionX + 30, b.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                    break;
                                case "spider":
                                    _spriteBatch.Draw(spider_texture, new Vector2((int)b.PositionX + 30, b.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                    break;
                                case "bee":
                                    _spriteBatch.Draw(bee_texture, new Vector2((int)b.PositionX + 30, b.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                    break;
                                case "dummy_apple":
                                    _spriteBatch.Draw(dummy_apple_texture, new Vector2((int)b.PositionX + 30, b.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0.0f);
                                    break;
                            }
                        }
                    }
                    // アイテムの描画
                    for (int i_i = 0; i_i < ObjectGenerator.Items.Count; i_i++)
                    {
                        Item i = ObjectGenerator.Items[i_i];
                        if (i != null)
                        {
                            switch (i.Info)
                            {
                                case "crystal":
                                    _spriteBatch.Draw(item_crystal_texture, new Vector2((int)i.PositionX + 30, i.PositionY), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                    break;
                            }
                        }
                    }

                    //　プレイヤーHPの描画
                    DrawHP(DataControl.Player.HP, new Vector2(480, 0));
                    // プレイヤーの状態
                    if(DataControl.Player.AbilityKind != null)
                    {
                        switch (DataControl.Player.AbilityKind)
                        {
                            case "crystal":
                                _spriteBatch.Draw(item_crystal_texture, new Vector2(300, 0), null, Color.White, 0.0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0.0f);
                                break;
                        }
                    }

                    // 文字の描画
                    _spriteBatch.Draw(score_texture, new Vector2(0, 0), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    DrawNumberLeft(DataControl.Player.Score, new Vector2(130, 0), null);

                    break;
                case DataControl.GameState.Result:
                    // リザルト文字
                    _spriteBatch.Draw(result_texture, new Vector2(0, 0), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    // 最終成績
                    DrawNumberLeft(DataControl.Player.FinalScore, new Vector2(170, 100), score_unit_texture);
                    // スコア
                    _spriteBatch.Draw(score_texture, new Vector2(20, 200), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    DrawNumberLeft(DataControl.Player.Score, new Vector2(170, 200), score_unit_texture);
                    // タイム
                    _spriteBatch.Draw(time_texture, new Vector2(20, 300), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    DrawNumberLeft(DataControl.Player.TimeScore, new Vector2(170, 300), sec_texture);
                    // 説明
                    _spriteBatch.Draw(result_text_texture, new Vector2(0, DataControl.EndPos), null, Color.White, 0.0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0.0f);
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // DrawHelper
        private void DrawNumberLeft(int num, Vector2 start_pos, Texture2D unit)
        {
            string s = num.ToString();
            Vector2 vec2 = start_pos;

            for(int i = 0; i < s.Length; i++)
            {
                if(s[i].ToString() == "-")
                {
                    _spriteBatch.Draw(minus_texture, new Vector2(vec2.X, vec2.Y), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else
                {
                    int n = int.Parse(s[i].ToString());
                    _spriteBatch.Draw(number_texture[n], new Vector2(vec2.X, vec2.Y), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                vec2.X += 30;
            }
            if (unit != null)
            {
                _spriteBatch.Draw(unit, new Vector2(vec2.X, vec2.Y), null, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            }
        }
        private void DrawHP(int hp, Vector2 start_pos)
        {
            Vector2 vec2 = start_pos;

            for(int i = 0; i < hp; i++)
            {
                if(i != 0 && i%3 == 0)
                {
                    vec2.X = start_pos.X;
                    vec2.Y += 60;
                }
                _spriteBatch.Draw(player_hp_texture, new Vector2(vec2.X, vec2.Y), null, Color.White, 0.0f, Vector2.Zero, 0.2f, SpriteEffects.None, 0.0f);
                vec2.X -= 60;
            }
        }
    }
}
