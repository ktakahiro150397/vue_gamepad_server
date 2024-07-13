
namespace Core.ItemService
{
    public class ItemService
    {
        private TaskCompletionSource<SendItem?> _tcs = new();
        private long _id = 0;

        public ItemService() { }

        public void Reset()
        {
            _tcs = new();
        }

        public void NotifyNewItemAvailable()
        {
            _tcs.TrySetResult(new SendItem($"New Item {_id}", Random.Shared.Next(0, 500)));
        }

        public Task<SendItem?> WaitForNewItem()
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);
                NotifyNewItemAvailable();
            });

            return _tcs.Task;
        }
    }



    public record SendItem(string Name, double price);
}