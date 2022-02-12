using System;
using System.Collections.Generic;
using System.Text;

namespace CodeBreakerBreaker
{
    class Grid
    {
        private int[,] numbers;

        public Grid()
        {
            numbers = new int[3, 3];
        }

        public Grid(int[] x) : this()
        {
            for(int j=0; j<3; j++)
            {
                for(int i=0; i<3; i++)
                {
                    numbers[i, j] = x[IJtoX(i, j)];
                }
            }
        }

        public Grid(Grid other)
        {
            numbers = new int[3, 3];

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    numbers[i, j] = other.GetNumber(i, j);
                }
            }
        }

        public void SetNumber(int i, int j, int number)
        {
            if(number>=1 && number<=9)
            {
                numbers[i, j] = number;
            }
        }

        public int GetNumber(int i, int j)
        {
            return numbers[i, j];
        }

        public bool Validate()
        {
            //Generate a list from 1 to 9 for numbers you can use
            List<int> numberpool = new List<int>();
            for(int i=1; i<=9; i++)
            {
                numberpool.Add(i);
            }

            //Iterate over all elements and remove the number from the pool if it's there, that way we check if there are any duplicates or numbers outside of [1,9] in numbers
            for(int j=0; j<3; j++)
            {
                for(int i=0; i<3; i++)
                {
                    if(numberpool.Contains(numbers[i, j]))
                    {
                        numberpool.Remove(numbers[i, j]);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool RowContains(int j, int number)
        {
            for (int i = 0; i < 3; i++)
            {
                if (numbers[i, j] == number)
                {
                    return true;
                }
            }

            return false;


        }

        public bool ColContains(int i, int number)
        {
            for (int j = 0; j < 3; j++)
            {
                if (numbers[i, j] == number)
                {
                    return true;
                }
            }

            return false;
        }

        public static int IJtoX(int i, int j)
        {
            return (j * 3) + i;
        }

        public void Print()
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    System.Console.Write(numbers[i, j] + " ");
                }
                System.Console.Write("\n");
            }
        }

        public GridComparison Compare(Grid guess)
        {
            return new GridComparison(this, guess);
        }

        public static int CalculateDifference(Grid a, Grid b)
        {
            int difference = 0;

            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++)
                {
                    difference += Math.Abs(a.numbers[i, j] - a.numbers[i, j]);
                }
            }

            return difference;
        }
    }
}
