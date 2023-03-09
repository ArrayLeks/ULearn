using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Incapsulation.RationalNumbers
{
    public class Rational
    {
        public int Numerator { get; private set; }
        public int Denominator { get; private set; }
        public bool IsNan { get; private set; }

        public Rational(int numerator, int denominator = 1) 
        { 
            Numerator = numerator;
            Denominator = Numerator == 0 ? 1 : denominator;
            if (Denominator < 0 && Numerator < 0)
            {
                Denominator *= -1;
                Numerator *= -1;
            }
            if (Math.Abs(Numerator) > 1 && Math.Abs(Denominator) > 1) 
                Reduce();
            if (Denominator == 0) IsNan = true;
            if ((numerator < 0 && denominator > 0) || (numerator > 0 && denominator < 0))
                Numerator = Numerator < 0 ? Numerator : -Numerator;
        }

        public static Rational operator +(Rational left, Rational right)
        {
            var diff = right.Denominator == 0 ? 1 : left.Denominator / right.Denominator;
            if(left.Denominator == 0 || right.Denominator == 0)
                return new Rational(1, 0);
            else if (diff > 1)
                return new Rational(
                    (left.Numerator * diff) + right.Numerator,
                    right.Denominator);
            else
                return new Rational(
                    left.Numerator + (right.Numerator * diff),
                    left.Denominator);
        }

        public static Rational operator -(Rational left, Rational right)
        {
            var diff = right.Denominator == 0 ? 1 : left.Denominator / right.Denominator;
            if(left.Denominator == 0 || right.Denominator == 0)
                return new Rational(1, 0);
            else if (diff > 1)
                return new Rational(
                    left.Numerator  - (right.Numerator * diff),
                    left.Denominator);
            else
                return new Rational(
                    (left.Numerator * (right.Denominator / left.Denominator)) - right.Numerator,
                    right.Denominator);
        }

        public static Rational operator *(Rational left, Rational right)
        {
            return new Rational(
                left.Numerator * right.Numerator,
                left.Denominator * right.Denominator);
        }

        public static Rational operator /(Rational left, Rational right)
        {
            if (right.Numerator == 0 || right.Denominator == 0)
                return new Rational(1, 0);
            return new Rational(
                left.Numerator / right.Numerator,
                left.Denominator / right.Denominator);
        }

        public static explicit operator int(Rational value)
        {
            if (value.Numerator % value.Denominator != 0)
                throw new Exception();
            return value.Numerator / value.Denominator;
        }

        public static implicit operator double(Rational value)
        {
            if(value.Denominator == 0 || value.Numerator == 0) 
                return double.NaN;
            return value.Numerator / (double)value.Denominator;
        }

        public static implicit operator Rational(int value)
        {
            return new Rational(value);
        }

        public void Reduce()
        {
            bool flag = false;
            while(true)
            {
                int i = Math.Abs(Numerator) > Math.Abs(Denominator) 
                    ? Math.Abs(Denominator) 
                    : Math.Abs(Numerator);
                while(i > 0)
                {
                    if (i == 1) { flag = true; break; }
                    if (Numerator % i == 0 && Denominator % i == 0)
                    {
                        Numerator /= i;
                        Denominator = Math.Abs(Denominator / i);
                        break;
                    }
                    i--;
                }
                if (flag) break;
            }
        }
    }
}