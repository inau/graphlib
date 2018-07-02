using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var g = new Graph.Core.Grid.GridGraph<int>(3,3);

            for (int y = 0; y < g.Mapping.RowSz; y++)
            {
                for (int x = 0; x < g.Mapping.ColSz; x++)
                {
                    if (y < g.Mapping.RowSz - 1)
                    {
                        g.AddEdge(x, y, x, y + 1, 1, true);
                    }

                    if (x == g.Mapping.ColSz - 1) continue;
                    g.AddEdge(x, y, x + 1, y, 1, true);
                }
            }

            var mst = Graph.Algorithms.MSTFactory.Kruskal<int>(g);
            mst.HasPathBetween(1,2);

            Graph.Util.Output.PrintGraph<int>(g);


        }
    }
}
