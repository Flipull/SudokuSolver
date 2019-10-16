using SudokuSolver.Logics.Techniques;
using SudokuSolver.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using static SudokuSolver.Logics.Techniques.PseudoBruteForce;

namespace SudokuSolver.Logics
{
    public class Solver
    {
        private class TechniqueResult
        {
            public bool hasFound = false;
            public int amount = 0;
            public int candidatesEliminated = 0;

            public TechniqueResult()
            {
            }
            public TechniqueResult(int amnt)
            {
                hasFound = true;
                amount = amnt;
            }
        }
        public int[][] Solve(int[][] sudoku)
        {

            Stack<GambleState> gamblelist = new Stack<GambleState>();
            bool finished = false;
            List<int>[,] candidates = CompleteCandidateList(sudoku);
            GambleState gamble = null;

            while (!finished) {

                List<TechniqueResult> result = new List<TechniqueResult>();
                candidates = CompleteCandidateList(sudoku);
                result.Add(ProcessBoard(sudoku, candidates, SoleCandidate.Execute));
                result.Add( ProcessBoard(sudoku, candidates, UniqueCandidate.Execute) );
                result.Add( ProcessBoard(sudoku, candidates, BlockInteraction.Execute) );
                
                bool isvalidated = Validation.validate(sudoku);
                if (isvalidated && isFinished(sudoku))
                {
                    finished = true;
                    continue;
                }
                
                int countresults = 0;
                foreach (TechniqueResult tech in result)
                    countresults += tech.amount;
                if (countresults > 0)
                    continue;
                
                //if invalid or unsolvable puzzle-state
                if ( (!isvalidated || (!isFinished(sudoku) && !hasCandidates(candidates))) )
                {//rollback to previous state
                    while (gamblelist.Count > 0 && !gamble.hasNextState() )
                    {
                        gamble = gamblelist.Pop();
                    }
                    
                    Debug.WriteLine("R: "+gamblelist.Count.ToString());
                    if (gamble.hasNextState())
                    {
                        sudoku = gamble.CloneOriginalSudoku();
                        candidates = gamble.CloneOriginalCandidates();
                            
                        Tuple<int, int, int> guesss = gamble.getNextState();
                        sudoku[guesss.Item1][guesss.Item2] = guesss.Item3;
                        candidates[guesss.Item1, guesss.Item2].Clear();

                        result.Add(new TechniqueResult(1));
                    } else
                    {//brute force exhausted all possibilities
                        finished = true;
                        continue;
                    }
                }
                else//if valid puzzle-solution or puzzle-state
                {//continue gambling
                    if (gamble != null) gamblelist.Push(gamble);
                    gamble = new GambleState(sudoku, candidates);
                    while (!gamble.hasNextState())
                    {
                        gamble = gamblelist.Pop();
                    }
                    Debug.WriteLine("C: "+gamblelist.Count.ToString());
                    sudoku = gamble.CloneOriginalSudoku();
                    candidates = gamble.CloneOriginalCandidates();
                    
                    //manually set sudoku-state
                    Tuple<int, int, int> guess = gamble.getNextState();
                    sudoku[guess.Item1][guess.Item2] = guess.Item3;
                    candidates[guess.Item1, guess.Item2].Clear();
                    result.Add(new TechniqueResult(1));
                }

            }
            return sudoku;
        }

        private TechniqueResult ProcessBoard(int[][] sudoku, List<int>[,] candidates,
                                    Func<int[][], List<int>[,], int, int, int> techniquefunction)
        {
            TechniqueResult result = new TechniqueResult();

            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    result.candidatesEliminated += techniquefunction(sudoku, candidates, i, j);
            
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (candidates[i, j].Count == 1)
                    {
                        sudoku[i][j] = candidates[i, j].ElementAt(0);
                        candidates[i, j].RemoveAt(0);
                        result.hasFound = true;
                        result.amount++;
                    }
                }
            
            return result;
        }
        public int[][] Create(int[][] sudoku)
        {
            return sudoku;
        }
        //////////////////////////////
        private List<int>[,] CompleteCandidateList(int[][] sudoku)
        {
            List<int>[,] candidates = new List<int>[9, 9];

            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                {
                    candidates[i, j] = new List<int>();
                    if (sudoku[i][j] == 0)
                        for (byte value = 1; value <= 9; value++)
                        {
                            candidates[i, j].Add(value);
                        }
                    //else
                    //    candidates[i, j].Add(sudoku[i][j]);
                }
            return candidates;
        }

        private bool hasCandidates(List<int>[,] candidates)
        {
            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                    if (candidates[i, j].Count > 0)
                        return true;
            return false;
        }

        private bool isFinished(int[][] sudoku)
        {
            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                    if (sudoku[i][j] == 0)
                        return false;
            return true;
        }

        /////////////////////////////////

        //helpers
    }
}