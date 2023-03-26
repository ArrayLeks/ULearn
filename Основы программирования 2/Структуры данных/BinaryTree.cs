using Microsoft.VisualBasic;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BinaryTrees;

public class BinaryTree<T> : IEnumerable<T>, IEnumerable
    where T : IComparable
{
    public TreeNode<T> root;

    public void Add(T key)
    {
        if (root == null) root = new TreeNode<T> { Value = key };
        else
        {
            TreeNode<T> node = root;
            while (true)
            {
                if (key.CompareTo(node.Value) < 0)
                    if (node.Left != null) node = node.Left;
                    else
                    {
                        node.Left = new TreeNode<T> { Value = key};
                        break;
                    }
                else if (node.Right != null) node = node.Right;
                else
                {
                    node.Right = new TreeNode<T> { Value = key};
                    break;
                }
            }
        }
    }

    public bool Contains(T key)
    {
        if (root == null) return false;
        else if(root.Value.Equals(key)) return true;

        TreeNode<T> node = root;
        
        while(true)
        {
            if(key.CompareTo(node.Value) < 0)
                node = node.Left;
            else if (key.CompareTo(node.Value) >= 0)
                node = node.Right;
            if (node == null) return false;
            else if (node.Value.Equals(key)) return true;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        if (root == null) throw new ArgumentException();
        TreeNode<T> node = root;
        return MakeEnumerate(node);
    }

    private IEnumerator<T> MakeEnumerate(TreeNode<T> node)
    {
        if (node.Left == null) yield return node.Value;
        else MakeEnumerate(node.Left);
        if (node.Right == null) yield break;
        else MakeEnumerate(node.Right);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class TreeNode<T>
{
    public T Value;
    public TreeNode<T> Left, Right;
}