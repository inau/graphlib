using Graph.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    namespace Algorithms
    {

        public class UnionFind
        {
            readonly int[] Rank, Parent;
            public int Vertices { get; private set; }
            public int Components { get; private set; }

            public UnionFind(int VertexCount)
            {
                Vertices = VertexCount;
                Components = Vertices;

                Rank = new int[Vertices];
                Parent = new int[Vertices];
                for (int i = 0; i < VertexCount; i++)
                {
                    Parent[i] = i;
                    Rank[i] = 1;
                }
            }

            private int findRoot(int v)
            {
                var r = v;

                while (!isRoot(r))
                {
                    Parent[r] = Parent[ Parent[r] ];
                    r = Parent[r];
                }
                return r;
            }

            private bool isRoot(int v)
            {
                return Parent[v] == v;
            }

            private void attachComponent(int VertexId, int RootId)
            {
                Parent[VertexId] = RootId;
                Rank[RootId] += Rank[VertexId];
                Components--;
            }

            public bool Connected()
            {

            }

            public void Union(int v0, int v1)
            {
                int r0 = Find(v0), r1 = Find(v1);
                if (Rank[r0] >= Rank[r1])
                    attachComponent(v1, r0);
                else attachComponent(v0, r1);
            }

            public int Find(int v)
            {
                return findRoot(v);
            }

            public IEnumerable<int> GetComponentParentVertices()
            {
                var distinct = Parent.AsEnumerable().Distinct();
                if (distinct.Count() != Components)
                {
                    Console.WriteLine("\n-flattening");
                    for( int i = 0; i < Parent.Length; i++)
                    {
                        Console.Write(" " + i);
                        findRoot(i);
                    }
                    Console.WriteLine("\n--flattening");
                }
                else return distinct;

                return Parent.AsEnumerable().Distinct();
            }
        }

        public class MinimumSpannngTree<T> : List<Edge<T>> where T : IComparable {
            readonly SortedSet<int> vertices = new SortedSet<int>();
            public readonly UnionFind Uf;

            public MinimumSpannngTree(int Vertices) : base() {
                Uf = new UnionFind(Vertices);
            }

            private void _AddToTree(Edge<T> item)
            {
                vertices.Add(item.v0);
                vertices.Add(item.v1);
                this.Add(item);
            }

            public void AddToTree(Edge<T> item)
            {
                if(this.Count == 0)
                {
                    Uf.Union(item.v0, item.v1);
                    _AddToTree(item);
                    return;
                }

                //find parent affiliation
                int pi = Uf.Find(item.v0);
                int pu = Uf.Find(item.v1);

                //different components
                if (pi != pu)
                {
                    Uf.Union(item.v0, item.v1);
                    _AddToTree(item);
                }

            }

            public bool HasPathBetween(int v_start, int v_end)
            {
                return vertices.Contains(v_start) && vertices.Contains(v_end);
            }

            public IEnumerable<int> ConnectedVertices()
            {
                return vertices;
            }
        }

        public static class MSTFactory
        {
            public static MinimumSpannngTree<T> Kruskal<T>(Graph<T> g) where T : IComparable
            {
                var MST = new MinimumSpannngTree<T>( g.Size );

                var sortedEdges = g.GetEdges().OrderBy(x => x.weight);               

                foreach (var edge in sortedEdges)
                {
                    MST.AddToTree(edge);

                    //if edges added reach V-1 we have a edge for every pair of nodes
                    //if (!(MST.Count < g.Size)) break;
                }

                return MST;
            }
        }
    }
}
