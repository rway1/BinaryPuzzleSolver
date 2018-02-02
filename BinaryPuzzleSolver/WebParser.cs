using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BinaryPuzzleSolver
{
    class WebParser
    {
        public class StringParamEventArgs : EventArgs
        {
            public string Message { get; set; }

            public StringParamEventArgs(string message)
            {
                Message = message;
            }
        }

        public static EventHandler<StringParamEventArgs> TaskStarted;

        public static EventHandler<StringParamEventArgs> FileAdded;

        private static string HTMLTEMPLATE = "http://binarypuzzle.com/puzzles.php?size={0}&level={1}&nr={2}";

        private static string SizeToDirectory(int size)
        {
            return size + "x" + size + @"\";
        }

        private static string LevelToDirectory(int level)
        {
            switch (level)
            {
                case 1:
                    return @"easy\";
                case 2:
                    return @"medium\";
                case 3:
                    return @"hard\";
                case 4:
                    return @"veryhard\";
                default:
                    throw new ArgumentException();
            }
        }

        private static string PuzzleNoToFileName(int puzzleno)
        {
            return "PuzzleNumber" + puzzleno + "_Solution.txt";
        }

        public static void GetBinaryPuzzleFile(int size = 6, int level = 1, int puzzleno = 1)
        {
            if (size / 2 < 3 || size / 2 > 7) throw new ArgumentException("size must be one of the following values: 6, 8, 10, 12, 14");
            if (puzzleno < 1 || puzzleno > 100) throw new ArgumentException("Puzzle no must be between 1 and 100");
            if (level < 1 || level > 4) throw new ArgumentException("Level must be between 1 and 4");

            HtmlWeb web = new HtmlWeb();

            TaskStarted?.Invoke(null, new StringParamEventArgs($"Task with size: {size}\tlevel:{level}\tpuzzleno:{puzzleno} started"));

            var htmlDoc = web.Load(string.Format(HTMLTEMPLATE, size, level, puzzleno));

            ////ParsePuzzle
            //var nodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'puzzlecel')]");
            //char[,] puzzle = new char[size, size];
            //for (int i = 0; i < size; i++)
            //{
            //    for (int j = 0; j < size; j++)
            //    {
            //        if (int.TryParse(nodes[i + size * j].InnerText, out int cellValue)) puzzle[i, j] = cellValue.ToString()[0];
            //        else puzzle[i, j] = '-';
            //    }
            //}

            //List<string> lines = new List<string>();
            //for (int i = 0; i < size; i++)
            //{
            //    string line = string.Empty;
            //    for (int j = 0; j < size; j++)
            //    {
            //        line += puzzle[j, i] + " ";
            //    }
            //    lines.Add(line);
            //}

            //ParseSolution
            var nodes = htmlDoc.DocumentNode.SelectNodes("//script");
            var regex = new Regex(@"oplossing\[\d+\]\[\d+\] = \'\d\'");
            var matches = nodes.Last().InnerText.Split("\n").ToList().FindAll((line) => regex.IsMatch(line));

            char[,] puzzle = new char[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (int.TryParse(matches[i + size * j].Substring(matches[i + size * j].Length - 4, 1), out int result)) puzzle[i, j] = result.ToString()[0];
                }
            }


            List<string> lines = new List<string>();
            for (int i = 0; i < size; i++)
            {
                string line = string.Empty;
                for (int j = 0; j < size; j++)
                {
                    line += puzzle[j, i] + " ";
                }
                lines.Add(line);
            }


            string directory = Directory.GetCurrentDirectory() + @"\Puzzles\" + SizeToDirectory(size);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            directory += LevelToDirectory(level);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            directory += PuzzleNoToFileName(puzzleno);

            System.IO.File.WriteAllLines(directory, lines);
            StringParamEventArgs e = new StringParamEventArgs($"File {directory} added");
            FileAdded?.Invoke(null, e);
        }
    }
}
