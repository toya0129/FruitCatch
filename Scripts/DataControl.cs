using System;
using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Input;

namespace CatchGame
{
    public static class DataControl
    {
        // ゲームの状態
        public enum GameState
        {
            Title,
            Introduction,
            Game,
            Result
        }
        // 位置情報
        public enum Position
        {
            Left = 20,
            Middle = 200,
            Right = 380
        }

        private static Player player;

        // 入力で使用できるキー
        private static readonly Keys[] active_keys =
        {
            Keys.Right, Keys.Left, Keys.Space
        };

        // ウィンドウの幅
        private static readonly int window_width = 540;
        // ウィンドウの高さ
        private static readonly int window_height = 720;

        // 現在のゲームの状態
        private static GameState now_game_state = GameState.Title;

        // プレイヤーの位置とオブジェクトを消す位置
        private static int end_pos = 550;


        // ゲーム状態の遷移
        public static void NextGameState()
        {
            switch (now_game_state)
            {
                case GameState.Title:
                    now_game_state = GameState.Introduction;                    
                    break;
                case GameState.Introduction:
                    now_game_state = GameState.Game;
                    Game1.GameStart();
                    break;
                case GameState.Game:
                    now_game_state = GameState.Result;
                    break;
                case GameState.Result:
                    now_game_state = GameState.Title;
                    Game1.Clear();
                    break;
            }
        }

        // getter and setter
        public static Keys[] ActiveKeys
        {
            get { return active_keys; }
        }
        public static int WindowWidth
        {
            get { return window_width; }
        }
        public static int WindowHeight
        {
            get { return window_height; }
        }

        public static Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public static GameState NowGameState
        {
            get { return now_game_state; }
        }
        public static int EndPos
        {
            get { return end_pos; }
        }

    }
}
