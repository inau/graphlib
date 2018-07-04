using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph.Core.Grid;

namespace Graph
{
    namespace GameLevels
    {
        class Mapper2D {
            public readonly int CellSz, WallSz;

            public Mapper2D(int CellSz, int WallThickness)
            {
                this.CellSz = CellSz;
                this.WallSz = WallThickness;
            }

            public Tuple<float, float> get2DCordinate(GridVertex gv)
            {
                float   x = gv.col * CellSz + (gv.col * WallSz) + WallSz,
                        y = gv.row * CellSz + (gv.row * WallSz) + WallSz;
                
                return new Tuple<float, float>(1f, 1f);
            }
        }

        public class Maze {

            public Maze(int cellCols, int cellRows, int cellSz, int wallThickness)
            {
                Mapper2D m2d = new Mapper2D(cellSz, wallThickness);
                
            }

        }


        public class RandomMaze
        {
            public readonly int SeedMap, SeedPU;
            private Random randM, randPU;
            public readonly GridGraph<int> G;
            
            public RandomMaze(int cols, int rows, int seedMap, int seedPowerups)
            {
                SeedMap = seedMap;
                SeedPU = seedPowerups;
                G = new GridGraph<int>(cols, rows);
                
                randM = new Random(SeedMap);
                randPU = new Random(SeedPU);
                
                GenWalls();
            }

            bool RollWall()
            {
                return randM.Next(4) == 2;
            }
            
            void GenWalls()
            {
                
                for (int y = 0; y < G.Mapping.RowSz; y++)
                {
                    for (int x = 0; x < G.Mapping.ColSz; x++)
                    {
                        //connect to next col
                        if ( RollWall() )
                        {
                            if (x != G.Mapping.ColSz - 1) G.AddEdge(x, y, x + 1, y, 1, true);                            
                        }

                        //connect to row below
                        if (RollWall())
                        {
                            if (y < G.Mapping.RowSz - 1) G.AddEdge(x, y, x, y + 1, 1, true); 
                        }
                    }
                }
            }

            public IEnumerable<int> getSequence()
            {
                return new int[5] { randM.Next(100), randM.Next(100),randM.Next(100),randM.Next(100),randM.Next(100) };
            }
        }
        
    }
}
