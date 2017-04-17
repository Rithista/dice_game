using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;

namespace dice_game.cs
{

  public class Game
  {
    public static int playerTotal;
    public static bool Opponent = true;

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
      }else if (Opponent == false)
      {
        playerTotal = 2;
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
          Console.Clear();
          PrintStart();
          Player p = _players.ElementAt(i);
          p.takeTurn();
          p.printScore();
          isGameWon = p.isWinner();
          if (isGameWon)
          {
            //Console.Write(p.getHistory());
            Game.printHistory();
            break;
          }
          // Console.WriteLine("Now its player {0}'s turn!", _players[i + 1]);
          if(Opponent == true)
          {
            Console.WriteLine("Press enter to take turn");
            //Console.ReadLine();
          }
        }
      }
    }
    public static int whoAmI()
    {
      return Convert.ToInt32(Opponent);
    }
    static void printHistory()
    {
      for (int i = 0; i < playerTotal; i++)
      {
        _players.ElementAt(i).printHistory();
      }
     }
  }

  public class Player
  {
    private int _score = 0;
    private int _playerNumber;
    private static int currentTurn;
    private string _history;
    private List<List<int>> _l = new List<List<int>>();


    public void printScore()
    {
      Console.WriteLine("Player {0} score is {1}", _playerNumber, _score);
    }
    public Player(int number)
    {
      _playerNumber = number;
      int isComputer = 0;
      if(_playerNumber == 2)
      {
        isComputer = 1;
      }
      if((isComputer == 1) && (Game.whoAmI() == 0))
      {
        _playerNumber = Convert.ToInt32("Computer");
      }

    }
    public int getScore()
    {
      return _score;
    }

    public void takeTurn()
    {
      Turn t = new Turn();


      if (t.findTurnNumber(_playerNumber) == 1)
      {
        currentTurn++;
      }

      Console.WriteLine("Player {0}: taking turn {1}", _playerNumber, currentTurn);

      _score += t.takeTurn();
      _history = string.Format("{0}\nPlayer {1}, Rolled {2} on turn {3}.", _history, _playerNumber, t.getHistory(), currentTurn);
      _l.Add(t.getHistoryList());

    }

    public void printHistory()
    {
      int tn = 1;
      int throwTotal = 0;
      int throwAverage = 0;
      Dictionary<int, int> dieAverage = new Dictionary<int, int>();
      foreach(var le in _l)
      {
        Console.Write(string.Format("Turn {0} Player {1}: ", tn, _playerNumber));
        foreach(var i in le)
        {
          Console.Write(string.Format("{0} ", i));
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
        Console.Write("Throw total: {0}", throwTotal);
        throwTotal = 0;
        Console.WriteLine("");
        tn++;
      }
      throwAverage = (throwAverage / tn - 1);
      Console.WriteLine("Throw Average: {0}", throwAverage);
      foreach (KeyValuePair<int, int> entry in dieAverage)
      {
        Console.WriteLine("You rolled {0} {1} times", entry.Key, entry.Value);
      }
    }


    public int getPlayer()
    {
      return _playerNumber;
    }

    public string getHistory()
    {
      return _history;
    }

    public int getTurn()
    {
      return currentTurn;
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
    private string _history;
    private List<int> _hl;

    public string getHistory()
    {
      return _history;
    }
    public List<int> getHistoryList()
    {
      return _hl;
    }




    public int findTurnNumber(int number)
    {
      if (number == 1)
      {
        return 1;
      }
      else
      {
        return 0;
      }

    }

    public int takeTurn()
    {
      int dieFace = 0;
      List<int> results;
      Roll r = new Roll();
      results = r.doRoll(4);
      r.printRoll();
      _history = string.Join(", " ,results);
      _hl = results;
      this.findKind(results);
      dieFace = this.highestKey();
      int hc = highestCount();
      this.keepKey(dieFace, hc);
      this.printDict();





      if (hc == 2)
      {

        Console.WriteLine("\nYou found 2 {0}'s, press enter to re-roll 3 dice...", dieFace);
        //Console.ReadLine();
        results = r.doRoll(2);
        r.printRoll();
        this.findKind(results);
        dieFace = this.highestKey();
        this.printDict();
        hc = highestCount();
        _history = string.Join(", ", results);
        _history = string.Format("{0}, {1}, {2}",_history, dieFace, dieFace);
        _hl = results;
        _hl.Add(dieFace);
        _hl.Add(dieFace);
        if (hc == 2)
        {
          Console.WriteLine("\nYou found no pairs of 3 and score 0 points this turn.");
        }
      }


      if (hc == 3)
      {
        _score = 3;
        Console.WriteLine("\nYou found 3 {0}'s and scored {1} points!", dieFace, _score);
      }
      if (hc == 4)
      {
        _score = 6;
        Console.WriteLine("\nYou found 4 {0}'s and scored {1} points!", dieFace, _score);
      }
      if (hc == 5)
      {
        _score = 12;
        Console.WriteLine("\nYou found 5 {0}'s and scored {1} points!", dieFace, _score);
      }
      if (hc == 1)
      {
        Console.WriteLine("\nYou found no pairs and score 0 points this turn.");
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
    //##HISTORY BUILDER##
    public void printHistory()
    {
      Console.WriteLine(_history);
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
      rollOutcome = string.Format("!!!ROLLING| {0} |ROLLING!!!", rollOutcome);
      Console.WriteLine("\n{0}", rollOutcome);
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
