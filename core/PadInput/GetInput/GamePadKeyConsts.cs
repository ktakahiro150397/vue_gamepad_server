using System;
using System.Collections.Generic;
using System.Text;

namespace PadInput.GamePadInput
{

    /// <summary>
    /// 方向キー押下時の値の定数群を保持するクラス。
    /// </summary>
    public class POVInputValues
    {
        public const int POV_ZERO_VALUE = 0;
        public const int POV_MAX_VALUE = 65535;
        public const int POV_NEUTRAL_VALUE = 32767;
    }

    /// <summary>
    /// キー入力押下を判別するキー別の定数を表します。
    /// </summary>
    [Flags]
    public enum GamePadButtons
    {
        PAD_BUTTON_0 = 1,
        PAD_BUTTON_1 = 2,
        PAD_BUTTON_2 = 4,
        PAD_BUTTON_3 = 8,
        PAD_BUTTON_4 = 16,
        PAD_BUTTON_5 = 32,
        PAD_BUTTON_6 = 64,
        PAD_BUTTON_7 = 128,
        PAD_BUTTON_8 = 256,
        PAD_BUTTON_9 = 512,
        PAD_BUTTON_10 = 1024,
        PAD_BUTTON_11 = 2048,
    }

    /// <summary>
    /// ゲームパッドの方向キーを表す列挙体。
    /// </summary>
    public enum GamePadPOVDirection
    {
        DownLeft = 1,
        Down = 2,
        DownRight = 3,
        Left = 4,
        Neutral = 5,
        Right = 6,
        UpLeft = 7,
        Up = 8,
        UpRight = 9,
    }

}
