using Microsoft.Xna.Framework.Input;

namespace CatchGame
{
    public class InputKeyboard
    {
        // 入力で使用するキーを設定
        private Keys[] control_keys;

        // nullをリターンできないため設定
        private readonly Keys null_keys = Keys.Up;

        private KeyboardState keyboard_state;

        // 入力受付状態であるかの判定
        private bool key_down_flag = true;

        public InputKeyboard()
        {
            control_keys = DataControl.ActiveKeys;
        }

        // 利用できるキーが入力されたか
        public Keys ActiveKeyControl()
        {
            if (key_down_flag)
            {
                keyboard_state = Keyboard.GetState();

                foreach (Keys k in keyboard_state.GetPressedKeys())
                {
                    foreach(Keys c in control_keys)
                    {
                        if (k == c)
                        {
                            key_down_flag = false;
                            return c;
                        }
                    }
                }
            }

            KeybordUp();

            return null_keys;
        }

        // 連打防止
        private void KeybordUp()
        {
            foreach(Keys key in control_keys)
            {
                if (!Keyboard.GetState().IsKeyUp(key))
                {
                    return;
                }
            }
            key_down_flag = true;
        }
    }
}