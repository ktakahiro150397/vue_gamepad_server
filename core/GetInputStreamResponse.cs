
using PadInput.GamePadInput;

namespace Core.Response
{
    /// <summary>
    /// フロントエンドへ返却するパッド入力情報
    /// </summary>
    class GetInputStreamResponse
    {
        public GamePadPOVDirection gamePadPOVDirection;

        public List<bool> buttonState;


        public int direction_state
        {
            get
            {
                return (int)gamePadPOVDirection;
            }
        }

        public bool[] button_state
        {
            get
            {
                return buttonState.ToArray();
            }
        }

        public long time_stamp { get; set; }

        public GetInputStreamResponse()
        {
            buttonState = Enumerable.Repeat(false, 16).ToList();
        }

        /// <summary>
        /// 現在のインスタンスに方向キーの状態を設定します。
        /// </summary>
        /// <param name="dwPOV"></param>
        public void SetDirectionState(int dwPOV)
        {
            switch (dwPOV)
            {
                case 0:
                    gamePadPOVDirection = GamePadPOVDirection.Up;
                    break;
                case 4500:
                    gamePadPOVDirection = GamePadPOVDirection.UpRight;
                    break;
                case 9000:
                    gamePadPOVDirection = GamePadPOVDirection.Right;
                    break;
                case 13500:
                    gamePadPOVDirection = GamePadPOVDirection.DownRight;
                    break;
                case 18000:
                    gamePadPOVDirection = GamePadPOVDirection.Down;
                    break;
                case 22500:
                    gamePadPOVDirection = GamePadPOVDirection.DownLeft;
                    break;
                case 27000:
                    gamePadPOVDirection = GamePadPOVDirection.Left;
                    break;
                case 31500:
                    gamePadPOVDirection = GamePadPOVDirection.UpLeft;
                    break;
                default:
                    gamePadPOVDirection = GamePadPOVDirection.Neutral;
                    break;
            }
        }

        /// <summary>
        /// 現在のインスタンスにボタンの状態を設定します。
        /// </summary>
        /// <param name="dwButtonNumber"></param>
        public void SetButtonState(int dwButtons)
        {
            for (int i = 0; i < 16; i++)
            {
                buttonState[i] = (dwButtons & (1 << i)) != 0;
            }
        }
    }
}