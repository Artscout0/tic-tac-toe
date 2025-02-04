using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace tic_tac_toe_Websocket
{
    internal class Client
    {
        public WebSocket WebSocket { get;}

        public string Name { get; set; }

        // ------- more data as needed ---------

        public Client(WebSocket ws)
        {
            WebSocket = ws;
        }

        public (Actions, string) HandleMessage(string message)
        {
            Message messageObject;
            Response response = new Response();

            // Ignore null
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

            try
            {
                messageObject = JsonSerializer.Deserialize<Message>(message);
            }
            catch (JsonException ex)
            {

                response.Errors = new string[] { ex.Message };

                return (Actions.Response, JsonSerializer.Serialize(response, options)); 
            }

            switch (messageObject.Type)
            {
                case "Identify":

                    if (messageObject.Name == null)
                    {
                        response.Errors = new string[] { "No name provided" };

                        return (Actions.Response, JsonSerializer.Serialize(response, options));
                    }

                    Name = messageObject.Name;

                    break;

                case "ConnectWith":
                    throw new NotImplementedException();
                    break;
                case "Message":
                    throw new NotImplementedException();

                    break;
                case "Move":
                    throw new NotImplementedException();

                    break;

                default:

                    response.Errors = new string[] { "Message Type not valid" };

                    return (Actions.Response, JsonSerializer.Serialize(response, options));
                    break;
            }






            return (Actions.Nothing, "");
        }




        /// <summary>
        /// Helper class that messages deserialize into. Incidentally the reason I prefer websockets in web/php.
        /// </summary>
        private class Message()
        {
            public string Type { get; set; }

            public string? Name { get; set; }
        }

        /// <summary>
        /// Helper class that is serialised into responses. Incidentally an even bigger reason I'd rather use this in PHP.
        /// Really makes you think about some languages being better suited for some stuff than others. 
        /// </summary>
        private class Response()
        {
            public string[] Errors { get; set; }

            public string Message { get; set; }

        }
    }
}
