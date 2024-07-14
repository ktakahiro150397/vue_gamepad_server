using PadInput.Win32Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace PadInput.GamePadInput
{

    /// <summary>
    /// ゲームパッド入力に関する機能を提供します。
    /// </summary>
    public class GamepadInput : IGamePadInput
    {

        public GamepadInput()
        {
            joyInfo = new JOYINFOEX();
        }

        /// <summary>
        /// 入力情報取得後、前フレームとこのフレームの入力情報が変化している場合True。
        /// </summary>
        bool IGamePadInput.IsInputChangeFromPreviousFrame => joyInfo != prevFrameJoyInfo;

        /// <summary>
        /// このインスタンスの入力情報を表します。
        /// </summary>
        public JOYINFOEX joyInfo;

        /// <summary>
        /// 1フレーム前の入力情報を表します。
        /// </summary>
        private JOYINFOEX prevFrameJoyInfo;

        public JOYINFOEX GetPadInput(int uJoyId, int dwFlags = JoyReturnFlagValues.JOY_RETURNALL)
        {

            //前フレームの結果を格納
            prevFrameJoyInfo = joyInfo.GetCopyStructure();

            joyInfo.dwSize = Marshal.SizeOf(joyInfo);
            joyInfo.dwFlags = dwFlags;

            var retjoy = NativeMethods.joyGetPosEx(uJoyId, ref joyInfo);

            if (JoyGetPosExReturnValue.JOYERR_NOERROR == retjoy)
            {
                return joyInfo;
            }
            else
            {
                throw new ApplicationException($"joyGetPosEx関数でエラーが発生しました。エラーコード:{retjoy}");
            }

        }
    }
}
