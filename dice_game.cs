using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace dice_game.cs
{
  //Start of the Game class
  public class Game
  {
    //Declaring variables at top of class
    //playerTotal will hold the amount of the players the user wants to play against
    public static int playerTotal;

    //boolean to determine if you are playing against the computer or other players
    public static bool Opponent = true;

    //a list that will store all of the player objects
    private static List<Player> _players = new List<Player>();

    //a method which allows for the console screen to be clear and then print the game text at the top
    public static void PrintStart()
    {
      Console.Clear();
      Console.WriteLine("==========================================================================================");
      Console.WriteLine("--Harry's Dice Game--");
      Console.WriteLine("==========================================================================================");
    }

    //Start of Main inside Game class
    public static void Main()
    {
      //Prints out the start
      PrintStart();

      //Some game info text for the user
      Console.WriteLine("--RULES!--");
      Console.WriteLine("* Roll 5 dice and match as many as you can!");
      Console.WriteLine("* You score points if you match 3, 4 or 5 Die.");
      Console.WriteLine("* If you match 2 Die, you can re-roll the other 3 Die to score again!");
      Console.WriteLine("* You can also select to not re-roll before your turn and score double points.");
      Console.WriteLine("* First player to 50 points will win the game!");
      Console.Write("\n\nEnter P to play against Players, or C to play against the Computer: ");

      //goto point to loop back over for validation 
      playerInput0:

      //User input to decide who they are playing against
      string playerInput = Console.ReadLine();

      //If code which checks what the user input was, could use .ToLower but this shows the or function of an IF
      if (playerInput.Contains("p") || playerInput.Contains("P"))
      {
        //If the player selects Players, they are playing against an opponent so it becomes true
        Opponent = true;
        Console.WriteLine("\nYou have selected to play against other Players!");
      }
      else if (playerInput.Contains("c") || playerInput.Contains("C"))
      {
        //If the player selects the Computer, the opponent is set to false because they are playing against the computer
        Opponent = false;
        Console.WriteLine("\nYou have selected to play against the Computer!");
      }
      else
      {
        //If they enter an invalid letter it will ask again
        //Prints the start method
        PrintStart();
        Console.Write("Please enter a valid letter: ");

        //Takes user back to try again, could use a loop with break;
        goto playerInput0;
      }

      //Player creation
      playerTotal = 0;

      //If statement which asks the user how many players they wish to play against
      if (Opponent == true)
      {
        Console.Write("\nPlease enter the amount of players you wish to play against: ");

        //the goto loop for the try and catch
        playerTotal0:

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
            Console.Write("Please enter a value greater than 1: ");

            //Asks for input again
            playerTotal = Convert.ToInt32(Console.ReadLine());
          }
        }
        //Catch which catches errors which could cause the program to stop, i.e entering a char instead of an int
        catch
        {
          //Prints start method
          PrintStart();
          Console.Write("Please enter an Integer: ");

          //loops back to the top to try again, could use while loop
          goto playerTotal0;
        }

        //Prints start method, also cleans up the console because the player is past the input stage
        PrintStart();
        Console.WriteLine("You are playing against {0} opponents, good luck!", playerTotal);
      }
      //other part of the If which is for playing against the computer
      else if (Opponent == false)
      {
        //playerTotal has to = 2 because there is only one computer AI
        playerTotal = 2;
      }

      //for loop which creates a player object and adds it into the _players list depending on the int playerTotal
      for (int i = 1; i <= playerTotal; i++)
      {
        //adding players to _players list
        _players.Add(new Player(i));
      }
      Console.WriteLine("GOT TO HERE");
      //####Game START#####
      //isGameWon is set to false at the start, when it becomes true someone has reached 50+ points and the game is over
      bool isGameWon = false;

      //while loop which runs the game, once isGameWon = true the game stops
      while (!isGameWon)
      {
        //for loop for each player in int playerTotal
        for (int i = 0; i < playerTotal; i++)
        {
          //Prints start method
          PrintStart();

          //current player = i's current value from the for loop, current player is now p
          Player p = _players.ElementAt(i);

          //user input to start turn, if they are computer it will start turn automatically
          if (Opponent == true)
          {
            Console.WriteLine("Player {0} Press enter to take turn |current score {1}|", i+1, p.getScore());
            Console.ReadLine();
          }
          //current player takes a turn
          p.takeTurn();

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
            Console.WriteLine("Press enter to end turn");
            Console.ReadLine();
        }
      }
      //User input to decide if they want to play again
      Console.WriteLine("\nPress enter to play again...");
      Console.ReadLine();
      Main();
    }//End of Main
    public static int whoAmI()
    {
      return Convert.ToInt32(Opponent);
    }

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
    private static int currentTurn;

    //list of lists which will store all the history data
    private List<List<int>> _l = new List<List<int>>();

    //list which stores scores for history
    private List<int> _scoreList = new List<int>();

    //method which prints out the players number and score
    public void printScore()
    {
      Console.WriteLine("\n|Player {0} score is {1}|\n", _playerNumber, _score);
    }

    //class constructor which gets input from Game about which player is currently taking their turn
    public Player(int number)
    {
      //information passed in from game class sets the _playerNumber (who is the current player)
      _playerNumber = number;
      int isComputer = 0;
      if (_playerNumber == 2)
      {
        isComputer = 1;
      }
      if ((isComputer == 1) && (Game.whoAmI() == 0))
      {
        _playerNumber = Convert.ToInt32("Computer");
      }

    }

    //method which takes a turn for the player
    public void takeTurn()
    {
      //new turn object for the player
      Turn t = new Turn();

      //if statement which increments currentTurn each time the list loops back around to player 1
      if (_playerNumber == 1)
      {
        currentTurn++;
      }
      //print out which tells the user which player is taking which turn
      Console.WriteLine("Player {0}: taking turn {1}\n", _playerNumber, currentTurn);

      //adds the score from takeTurn from the turn class to its self, this is the players score
      _score += t.takeTurn();

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
      Console.Write("===========Player {0}'s History===========", _playerNumber);
      if(_score >= 50)
      {
        Console.WriteLine("(WINNER)====");
      }
      Console.Write("\n");
      //foreach loop which iterates over each value in the list _l
      foreach (var le in _l)
      {
        //prints out a turn for each bit of information in _l
        Console.Write(string.Format("Turn {0}: |", tn));

        //foreach loop which loops over every bit of information in the var le, its a nested loop
        foreach (var i in le)
        {
          //prints out each of the final die values from the turn class
          Console.Write(string.Format(" {0} ", i));

          //adds value to each of the
          throwTotal = throwTotal + i;
          throwAverage = throwAverage + i;
          if (dieAverage.ContainsKey(i))
          {
            dieAverage[i]++;
          }
          else
          {
            dieAverage.Add(i, 1);
          }
        }
        Console.Write("|");
        Console.Write("Throw total: {0}", throwTotal);
        Console.Write(" |Score: {0}|", _scoreList.ElementAt(tn - 1));
        if(_scoreList.ElementAt(tn-1) >= 50)
        {
          Console.Write("<---- Winning Turn");
        }
        throwTotal = 0;
        Console.WriteLine("");
        tn++;
      }
      throwAverage = (throwAverage / (tn - 1));
      Console.WriteLine("======Statistics======");
      Console.WriteLine("Throw Average: {0}", throwAverage);
      Console.WriteLine("-----------------------");
      foreach (KeyValuePair<int, int> entry in dieAverage)
      {
        Console.WriteLine("You rolled |{0}| {1} times", entry.Key, entry.Value);
        Console.WriteLine("-----------------------");
      }
    }


    public bool isWinner()
    {
      if (_score >= 50)
      {
        Console.WriteLine("-=-=-=-=-=-=-Player {0} is the WINNER!-=-=-=-=-=-=-\n", _playerNumber);
        return true;
      }
      else
      {
        return false;
      }
    }
  }



  public class Turn
  {
    private int _score = 0;
    private Dictionary<int, int> _kind = new Dictionary<int, int>();
    private List<int> _hl;

    public List<int> getHistoryList()
    {
      return _hl;
    }


    public int takeTurn()
    {
      int dieFace = 0;
      List<int> results;
      Roll r = new Roll();
      results = r.doRoll(4);
      Console.Write("ROLLING");
      System.Threading.Thread.Sleep(100);
      Console.Write(".");
      System.Threading.Thread.Sleep(100);
      Console.Write(".");
      System.Threading.Thread.Sleep(100);
      Console.Write(".");
      r.printRoll();
      System.Threading.Thread.Sleep(200);
      _hl = results;
      this.findKind(results);
      dieFace = this.highestKey();
      int hc = highestCount();
      this.keepKey(dieFace, hc);




      if (hc == 2)
      {

        Console.WriteLine("\n\nYou found 2 |{0}|'s, press enter to re-roll 3 dice...", dieFace);
        Console.ReadLine();
        results = r.doRoll(2);
        Console.Write("ROLLING");
        System.Threading.Thread.Sleep(100);
        Console.Write(".");
        System.Threading.Thread.Sleep(100);
        Console.Write(".");
        System.Threading.Thread.Sleep(100);
        Console.Write(".");
        r.printRoll();
        System.Threading.Thread.Sleep(100);
        Console.Write(" + ");
        System.Threading.Thread.Sleep(200);
        Console.Write("| {0}, {1} |", dieFace, dieFace);
        System.Threading.Thread.Sleep(100);
        this.findKind(results);
        dieFace = this.highestKey();
        hc = highestCount();
        _hl = results;
        _hl.Add(dieFace);
        _hl.Add(dieFace);
        if (hc == 2)
        {
          Console.WriteLine("\n\nYou found no pairs of 3 and score 0 points this turn.");
        }
      }


      if (hc == 3)
      {
        _score = 3;
        Console.WriteLine("\n\nYou found 3 |{0}|'s and scored {1} points!", dieFace, _score);
      }
      if (hc == 4)
      {
        _score = 6;
        Console.WriteLine("\n\nYou found 4 |{0}|'s and scored {1} points!", dieFace, _score);
      }
      if (hc == 5)
      {
        _score = 12;
        Console.WriteLine("\n\nYou found 5 |{0}|'s and scored {1} points!", dieFace, _score);
      }
      if (hc == 1)
      {
        Console.WriteLine("\n\nYou found no pairs and score 0 points this turn.");
      }
      this._kind.Clear();
      return _score;
    }

    public void printDict()
    {
      foreach (KeyValuePair<int, int> kvp in _kind)
      {
        Console.WriteLine("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
      }
    }

    private void findKind(List<int> r)
    {


      for (int i = 0; i < r.Count(); i++)
      {
        int v = r.ElementAt(i);
        if (_kind.ContainsKey(v))
        {
          _kind[v]++;
        }
        else
        {
          _kind.Add(v, 1);
        }
      }
    }

    private int highestCount()
    {
      int h = 0;
      foreach (KeyValuePair<int, int> entry in _kind)
      {
        if (h < entry.Value)
        {
          h = entry.Value;
        }
      }
      return h;
    }
    private int highestKey()
    {
      //Console.WriteLine("Got to here!!");
      int h = 0;
      int k = 0;
      foreach (KeyValuePair<int, int> entry in _kind)
      {
        if (h < entry.Value)
        {
          h = entry.Value;
          k = entry.Key;
          // Console.WriteLine("This is the key {0}", k);
        }
      }
      return k;
    }

    private void keepKey(int k, int v)
    {
      _kind.Clear();
      _kind.Add(k, v);

    }
  }

  public class Roll
  {
    private Die dice;
    private List<int> _results = new List<int>();

    public Roll()
    {
      dice = new Die();
    }

    public void printRoll()
    {
      string rollOutcome = string.Join(", ", _results);
      rollOutcome = string.Format("| {0} |", rollOutcome);
      Console.Write("{0}", rollOutcome);
    }

    public List<int> doRoll(int number)
    {

      _results.Clear();

      for (int i = 0; i <= number; i++)
      {
        _results.Add(dice.roll());
      }
      return _results;
    }
  }

  public class Die
  {
    private static Random rnd = new Random();
    private int _value;
    public Die()
    {

    }
    public int roll()
    {
      _value = rnd.Next(1, 7);
      return _value;
    }
    public void printDie()
    {
      Console.WriteLine("Dice {0}", _value);
    }
  }
}
