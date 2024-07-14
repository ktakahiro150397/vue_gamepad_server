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

        // /// <summary>
        // /// データ取得後、その入力内容を表す文字列を取得します。
        // /// 文末に改行が含まれます。
        // /// </summary>
        // /// <returns></returns>
        // public string GetInputInfo();

        /// <summary>
        /// 現在フレームの入力情報を表す文字列を返します。
        /// </summary>
        /// <returns></returns>
        public string GetStructureInfoCurrentFrame();

        /// <summary>
        /// 前フレームの入力情報を表す文字列を返します。
        /// </summary>
        /// <returns></returns>
        public string GetStructureInfoPreviousFrame();

        // /// <summary>
        // /// インスタンスに設定されている方向キー入力の方向を返します。
        // /// </summary>
        // /// <returns></returns>
        // public IGamePadDirectionData GetPOVDirectionFromCurrentState();

        // /// <summary>
        // /// インスタンスに設定されているボタン入力のリストを返します。
        // /// </summary>
        // /// <returns></returns>
        // public IList<IGamePadSingleButtonData> GetPushedButtonsFromCurrentState();

        /// <summary>
        /// 前フレームから入力に変化がある場合はTrue。
        /// </summary>
        public bool IsInputChangeFromPreviousFrame { get; }

    }
}