﻿using System.Collections.Concurrent;
using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace tic_tac_toe_Websocket
{
    internal class Program
    {
        public static readonly ConcurrentDictionary<string, Client> Clients = new ConcurrentDictionary<string, Client>();

        public static List<Game> Games = new List<Game>();

        /// <summary>
        /// Main loop for the websocket.
        /// 
        /// Most lines of the output to do with a client will be structured: "Client UUID(username) did something"
        /// </summary>
        static async Task Main()
        {
            HttpListener httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://localhost:8888/");
            httpListener.Start();

            Console.WriteLine("Listening for WebSocket connections...");

            while (true)
            {
                HttpListenerContext context = await httpListener.GetContextAsync();

                // refuse non-websocket requests
                if (!context.Request.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    context.Response.StatusDescription = "Must be a websocket connection";

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
        /// Handles a single client when they join. Listens to their demands, and works accordingly.
        /// </summary>
        /// <param name="clientId">The GUID of the client that joined, assigned on join</param>
        /// <param name="client">The client itself, with all related data</param>
        private static async Task HandleClient(string clientId, Client client)
        {
            byte[] buffer = new byte[8192];
            Client client2;
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
                                await client.WebSocket.SendAsync(Encoding.UTF8.GetBytes(response.Item2), WebSocketMessageType.Text, true, CancellationToken.None);
                                break;
                            case Actions.Broadcast:
                                await BroadcastMessage(response.Item2);
                                break;
                            case Actions.ConnectWith:

                                client2 = Clients.FirstOrDefault(kvp => kvp.Value.Name == response.Item2).Value;

                                if (client2 == default || client2 == client)
                                {
                                    await client.WebSocket.SendAsync(Encoding.UTF8.GetBytes("\"Type\":\"Error\", \"Errors\":[\"User with this name doesn't exist, or isn't among the users you can challenge.\"]"), WebSocketMessageType.Text, true, CancellationToken.None);
                                    continue;
                                }

                                Games.Add(new Game(client, client2));

                                await client.WebSocket.SendAsync(Encoding.UTF8.GetBytes(response.Item2), WebSocketMessageType.Text, true, CancellationToken.None);

                                break;
                            case Actions.MessageOther:

                                Game game = Games.FirstOrDefault(game => game.IsPlayer(client));

                                if (game == default)
                                {
                                    await client.WebSocket.SendAsync(Encoding.UTF8.GetBytes("\"Type\":\"Error\", \"Errors\":[\"You aren't in a game.\"]"), WebSocketMessageType.Text, true, CancellationToken.None);
                                    continue;
                                }

                                await game.GetOther(client).WebSocket.SendAsync(Encoding.UTF8.GetBytes(response.Item2), WebSocketMessageType.Text, true, CancellationToken.None);

                                break;
                            case Actions.InformOther:
                                throw new NotImplementedException();
                                break;
                            case Actions.Message:

                                string[] temp = response.Item2.Split(';');

                                Console.WriteLine(temp);

                                string clientName = temp[0];
                                string msg = temp[1];

                                client2 = Clients.FirstOrDefault(kvp => kvp.Value.Name == clientName).Value;

                                if (client2 == default || client2 == client)
                                {
                                    client.WebSocket.SendAsync(Encoding.UTF8.GetBytes("\"Type\":\"Error\", \"Errors\":[\"User with this name doesn't exist, or isn't among the users you can message.\"]"), WebSocketMessageType.Text, true, CancellationToken.None);
                                }

                                client2.WebSocket.SendAsync(Encoding.UTF8.GetBytes(msg), WebSocketMessageType.Text, true, CancellationToken.None);
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
