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
            int[] rank, parent;
            readonly int vertices;

            public UnionFind(int VertexCount)
            {
                vertices = VertexCount;

                rank = new int[vertices];
                parent = new int[vertices];
            }

            private int findRoot(int v)
            {
                var r = v;

                while (!isRoot(r))
                {
                    r = parent[r];
                }

                //compress
                parent[v] = r;

                return r;
            }

            private bool isRoot(int v)
            {
                return parent[v] == v;
            }

            private void attachComponent(int VertexId, int RootId)
            {
                parent[VertexId] = RootId;
                rank[RootId] += rank[VertexId];
            }

            public void Union(int v0, int v1)
            {
                int r;
                if (rank[v0] >= rank[v1])
                {
                    r = find(v0);
                    attachComponent(v1, r);
                }
                else
                {
                    r = find(v1);
                    attachComponent(v0, r);
                }
            }

            public int find(int v)
            {
                return findRoot(v);
            }
        }

        public class MinimumSpannngTree<T> : List<Edge<T>> where T : IComparable {
            SortedSet<int> vertices = new SortedSet<int>();

            public void AddToTree(Edge<T> item)
            {
                vertices.Add(item.v0);
                vertices.Add(item.v1);
                this.Add(item);
            }

            public bool HasPathBetween(int v_start, int v_end)
            {
                return vertices.Contains(v_start) && vertices.Contains(v_end);
            }
        }

        public class MSTFactory
        {
            public static MinimumSpannngTree<T> Kruskal<T>(Graph<T> g) where T : IComparable
            {
                var MST = new MinimumSpannngTree<T>();

                var edges = g.GetEdges().OrderBy(x => x.weight);
                UnionFind uf = new UnionFind(g.Size);

                Edge<T> e = edges.First();
                uf.Union(e.v0, e.v1);

                foreach (var edge in edges)
                {
                    if (edge.Equals(e)) continue;

                    //find parent affiliation
                    int pi = uf.find(edge.v0);
                    int pu = uf.find(edge.v1);

                    //different components
                    if (pi != pu)
                    {
                        uf.Union(edge.v0, edge.v1);
                        MST.AddToTree(edge);
                    }

                    //if edges added reach V-1 we have a edge for every pair of nodes
                    if (!(MST.Count < g.Size)) break;
                }

                return MST;
            }
        }
    }
}
