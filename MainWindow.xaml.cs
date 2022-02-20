using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CodeBreakerBreaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Grid> potentialSolutions;
        List<GridComparison> potentialComparisons;
        Grid guess;
        Grid trial;

        HelpWindow helpWindow;

        public MainWindow()
        {
            InitializeComponent();

            helpWindow = new HelpWindow();

            Reset();
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        public void Reset()
        {
            guess = new Grid();

            for(int j=0; j<3; j++)
            {
                for(int i=0; i<3; i++)
                {
                    guess.SetNumber(i, j, Grid.IJtoX(i, j) + 1);
                }
            }

            if(!guess.Validate())
            {
                System.Console.WriteLine("Error validating guess grid");
                return;
            }

            trial = new Grid();
            potentialSolutions = new List<Grid>();
            potentialComparisons = new List<GridComparison>();

            System.Console.WriteLine("Generating comparisons for: ");
            guess.Print();
            System.Console.WriteLine();


            //Generate a pool of numbers from 1 to 9
            List<int> numberpool = new List<int>();
            for (int i = 1; i <= 9; i++)
            {
                numberpool.Add(i);
            }

            ChooseNumber(0, 0, numberpool);

            UpdatePotentialSolutionsLabel();
            UpdateGuessTextBoxes();

            UpdateInputs();

            System.Console.WriteLine("Finished generating");
        }

        public void UpdateInputs()
        {
            ColH_0.Text = "";
            ColH_1.Text = "";
            ColH_2.Text = "";
            RowH_0.Text = "";
            RowH_1.Text = "";
            RowH_2.Text = "";
            ColB_0.Text = "";
            ColB_1.Text = "";
            ColB_2.Text = "";
            RowB_0.Text = "";
            RowB_1.Text = "";
            RowB_2.Text = "";
        }

        public void UpdatePotentialSolutionsLabel()
        {
            LabelPotentialSolutions.Content = potentialSolutions.Count;
        }

        public void UpdateGuessTextBoxes()
        {
            Guess_0_0.Text = guess.GetNumber(0, 0) + "";
            Guess_1_0.Text = guess.GetNumber(1, 0) + "";
            Guess_2_0.Text = guess.GetNumber(2, 0) + "";
            Guess_0_1.Text = guess.GetNumber(0, 1) + "";
            Guess_1_1.Text = guess.GetNumber(1, 1) + "";
            Guess_2_1.Text = guess.GetNumber(2, 1) + "";
            Guess_0_2.Text = guess.GetNumber(0, 2) + "";
            Guess_1_2.Text = guess.GetNumber(1, 2) + "";
            Guess_2_2.Text = guess.GetNumber(2, 2) + "";
        }

        private void ChooseNumber(int i, int j, List<int> numberpool)
        {
            if (i == 2 && j == 2)
            {
                //Last slot has to be the only number left in the pool so we can set it to that, create a comparison and check if it's equal to the input
                trial.SetNumber(2, 2, numberpool[0]);
                GridComparison comparison = trial.Compare(guess);
                potentialComparisons.Add(comparison);
                potentialSolutions.Add(new Grid(trial));
            }
            else
            {
                //iterate over remaining numbers in pool. i<9-Grid.IJtoX(i,j) means for i=0, j=0 it will go to i<9 which is correct as there are 9 possibilities for slot 1
                for (int x = 0; x < 9 - Grid.IJtoX(i, j); x++)
                {
                    trial.SetNumber(i, j, numberpool[x]);
                    List<int> newpool = new List<int>(numberpool);
                    newpool.RemoveAt(x);

                    if (i==2)
                    {
                        //Advance to the next row if it's at the end
                        ChooseNumber(0, j + 1, newpool);
                    }
                    else
                    {
                        //Advance to the next cell in the row
                        ChooseNumber(i + 1, j, newpool);
                    }
                }
            }
        }

        private void Button_Compute_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidateInputs())
            {
                MessageBox.Show("Inputs are incorrect. Please enter either 0, 1, 2 or 3 in each box, or leave it blank");
                return;
            }


            GridComparison inputComparison = new GridComparison();
            inputComparison.RowH[0] = Int32.Parse(RowH_0.Text);
            inputComparison.RowH[1] = Int32.Parse(RowH_1.Text);
            inputComparison.RowH[2] = Int32.Parse(RowH_2.Text);
            inputComparison.ColH[0] = Int32.Parse(ColH_0.Text);
            inputComparison.ColH[1] = Int32.Parse(ColH_1.Text);
            inputComparison.ColH[2] = Int32.Parse(ColH_2.Text);
            inputComparison.RowB[0] = Int32.Parse(RowB_0.Text);
            inputComparison.RowB[1] = Int32.Parse(RowB_1.Text);
            inputComparison.RowB[2] = Int32.Parse(RowB_2.Text);
            inputComparison.ColB[0] = Int32.Parse(ColB_0.Text);
            inputComparison.ColB[1] = Int32.Parse(ColB_1.Text);
            inputComparison.ColB[2] = Int32.Parse(ColB_2.Text);


            System.Console.WriteLine("Computing remaining solutions for: ");
            inputComparison.Print();

            List<Grid> newsolutions = new List<Grid>();

            for (int i=0;  i < potentialComparisons.Count; i++)
            {
                if(inputComparison.Equals(potentialComparisons[i]))
                {
                    //This could be a solution so we should keep it
                    newsolutions.Add(potentialSolutions[i]);
                }
            }

            //Update solution list
            potentialSolutions = newsolutions;

            if (potentialSolutions.Count > 0)
            {
                //Update guess to be the one with the fewest total changes required
                CalculateNextGuess();

                UpdateGuessTextBoxes();
            }
            else
            {
                MessageBox.Show("Error: no solutions found. Resetting data.");
                Reset();
            }

            //for each remaining potential solution, generate new potential comparisons as we will be comparing against a different grid now
            potentialComparisons = new List<GridComparison>();

            foreach (Grid trial in potentialSolutions)
            {
                potentialComparisons.Add(trial.Compare(guess));
            }

            UpdatePotentialSolutionsLabel();

            UpdateInputs();

            System.Console.WriteLine("Finished computing");
        }

        private void CalculateNextGuess()
        {
            int besti = -1;
            int lowestMoves = -1;

            for(int i=0; i<potentialSolutions.Count; i++)
            {
                int difference = Grid.CalculateDifference(guess, potentialSolutions[i]);
                if (lowestMoves == -1 || difference < lowestMoves)
                {
                    besti = i;
                    lowestMoves = difference;
                }
            }

            guess = potentialSolutions[besti];
        }

        private bool ValidateInput(TextBox tbInput)
        {
            //Check if this is numerical and in [0,3], if not then set to 0
            if (tbInput.Text == "")
            {
                tbInput.Text = "0";
                return true;
            }
            else if (!int.TryParse(tbInput.Text, out int i))
            {
                tbInput.Text = "";
                return false;
            }
            else if (i < 0 || i > 3)
            {
                tbInput.Text = "0";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool ValidateInputs()
        {
            return ValidateInput(RowH_0) && ValidateInput(RowH_1) && ValidateInput(RowH_2) && ValidateInput(ColH_0) && ValidateInput(ColH_1) && ValidateInput(ColH_2)
            && ValidateInput(RowB_0) && ValidateInput(RowB_1) && ValidateInput(RowB_2) && ValidateInput(ColB_0) && ValidateInput(ColB_1) && ValidateInput(ColB_2);
        }

        private void Help_Button_Click(object sender, RoutedEventArgs e)
        {
            helpWindow.Show();
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox TbSender = sender as TextBox;
            if (TbSender != null)
            {
                ValidateInput(TbSender);
            }
        }

        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {
            TextBox TbSender = sender as TextBox;
            if (TbSender != null)
            {
                TbSender.SelectAll();
            }
        }

        private void SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            TextBox TbSender = sender as TextBox;
            if (TbSender != null)
            {
                TbSender.Focus();
                e.Handled = true;
            }
        }

    }
}
