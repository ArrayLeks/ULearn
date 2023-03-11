using System;
using System.Collections.Generic;
using System.Linq;

namespace SRP.ControlDigit
{
    public static class Extensions
    {
        public static IEnumerable<int> DigitSelectionFromEnd(this long number)
        {
            while (number > 9)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
            yield return (int)number;
        }

        public static IEnumerable<int> DigitSelectionFromEnd(this int number)
        {
            while (number > 9)
            {
                yield return (int)(number % 10);
                number /= 10;
            }
            yield return number;
        }

        public static int MultiplyDigitOnPosition(long number, int sum, int startPosition)
        {
            foreach (var digit in number.DigitSelectionFromEnd())
            {
                sum += (startPosition * digit);
                startPosition++;
            }
            return sum;
        }
    }

    public static class ControlDigitAlgo
    {
        public static int Upc(long number)
        {
            int sum = CalculateSum(number.DigitSelectionFromEnd(), 0);
            int result = sum % 10;
            return result != 0 ? 10 - result : result;
        }

        public static int Isbn10(long number)
        {
            int sum = MultiplyDigitOnPosition(number.DigitSelectionFromEnd(), 0, 2);
            int result = (11 - (sum % 11)) % 11;
            return result == 10 ? 'X' : result.ToString()[0];
        }

        public static int Luhn(long number)
        {
            int sum = LuhnSum(number.DigitSelectionFromEnd(), 0, 1);
            return (10 - (sum % 10)) % 10;
        }

        private static int CalculateSum(IEnumerable<int> numbers, int sum)
        {
            int position = 1;
            foreach (var digit in numbers)
            {
                if (position % 2 == 1) sum += (3 * digit);
                else sum += digit;
                position++;
            }
            return sum;
        }

        private static int MultiplyDigitOnPosition(IEnumerable<int> numbers, int sum, int startPosition)
        {
            foreach (var digit in numbers)
            {
                sum += (startPosition * digit);
                startPosition++;
            }
            return sum;
        }

        private static int LuhnSum(IEnumerable<int> numbers, int sum, int position)
        {
            foreach (var digit in numbers)
            {
                if (position % 2 == 1)
                    sum += digit + digit < 9
                        ? digit + digit
                        : (digit + digit).DigitSelectionFromEnd().Aggregate((i, j) => i + j);
                else sum += digit;
                position++;
            }
            return sum;
        }
    }
}