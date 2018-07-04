using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph.Core;
using Graph.GameLevels;

namespace Test
{
    static class Program
    {

        static void ConnectedGraphTest()
        {
            var g = new Graph.Core.Grid.GridGraph<int>(3,3);

            for (int y = 0; y < g.Mapping.RowSz; y++)
            {
                for (int x = 0; x < g.Mapping.ColSz; x++)
                {
                    //connect to row below
                    if (y < g.Mapping.RowSz - 1)
                    {
                        g.AddEdge(x, y, x, y + 1, 1, true);
                    }

                    //connect to next col
                    if (x == g.Mapping.ColSz - 1) continue;
                    g.AddEdge(x, y, x + 1, y, 1, true);
                }
            }
            
            Graph.Util.Output.PrintGraph(g);
        }
        
        static void SparseGraphTest()
        {
            var g = new Graph.Core.Grid.GridGraph<int>(7,7);

            for (int y = 0; y < g.Mapping.RowSz; y++)
            {
                for (int x = 0; x < g.Mapping.ColSz; x++)
                {
                    //connect to next col
                    if (x == g.Mapping.ColSz - 1) continue;
                    g.AddEdge(x, y, x + 1, y, 1, true);
                }
                
                //connect to row below
                if (y < g.Mapping.RowSz - 1)
                {
                    g.AddEdge(g.Mapping.ColSz / 2, y, g.Mapping.ColSz / 2, y + 1, 1, true);
                }

            }
            
            Graph.Util.Output.PrintGraph(g);
        }

        static void TestRandomMaze()
        {
            Graph.GameLevels.RandomMaze rm = new RandomMaze(5,5, 42, 24), orm = new RandomMaze(5,5, 42, 24);
            
            Graph.Util.Output.PrintGraph(rm.G);
            Graph.Util.Output.PrintGraph(orm.G);

            var mst = Graph.Algorithms.MSTFactory.Kruskal(rm.G);

            Console.WriteLine("++MST");
            foreach (var t in mst)
            {
                Console.Write( " ("+t.v0+", "+t.v1+")" );
            }
            Console.WriteLine("\n--MST");
        }
        
        static void TestRandomSequence()
        {
            Random r = new Random();
            for (int i = 0; i < 22; i++)
            {
                var seed = r.Next();
                Console.Write("Seed "+seed + " " );
                Graph.GameLevels.RandomMaze
                rm1 = new RandomMaze(1,1, seed, seed), rm2 = new RandomMaze(1,1,seed,seed);

                var res = rm1.getSequence().Zip( rm2.getSequence(), (x,y) => new Tuple<int,int>(x,y) );
                
                foreach (var t in res )
                {
                    if (t.Item1 != t.Item2) throw new Exception("Sequences given mismatch: " + t.Item1 + ", " + t.Item2 );
                    Console.Write(" "+t.Item1 );
                }
                Console.WriteLine("");
            }
        }
        
        static void Main(string[] args)
        {

            ConnectedGraphTest();
            SparseGraphTest();
            TestRandomSequence();
            TestRandomMaze();
        }
    }
}
