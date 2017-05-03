/*
+------------------------------------------------------------+
| Module Name: Programming and data structures               |
| Module Purpose: Create dice game                           |
| Inputs: User                                               |
| Outputs: Console                                           |
| Programmer name: Harry Jones                               |
+------------------------------------------------------------+
 */

//Importing C# syntax
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;


namespace dice_game.cs //Namespace for whole program
{

  public class Game //Class Game begins!
  {
    //Declaring variables at top of class
    //because they are used in enclosed loops so
    //they need to accessed everywhere throughout
    //the class
    public static int playerTotal;
    public static int playersNumber;
    private static bool _isThrowOnce;
    private static Random _computerThrowOnce = new Random();
    private static bool Opponent;
    private static List<Player> _players = new List<Player>();

    //public static method which prints out the start of the game title, it also clears and checks the height of the console window
    public static void PrintStart()
    {
      Console.BufferHeight = Int16.MaxValue - 1;
      var w = Console.WindowWidth;
      var h = Console.WindowHeight;
      if (w < 200 || h < 75)
      {
        Console.SetWindowSize(200, 75);
      }
      Console.Clear();
      Console.WriteLine("==========Harry's Dice Game===========");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("               (( _______");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("     _______     /\\O    O\\");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("    /O     /\\   /  \\      \\");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("   /   O  /O \\ / O  \\O____O\\ ))");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("((/_____O/    \\    /O     /");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("  \\O    O\\    / \\  /   O  /");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("   \\O    O\\ O/   \\/_____O/");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("    \\O____O\\/ ))          ))");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("  ((");
      System.Threading.Thread.Sleep(10);
      Console.WriteLine("======================================");
    }

    //Start of Main inside Game class
    public static void Main()
    {
      //Prints out the start
      PrintStart();
      _players.Clear();

      //Some game info text for the user
      Console.WriteLine("--RULES!--");

      //Thread sleep is used to create delay, designed to make it seem as if the program is loading...
      System.Threading.Thread.Sleep(400);
      Console.WriteLine("* Roll 5 dice and match as many as you can!");
      System.Threading.Thread.Sleep(100);
      Console.WriteLine("* You score points if you match 3, 4 or 5 Dice.");
      System.Threading.Thread.Sleep(100);
      Console.WriteLine("* If you match 2 Dice, you can re-roll the other 3 Dice to score again!");
      System.Threading.Thread.Sleep(100);
      Console.WriteLine("* You can also select to only roll once to score double points.");
      System.Threading.Thread.Sleep(100);
      Console.WriteLine("* First player to reach 50 points or more will win the game!");
      System.Threading.Thread.Sleep(100);
      Console.Write("\n\nEnter P to play against Players, or C to play against the Computer: ");


      //while loop which runs until the players input matches the criteria
      bool playerSelect = false;
      while (!playerSelect)
      {
        //User input to decide who they are playing against
        string playerInput = Console.ReadLine();

        //If code which checks what the user input was, could use .ToLower but this shows the or function of an IF
        if (playerInput.Contains("p") || playerInput.Contains("P"))
        {
          //If the player selects Players, they are playing against an opponent so it becomes true
          Opponent = true;
          playerSelect = true;
          Console.WriteLine("\nYou have selected to play against other Players!");
          break;
        }
        else if (playerInput.Contains("c") || playerInput.Contains("C"))
        {
          //If the player selects the Computer, the opponent is set to false because they are playing against the computer
          Opponent = false;
          playerSelect = true;
          Console.WriteLine("\nYou have selected to play against the Computer, good luck!");
          System.Threading.Thread.Sleep(2000);
          break;
        }
        else
        {
          //If they enter an invalid letter it will ask again
          //Prints the start method
          PrintStart();
          Console.Write("Please enter a valid letter, P to play against Players, or C to play against the Computer: ");
        }
      }

      //Player creation
      playerTotal = 0;

      //If statement which asks the user how many players they wish to play against
      if (Opponent == true)
      {
        Console.Write("\nPlease enter the amount of players you wish to play against: ");
        bool playerInputSelect = false;
        while (!playerInputSelect)
        {

          //Try method to get user input
          try
          {
            //user input for playerTotal
            playerTotal = Convert.ToInt32(Console.ReadLine());

            //While loop runs if playerTotal is less than 2 because you can't play against your self, could also be <= 1
            while (playerTotal < 2)
            {
              //Prints the start method
              PrintStart();
              Console.Write("\nPlease enter a value greater than 1: ");

              //Asks for input again
              playerTotal = Convert.ToInt32(Console.ReadLine());
            }
            if (playerTotal >= 2)
            {
              playerInputSelect = true;
              break;
            }
          }
          //Catch which catches errors which could cause the program to stop, i.e entering a char instead of an int
          catch
          {
            //Prints start method
            PrintStart();
            Console.Write("\nPlease enter an Integer: ");
          }
        }

        //Prints start method, also cleans up the console because the player is past the input stage
        PrintStart();
        Console.WriteLine("{0} Players are playing, good luck!", playerTotal);
        //for loop which creates a player object and adds it into the _players list depending on the int playerTotal
        for (int i = 1; i <= playerTotal; i++)
        {
          //adding players to _players list
          _players.Add(new Player(i, Opponent));
        }
        System.Threading.Thread.Sleep(2000);
      }
      //other part of the If which is for playing against the computer
      else if (Opponent == false)
      {
        //playerTotal has to = 2 because there is only one user and the computer AI
        playerTotal = 2;
        _players.Add(new Player(1, true));
        _players.Add(new Player(2, false));
      }

      //current turn number, increases by 1 at end of while loop
      int currentTurn = 1;

      //####Game START#####
      //isGameWon is set to false at the start, when it becomes true someone has reached 50+ points and the game is over
      bool isGameWon = false;

      //while loop which runs the game, once isGameWon = true the game stops
      while (!isGameWon)
      {
        //for loop for each player in int playerTotal
        for (int i = 0; i < playerTotal; i++)
        {
          playersNumber = (i + 1); //number starts at 0, so 0 is player 1

          //Prints start method
          PrintStart();

          //current player = i's current value from the for loop, current player is now p
          Player p = _players.ElementAt(i);

          //user input to start turn, if they are computer it will start turn automatically
          if (p.isHuman())
          {
            Console.WriteLine("Player {0} |current score {1}||Current turn {2}|\n", i + 1, p.getScore(), currentTurn);
          }
          else
          {
            Console.Write("Computer |current score {0}||Current turn {1}|.", p.getScore(), currentTurn);
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
            System.Threading.Thread.Sleep(500);
            Console.Write(".\n\n");
          }

          if (p.isHuman())
          {
            //Option to throw all once
            _isThrowOnce = false;
            Console.Write("\nThrow Once: Y | N : ");
            bool throwSelected = false;
            ConsoleKeyInfo throwOnce = Console.ReadKey();
            Console.WriteLine("\n");
            while (!throwSelected)
            {
              if (throwOnce.KeyChar == 'y' || throwOnce.KeyChar == 'Y')
              {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("You are throwing ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("#ONCE#\n\n");
                Console.ForegroundColor = ConsoleColor.White;
                _isThrowOnce = true;
                throwSelected = true;
                break;
              }
              else if (throwOnce.KeyChar == 'n' || throwOnce.KeyChar == 'N')
              {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You are throwing!\n");
                Console.ForegroundColor = ConsoleColor.White;
                _isThrowOnce = false;
                throwSelected = true;
                break;
              }
              else
              {
                Console.Write("\nPlease enter a valid Throw character: ");
                throwOnce = Console.ReadKey();
                Console.WriteLine("\n");
              }
            }
          }
          else //Computer option to throw once, random number between 1-3, if number is 1 then throw once
               //this means there is a 1/3 chance for the computer to throw once
          {
            if (_computerThrowOnce.Next(1, 4) == 1)
            {
              _isThrowOnce = true;
              Console.ForegroundColor = ConsoleColor.Yellow;
              Console.Write("Computer throwing ");
              Console.ForegroundColor = ConsoleColor.Red;
              Console.Write("#ONCE#\n\n");
              Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
              _isThrowOnce = false;
              Console.ForegroundColor = ConsoleColor.Yellow;
              Console.WriteLine("Computer thowing!\n");
              Console.ForegroundColor = ConsoleColor.White;
            }
          }


          //current player takes a turn
          p.takeTurn(_isThrowOnce);

          //current player prints their score so far
          p.printScore();

          //current player checks to see if their score is >= 50, if so it returns true
          isGameWon = p.isWinner();

          //if isGameWon = True then this loop runs
          if (isGameWon)
          {
            Console.WriteLine("Press enter to see each Players Game history...");
            Console.ReadLine();

            //Prints start method
            PrintStart();

            //Runs the printHistory method in class game
            Game.printHistory();
            //breaks out of the if(!isGameWon) loop and the game is over
            break;
          }
          //user input to confirm turn end
          //checks to see if its a player or computer turn
          //if computer turn then it will automatically end the turn
          //if code using check from users input, as well as current player number
          //this means a computer turn can only exist if, Opponent == false and the player number is 2
          if (Opponent == true || (i + 1) != 2)
          {
            Console.WriteLine("Press enter to end turn");
            Console.ReadLine();
          }
          else if (Opponent == false && ((i + 1) == 2))
          {
            Console.Write("Computer ending turn.");
            System.Threading.Thread.Sleep(1000);
            Console.Write(".");
            System.Threading.Thread.Sleep(1000);
            Console.Write(".");
            System.Threading.Thread.Sleep(1000);
            Console.Write(".");
            System.Threading.Thread.Sleep(1000);
          }
        }
        //increase current turn 
        currentTurn++;
      }
      //User input to decide if they want to play again
      //while loops runs until the user input matches the criteria
      //if play again is selected then Main() is called
      Console.Write("\n\nType A to play Again, or E to Exit the game: ");
      string againOrExit = Console.ReadLine().ToLower();
      bool exitAgain = false;
      while (!exitAgain)
      {
        if (againOrExit == "a") //play again
        {
          Console.Write("\n\nReloading game in 3 seconds.");
          System.Threading.Thread.Sleep(1000);
          Console.Write(".");
          System.Threading.Thread.Sleep(1000);
          Console.Write(".");
          System.Threading.Thread.Sleep(1000);
          Main();
        }
        else if (againOrExit == "e") //exit
        {
          Console.Write("\n\nExiting the game in 3 seconds.");
          System.Threading.Thread.Sleep(1000);
          Console.Write(".");
          System.Threading.Thread.Sleep(1000);
          Console.Write(".");
          System.Threading.Thread.Sleep(1000);
          Environment.Exit(0);
        }
        else //try again (wrong input)
        {
          PrintStart();
          Console.Write("\n\nPlease enter a valid letter, A to play Again, or E to Exit the game: ");
          againOrExit = Console.ReadLine().ToLower();
        }
      }
    }//End of Main

    //void method which for how ever many players are in the game, output the player number to the player class, will look like this: _players(2).printHistory(), or _players(12).printHistory.
    static void printHistory()
    {
      for (int i = 0; i < playerTotal; i++)
      {
        _players.ElementAt(i).printHistory();
      }
    }
  }//End class Game

  //Start of class Player
  public class Player
  {
    //Initializing private variables exclusive for this class
    private int _score = 0;
    private int _playerNumber;
    private bool _isHuman;
    private string _name;

    //list of lists which will store all the history data
    private List<List<int>> _l = new List<List<int>>();

    //list which stores scores for history
    private List<int> _scoreList = new List<int>();

    //class constructor which gets input from Game about which player is currently taking their turn
    public Player(int number, bool isHuman)
    {
      //argument passed in from game class sets the _playerNumber (who is the current player)
      //argument also passed in which determins if the player is human or computer
      _playerNumber = number;
      _isHuman = isHuman;
      if (isHuman)
      {
        _name = String.Format("Player {0}", _playerNumber);
      }
      else
      {
        _name = "Computer";
      }
    }


    //method which prints out the players number and score
    public void printScore()
    {
      Console.WriteLine("\n|{0} score is {1}|\n", _name, _score);
    }

    //return the players state (user or computer)
    public bool isHuman()
    {
      return _isHuman;
    }

    //method which takes a turn for the player
    public void takeTurn(bool isThrowOnce)
    {
      //new turn object for the player
      Turn t = new Turn();


      //adds the score from takeTurn from the turn class to its self, this is the players score
      _score += t.takeTurn(_isHuman, isThrowOnce);

      //adds information about the rolls to the _l list of lists
      _l.Add(t.getHistoryList());

      //adds score to the score list
      _scoreList.Add(_score);
    }

    //method which returns the current players score
    public int getScore()
    {
      return _score;
    }

    //method which prints out the game history at the end
    public void printHistory()
    {
      //tn is the turn number which starts at one, it is a seperate int from currentTurn because this is printed out at the end
      int tn = 1;

      //initliazing other ints for statistics
      int throwTotal = 0;
      int throwAverage = 0;

      //dictionary which holds die values
      Dictionary<int, int> dieAverage = new Dictionary<int, int>();

      //print out which declares which players information this is
      Console.Write("==========={0}'s History===========", _name);

      //Checks to see if if the current player is the winning player as only one player can have 50 points or over because the game stops
      if (_score >= 50)
      {
        Console.Write("(WINNER)====");
      }
      Console.WriteLine();

      //foreach loop which iterates over each value in the list _l
      foreach (var le in _l)
      {
        //prints out a turn for each bit of information in _l
        Console.Write(string.Format("Turn {0,-3}|", tn));

        //foreach loop which loops over every bit of information in the var le, its a nested loop
        foreach (var i in le)
        {
          //prints out each of the final die values from the turn class
          Console.Write(string.Format(" {0} ", i));

          //adds to throwTotal for each value of i, i will be what ever the current value of the die it is checking is
          throwTotal = throwTotal + i;

          //seperate method than throwTotal because throw total is for each turn and needs to be reset while throwAverage is for the entire player's history
          throwAverage = throwAverage + i;

          //If loop which adds the history dice to a dictionary
          if (dieAverage.ContainsKey(i))
          {
            //If the dictionary already contains the current key (die value) then + 1 to it
            dieAverage[i]++;
          }
          else
          {
            //Else add the key to the dictionary and give it a value of 1
            dieAverage.Add(i, 1);
          }
        }
        //Print outs which format the history of each player
        Console.Write("|");

        //prints out the throwTotal for each turn the player took
        Console.Write("Throw total: {0}", throwTotal);

        //Prints out the score of each turn, (tn - 1) is because game turn starts from 1 <, and the program stores the turn starting from 0
        Console.Write(" |Score: {0}|", _scoreList.ElementAt(tn - 1));

        //Prints out the winning turn for the game
        if (_scoreList.ElementAt(tn - 1) >= 50)
        {
          Console.Write("<---- Winning Turn");
        }

        //Sets the throw total back to 0 for each turn, stops the total from increasing past the turn
        throwTotal = 0;
        Console.WriteLine("");

        //increases the turn number by 1, this is important because all the lists are stored on a turn by turn basis and tn will point to each turn as it increases
        tn++;
      }

      //throwAverage calculation, it is the total number of dice thrown/ what ever the last turn number for the player was (-1 because it starts from 0)
      throwAverage = (throwAverage / (tn - 1));

      //Statistics print out
      Console.WriteLine("======Statistics======");
      Console.WriteLine("Throw Average: {0}", throwAverage);
      Console.WriteLine("-----------------------");

      //For each loop which will print out the dictionary values from dieAverage
      foreach (KeyValuePair<int, int> entry in dieAverage)
      {
        //entry.Key is the die number, and entry.Value is how many times it was entered into the dictionary
        Console.WriteLine("You rolled |{0}| {1} times", entry.Key, entry.Value);
        Console.WriteLine("-----------------------");
      }
    }

    //method which checks to see if the player has won yet
    public bool isWinner()
    {

      //if code to check players score, could also be > 49
      if (_score >= 50)
      {

        //winning player print out
        Console.WriteLine("-=-=-=-=-=-=-{0} is the WINNER!-=-=-=-=-=-=-\n", _name);

        //returns value true for the isGameWon boolean
        return true;
      }
      else
      {
        //if score < 50 then return false
        return false;
      }
    }
  }//End of class Player


  //Start of class Turn, this does all of the turn logic for the player
  public class Turn
  {
    //int which will store the score for the turn
    private int _score = 0;

    //dictionary which will store the die values, will eventually return which die face was the most common with key value pairs
    private Dictionary<int, int> _kind = new Dictionary<int, int>();

    //history list creation, this will store the results from the dice rolls and allow it to be printed out at the end
    private List<int> _hl;

    //method which returns the history list
    public List<int> getHistoryList()
    {
      return _hl;
    }

    //method which does all of the turn logic
    public int takeTurn(bool isHuman, bool isThrowOnce)
    {
      //int which will store what the face value of the dice was in the pair
      int dieFace = 0;

      //list which will store all of the rolls for the turn
      List<int> results;

      //new roll object created
      Roll r = new Roll();

      //results list will = the output of the random numbers from r.doRoll in the roll class
      results = r.doRoll(4);

      //Some print to show rolling the die
      Console.Write("ROLLING");
      System.Threading.Thread.Sleep(300);
      Console.Write(".");
      System.Threading.Thread.Sleep(300);
      Console.Write(".");
      System.Threading.Thread.Sleep(300);
      Console.Write(".");

      //will format and print the answers from the roll class, could be done in turn class but in the roll class you have access to each individual value instead just 5 numbers
      r.printRoll();

      //more delay creation in the program
      System.Threading.Thread.Sleep(200);

      //historyList now = results list, keeps them seperate as they are used for different functions
      _hl = results;

      //runs the findKind method in the Turn class which will store each of the dice rolls into a dictionary and set its value to how many occured
      this.findKind(results);

      //method in class Turn which sets dieFace to the Key of the highest value in the dictionary of rolls
      dieFace = this.highestKey();

      //integer hc = highestCount which is a method that finds the highest value in the dictionary, this value will be the pair number, so 2 2's will be 2 or 3 2's will be 3
      int hc = this.highestCount();

      //keepKey is a method which will clear out the dictionary and only add back the pair that has the highest value
      //this is only useful if the user is re-rolling
      this.keepKey(dieFace, hc);

      //If codes which check to see what type of pair you have found
      //hc == 2 means you have found 2 die that match
      if (hc == 2 && !isThrowOnce)
      {
        //print out which lets the user know what they have found
        if (isHuman)
        {
          //Human player
          Console.WriteLine("\n\nYou found 2 |{0}|'s, press enter to re-roll 3 dice...", dieFace);
          Console.ReadLine();
        }
        else
        {
          //Computer player
          Console.WriteLine("\n\nThe computer has found 2 |{0}|'s\n", dieFace);
          System.Threading.Thread.Sleep(2000);
        }

        //once enter pressed 3 die are rerolled from the r.doRoll method in roll, the new values are stored into the results list again
        results = r.doRoll(2);

        //some print out for the user with delays
        Console.Write("ROLLING");
        System.Threading.Thread.Sleep(300);
        Console.Write(".");
        System.Threading.Thread.Sleep(300);
        Console.Write(".");
        System.Threading.Thread.Sleep(300);
        Console.Write(".");
        r.printRoll();
        System.Threading.Thread.Sleep(100);
        Console.Write(" + ");
        System.Threading.Thread.Sleep(200);
        Console.Write("| {0}, {1} |", dieFace, dieFace);
        System.Threading.Thread.Sleep(100);

        //findKind adds the results to the dictionary with their keyvaluepairs
        this.findKind(results);

        //dieFace now = which die face appears the most
        dieFace = this.highestKey();

        //hc = the new count after the second roll, the pair from the first roll are also added in
        hc = this.highestCount();

        //history list = the new results
        _hl = results;

        //because this is in the if for re rolling 3 die, the 2 die from the first roll are also added to the history list twice because they were the 2 same die
        _hl.Add(dieFace);
        _hl.Add(dieFace);

      }
      //if throwOnce has been selected then set the score multiplier to 2
      int mult = 1;
      if (isThrowOnce)
      {
        mult = 2;
      }

      //if code runs if the pair count is 3
      if (hc == 3)
      {
        //sets the score to 3
        _score = 3 * mult;

        if (isHuman)
        {
          //print out for the user
          Console.WriteLine("\n\nYou found 3 |{0}|'s and scored {1} points!", dieFace, _score);
        }
        else
        {
          Console.WriteLine("\n\nThe Computer found 3 |{0}|'s and scored {1} points!", dieFace, _score);
          System.Threading.Thread.Sleep(2000);
        }
      }

      //if code runs if the pair cout is 4
      if (hc == 4)
      {
        //set score to 6
        _score = 6 * mult;

        if (isHuman)
        {
          //print out for the user
          Console.WriteLine("\n\nYou found 4 |{0}|'s and scored {1} points!", dieFace, _score);
        }
        else
        {
          Console.WriteLine("\n\nThe Computer found 4 |{0}|'s and scored {1} points!", dieFace, _score);
          System.Threading.Thread.Sleep(2000);
        }
      }

      //if code runs if you match all 5 die the same
      if (hc == 5)
      {
        //sets score to 12
        _score = 12 * mult;

        //print out for user
        if (isHuman)
        {
          Console.WriteLine("\n\nYou found 5 |{0}|'s and scored {1} points!", dieFace, _score);
        }
        else
        {
          Console.WriteLine("\n\nThe Computer found 5 |{0}|'s and scored {1} points!", dieFace, _score);
          System.Threading.Thread.Sleep(2000);
        }
      }

      //if code runs if no value of pair was found to be more than 1
      if (hc == 2)
      {
        if (isHuman)
        {
          Console.WriteLine("\n\nYou did not find 3 of a kind and scored no points");
        }
        else
        {
          Console.WriteLine("\n\nThe Computer did not find 3 of a kind and scored no points");
          System.Threading.Thread.Sleep(2000);
        }
      }

      //if code runs if no value of pair was found to be more than 1
      if (hc == 1)
      {
        if (isHuman)
        {
          Console.WriteLine("\n\nYou did not find any pairs of dice and scored no points");
        }
        else
        {
          Console.WriteLine("\n\nThe Computer did not find any pairs of dice and scored no points");
          System.Threading.Thread.Sleep(2000);
        }
      }



      //at this point the turn is over and the dictionary is cleared ready for new die
      this._kind.Clear();

      //returns the final score back to the player
      return _score;
    }

    //printDict isn't used in the game but it is useful to see the key and value of all of the dictionary entries
    public void printDict()
    {
      //foreach keyvaluepair in dictionary print out its key and value
      foreach (KeyValuePair<int, int> kvp in _kind)
      {
        Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
      }
    }

    //method find kind adds the roll outcomes to the dictionary _kind
    private void findKind(List<int> r)
    {
      //for every value in r, which is how ever many entries there are in the results list
      for (int i = 0; i < r.Count(); i++)
      {
        //r.ElementAt(i) will cycle through every value in the list
        int v = r.ElementAt(i);

        //if the dictionary contains the key then add 1 to its value
        if (_kind.ContainsKey(v))
        {
          _kind[v]++;
        }

        //if not then add it to the dictionary and set its value to 1
        else
        {
          _kind.Add(v, 1);
        }
      }
    }

    //method highestCount finds out which the pair number is, 3,3,3 would output 3 and 1,1,1,1 would output 4
    private int highestCount()
    {
      //default pair is 0 (no pairs), could also be 1
      int h = 0;

      //for every entry in the dictionary run this
      foreach (KeyValuePair<int, int> entry in _kind)
      {

        //this if code always sets h to the highest value it comes accross, the values are linked with how many times it has appeard in the roll
        if (h < entry.Value)
        {
          h = entry.Value;
        }
      }
      //returns the final value of h which is the largest value in the dictionary _kind
      return h;
    }

    //method highestKey find out the die value of the pair it has found
    private int highestKey()
    {
      //declaring ints that will be returned
      int h = 0;
      int k = 0;

      //for every value in the dictionary _kind
      foreach (KeyValuePair<int, int> entry in _kind)
      {
        //finds the highest value in _kind
        if (h < entry.Value)
        {
          //h will now = the current value
          h = entry.Value;

          //k will not = the current key
          k = entry.Key;
        }
      }
      //return the highest key in _kind
      return k;
    }

    //method which is used to get rid of the 3 remaining die that are being rerolled from the dictionary
    //(int k, int v) inputs the highest key and its value from the first roll
    private void keepKey(int k, int v)
    {
      //clears the dictionary _kind
      _kind.Clear();

      //adds back in the highest key, highest value from first roll
      _kind.Add(k, v);
    }
  }//end of class Turn

  //Start of class Roll
  public class Roll
  {
    //Die object declaration
    private Die dice;

    //list which will privately store the results from the die class
    private List<int> _results = new List<int>();

    //constructor which creates new dice when roll is called
    public Roll()
    {
      dice = new Die();
    }

    //method printRoll formats the information recieved from the die class and outputs it so its user readable
    public void printRoll()
    {
      //string rollOutcome will = all of the results in the results list joined by a ,
      string rollOutcome = string.Join(", ", _results);

      //rollOutcome is formatted so that all the results are displayed within a box
      rollOutcome = string.Format("| {0} |", rollOutcome);

      //Print out final string to user
      Console.Write("{0}", rollOutcome);
    }

    //method which rolls the die depending on how many are needed to be rolled, this means the program can be easily changed to fit different criteria
    public List<int> doRoll(int number)
    {
      //removes entries from the _results list so it can accept new ones
      _results.Clear();

      //for loop which will roll the die depending on how many are needed, this is determined by the input from the user as number
      for (int i = 0; i <= number; i++)
      {
        //adds the outcome from roll method in the Die class to results, dice is created in the constructor for each roll
        _results.Add(dice.roll());
      }

      //return the _results list
      return _results;
    }
  }//End of class Roll

  //Start of class Die
  public class Die
  {
    //New random number called rnd, this is static so that there is only one instance and different random numbers are generated each time because it is seed based
    private static Random rnd = new Random();

    //int _value will return the die face value
    private int _value;

    //method roll which will output a random number between 1 and 6
    public int roll()
    {
      //_value now = the random number between 1-6, rnd.Next(int.m, int.M) is how random numbers are made
      _value = rnd.Next(1, 7);

      //return the die face value
      return _value;
    }
  }//End of class Die
}//End of namespace dice_game
//End of C# program
