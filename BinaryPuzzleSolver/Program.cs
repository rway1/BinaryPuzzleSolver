using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static BinaryPuzzleSolver.Solver;

namespace BinaryPuzzleSolver
{
    class Program
    {
        private static string SizeToDirectory(int size)
        {
            return size + "x" + size ;
        }

        private static string LevelToDirectory(int level)
        {
            switch (level)
            {
                case 1:
                    return @"easy";
                case 2:
                    return @"medium";
                case 3:
                    return @"hard";
                case 4:
                    return @"veryhard";
                default:
                    throw new ArgumentException();
            }
        }

        private static string PuzzleNoToFileName(int puzzleno, bool solution)
        {
            return "PuzzleNumber" + puzzleno + (solution? "_Solution" : "");
        }
        public static string GridFileTemplate = Directory.GetCurrentDirectory() + @"\Puzzles\{0}\{1}\{2}.txt";
        static void Main(string[] args)
        {
            bool failed = false;
            for (int level = 3; level <= 3 && !failed; level++)
            {
                for (int size = 3; size <= 7; size++)
                {
                    for (int puzzleno = 1; puzzleno <= 100 && !failed; puzzleno++)
                    {
                        string gridFile = string.Format(GridFileTemplate, SizeToDirectory(size*2), LevelToDirectory(level), PuzzleNoToFileName(puzzleno, false));
                        string solutionFile = string.Format(GridFileTemplate, SizeToDirectory(size*2), LevelToDirectory(level), PuzzleNoToFileName(puzzleno, true));

                        if (!File.Exists(gridFile) || !File.Exists(solutionFile)) throw new Exception("File not found");

                        List<List<char>> original_grid = GetGridFromFile(gridFile);
                        List<List<char>> solution_grid = GetGridFromFile(solutionFile);


                        Solver solver = new Solver(original_grid);
                        List<List<char>> resulting_grid = solver.Grid;

                        if (AreGridsSame(resulting_grid, solution_grid)) Console.WriteLine($"Passed size: {size} level: {level} puzzleno: {puzzleno}");
                        else
                        {
                            Console.WriteLine("Resulting Grid");
                            PrintGrid(resulting_grid);
                            Console.WriteLine("Solution Grid");
                            PrintGrid(solution_grid);
                            failed = true;
                        }
                    }
                }
            }

            Console.ReadKey();
        }

        static void PrintGrid(List<List<char>> grid)
        {
            foreach (var row in grid)
            {
                foreach (var item in row)
                {
                    Console.Write($"{item} ");
                }
                Console.WriteLine();
            }
        }

        static List<List<char>> GetGridFromFile(string fileName)
        {
            List<List<char>> grid = new List<List<char>>();
            List<string> lines = new List<string>(System.IO.File.ReadAllLines(fileName));
            foreach (string line in lines)
            {
                List<char> list = new List<string>(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ConvertAll((item) => { char.TryParse(item, out char found); return found; });
                grid.Add(list);
            }
            return grid;
        }

        static bool AreGridsSame(List<List<char>> first, List<List<char>> second)
        {
            if (first.Count != second.Count) throw new ArgumentException("Grids do not have the same dimensions");

            for (int i = 0; i < first.Count; i++)
            {
                if (first[i].Count != second[i].Count) throw new ArgumentException("Grids do not have the same dimensions");

                for (int j = 0; j < first[i].Count; j++)
                {
                    if (first[i][j] != second[i][j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
