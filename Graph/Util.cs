using Graph.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph
{
    namespace Util
    {
        public class Output
        {
            public static void PrintGraph<T>(Graph.Core.Graph<T> g) where T : IComparable
            {
                foreach (Vertex v in g.GetVertices())
                {
                    Console.Write(" " + v.GetAdjacent().Count());
                }
                //                Console.WriteLine(  );
            }

            public static void PrintGraph<T>(Graph.Core.Grid.GridGraph<T> g) where T : IComparable
            {
                for (int row = 0; row < g.Mapping.RowSz; row++)
                {
                    var r_data = g.GetVertices().Skip(row * g.Mapping.ColSz).Take(g.Mapping.ColSz);
                    foreach (Vertex v in r_data)
                    {
                        Console.Write(" " + v.GetAdjacent().Count());
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
