using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryPuzzleSolver
{
    class Solver
    {
        public static char Opposite(char value)
        {
            if (value == '1')
            {
                return '0';
            }
            return '1';
        }

        public int n;

        private List<Move> movesQueue = new List<Move>();

        private List<List<CellParameter<char>>> m_grid = new List<List<CellParameter<char>>>();

        public List<List<char>> Grid
        {
            get { return m_grid.ConvertAll((row) => { return row.ConvertAll((item) => { return item.Value; }); }); }
        }

        private List<TripletMonitor> triplets = new List<TripletMonitor>();

        private List<RowMonitor> rowMonitors = new List<RowMonitor>();

        public Solver(List<List<char>> grid)
        {
            if (grid.Count != grid[0].Count) throw new ArgumentException("Grid must be nxn values");
            n = grid.Count;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] != '-')
                        movesQueue.Add(new Move { xposition = j, yposition = i, move = grid[i][j] });
                }
            }

            InitInternalGrid();
            InitTriplets();
            InitRowMonitors();
            ProcessMoves();
        }

        private void InitRowMonitors()
        {
            foreach (var row in m_grid)
            {
                rowMonitors.Add(new RowMonitor(row));
            }

            for (int i = 0; i < m_grid.Count; i++)
            {
                List<CellParameter<char>> list = new List<CellParameter<char>>();
                for (int j = 0; j < m_grid.Count; j++)
                {
                    list.Add(m_grid[j][i]);
                }
                rowMonitors.Add(new RowMonitor(list));
            }
        }

        private void ProcessMoves()
        {
            foreach (var move in movesQueue)
            {
                m_grid[move.yposition][move.xposition].Value = move.move;
            }
        }

        private void InitTriplets()
        {
            for (int j = 0; j < m_grid.Count; j++)
            {
                for (int i = 0; i < m_grid[j].Count - 2; i++)
                {
                    triplets.Add(new TripletMonitor(m_grid[j][i], m_grid[j][i + 1], m_grid[j][i + 2]));
                } 
            }

            for (int i = 0; i < m_grid[0].Count -2 ; i++)
            {
                for (int j = 0; j < m_grid.Count; j++)
                {
                    triplets.Add(new TripletMonitor(m_grid[i][j], m_grid[i + 1][j], m_grid[i + 2][j]));
                }
            }
        }

        private void InitInternalGrid()
        {
            for (int i = 0; i < n; i++)
            {
                List<CellParameter<char>> list = new List<CellParameter<char>>();
                for (int j = 0; j < n; j++)
                {
                    list.Add(new CellParameter<char>() { Value = '-' });
                }
                m_grid.Add(list);
            }
        }

        public void PrintGrid()
        {
            Console.WriteLine("Printing Internal Grid...");
            foreach (var row in m_grid)
            {
                foreach (var item in row)
                {
                    Console.Write(item.Value + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\n\n");
        }

        public void PrintMovesList()
        {
            Console.WriteLine("Printing Internal Grid...");
            foreach (var move in movesQueue)
            {
                Console.WriteLine($"{move.xposition}\t{move.yposition}\t{move.move}");
            }

        }

        private class Move
        {
            public int xposition;
            public int yposition;
            public char move;
        }
    }
}
