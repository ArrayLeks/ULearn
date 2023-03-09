using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reflection.Randomness
{
    public class Generator<T>
        where T : new()
    {
        public readonly PropertyInfo[] Properties;
        public readonly IList<CustomAttributeTypedArgument>[] Arguments;

        public Generator()
        {
            Properties = typeof(T)
                .GetProperties()
                .Where(s => s.GetCustomAttribute<FromDistributionAttribute>() != null)
                .ToArray();

            Arguments = Properties
                .Select(s => s.GetCustomAttributesData().FirstOrDefault().ConstructorArguments)
                .ToArray();
        }

        public T Generate(Random rnd)
        {
            var item = new T();

            for (int i = 0; i < Properties.Length; i++)
            {
                if ((Type)Arguments[i][0].Value == typeof(NormalDistribution))
                    item = UseNormalDistribution(item, rnd, i);
                else if ((Type)Arguments[i][0].Value == typeof(ExponentialDistribution))
                    Properties[i].SetValue(item,
                        new ExponentialDistribution((double)Arguments[i][1].Value).Generate(rnd));
                else throw new ArgumentException("String containing List");
            }

            return item;
        }

        public T UseNormalDistribution(T item, Random rnd, int i)
        {
            if (Arguments[i].Count > 3) throw new ArgumentException("String containing NormalDistribution");
            else if (Arguments[i].Count == 3)
                Properties[i].SetValue(item,
                    new NormalDistribution((double)Arguments[i][1].Value,
                    (double)Arguments[i][2].Value).Generate(rnd));
            else if (Arguments[i].Count == 1)
                Properties[i].SetValue(item,
                    new NormalDistribution().Generate(rnd));
            return item;
        }
    }

    public class FromDistributionAttribute : Attribute
    {
        public Type Type { get; set; }
        public double First { get; set; }
        public double Second { get; set; }
        public double Third { get; set; }

        public FromDistributionAttribute(Type type)
        {
            Type = type;
        }

        public FromDistributionAttribute(Type type, double first) : this(type)
        {
            First = first;
        }

        public FromDistributionAttribute(Type type, double first, double second) : this(type, first)
        {
            Second = second;
        }

        public FromDistributionAttribute(Type type, double first, double second, double third) : this(type, first)
        {
            Third = third;
        }
    }
}
