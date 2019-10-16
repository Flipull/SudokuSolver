using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Techniques
{
    public class UniqueCandidate
    {
        internal static int Execute(int[][] sudoku, List<int>[,] candidates, int y, int x)
        {
            int candidates_eliminated = 0;
            if (sudoku[y][x] != 0)//when square filled, do ray-test
                candidates_eliminated += UniqueRayTest(sudoku, candidates, y, x);

            if (sudoku[y][x] == 0)//when square not filled, do test
                candidates_eliminated += UniqueTest(sudoku, candidates, y, x);
            return candidates_eliminated;
        }
        static private int UniqueRayTest(int[][] sudoku, List<int>[,] candidates, int y, int x)
        {
            int candidates_eliminated = 0;
            for (byte horizontal = 0; horizontal < 9; horizontal++)
                if (candidates[y, horizontal].Remove(sudoku[y][x]))
                    candidates_eliminated++;
            for (byte vertical = 0; vertical < 9; vertical++)
                if (candidates[vertical, x].Remove(sudoku[y][x]))
                    candidates_eliminated++;
            
            int x_block = x / 3;
            int y_block = y / 3;
            for (byte horizontal_square = 0; horizontal_square < 3; horizontal_square++)
                for (byte vertical_square = 0; vertical_square < 3; vertical_square++)
                {
                    int x_offset = 3 * x_block + horizontal_square;
                    int y_offset = 3 * y_block + vertical_square;
                    if (candidates[y, x].Remove(sudoku[y_offset][x_offset]))
                        candidates_eliminated++;
                }

            return candidates_eliminated;
        }


        static private int UniqueTest(int[][] sudoku, List<int>[,] candidates, int y, int x)
        {
            int candidates_eliminated = 0;

            int x_block = x / 3;
            int y_block = y / 3;

            bool FindNumberInSudokuGrid(byte number)
            {
                for (byte horizontal_square = 0; horizontal_square < 3; horizontal_square++)
                    for (byte vertical_square = 0; vertical_square < 3; vertical_square++)
                    {
                        int x_offset = 3 * x_block + horizontal_square;
                        int y_offset = 3 * y_block + vertical_square;
                        if (sudoku[y_offset][x_offset] == number)
                            return true;
                    }
                return false;
            }
            
            for (byte uniquestest = 1; uniquestest <= 9; uniquestest++)
            {
                if (FindNumberInSudokuGrid(uniquestest))
                    continue;
                List<Tuple<int, int>> testnumber_insquare = new List<Tuple<int, int>>();
                for (byte horizontal_square = 0; horizontal_square < 3; horizontal_square++)
                    for (byte vertical_square = 0; vertical_square < 3; vertical_square++)
                    {
                        int x_offset = 3 * x_block + horizontal_square;
                        int y_offset = 3 * y_block + vertical_square;
                        if (candidates[y_offset, x_offset].Contains(uniquestest))
                            testnumber_insquare.Add(Tuple.Create(y_offset, x_offset));
                    }

                if (testnumber_insquare.Count == 1)
                {
                    
                    int unique_y = testnumber_insquare.First().Item1;
                    int unique_x = testnumber_insquare.First().Item2;

                    candidates_eliminated += candidates[unique_y, unique_x].Count - 1;
                    candidates[unique_y, unique_x].Clear();
                    candidates[unique_y, unique_x].Add(uniquestest);

                }
            }

            return candidates_eliminated;

        }
    }
}