using System.Numerics;

namespace Tickets
{
    public class TicketsTask
    {
        public static BigInteger Solve(int halfLen, int totalSum)
        {
            if (totalSum % 2 != 0) return 0;

            var tickets = InitializeContainer(halfLen + 1, totalSum + 1);

            return BigInteger.Pow(CountTickets(tickets, halfLen, totalSum / 2), 2);
        }

        private static BigInteger[,] InitializeContainer(int length, int sum)
        {
            var tickets = new BigInteger[length + 1, sum + 1];

            for (var i = 0; i < length; i++)
                for (var j = 0; j < sum; j++)
                    tickets[i, j] = -1;

            return tickets;
        }

        private static BigInteger CountTickets(BigInteger[,] tickets, int length, int sum)
        {
            if (tickets[length, sum] >= 0) return tickets[length, sum];
            if (sum == 0) return 1;
            if (length == 0) return 0;

            tickets[length, sum] = 0;
            for (var i = 0; i < 10; i++)
                if (sum - i >= 0)
                    tickets[length, sum] += CountTickets(tickets, length - 1, sum - i);

            return tickets[length, sum];
        }
    }
}