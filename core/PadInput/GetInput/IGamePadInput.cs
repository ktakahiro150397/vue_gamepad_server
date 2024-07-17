using PadInput.Win32Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PadInput.GamePadInput
{

    /// <summary>
    /// ゲームパッド入力に関するクラスが提供する機能を表すインターフェース。
    /// </summary>
    interface IGamePadInput
    {

        /// <summary>
        /// 対象のゲームパッドの入力状態を表す構造体を返します。
        /// </summary>
        /// <param name="uJoyId">Identifier of the joystick to be queried. Valid values for uJoyID range from zero (JOYSTICKID1) to 15.</param>
        /// <returns></returns>
        public JOYINFOEX GetPadInput(int uJoyId, int dwFlags = JoyReturnFlagValues.JOY_RETURNALL);

        /// <summary>
        /// 前フレームから入力に変化がある場合はTrue。
        /// </summary>
        public bool IsInputChangeFromPreviousFrame { get; }

    }
}