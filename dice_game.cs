using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace dice_game.cs
{

    public class Game
    {
            public static int playerTotal;
            public static void PrintStart()
            {
                Console.Clear();
                Console.WriteLine("==========================================================================================");
                Console.WriteLine("\tWelcome to Harry's Dice Game");
                Console.WriteLine("\tFirst to 50 points will win the game!");
                Console.WriteLine("==========================================================================================");

            }

        public static void Main()
        {

            PrintStart();
            Console.Write("Enter P to play against Players, or C to play against the Computer:");

        //goto point to loop back over for validation 
        playerInput0:

            //User input
            string playerInput = Console.ReadLine();
            bool Opponent = true;

            if (playerInput.Contains("p") || playerInput.Contains("P"))
            {
                Opponent = true;
                Console.WriteLine("\nYou have selected to play against other Players!");
            }
            else if (playerInput.Contains("c") || playerInput.Contains("C"))
            {
                Opponent = false;
                Console.WriteLine("\nYou have selected to play against the Computer!");
            }
            else
            {
                PrintStart();
                Console.Write("Please enter a valid letter: ");
                goto playerInput0;
            }

            //Player creation
            playerTotal = 0;
            if (Opponent == true)
            {
                Console.Write("\nPlease enter the amount of players you wish to play against: ");

            playerTotal0:

                try
                {
                    playerTotal = Convert.ToInt32(Console.ReadLine());
                    while (playerTotal < 2)
                    {
                        PrintStart();
                        Console.Write("Please enter a value greater than 1: ");
                        playerTotal = Convert.ToInt32(Console.ReadLine());
                    }
                }
                catch
                {
                    PrintStart();
                    Console.Write("Please enter an Integer: ");
                    goto playerTotal0;
                }

                PrintStart();
                Console.WriteLine("You are playing against {0} opponents, good luck!", playerTotal);
            }

            //####Game START#####

            while (1 <= 50)//Turn.topScore()
            {

                Turn.takeTurn();
                int[] results;
                Roll r = new Roll();
                results = r.roll5();



                Console.WriteLine("Turn number is {0}", Turn.getTurnNumber());
                string turnHistory = String.Join("|\n|", Turn.turnTotal);
                turnHistory = string.Format("|{0}|", turnHistory);
                Console.WriteLine(turnHistory);
                //Console.WriteLine(Player.currentPlayer());
                //Console.WriteLine("GOT TO HERE");
                string rollOutcome = string.Join(", ", results);
                Console.WriteLine("#################ROLLING################");
                rollOutcome = string.Format("\t    | {0} |", rollOutcome);
                Console.WriteLine("{0}", rollOutcome);
                Console.WriteLine("#################ROLLING################");
                Console.ReadLine();

            }
           // Console.WriteLine("Player {0] is the winner and has reached 50 points!", topPlayerNumber());
        }
    }

    public class Player
    {
        private static int totalPlayers = Game.playerTotal;
        public static int currentPlayerTally = 1;

        public static int currentPlayer()
        {
            for(int i = 1; i <= totalPlayers; i++)
            {
                currentPlayerTally = i;

                if (i == totalPlayers)
                {
                    i = 1;
                }
            }
            return currentPlayerTally;
        }
    }

    public class Die
    {
        private Random rnd;

        public Die()
        {
            rnd = new Random();
        }
        public int roll()
        {
            return rnd.Next(1, 7);
        }
    }

    public class Turn
    {
        private static int i = 0;
        public static List<int> turnTotal = new List<int>();
        

        public static int takeTurn()
        {
            return 1;
        }

        public static int getTurnNumber()
        {
            i += 1;
            turnTotal.Add(i);
            return i;

        }
       // public static int topScore()
       // {
       //
       // }
    }

    public class Roll
    {
        private Die dice;
        public int[] results;

        public int[] roll5()
        {
            for (int i = 0; i <= 4; i++)
            {
                results[i] = dice.roll();
            }
            return results;
        }

        public Roll()
        {
            dice = new Die();
            results = new int[5];
        }
    }
}
