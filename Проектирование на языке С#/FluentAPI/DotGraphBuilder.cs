using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentApi.Graph
{
    public enum NodeShape
    {
        Box,
        Ellipse
    }

    public interface IGraphBuilder
    {
        NodeBuilder AddNode(string nodeName);
        EdgeBuilder AddEdge(string sourceNode, string destinationNode);
        string Build();
    }

    public class DotGraphBuilder : IGraphBuilder
    {
        private Graph Graph { get; }

        private DotGraphBuilder(string graphName, bool directed)
        {
            Graph = new Graph(graphName, directed, false);
        }

        public static DotGraphBuilder DirectedGraph(string graphName)
        {
            return new DotGraphBuilder(graphName, true);
        }

        public static DotGraphBuilder UndirectedGraph(string graphName)
        {
            return new DotGraphBuilder(graphName, false);
        }

        public NodeBuilder AddNode(string nodeName)
        {
            return new NodeBuilder(Graph.AddNode(nodeName), this);
        }

        public EdgeBuilder AddEdge(string sourceNode, string destinationNode)
        {
            return new EdgeBuilder(Graph.AddEdge(sourceNode, destinationNode), this);
        }

        public string Build() => Graph.ToDotFormat();
    }

    public abstract class Builder : IGraphBuilder
    {
        protected IGraphBuilder Parent { get; set; }

        public EdgeBuilder AddEdge(string sourceNode, string destinationNode)
        {
            return Parent.AddEdge(sourceNode, destinationNode);
        }

        public NodeBuilder AddNode(string nodeName)
        {
            return Parent.AddNode(nodeName);
        }

        public string Build()
        {
            return Parent.Build();
        }
    }

    public class NodeBuilder : Builder
    {
        private GraphNode Node { get; }

        public NodeBuilder(GraphNode node, IGraphBuilder parent)
        {
            Node = node;
            Parent = parent;
        }

        public IGraphBuilder With(Action<NodeSettings> settings)
        {
            settings.Invoke(new NodeSettings(Node));
            return Parent;
        }
    }

    public class EdgeBuilder : Builder
    {
        private GraphEdge Edge { get; }

        public EdgeBuilder(GraphEdge edge, IGraphBuilder parent)
        {
            Edge = edge;
            Parent = parent;
        }

        public IGraphBuilder With(Action<EdgeSettings> settings)
        {
            settings.Invoke(new EdgeSettings(Edge));
            return Parent;
        }
    }

    public abstract class Settings<T>
        where T : Settings<T>
    {
        protected Dictionary<string, string> Dict { get; private set; }

        public Settings(Dictionary<string, string> settings)
        {
            Dict = settings;
        }

        public T Color(string color)
        {
            Dict["color"] = color;
            return (T)this;
        }

        public T FontSize<Tin>(Tin fontsize)
        {
            Dict["fontsize"] = $"{fontsize}";
            return (T)this;
        }

        public T Label(string label)
        {
            Dict["label"] = label;
            return (T)this;
        }
    }

    public class NodeSettings : Settings<NodeSettings>
    {
        private GraphNode Node { get; }

        public NodeSettings(GraphNode node) : base(node.Attributes)
        {
            Node = node;
        }

        public NodeSettings Shape(NodeShape nodeShape)
        {
            Dict["shape"] = nodeShape.ToString().ToLower();
            return this;
        }
    }

    public class EdgeSettings : Settings<EdgeSettings>
    {
        private GraphEdge Edge { get; }

        public EdgeSettings(GraphEdge edge) : base(edge.Attributes)
        {
            Edge = edge;
        }

        public EdgeSettings Weight<T>(T weight)
        {
            Dict["weight"] = weight.ToString();
            return this;
        }
    }
}