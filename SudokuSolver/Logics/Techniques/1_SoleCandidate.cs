using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Techniques
{
    public class SoleCandidate
    {
        internal static int Execute(int[][] sudoku, List<int>[,] candidates, int y, int x)
        {
            int candidates_eliminated = 0;
            if (sudoku[y][x] != 0) return 0;//when square already filled, return
            //fixes bug in grid-comparison where number n is erased because number n already in [3..3] grid (when n finds itself)

            for (byte horizontal = 0; horizontal < 9; horizontal++)
                if (candidates[y, x].Remove(sudoku[y][horizontal]))
                    candidates_eliminated++;
            
            for (byte vertical = 0; vertical < 9; vertical++)
                if (candidates[y, x].Remove(sudoku[vertical][x]))
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
    }
}