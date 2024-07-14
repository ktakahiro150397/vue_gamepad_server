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

            GamePadPOVDirectionStr = new Dictionary<GamePadPOVDirection, string>()
            {
                {GamePadPOVDirection.Neutral,"N"},
                {GamePadPOVDirection.Up,"↑"},
                {GamePadPOVDirection.Down,"↓"},
                {GamePadPOVDirection.Left,"←"},
                {GamePadPOVDirection.Right,"→"},
                {GamePadPOVDirection.UpRight,"↗"},
                {GamePadPOVDirection.UpLeft,"↖"},
                {GamePadPOVDirection.DownRight,"↘"},
                {GamePadPOVDirection.DownLeft,"↙"},

            };
        }

        /// <summary>
        /// 入力情報取得後、前フレームとこのフレームの入力情報が変化している場合True。
        /// </summary>
        bool IGamePadInput.IsInputChangeFromPreviousFrame => joyInfo != prevFrameJoyInfo;

        private Dictionary<GamePadPOVDirection, string> GamePadPOVDirectionStr;

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

        public string GetStructureInfoCurrentFrame()
        {
            return GetStructureInfo(joyInfo);
        }

        public string GetStructureInfoPreviousFrame()
        {
            return GetStructureInfo(prevFrameJoyInfo);
        }

        private string GetStructureInfo(JOYINFOEX joyInfo)
        {
            return $"dwSize : {joyInfo.dwSize} / " + Environment.NewLine +
                    $"dwFlags : {joyInfo.dwFlags} " + Environment.NewLine +
                    $"dwXpos : {joyInfo.dwXpos} / " + Environment.NewLine +
                    $"dwYpos : {joyInfo.dwYpos} / " + Environment.NewLine +
                    $"dwZpos : {joyInfo.dwZpos} / " + Environment.NewLine +
                    $"dwRpos : {joyInfo.dwRpos} / " + Environment.NewLine +
                    $"dwUpos : {joyInfo.dwUpos} / " + Environment.NewLine +
                    $"dwVpos : {joyInfo.dwVpos} / " + Environment.NewLine +
                    $"dwButtons : {joyInfo.dwButtons} / " + Environment.NewLine +
                    $"dwButtonNumber : {joyInfo.dwButtonNumber} / " + Environment.NewLine +
                    $"dwPOV : {joyInfo.dwPOV} / " + Environment.NewLine +
                    $"dwReserved1 : {joyInfo.dwReserved1} / " + Environment.NewLine +
                    $"dwReserved2 : {joyInfo.dwReserved2}";
        }

        //         string IGamePadInput.GetInputInfo()
        //         {

        //             IGamePadDirectionData currentDir = GetPOVDirectionFromCurrentState();


        //             var pushedButtons = GetPushedButtonsFromCurrentState();

        //             var buttonStr = string.Join(" ", pushedButtons);

        //             return $"XXX F / Changed:{joyInfo != prevFrameJoyInfo} / {GamePadPOVDirectionStr[currentDir.Direction]} / {joyInfo.dwButtonNumber} buttons / {buttonStr} " + Environment.NewLine;

        //         }

        //         public IGamePadDirectionData GetPOVDirectionFromCurrentState()
        //         {

        //             if (joyInfo.dwXpos == POVInputValues.POV_NEUTRAL_VALUE
        //                 && joyInfo.dwYpos == POVInputValues.POV_NEUTRAL_VALUE)
        //             {
        //                 return new GamePadDirectionData(GamePadPOVDirection.Neutral, settings);
        //             }

        //             //上方向入力の確認
        //             if (joyInfo.dwYpos == POVInputValues.POV_ZERO_VALUE)
        //             {
        //                 if (joyInfo.dwXpos == POVInputValues.POV_NEUTRAL_VALUE)
        //                 {
        //                     return new GamePadDirectionData(GamePadPOVDirection.Up, settings);

        //                 }
        //                 else if (joyInfo.dwXpos == POVInputValues.POV_ZERO_VALUE)
        //                 {

        //                     return new GamePadDirectionData(GamePadPOVDirection.UpLeft, settings);

        //                 }
        //                 else if (joyInfo.dwXpos == POVInputValues.POV_MAX_VALUE)
        //                 {
        //                     return new GamePadDirectionData(GamePadPOVDirection.UpRight, settings);
        //                 }
        //             }

        //             //下方向入力の確認
        //             if (joyInfo.dwYpos == POVInputValues.POV_MAX_VALUE)
        //             {
        //                 if (joyInfo.dwXpos == POVInputValues.POV_NEUTRAL_VALUE)
        //                 {
        //                     return new GamePadDirectionData(GamePadPOVDirection.Down, settings);

        //                 }
        //                 else if (joyInfo.dwXpos == POVInputValues.POV_ZERO_VALUE)
        //                 {

        //                     return new GamePadDirectionData(GamePadPOVDirection.DownLeft, settings);

        //                 }
        //                 else if (joyInfo.dwXpos == POVInputValues.POV_MAX_VALUE)
        //                 {
        //                     return new GamePadDirectionData(GamePadPOVDirection.DownRight, settings);
        //                 }
        //             }

        //             //左右入力の確認
        //             if (joyInfo.dwYpos == POVInputValues.POV_NEUTRAL_VALUE)
        //             {
        //                 if (joyInfo.dwXpos == POVInputValues.POV_ZERO_VALUE)
        //                 {
        //                     return new GamePadDirectionData(GamePadPOVDirection.Left, settings);
        //                 }
        //                 else if (joyInfo.dwXpos == POVInputValues.POV_MAX_VALUE)
        //                 {
        //                     return new GamePadDirectionData(GamePadPOVDirection.Right, settings);
        //                 }
        //             }

        //             //該当なしの場合例外
        //             throw new ApplicationException($"方向キー入力が検知できませんでした。" + Environment.NewLine +
        //                 $"入力値 dwXpos : {joyInfo.dwXpos} / dwXpos : {joyInfo.dwYpos} ");

        //         }


        //         public IList<IGamePadSingleButtonData> GetPushedButtonsFromCurrentState()
        //         {
        //             var ret = new List<IGamePadSingleButtonData>();

        //             foreach (GamePadButtons button in Enum.GetValues(typeof(GamePadButtons)))
        //             {

        //                 if (CheckIsButtonPushed(joyInfo.dwButtons, button))
        //                 {
        //                     //このボタンは押下されている

        //                     //同時押し設定のチェック
        //                     if (settings.SimultaneouslySettings.Any(elem => elem.ParentButton == button))
        //                     {
        //                         //同時押し設定あり
        //                         var targetButtons = settings.SimultaneouslySettings.First(elem => elem.ParentButton == button);

        //                         foreach (var childButton in targetButtons.ChildButtons)
        //                         {
        //                             ret.Add(new GamePadSingleButtonData(childButton, settings));
        //                         }
        //                     }
        //                     else
        //                     {
        //                         //同時押し設定なし
        //                         ret.Add(new GamePadSingleButtonData(button, settings));
        //                     }


        // ;
        //                 }
        //             }

        //             return ret;
        //         }

        /// <summary>
        /// 対象のボタンが押下されているかを確認し、その結果を返します。
        /// </summary>
        /// <param name="fieldValue"></param>
        /// <param name="button"></param>
        private bool CheckIsButtonPushed(int fieldValue, GamePadButtons button)
        {

            var checkValue = (GamePadButtons)fieldValue & button;

            return checkValue != 0;

        }



    }
}
