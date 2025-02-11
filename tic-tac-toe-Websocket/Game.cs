namespace tic_tac_toe_Websocket
{
    internal class Game
    {
        private Client Client1;
        private Client Client2;

        public Game(Client client1, Client client2)
        {

        }

        public bool IsPlayer(Client client)
        {
            return client == Client1 || client == Client2;
        }

        /// <summary>
        /// Assumes the client you give it is in fact part of the game.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Client GetOther(Client client) {
            return client == Client1 ? Client2 : Client1;
        }
    }
}
