using System.Text.Json;
using System.Text.Json.Serialization;

namespace tic_tac_toe
{
    internal class GameManagerOnline : GameManager
    {
        private WsClient _client;

        public Action OnDataUpdated;

        public GameManagerOnline()
        {
            _client = new WsClient();
            _client.OnReceive += ActOnReceive;

            // Done this way as I can't async-await a constructor. Which, don't get me wrong, makes sense, but it annoying. 
            Task connect = Task.Run( async () => { 
                await _client.ConnectAsync("localhost:8888");
                if (_client.WebSocketState != System.Net.WebSockets.WebSocketState.Open) throw new Exception("Failed to connect to server");
            });

            // With how noodly this code feels I might as well become a chef.
            try
            {
                connect.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                {
                    throw ex;
                }
            }
        }

        public int this[int index]
        {
            get { return base[index]; }
            set
            {
                _tileData[index] = (short)value;
                SendDataToOpponent();
            }
        }

        private static void SendDataToOpponent()
        {
            // TODO: send this to WS
        }

        private void ActOnReceive(string message)
        {
            // TODO: JSON decode message, act depending on type.
        }

        public void GetConnected()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            Message message = new Message();
            message.Type = "CheckOnline";

            _client.SendMessageAsync(JsonSerializer.Serialize(message, options));
        }

        private class Message()
        {
            public string? Type {  get; set; }
        }

        private class Response()
        {

        }
    }
}
