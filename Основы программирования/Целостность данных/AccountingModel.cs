using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    public class AccountingModel : ModelBase
    {
        private double price;
        private int nightsCount;
        private double discount;
        private double total;

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                if (value < 0) throw new ArgumentException();
                price = value;
                CalculateTotal();
                Notify(nameof(Price));
                Notify(nameof(Total));
            }
        }

        public int NightsCount
        {
            get
            {
                return nightsCount;
            }
            set
            {
                if (value <= 0) throw new ArgumentException();
                nightsCount = value;
                CalculateTotal();
                Notify(nameof(NightsCount));
                Notify(nameof(Total));
            }
        }

        public double Total
        {
            get
            {
                return total;
            }
            set
            {
                if (value <= 0) throw new ArgumentException();
                total = value;
                CalculateDiscont();
                Notify(nameof(Total));
                Notify(nameof(Discount));
            }
        }
        public double Discount 
        {
            get
            {
                return discount;
            }
            set
            {
				if (value > 100) throw new ArgumentException();
				discount = value;
                CalculateTotal();
                Notify(nameof(Discount));
                Notify(nameof(Total));
            }
        }

        private void CalculateTotal()
        {
            total = price * nightsCount * (1 - discount / 100);
        }

        private void CalculateDiscont()
        {
            discount = (1 - total / (price * nightsCount)) * 100;
        }
    }
}