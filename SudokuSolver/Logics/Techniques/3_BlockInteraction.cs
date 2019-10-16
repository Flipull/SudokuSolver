using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Techniques
{
    public class BlockInteraction
    {
        internal static int Execute(int[][] sudoku, List<int>[,] candidates, int y, int x)
        {
            int candidates_eliminated = 0;
            if (sudoku[y][x] == 0)//when square not filled
                candidates_eliminated = BlockTest(sudoku, candidates, y, x);
            return candidates_eliminated;
        }

        static private int BlockTest(int[][] sudoku, List<int>[,] candidates, int y, int x)
        {
            int candidates_eliminated = 0;
            for (byte number_loop = 1; number_loop <= 9; number_loop++)
            {

                List<Tuple<int, int>> cells = CandidatesInGridCells(sudoku, candidates, y, x, number_loop);
                
                //calculate rows/columns which can't contain this number
                List<int> included_cols = new List<int>();
                List<int> included_rows = new List<int>();
                foreach (Tuple<int, int> cell in cells)
                {
                    included_cols.Add(cell.Item2);
                    included_rows.Add(cell.Item1);
                }
                
                //remove the row/column exclusions from neighbouring grids
                List<Tuple<int, int>> neighbours = GetHorizontalNeighbourGrids(y, x);
                if (included_rows.Count == 1 )
                {
                    foreach (Tuple<int, int> gridnumber in neighbours)
                        for (int col_loop = 0; col_loop < 3; col_loop++)
                            if (candidates[included_rows[0], gridnumber.Item2 * 3 + col_loop].Remove(number_loop) )
                                candidates_eliminated++;
                }

                neighbours = GetVerticalNeighbourGrids(y, x);
                if (included_cols.Count == 1)
                {
                    foreach (Tuple<int, int> gridnumber in neighbours)
                        for (int row_loop = 0; row_loop < 3; row_loop++)
                            if (candidates[gridnumber.Item1 * 3 + row_loop, included_cols[0]].Remove(number_loop))
                                candidates_eliminated++;
                }
            }

            return candidates_eliminated;

        }
        ///////////////
        static private List<Tuple<int, int>> GetHorizontalNeighbourGrids(int y, int x)
        {
            int x_block = x / 3;
            int y_block = y / 3;

            List<Tuple<int, int>> gridlist = new List<Tuple<int, int>>();
            for (byte grid_loop = 0; grid_loop < 3; grid_loop++)
            {
                if (x_block == grid_loop) continue;//skip x
                gridlist.Add(new Tuple<int, int>(y_block, grid_loop));
            }
            return gridlist;
        }

        static private List<Tuple<int, int>> GetVerticalNeighbourGrids(int y, int x)
        {
            int x_block = x / 3;
            int y_block = y / 3;

            List<Tuple<int, int>> gridlist = new List<Tuple<int, int>>();
            for (byte grid_loop = 0; grid_loop < 3; grid_loop++)
            {
                if (y_block == grid_loop) continue;//skip y
                gridlist.Add(new Tuple<int, int>(grid_loop, x_block));
            }
            return gridlist;
        }

        static private List<int> GetNeighbourRowNumbers(int y, int x)
        {
            List<int> numberlist = new List<int>(); 
            int x_block = x / 3;
            int block_row = x % 3;
            
            for (byte row_loop=0; row_loop < 3; row_loop++)
            {
                if (block_row == row_loop) continue;//skip x
                numberlist.Add(x_block*3 + row_loop);
            }
            return numberlist;
        }
        static private List<int> GetNeighbourColNumbers(int y, int x)
        {
            List<int> numberlist = new List<int>();
            int y_block = y / 3;
            int block_col = y % 3;

            for (byte col_loop = 0; col_loop < 3; col_loop++)
            {
                if (block_col == col_loop) continue;//skip x
                numberlist.Add(y_block *3 + col_loop);
            }
            return numberlist;
        }


        static private List<Tuple<int, int>> CandidatesInGridCells(int[][] sudoku, List<int>[,] candidates, int y, int x, int value)
        {
            int x_block = x / 3;
            int y_block = y / 3;
            
            bool FindNumberInSudokuGrid(int number)
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

            List<Tuple<int, int>> candidates_ingrid = new List<Tuple<int, int>>();
            if (FindNumberInSudokuGrid(value))
                return candidates_ingrid;
            for (byte horizontal_square = 0; horizontal_square < 3; horizontal_square++)
                for (byte vertical_square = 0; vertical_square < 3; vertical_square++)
                {
                    int x_offset = 3 * x_block + horizontal_square;
                    int y_offset = 3 * y_block + vertical_square;
                    if (candidates[y_offset, x_offset].Contains(value))
                        candidates_ingrid.Add(Tuple.Create(y_offset, x_offset));
                }
            return candidates_ingrid;
        }
    }
}