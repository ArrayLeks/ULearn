using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        private static int hash = int.MinValue;
        private int hashCategory;
        public string Product { get; set; }
        public MessageType Type { get; set; }
        public MessageTopic Topic { get; set; }
        public Category(string product, MessageType type, MessageTopic topic)
        {
            hashCategory = Category.hash++;
            Product = product ?? "";
            Type = type;
            Topic = topic;
        }

        public override string ToString()
        {
            return $"{Product}.{Type}.{Topic}";
        }

        public int CompareTo(object obj)
        {
            if (obj is null) return 1;
            var category = obj as Category;
            return (Product, Type, Topic)
                .CompareTo((category.Product, category.Type, category.Topic));
        }

        public override int GetHashCode()
        {
            return hashCategory;
        }

        public override bool Equals(object obj)
        {
            if(obj as Category is null) return false;
            var category = obj as Category;
            return string.Compare(Product, category.Product) == 0
                && Type == category.Type
                && Topic == category.Topic;
        }

        public static bool operator >(Category a, Category b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(Category a, Category b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator >=(Category a, Category b)
        {
            return a.CompareTo(b) >= 0;
        }

        public static bool operator <=(Category a, Category b)
        {
            return a.CompareTo(b) <= 0;
        }

        public static bool operator ==(Category a, Category b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Category a, Category b)
        {
            return !a.Equals(b);
        }
    }
}