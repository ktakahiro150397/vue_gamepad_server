
namespace Core.PadInput
{
    interface IPadInputService
    {
        /// <summary>
        /// 次のパッド入力が行われるまで待機し、入力情報を返します。
        /// </summary>
        /// <returns></returns>
        Task<PadInputInfo> WaitForPadInput();

        /// <summary>
        /// <see cref="IPadInputService"/> で取得した入力情報をリセットします。
        /// </summary>
        void TaskReset();
    }

    public class PadInputInfo
    {
        /// <summary>
        /// パッド方向キーの入力方向。
        /// </summary>
        public PadDirection Direction { get; set; }

        /// <summary>
        /// 対応するインデックスのボタンが押下されている場合true。
        /// </summary>
        public List<bool> ButtonPressState { get; set; }

        public PadInputInfo()
        {
            ButtonPressState = new List<bool>();
        }
    }

    public enum PadDirection
    {
        Up,
        Down,
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        Neutral,
    }


}

