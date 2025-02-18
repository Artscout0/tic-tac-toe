using tic_tac_toe_Utils;

namespace tic_tac_toe_Websocket
{
    internal class Game
    {
        private Client Client1;
        private Client Client2;
        private int CurrentState = 0;
        private int PlayerToMove;

        public Game(Client client1, Client client2)
        {
            Random random = new Random();

            Client1 = client1;
            Client2 = client2;
            PlayerToMove = random.Next(0, 2);
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

        public bool IsValidMove(int move)
        {
            short[] moveData = Utils.IntToShortArray(move);
            short[] oldState = Utils.IntToShortArray(CurrentState);

            int same = 0;
            int overrides = 0;
            for (int i = 0; i < 9; i++)
            {
                if (moveData[i] == oldState[i]) 
                {
                    same++; // must have exaclty 1 change, meaning 8 things stay the same.
                } else
                {
                    if (moveData[i] != 0)
                    {
                        overrides++; // If any value that was previously set is now overriden, we have a problem.
                    }
                }
            }

            // if not 1 change, or any overrides, move is invalid.
            return !(same != 8 || overrides != 0);
        }
    }
}
