using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBreakerBreaker
{
    class GridComparison
    {
        public int[] ColH; //ColH[i] is how many numbers in column i are exact matches for the solution
        public int[] RowH; //RowH[j] is how many numbers in row j are exact matches for the solution
        public int[] ColB; //ColB[i] is how many numbers in column i are in column i but in the wrong spot
        public int[] RowB; //RowB[j] is how many numbers in row j are in row j but in the wrong spot

        public GridComparison()
        {
            ColH = new int[3];
            RowH = new int[3];
            ColB = new int[3];
            RowB = new int[3];
        }

        public GridComparison(Grid solution, Grid guess) : this()
        {
            for(int i=0; i<3; i++)
            {
                for(int j=0; j<3; j++)
                {
                    if(guess.GetNumber(i, j) == solution.GetNumber(i, j))
                    {
                        //This one is in the exact correct position, so we should increase the count for both this column and row
                        ColH[i]++;
                        RowH[j]++;
                    }
                    else if (solution.RowContains(j, guess.GetNumber(i, j)))
                    {
                        //This is in the correct row but not in the exact correct place
                        RowB[j]++;
                    }
                    else if(solution.ColContains(i, guess.GetNumber(i, j)))
                    {
                        //This is in the correct column but not in the exact correct place
                        ColB[i]++;
                    }
                }
            }
        }
        /*
                public bool Equals(GridComparison other)
                {
                    return ArrayEquals(ColH, other.ColH) && ArrayEquals(RowH, other.RowH) && ArrayEquals(ColB, other.ColB) && ArrayEquals(RowB, other.RowB);
                }

                private static bool ArrayEquals(int[] a, int[] b)
                {
                    if (a.Length != b.Length)
                    {
                        return false;
                    }

                    for(int i=0; i<a.Length; i++)
                    {
                        if(a[i] != b[i])
                        {
                            return false;
                        }
                    }

                    return true;
                }*/

        public bool Equals(GridComparison other)
        {
            for (int i = 0; i < 3; i++)
            {
                if (ColH[i] != other.ColH[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (RowH[i] != other.RowH[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (ColB[i] != other.ColB[i])
                {
                    return false;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (RowB[i] != other.RowB[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void Print()
        {
            System.Console.Write("ColH= ");
            PrintArray(ColH);
            System.Console.WriteLine();

            System.Console.Write("RowH= ");
            PrintArray(RowH);
            System.Console.WriteLine();

            System.Console.Write("ColB= ");
            PrintArray(ColB);
            System.Console.WriteLine();

            System.Console.Write("RowB= ");
            PrintArray(RowB);
            System.Console.WriteLine();
        }

        private static void PrintArray(int[] array)
        {
            foreach(int i in array)
            {
                System.Console.Write(i + " ");
            }

            System.Console.Write("\n");
        }
    }
}
