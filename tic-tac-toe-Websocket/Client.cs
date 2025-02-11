using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tic_tac_toe_Websocket
{
    /// <summary>
    /// This class stores the connection, as well as data related to the user.
    /// </summary>
    internal class Client
    {
        public WebSocket WebSocket { get; }

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
                response.Type = "Error";

                response.Errors = new List<string> { ex.Message };

                return (Actions.Response, JsonSerializer.Serialize(response, options));
            }


            switch (messageObject.Type)
            {
                // Tell the WS server who you are
                case "Identify":

                    if (messageObject.Name == null)
                    {
                        response.Type = "Error";

                        response.Errors = new List<string> { "No name provided" };

                        return (Actions.Response, JsonSerializer.Serialize(response, options));
                    }

                    Name = messageObject.Name;

                    response.Type = "UserLogin";
                    response.Message = "Logged In";
                    response.IdentifiedAs = Name;

                    return (Actions.Broadcast, JsonSerializer.Serialize(response, options));

                // Challenge someone to a game
                case "ConnectWith":

                    if (Name == null)
                    {
                        response.Type = "Error";
                        response.Errors = new List<string> { "Must be logged in with a name to challenge someone" };
                    }

                    if (messageObject.Challenged == null)
                    {
                        response.Type = "Error";
                        if (response.Errors == null)
                        {
                            response.Errors = new List<string> { "Username of challenged user must be provided" };
                        }
                        else
                        {
                            response.Errors.Add("Username of challenged user must be provided");
                        }
                    }

                    // Early exit in case of errors
                    if (response.Type == "Error")
                    {
                        return (Actions.Response, JsonSerializer.Serialize(response, options));
                    }

                    // Handled differently, therefore not packaged as a response
                    return (Actions.ConnectWith, messageObject.Challenged);

                // Send someone a message
                case "MessageOther":

                    if (Name == null || messageObject.Payload == null)
                    {
                        response.Type = "Error";
                        response.Errors = new List<string> { "Must be logged in with a name to send a message" };
                        return (Actions.Response, JsonSerializer.Serialize(response, options));
                    }

                    response.Type = "Message";
                    response.Message = messageObject.Payload;

                    return (Actions.MessageOther, JsonSerializer.Serialize(response, options));

                case "Message":

                    if (Name == null || messageObject.OtherName == null || messageObject.OtherName == Name || messageObject.Payload == null)
                    {
                        response.Type = "Error";
                        response.Errors = new List<string> { "Must be logged in with a name to send a message, that must contain a Payload, and the name of the other" };
                        return (Actions.Response, JsonSerializer.Serialize(response, options));
                    }
                    response.Type = "Message";
                    response.Message = messageObject.Payload;

                    Console.WriteLine(messageObject.OtherName + ";" + JsonSerializer.Serialize(response, options));

                    return (Actions.Message, messageObject.OtherName+";"+JsonSerializer.Serialize(response, options));

                // Make a move in a game
                case "Move":
                    throw new NotImplementedException();

                    break;

                // Get every online user
                case "CheckOnline":

                    response.Type = "Users";

                    response.UsersOnline = new List<string>();

                    foreach (KeyValuePair<string, Client> kvp in Program.Clients)
                    {
                        // If not logged in, or if self, ignore.
                        if (kvp.Value == null || kvp.Value.Name == Name)
                        {
                            continue;
                        }

                        response.UsersOnline.Add(kvp.Value.Name);
                    }

                    return (Actions.Response, JsonSerializer.Serialize(response, options));

                default:

                    response.Errors = new List<string> { "Message Type not valid" };

                    return (Actions.Response, JsonSerializer.Serialize(response, options));
            }

            return (Actions.Nothing, "");
        }

        public static bool operator ==(Client c1, Client c2)
        { 
            // If either are null, we say they aren't the same.
            if (Equals(c1, null) || Equals(c2, null)) return false;

            // If the same Name or the same Connection are used, then they are equal.
            return c1.Name == c2.Name || c1.WebSocket == c2.WebSocket;
        }

        public static bool operator !=(Client c1, Client c2)
        {
            return !(c1 == c2);
        }


        /// <summary>
        /// Helper class that messages deserialize into. Incidentally the reason I prefer websockets in web/php.
        /// </summary>
        private class Message()
        {
            public string? Type { get; set; }

            public string? Name { get; set; }

            public string? Payload { get; set; }

            public string? Challenged { get; set; }

            public string? OtherName { get; set; }

            public override string ToString()
            {
                return $"Type: {Type}; Name: {Name}; Challenged: {Challenged}";
            }
        }

        /// <summary>
        /// Helper class that is serialised into responses. Incidentally an even bigger reason I'd rather do this in PHP.
        /// Really makes you think about some languages being better suited for some stuff than others. 
        /// </summary>
        private class Response()
        {
            public string? Type { get; set; }

            public List<string>? Errors { get; set; }

            public string? Message { get; set; }

            public string? IdentifiedAs { get; set; }

            public List<string>? UsersOnline { get; set; }

        }
    }
}
