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
