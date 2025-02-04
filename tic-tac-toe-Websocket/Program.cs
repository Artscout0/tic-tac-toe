using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace tic_tac_toe_Websocket
{
    internal class Program
    {
        private static readonly ConcurrentDictionary<string, Client> Clients = new ConcurrentDictionary<string, Client>();

        /// <summary>
        /// Main loop for the websocket.
        /// 
        /// Most lines of the output to do with a client will be structured: "Client UUID(username) did something"
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:443/");
            httpListener.Start();

            Console.WriteLine("Listening for WebSocket connections...");

            while (true)
            {
                HttpListenerContext context = await httpListener.GetContextAsync();

                // refuse non-websocket requests
                if (!context.Request.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                    continue;
                }

                // Accept connection otherwise
                HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(subProtocol: null);
                WebSocket webSocket = webSocketContext.WebSocket;

                // Assign a unique ID
                string clientId = Guid.NewGuid().ToString();
                Client client = new Client(webSocket);
                Clients.TryAdd(clientId, client);

                Console.WriteLine($"Client {clientId} connected. Total clients: {Clients.Count}");

                _ = HandleClient(clientId, client);
            }
        }

        /// <summary>
        /// Handles a single client when they join.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        private static async Task HandleClient(string clientId, Client client)
        {
            byte[] buffer = new byte[8192];

            try
            {
                while (client.WebSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult receiveResult = await client.WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    if (receiveResult.MessageType == WebSocketMessageType.Text)
                    {
                        // This part of the code might require some bolognese to get through, I apologise in advance. 

                        string message = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
                        Console.WriteLine($"Received from {clientId}({client.Name ?? "unidentified"}): {message}");

                        (Actions, string) response = client.HandleMessage(message);

                        switch (response.Item1)
                        {
                            case Actions.Nothing:
                                break;
                            case Actions.Response:
                                client.WebSocket.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
                                break;
                            case Actions.Broadcast:
                                BroadcastMessage(response.Item2);
                                break;
                            case Actions.ConnectWith:
                                throw new NotImplementedException();
                                break;
                            case Actions.MessageOther:
                                throw new NotImplementedException();
                                break;
                            case Actions.InformOther:
                                throw new NotImplementedException();
                                break;
                            default:
                                break;
                        }
                    }

                    else if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        await DisconnectClient(clientId);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error with client {clientId}({client.Name ?? "unidentified"}): {ex.Message}");
            }

            await DisconnectClient(clientId);
        }

        /// <summary>
        /// Broadcast a message to everyone
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private static async Task BroadcastMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            var tasks = new List<Task>();

            foreach (var client in Clients)
            {
                if (client.Value.WebSocket.State == WebSocketState.Open)
                {
                    tasks.Add(client.Value.WebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None));
                }
            }

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Disconnect a specific client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        private static async Task DisconnectClient(string clientId)
        {
            if (Clients.TryRemove(clientId, out Client client))
            {
                WebSocket webSocket = client.WebSocket;

                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected", CancellationToken.None);
                }
                
                Console.WriteLine($"Client {clientId}({client.Name ?? "unidentified"}) disconnected. Total clients: {Clients.Count}");
            }
        }
    }
}
