using Graph.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph.Core.Grid;

namespace Graph
{
    namespace Util
    {
        public static class Output
        {
            public static void PrintGraph<T>(Graph.Core.Graph<T> g) where T : IComparable
            {
                foreach (Vertex v in g.GetVertices())
                {
                    Console.Write(" " + v.GetAdjacentEdgeIds().Count());
                }
                //                Console.WriteLine(  );
            }
            
            public static void PrintGraph<T>(Graph.Core.Grid.GridGraph<T> g) where T : IComparable
            {
                for (int row = 0; row < g.Mapping.RowSz; row++)
                    Console.Write(" _");
                Console.WriteLine();

                for (int row = 0; row < g.Mapping.RowSz; row++)
                {
                    Console.Write("|");
                    for (int col = 0; col < g.Mapping.ColSz; col++)
                    {
                        GridVertex v = g.GetVertex(col, row);
                        var b = v.ConnectsToVertex(v.DOWN) ? " " : "_";
                        var r = v.ConnectsToVertex(v.RIGHT) ? " " : "|";
                        Console.Write(b+r);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
