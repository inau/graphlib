using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    namespace Core
    {
        namespace Grid
        {

            public class GridMapper
            {
                public readonly int ColSz, RowSz;

                public GridMapper(int cols, int rows)
                {
                    ColSz = cols;
                    RowSz = rows;
                }

                public int VertexIndex(int col, int row)
                {
                    return col + row * ColSz;
                }
            }

            public class GridVertex : Vertex
            {
                public readonly int col, row;

                public GridVertex(int c, int r, Grid.GridMapper gm) : base(gm.VertexIndex(c, r))
                {
                    col = c;
                    row = r;
                }
            }

            public class GridGraph<T> : Graph<T> where T : IComparable
            {
                public readonly GridMapper Mapping;
                public GridGraph(int cols, int rows)
                    : base(cols * rows)
                {
                    Mapping = new GridMapper(cols, rows);
                }

                public void AddEdge(int x1, int y1, int x2, int y2, T weight, bool BiDirectional = false)
                {
                    int id1 = Mapping.VertexIndex(x1, y1), id2 = Mapping.VertexIndex(x2, y2);
                    //lazy instantiation
                    Vertex n1 = nodes[id1], n2 = nodes[id2];
                    if (n1 == null)
                        nodes[id1] = new GridVertex(x1,y1, Mapping);
                    if (n2 == null)
                        nodes[id2] = new GridVertex(x2, y2, Mapping);

                    _AddEdge(Mapping.VertexIndex(x1, y1), Mapping.VertexIndex(x2, y2), weight, BiDirectional);
                }

                public Vertex GetVertex(int x, int y)
                {
                    return nodes[Mapping.VertexIndex(x, y)];
                }

            }

        }

        public class Edge<T> where T : IComparable
        {
            public readonly T weight;
            public readonly int v0, v1;

            public Edge(int v0, int v1, T w)
            {
                weight = w;
                this.v0 = v0;
                this.v1 = v1;
            }

            public static bool operator <(Edge<T> a, Edge<T> b)
            {
                return a.weight.CompareTo(b.weight) < 0;
            }

            public static bool operator >(Edge<T> a, Edge<T> b)
            {
                return b < a;
            }

            /**
                        public static bool operator== (Edge<T> a, Edge<T> b)
                        {
                            return !(a<b) && !(b<a);
                        }

                        public static bool operator!= (Edge<T> a, Edge<T> b)
                        {
                            return !(a == b);
                        }
            **/
        }

        public class Vertex
        {
            public readonly int id;
            List<int> Adjacent = new List<int>();

            public Vertex(int id) { this.id = id; }


            public void AddEdgeTo(int v_other)
            {
                Adjacent.Add(v_other);
            }

            public IEnumerable<int> GetAdjacent()
            {
                return Adjacent.AsEnumerable<int>();
            }

        }

        public class Graph<T> where T : IComparable
        {
            protected Vertex[] nodes;
            protected List<Edge<T>> edges = new List<Edge<T>>();

            public readonly int Size;

            public Graph(int Vertices)
            {
                nodes = new Vertex[Vertices];
                Size = Vertices;
            }

            protected void lazyInstantiateVertex(Vertex v)
            {
                if (nodes[v.id] == null)
                    nodes[v.id] = v;
            }

            protected void _AddEdge(int v1, int v2, T w, bool BiDirectional = false)
            {
                Edge<T> e = new Edge<T>(v1, v2, w);
                edges.Add(e);
                int e_id = edges.Count - 1;

                nodes[v1].AddEdgeTo(e_id);

                if (BiDirectional)
                    nodes[v2].AddEdgeTo(e_id);
            }

            public void AddEdge(int v1, int v2, T w, bool BiDirectional = false)
            {
                //lazy instantiation
                Vertex n1 = nodes[v1], n2 = nodes[v2];
                if (n1 == null)
                    nodes[v1] = new Vertex(v1);
                if (n2 == null)
                    nodes[v2] = new Vertex(v2);

                _AddEdge(v1, v2, w, BiDirectional);
            }

            public Vertex GetVertex(int v_id)
            {
                return nodes[v_id];
            }

            public IEnumerable<Edge<T>> GetEdges()
            {
                return edges.AsEnumerable<Edge<T>>();
            }

            internal IEnumerable<Vertex> GetVertices()
            {
                return nodes.AsEnumerable<Vertex>();
            }
        }

      


    }
}
