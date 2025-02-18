namespace tic_tac_toe_Utils
{
    /// <summary>
    /// A helper class useable from both projects.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Works as long as both bases are 2 <= b <= 10. Used here as I often need to convert b10 to b3, and b3 to b10.
        /// </summary>
        /// <param name="num">The original number</param>
        /// <param name="ogBase">The base of the original number</param>
        /// <param name="newBase">the base of the return</param>
        /// <returns></returns>
        public static int ChangeIntBase(int num, int ogBase, int newBase)
        {
            // Step 1: Convert num from the original base to base 10
            int decimalValue = 0;
            int power = 0;
            while (num > 0)
            {
                int digit = num % 10;
                if (digit >= ogBase)
                {
                    throw new ArgumentException("Invalid digit for the original base.");
                }
                decimalValue += digit * (int)Math.Pow(ogBase, power);
                power++;
                num /= 10;
            }


            // Step 2: Convert the base 10 value to the new base
            int result = 0;
            int multiplier = 1;
            while (decimalValue > 0)
            {
                int remainder = decimalValue % newBase;
                result += remainder * multiplier;
                multiplier *= 10;
                decimalValue /= newBase;
            }

            return result;
        }

        /// <summary>
        /// Convers integer to an array of short, all the values of which hold between 0 and 2
        /// </summary>
        /// <param name="state">Integer to convert</param>
        /// <returns>An array of shorts, of minimal length 9</returns>
        public static short[] IntToShortArray(int state)
        {
            int state_b3 = Utils.ChangeIntBase(state, 10, 3);

            string state_string = state_b3.ToString();

            // add leading zeros
            while (state_string.Length < 9)
            {
                state_string = '0' + state_string;
            }

            char[] chars = state_string.ToCharArray();
            
            // a list of 0s, 1s, and 2s.
            short[] nums = chars.Select(c => (short)(c - '0')).ToArray(); 

            return nums;
        }
    }
}
