using System;
using System.Collections.Generic;
using TechOneCore.Services.Interface;

namespace TechOneCore.Services.Service
{
    /// <summary>
    /// Service class that converts numerical values to their corresponding English words representation.
    /// </summary>
    public class NumberToWordsConverter : INumberToWordsConverter
    {
        // Dictionaries for number-to-word mappings
        // added space after each word to make it easier to concatenate words later, and trim the final result.
        private static readonly Dictionary<int, string> ones = new Dictionary<int, string>
        {
            {0, "ZERO "}, {1, "ONE "}, {2, "TWO "}, {3, "THREE " }, {4, "FOUR "}, {5, "FIVE "}, {6, "SIX " }, {7, "SEVEN "}, {8, "EIGHT "}, {9, "NINE "}
        };

        private static readonly Dictionary<int, string> teens = new Dictionary<int, string>
        {
            {10, "TEN "}, {11, "ELEVEN "}, {12, "TWELVE "}, {13, "THIRTEEN "}, { 14, "FOURTEEN "}, {15, "FIFTEEN "}, {16, "SIXTEEN "}, {17, "SEVENTEEN "}, {18, "EIGHTEEN "}, {19, "NINETEEN "}
        };

        private static readonly Dictionary<int, string> tens = new Dictionary<int, string>
        {
            {20, "TWENTY "}, {30, "THIRTY "}, {40, "FORTY "}, {50, "FIFTY "}, {60, "SIXTY "}, {70, "SEVENTY "}, {80, "EIGHTY "}, {90, "NINETY "}
        };

        private static readonly Dictionary<int, string> scales = new Dictionary<int, string>
        {
            {0, ""}, {1, "THOUSAND "}, {2, "MILLION "}, {3, "BILLION "}, {4, "TRILLION "},{5, "QUADRILLION "}, {6, "QUINTILLION "}
        };

        private const string hundredSuffix = "HUNDRED ";
        private const string minusStr = "MINUS ";
        private const string andStr = "AND ";

        private const string dollarStr = "DOLLAR";
        private const string centStr = "CENT";

        // The maximum whole amount that can be converted to words, based on the range of a long integer.
        private const decimal maxWholeAmount = long.MaxValue;   // 9,223,372,036,854,775,807

        /// <summary>
        /// main method to convert a decimal value to its corresponding English words representation.
        /// Code review standards: functions should be small and focused on a single task, and this method is doing just that. It is also handling exceptions and providing meaningful error messages.
        /// </summary>
        /// <param name="amount">
        /// requirement does not say anything about null or zero input, so it is allowed by default. 
        /// Null input will throw an exception, as it is not a valid input. Zero input will return "ZERO DOLLARS".
        /// </param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string ConvertValueToString(decimal? amount)
        {
            try
            {
                // allow null input as requirement does not mention about it, but throw exception if null is passed in, as it is not a valid input.
                if (amount == null)
                    throw new ArgumentNullException(nameof(amount), "Input amount cannot be null.");

                amount = Math.Round(amount.Value, 2, MidpointRounding.AwayFromZero);

                // Check if the absolute value of the amount exceeds the maximum whole amount
                if (Math.Floor(Math.Abs(amount.Value)) > maxWholeAmount)
                    throw new ArgumentOutOfRangeException(nameof(amount),
                        $"Input amount is out of range. Must be between -{maxWholeAmount:N0} and {maxWholeAmount:N0}.");

                var retValue = string.Empty;

                // Handle negative amounts
                if (amount < 0)
                {
                    retValue += minusStr;
                    amount = Math.Abs(amount.Value);
                }

                // Split the amount into dollars and cents
                long dollars = (long)Math.Floor(amount.Value);
                int cents = (int)Math.Round((amount.Value - dollars) * 100);

                // Convert dollars to words, adding "DOLLAR" or "DOLLARS" based on the value
                retValue += dollars == 0 ? ones[0] : ConvertNumber(dollars);
                retValue += dollars == 1 ? dollarStr : dollarStr + "S";

                // Convert cents to words, adding "CENT" or "CENTS" based on the value
                if (cents > 0)
                {
                    retValue += " " + andStr;
                    retValue += ConvertNumber(cents);
                    retValue += cents == 1 ? centStr : centStr + "S";
                }

                return retValue.Trim();
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting number to words.", ex);
            }
        }

        /// <summary>
        /// this method converts a long integer to its corresponding English words representation.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string ConvertNumber(long number)
        {
            if (number == 0) return ones[0];

            var groups = new List<int>();

            // Split the number into groups of three digits (thousands, millions, etc.)
            while (number > 0)
            {
                groups.Add((int)(number % 1000));
                number = number / 1000;
            }

            var numberValue = string.Empty;
            for (int i = groups.Count - 1; i >= 0; i--)
            {
                // Skip empty groups
                if (groups[i] == 0) continue;

                // Add "AND" for the last group if it's less than 100 and not the only group
                if (i == 0 && groups[i] < 100 && numberValue.Length > 0)
                    numberValue += andStr;

                numberValue += ConvertGroup(groups[i]);

                // Add the scale (thousand, million, etc.) if it's not the last group
                numberValue += scales[i];
            }

            return numberValue;
        }

        /// <summary>
        /// this method converts a three-digit group to its corresponding English words representation.
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private static string ConvertGroup(int number)
        {
            var groupValue = string.Empty;

            // Split the group into hundreds and the remainder
            int hundreds = number / 100;
            int remainder = number % 100;

            // Convert the hundreds place to words if it's greater than 0
            if (hundreds > 0)
                groupValue += ones[hundreds] + hundredSuffix;

            // Convert the remainder (tens and ones) to words if it's greater than 0
            if (remainder > 0)
            {
                // Add "AND" if there are hundreds and a remainder
                if (hundreds > 0) groupValue += andStr;

                // Convert the remainder to words based on its value
                if (remainder < 10)
                {
                    groupValue += ones[remainder];
                }
                // Handle numbers between 10 and 19 (teens)
                else if (remainder < 20)
                {
                    groupValue += teens[remainder];
                }
                // Handle numbers 20 and above
                else
                {
                    int onesDigit = remainder % 10;
                    int tensValue = remainder - onesDigit;

                    groupValue += onesDigit > 0
                        ? tens[tensValue].TrimEnd() + "-" + ones[onesDigit] : tens[tensValue];
                }
            }

            return groupValue;
        }
    }
}