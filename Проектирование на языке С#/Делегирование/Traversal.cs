using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public static class Traversal
    {
        public static IEnumerable<Product> GetProducts(ProductCategory root)
        {
            var results = new List<Product>();
            var categories = new Stack<ProductCategory>();
            var bypass = new Bypass<ProductCategory, Product>(
                node => true,
                (node, result) =>
                {
                    foreach (var product in node.Products)
                        result.Add(product);
                },
                (node, nodes) =>
                {
                    foreach (var category in node.Categories)
                        nodes.Push(category);
                });

            SingleMethod( bypass, root, results, categories);
            return results;
        }

        public static IEnumerable<Job> GetEndJobs(Job root)
        {
            var results = new List<Job>();
            var jobs = new Stack<Job>();
            var bypass = new Bypass<Job, Job>(
                node => node.Subjobs.Count == 0,
                (node, result) => result.Add(node),
                (node, nodes) =>
                {
                    foreach (var job in node.Subjobs)
                        nodes.Push(job);
                });

            SingleMethod(bypass, root, results, jobs);
            return results;
        }

        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> root)
        {
            var results = new List<T>();
            var binaryTrees= new Stack<BinaryTree<T>>();
            var bypass = new Bypass<BinaryTree<T>, T> (
                    node => node.Left == null && node.Right == null,
                    (node, result) => result.Add(node.Value),
                    (node, nodes) =>
                    {
                        if (node.Left != null) nodes.Push(node.Left);
                        if (node.Right != null) nodes.Push(node.Right);
                    });

            SingleMethod( bypass, root, results, binaryTrees);
            return results;
        }

        public static void SingleMethod<T1, T2>( 
            Bypass<T1, T2> bypass, T1 node, List<T2> results, Stack<T1> nodes)
        {
            if (bypass.ConditionsForAdding(node)) bypass.Adding(node, results);
            bypass.ConditionsForBypass(node, nodes);
            if(nodes.Count > 0) SingleMethod(bypass, nodes.Pop(), results, nodes);
        }
    }

    public class Bypass<TreeNode, Result>
    {
        public Predicate<TreeNode> ConditionsForAdding { get; private set; }
        public Action<TreeNode, List<Result>> Adding { get; private set; }
        public Action<TreeNode, Stack<TreeNode>> ConditionsForBypass { get; private set; }

        public Bypass(Predicate<TreeNode> conditionsForAdding, 
            Action<TreeNode, List<Result>> adding, 
            Action<TreeNode, Stack<TreeNode>> conditionsForBypass)
        {
            ConditionsForAdding = conditionsForAdding;
            Adding = adding;
            ConditionsForBypass = conditionsForBypass;
        }
    }
}