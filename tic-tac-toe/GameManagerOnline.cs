using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using tic_tac_toe_Utils;

namespace tic_tac_toe
{
    internal class GameManagerOnline : GameManager
    {
        private WsClient _client;

        /// <summary>
        /// Invoked when the data is updated.
        /// </summary>
        public Action OnDataUpdated;

        /// <summary>
        /// Whether the user is in a multiplayer game.
        /// </summary>
        public bool InMPGame = false;

        public List<string> OnlinePeople = new List<string>();

        public GameManagerOnline()
        {
            string host;
            if (!File.Exists("hostname.txt"))
            {
                host = "host.docker.internal";
            }
            else
            {
                host = File.ReadAllText("hostname.txt").Trim().ToLower();
            }

            _client = new WsClient();
            _client.OnReceive += ActOnReceive;

            // Done this way as I can't async-await a constructor. Which, don't get me wrong, makes sense, but it's annoying. 
            Task connect = Task.Run(async () =>
            {
                await _client.ConnectAsync($"ws://{host}:8888");
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
                if (InMPGame)
                {
                    SendDataToOpponent();
                }
            }
        }

        private void SendDataToOpponent()
        {
            // TODO: send this to WS
            Message msg = new Message();

            msg.Type = "Move";
            msg.Payload = BoardDataToB10(_tileData).ToString();

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            _client.SendMessageAsync(JsonSerializer.Serialize(msg, options));
        }

        private void ActOnReceive(string message)
        {
            // TODO: JSON decode message, act depending on type.
            Response msg = JsonSerializer.Deserialize<Response>(message);

            switch (msg.Type)
            {
                case "Error":
                    Debug.WriteLine("Errors: " + string.Join(", ", msg.Errors));
                    break;

                case "UserLogin":
                    Debug.WriteLine("User logged in as " + msg.IdentifiedAs);
                    break;

                case "ChallengeIssued":
                    Debug.WriteLine("Issued challenge");
                    break;

                case "ChallengeReceived":
                    Debug.WriteLine("Received challenge");
                    break;

                case "GameStart":
                    Debug.WriteLine("Game started");
                    break;

                case "Message":
                    Debug.WriteLine("Message: " + msg.Message + "  From: " + msg.From);
                    break;

                case "Move":
                    Debug.WriteLine("Move: " + msg.Move);
                    _tileData = Utils.IntToShortArray(int.Parse(msg.Move));
                    OnDataUpdated?.Invoke();
                    break;

                case "Users":
                    Debug.WriteLine("Users online: " + string.Join(", ", msg.UsersOnline));
                    OnlinePeople = msg.UsersOnline;
                    OnDataUpdated?.Invoke();
                    break;

                default:
                    break;
            }
        }

        public void Login(string username)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            _client.SendMessageAsync(JsonSerializer.Serialize(new Message { Type = "Identify", Name = username }, options));
        }

        public void GetConnected()
        {

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;


            Message message = new Message();
            message.Type = "CheckOnline";

            _client.SendMessageAsync(JsonSerializer.Serialize(message, options));
        }

        public void Challenge(string name)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            Message message = new Message();
            message.Type = "Challenge";
            message.Challenged = name;

            _client.SendMessageAsync(JsonSerializer.Serialize(message, options));
        }

        /// <summary>
        /// Class that messages to the server are serialized from
        /// </summary>
        private class Message()
        {
            public string? Type { get; set; }
            public string? Name { get; set; }
            public string? Payload { get; set; }
            public string? Challenged { get; set; }
            public string? OtherName { get; set; }
        }

        /// <summary>
        /// Class that responses from the server are serialized into
        /// </summary>
        private class Response()
        {
            public string? Type { get; set; }
            public List<string>? Errors { get; set; }
            public string? Message { get; set; }
            public string? IdentifiedAs { get; set; }
            public string? Move { get; set; }
            public string? From { get; set; }
            public List<string>? UsersOnline { get; set; }
        }
    }
}
