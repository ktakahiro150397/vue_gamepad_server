
using PadInput.GamePadInput;

namespace Core.Response
{
    /// <summary>
    /// フロントエンドへ返却するデバイス一覧情報
    /// </summary>
    class GetDevicesResponse
    {
        public int joyId { get; set; }

        public string device_id { get; set; }


        public string device_name { get; set; }

        public int server_tick { get; set; }


        public GetDevicesResponse()
        {
            device_id = "";
            device_name = "";
        }

    }
}