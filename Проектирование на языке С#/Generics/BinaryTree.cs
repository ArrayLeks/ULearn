using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T>, IEnumerable
        where T : IComparable<T>
    {
        public T Value 
        { 
            get { return Root.Value; } 
            set { Root.Value = value; } 
        }

        public Node<T> Root { get; set; }

        public Node<T> Left 
        {
            get { return Root.Left; }
        }
        
        public Node<T> Right 
        {
            get { return Root.Right; }
        }

        public BinaryTree(params T[] values)
        {
            foreach (var value in values)
                Add(value);
        }

        public void Add(T value)
        {
            if(Root is null) Root = new Node<T> { Value = value };
            else 
            {
                var node = Root;
                while (true)
                {
                    if (node.Value.CompareTo(value) < 0)
                    {
                        if (node.Right is null)
                        {
                            node.Right = new Node<T> { Value = value , Parent = node};
                            break;
                        }
                        node = node.Right;
                    }
                    else
                    {
                        if (node.Left is null)
                        {
                            node.Left = new Node<T> { Value = value , Parent = node };
                            break;
                        }
                        node = node.Left;
                    }
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Root == null) yield break;
            foreach (var item in Root?.GetValues())
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class BinaryTree
    {
        public static BinaryTree<T> Create<T>(params T[] values)
            where T : IComparable<T>
        {
            return new BinaryTree<T>(values);
        }
    }

    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T> Parent { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public IEnumerable<T> GetValues()
        {
            if (Left != null) 
                foreach (var value in Left.GetValues())
                    yield return value;
            yield return Value;
            if (Right != null)
                foreach (var value in Right.GetValues())
                    yield return value;
        }
    }
}