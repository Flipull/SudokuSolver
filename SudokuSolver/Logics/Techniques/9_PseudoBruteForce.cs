using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SudokuSolver.Logics.Techniques
{
    public class PseudoBruteForce
    {
        public class GambleState
        {
            private int[][] mementoSudoku;
            private List<int>[,] mementoCandidates;
            private List<Tuple<int,int, List<int>>> sortedCells;
            public GambleState(int[][] sudoku, List<int>[,] candidates)
            {
                mementoSudoku = CopySudoku(sudoku);
                mementoCandidates = CopyCandidates(candidates);
                sortedCells = createCellListSorted(mementoCandidates);
            }

            private Tuple<int,int, int> GetCurrentGuess()
            {
                if (sortedCells.Count == 0) throw new Exception();
                if (sortedCells[0].Item3.Count == 0) throw new Exception();
                
                Tuple<int,int,List<int>> NextSuggestion = sortedCells[0];
                Tuple < int,int,int> guess = 
                    Tuple.Create<int, int, int>(NextSuggestion.Item1, NextSuggestion.Item2, NextSuggestion.Item3[0]);
                return guess;
            }
            public Tuple<int,int,int> getNextState()
            {
                /*
                while (sortedCells.Count > 0 && sortedCells[0].Item3.Count == 0)
                        sortedCells.RemoveAt(0);
                if (sortedCells.Count == 0)
                    return null;

                
                Tuple<int, int, int> currentguess = GetCurrentGuess();
                sortedCells[0].Item3.RemoveAt(0);
                if (sortedCells[0].Item3.Count == 0)
                    sortedCells.RemoveAt(0);

                return currentguess;
                */
                Tuple<int, int, int> currentguess = GetCurrentGuess();
                sortedCells[0].Item3.RemoveAt(0);
                return currentguess;
            }

            public bool hasNextState()
            {
                /*
                while (sortedCells.Count > 0 && sortedCells[0].Item3.Count == 0)
                    sortedCells.RemoveAt(0);

                if (sortedCells.Count == 0) 
                    return false;
                else
                    return true;
               */
                return (sortedCells.Count > 0 && sortedCells[0].Item3.Count > 0);
            }

            public int[][] CloneOriginalSudoku()
            {
                return CopySudoku(mementoSudoku);
            }
            public List<int>[,] CloneOriginalCandidates()
            {
                return CopyCandidates(mementoCandidates);
            }
            //
            private int[][] CopySudoku(int[][] sudoku)
            {
                int[][] result = new int[9][];
                
                for (byte i = 0; i < 9; i++)
                {
                    result[i] = new int[9];
                    for (byte j = 0; j < 9; j++)
                    {
                        result[i][j] = sudoku[i][j];
                    }

                }
                return result;
            }
            private List<int>[,] CopyCandidates(List<int>[,] candidates)
            {
                List<int>[,] result = new List<int>[9,9];
                for (byte i = 0; i < 9; i++)
                    for (byte j = 0; j < 9; j++)
                    {
                        result[i, j] = candidates[i, j];
                    }
                return result;
            }
            private List<Tuple<int, int, List<int>>> createCellListSorted(List<int>[,] candidates)
            {
                List<Tuple<int, int, List<int>>> result = new List<Tuple<int, int, List<int>>>();
                for (byte i = 0; i < 9; i++)
                    for (byte j = 0; j < 9; j++)
                        if (candidates[i,j].Count > 1)    
                        {
                            Tuple<int, int, List<int>> celldata = Tuple.Create<int, int, List<int>>(i, j, new List<int>(candidates[i, j]) );
                            result.Add(celldata);
                        }
                
                result.Sort(
                        delegate (Tuple<int, int, List<int>> c1, Tuple<int, int, List<int>> c2)
                        {
                            return c1.Item3.Count.CompareTo(c2.Item3.Count);
                        });
                return result;
            }
            //
        }
        
        static GambleState Create(int[][] sudoku, List<int>[,] candidates)
        {
            return new GambleState(sudoku, candidates);
        }
        internal static int Execute(int[][] sudoku, List<int>[,] candidates)
        {
            return 0;   
        }

    }
}