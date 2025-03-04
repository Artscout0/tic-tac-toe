namespace tic_tac_toe_Websocket
{
    internal enum Actions
    {
        Nothing,      // Do nothing.
        Response,     // Send to person who sent the message.
        Broadcast,    // Send to everyone
        ConnectWith,  // Create a game and connect both players
        Accept,       // Accept a game
        MessageOther, // Message other person in game
        InformOther,  // Message move to other
        Message       // Message someone
    }
}
