using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;

namespace tic_tac_toe
{
    internal class WsClient : IDisposable
    {
        public int ReceiveBufferSize { get; set; } = 8192;

        private ClientWebSocket WS;
        private CancellationTokenSource CTS;

        public Action<string> OnReceive;

        public async Task ConnectAsync(string url)
        {
            if (WS != null)
            {
                // Dispose of websocket if necessary
                if (WS.State == WebSocketState.Open) return;
                else WS.Dispose();
            }

            WS = new ClientWebSocket();

            // same with the the CTS. 
            if (CTS != null) CTS.Dispose();

            CTS = new CancellationTokenSource();

            await WS.ConnectAsync(new Uri(url), CTS.Token);
            await Task.Factory.StartNew(ReceiveLoop, CTS.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task DisconnectAsync()
        {
            // Dispose of everything
            if (WS is null) return;
            if (WS.State == WebSocketState.Open)
            {
                CTS.CancelAfter(TimeSpan.FromSeconds(2));
                await WS.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
                await WS.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            WS.Dispose();
            WS = null;
            CTS.Dispose();
            CTS = null;
        }

        private async Task ReceiveLoop()
        {
            CancellationToken loopToken = CTS.Token;
            byte[] buffer = new byte[ReceiveBufferSize];
            try
            {
                // While cancelation not requested
                while (!loopToken.IsCancellationRequested)
                {

                    // save bytes to stream while we haven't reached the end of the message

                    MemoryStream outputStream = new MemoryStream(ReceiveBufferSize);
                    WebSocketReceiveResult receiveResult;
                    do
                    {
                        receiveResult = await WS.ReceiveAsync(buffer, loopToken);
                        if (receiveResult.MessageType == WebSocketMessageType.Close) break;

                        outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;

                    // Read the resulting stream
                    outputStream.Position = 0;
                    using var reader = new StreamReader(outputStream, Encoding.UTF8);
                    string receivedMessage = await reader.ReadToEndAsync();

                    // Debug it, and invoke a message for all listeners. 
                    Debug.WriteLine($"Message received: {receivedMessage}");
                    OnReceive.Invoke(receivedMessage);
                }
            }
            catch (TaskCanceledException) { }
        }

        public async Task SendMessageAsync(string message)
        {
            if (WS?.State != WebSocketState.Open) throw new InvalidOperationException("WebSocket is not connected.");

            // turn message to bytes, and use the built in send function.
            var messageBytes = Encoding.UTF8.GetBytes(message);
            await WS.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CTS.Token);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
