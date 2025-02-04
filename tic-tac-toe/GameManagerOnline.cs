using System.Text.Json;

namespace tic_tac_toe
{
    internal class GameManagerOnline : GameManager
    {
        public int this[int index]
        {
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
    }
}
