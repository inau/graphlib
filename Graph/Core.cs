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
                    if (col >= 0 && col < ColSz && row >= 0 && row < RowSz)
                        return col + row * ColSz;
                    else throw 
                        new ArgumentOutOfRangeException("column and row values range from 0 (inclusive) to size of dimension(exclusive) - provided values: " + col + " & " + row);
                }
            }

            public class GridVertex : Vertex
            {
                public readonly int col, row;
                
                //vertex shorthands
                public readonly int LEFT, UP, RIGHT, DOWN;

                public GridVertex(int c, int r, Grid.GridMapper gm) : base(gm.VertexIndex(c, r))
                {
                    col = c;
                    row = r;

                    LEFT = (c-1 < 0) ? -1 : gm.VertexIndex(c-1, r);
                    RIGHT = (c+1 < gm.ColSz) ? gm.VertexIndex(c+1, r) : -1;
                    UP = (r-1 < 0) ? -1 : gm.VertexIndex(c, r - 1);
                    DOWN = (r+1 < gm.RowSz) ? gm.VertexIndex(c, r + 1) : -1;
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
                    
                    //lazy instantiation
                    Vertex n1 = GetVertex(x1, y1), n2 = GetVertex(x2,y2);

                    _AddEdge(n1.Id, n2.Id, weight, BiDirectional);
                }

                public GridVertex GetVertex(int x, int y)
                {
                    return (GridVertex) (nodes[Mapping.VertexIndex(x, y)] ?? (nodes[Mapping.VertexIndex(x,y)] = new GridVertex(x,y, Mapping)));
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
            public readonly int Id;
            readonly List<int> AdjacentEdgeIds = new List<int>();
            readonly List<int> AdjacentVertices = new List<int>();

            public Vertex(int id) { this.Id = id; }


            public void AddEdgeTo<T>(int e_id, Edge<T> e) where T : IComparable
            {
                AdjacentEdgeIds.Add(e_id);
                AdjacentVertices.Add( e.v0 == Id ? e.v1 : e.v0 );
            }

            public IEnumerable<int> GetAdjacentEdgeIds()
            {
                return AdjacentEdgeIds.AsEnumerable<int>();
            }
            
            public IEnumerable<int> GetAdjacentVetices()
            {
                return AdjacentVertices.AsEnumerable<int>();
            }

            public bool ConnectsToVertex(int v)
            {
                return AdjacentVertices.Contains(v);
            }

        }

        public class Graph<T> where T : IComparable
        {
            protected Vertex[] nodes;
            private List<Edge<T>> edges = new List<Edge<T>>();

            public readonly int Size;

            protected Graph(int Vertices)
            {
                nodes = new Vertex[Vertices];
                Size = Vertices;
            }

            protected void _AddEdge(int v1, int v2, T w, bool BiDirectional = false)
            {
                Edge<T> e = new Edge<T>(v1, v2, w);
                edges.Add(e);
                int e_id = edges.Count - 1;

                nodes[v1].AddEdgeTo(e_id, e);

                if (BiDirectional)
                    nodes[v2].AddEdgeTo(e_id, e);
            }

            public void AddEdge(int v1, int v2, T w, bool BiDirectional = false)
            {
                //lazy instantiation
                Vertex n1 = GetVertex(v1), n2 = GetVertex(v2);

                _AddEdge(v1, v2, w, BiDirectional);
            }

            public Vertex GetVertex(int v_id)
            {
                return nodes[v_id] ?? (nodes[v_id] = new Vertex(v_id));
            }

            public IEnumerable<Edge<T>> GetEdges()
            {
                return edges.AsEnumerable<Edge<T>>();
            }

            public IEnumerable<Vertex> GetVertices()
            {
                return nodes.AsEnumerable<Vertex>();
            }
        }

     
    }
}
