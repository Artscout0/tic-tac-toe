using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Resources;
using tic_tac_toe_Utils;

namespace tic_tac_toe
{
    internal class GameManager
    {
        protected short[] _tileData = new short[] { 0, 0, 0, 
                                                    0, 0, 0, 
                                                    0, 0, 0 };

        public Bitmap[] Images { get => IntToImages(boardDataToB10(_tileData)).Item1; }

        public int this[int index]
        {
            get
            {
                return _tileData[index];
            }
            set
            {
                _tileData[index] = (short)value;
            }
        }

        public int CheckForVictory()
        {
            // there's probably a better way to do this.

            // diagonals
            if (this[4] == this[0] && this[4] == this[8] && this[4] != 0) return this[4];
            if (this[4] == this[2] && this[4] == this[6] && this[4] != 0) return this[4];

            // rows
            if (this[0] == this[1] && this[0] == this[2] && this[0] != 0) return this[0];
            if (this[3] == this[4] && this[3] == this[5] && this[3] != 0) return this[3];
            if (this[6] == this[7] && this[6] == this[8] && this[6] != 0) return this[6];

            // cols
            if (this[0] == this[3] && this[0] == this[6] && this[0] != 0) return this[0];
            if (this[1] == this[4] && this[1] == this[7] && this[1] != 0) return this[1];
            if (this[2] == this[5] && this[2] == this[8] && this[2] != 0) return this[2];

            if (!Any((x) => x == 0)) // if there's no empty spots
            {
                return -1;
            }
            return 0;
        }

        /// <summary>
        /// Converts an integer to a list of Images, as well as a 'board state' integer. Will make it simpler to send data through the websocket later.
        /// </summary>
        /// <param name="state">An integer between 0 and 19682, representing a board state.</param>
        /// <returns>a tuple, with the list of images to use, that should map to a board, and an array of short integers, which is what is supposed to be stored.</returns>
        public static (Bitmap[], short[]) IntToImages(int state)
        {
            short[] nums = Utils.IntToShortArray(state); 
            //Debug.WriteLine(string.Join(' ', nums));

            Bitmap[] bmps = new Bitmap[nums.Length]; // should be 9 always, but C# works in mysterious ways.

            ResourceManager resourceManager = Properties.Resources.ResourceManager;

            for (int i = 0; i < 9; i++)
            {
                string imgName = "";
                switch (nums[i])
                {
                    case 0:
                        imgName = "nothing";
                        break;
                    case 1:
                        imgName = "circle";
                        break;
                    case 2:
                        imgName = "cross";
                        break;
                    default:
                        imgName = "nothing"; // something went wrong
                        break;
                }


                bmps[i] = (Bitmap)resourceManager.GetObject(imgName);
            }

            return (bmps, nums);
        }

        /// <summary>
        /// Converts a list of shorts to an integer, ready to be sent through a websocket.
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static int boardDataToB10(short[] nums)
        {
            string num = string.Join("", nums);
            return Utils.ChangeIntBase(Convert.ToInt32(num), 3, 10);
        }

        protected bool Any(Func<int, bool> func)
        {
            for (int i = 0; i < _tileData.Length; i++)
            {
                if (func(_tileData[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
