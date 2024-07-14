

namespace Core.PadInput
{

    abstract class PadInputServiceBase : IPadInputService
    {
        protected TaskCompletionSource<PadInputInfo> _tcs = new();

        public void TaskReset()
        {
            _tcs = new();
        }

        public abstract Task<PadInputInfo> WaitForPadInput();
    }

    class TestPadInputService : PadInputServiceBase
    {

        public TestPadInputService()
        {
        }

        public override Task<PadInputInfo> WaitForPadInput()
        {
            // ここでパッド入力を待つ
            Task.Run(async () =>
            {
                await Task.Delay(1000);


                // ボタン入力をテスト用にランダム生成
                var buttonPressState = new List<bool>();
                for (int i = 0; i < 16; i++)
                {
                    buttonPressState.Add(Random.Shared.Next(0, 2) == 1);
                }

                _tcs.TrySetResult(new PadInputInfo()
                {
                    Direction = PadDirection.Down,
                    ButtonPressState = buttonPressState
                });
            });

            return _tcs.Task;
        }
    }
}
