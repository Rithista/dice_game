using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace dice_game.cs
{

  public class Game
  {
    public static int playerTotal;

    private static List<Player> _players = new List<Player>();

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
      for (int i = 1; i <= playerTotal; i++)
      {
        _players.Add(new Player(i));
      }

      //####Game START#####
      bool isGameWon = false;

      while (!isGameWon)//Turn.topScore()
      {
        for (int i = 0; i < playerTotal; i++)
        {
          Player p = _players.ElementAt(i);
          p.takeTurn();
          p.printScore();
          isGameWon = p.isWinner();
          if (isGameWon)
          {
            break;
          }
        }









        /*Turn.takeTurn();
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
        */
      }
      // Console.WriteLine("Player {0] is the winner and has reached 50 points!", topPlayerNumber());
    }
  }

  public class Player
  {
    private int _score = 0;
    private int _playerNumber;


    public void printScore()
    {
      Console.WriteLine("Player {0} score is {1}", _playerNumber, _score);
    }
    public Player(int number)
    {
      _playerNumber = number;
    }
    public int getScore()
    {
      return _score;
    }

    public void takeTurn()
    {
      Console.WriteLine("Player {0}: taking turn", _playerNumber);

      Turn t = new Turn();
      _score += t.takeTurn();
    }
    public bool isWinner()
    {
      if (_score >= 50)
      {
        Console.WriteLine("Player {0} is the winner", _playerNumber);
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

    public int takeTurn()
    {
      List<int> results;
      Roll r = new Roll();
      results = r.doRoll(4);
      r.printRoll();
      this.findKind(results);
      int hc = highestCount();
      if(hc == 2)
      {
        results = r.doRoll(2);
        r.printRoll();
        this.findKind(results);
        hc = highestCount();
      }
      if(hc == 3)
      {
        _score = 3;
      }
      if(hc == 4)
      {
        _score = 6;
      }
      if(hc == 5)
      {
        _score = 12;
      }
      return _score;
    }

    private void findKind(List<int> r)
    {
      for(int i = 0; i < r.Count(); i++)
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
        if(h < entry.Value)
        {
          h = entry.Value;
        }
      }
      return h;
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
      rollOutcome = string.Format("\t    | {0} |", rollOutcome);
      Console.WriteLine(rollOutcome);
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
