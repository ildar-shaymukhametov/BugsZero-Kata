using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Trivia
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public int Place { get; set; }
        public int Purse { get; set; }
        public bool IsInPenaltyBox { get; set; }
    }

    public class Game
    {
        private readonly List<Player> _players = new List<Player>();

        private readonly Stack<string> _popQuestions = new Stack<string>();
        private readonly Stack<string> _scienceQuestions = new Stack<string>();
        private readonly Stack<string> _sportsQuestions = new Stack<string>();
        private readonly Stack<string> _rockQuestions = new Stack<string>();

        private int _currentPlayer = 0;
        private bool _isGettingOutOfPenaltyBox;

        public Game(string player1, string player2)
        {
            Add(player1);
            Add(player2);

            for (int i = 50; i >= 0; i--)
            {
                _popQuestions.Push("Pop Question " + i);
                _scienceQuestions.Push("Science Question " + i);
                _sportsQuestions.Push("Sports Question " + i);
                _rockQuestions.Push("Rock Question " + i);
            }
        }
        
        public Game(string player1, string player2, string player3) : this(player1, player2)
        {
            Add(player3);
        }
        
        public Game(string player1, string player2, string player3, string player4) : this(player1, player2, player3)
        {
            Add(player4);
        }
        
        public Game(string player1, string player2, string player3, string player4, string player5) : this(player1, player2, player3, player4)
        {
            Add(player5);
        }
        
        public Game(string player1, string player2, string player3, string player4, string player5, string player6) : this(player1, player2, player3, player4, player5)
        {
            Add(player6);
        }

        public bool IsPlayable()
        {
            return _players.Count >= 2;
        }

        private bool Add(String playerName)
        {

            _players.Add(new Player(playerName));

            Console.WriteLine(playerName + " was Added");
            Console.WriteLine("They are player number " + _players.Count);

            return true;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(_players[_currentPlayer].Name + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (_players[_currentPlayer].IsInPenaltyBox)
            {
                if (roll % 2 != 0)
                {
                    _isGettingOutOfPenaltyBox = true;

                    Console.WriteLine(_players[_currentPlayer].Name + " is getting out of the penalty box");
                    _players[_currentPlayer].Place = _players[_currentPlayer].Place + roll;
                    if (_players[_currentPlayer].Place > 11)
                    {
                        _players[_currentPlayer].Place = _players[_currentPlayer].Place - 12;
                    }

                    Console.WriteLine(_players[_currentPlayer].Name
                            + "'s new location is "
                            + _players[_currentPlayer].Place);
                            
                    var currentCategory = CurrentCategory();
                    Console.WriteLine("The category is " + currentCategory);
                    AskQuestion(currentCategory);
                }
                else
                {
                    Console.WriteLine(_players[_currentPlayer].Name + " is not getting out of the penalty box");
                    _isGettingOutOfPenaltyBox = false;
                }
            }
            else
            {
                _players[_currentPlayer].Place = _players[_currentPlayer].Place + roll;
                if (_players[_currentPlayer].Place > 11)
                {
                    _players[_currentPlayer].Place = _players[_currentPlayer].Place - 12;
                }

                Console.WriteLine(_players[_currentPlayer].Name
                        + "'s new location is "
                        + _players[_currentPlayer].Place);
                
                var currentCategory = CurrentCategory();
                Console.WriteLine("The category is " + currentCategory);
                AskQuestion(currentCategory);
            }
        }

        private void AskQuestion(string currentCategory)
        {
            Stack<string> questions = null;

            switch (currentCategory)
            {
                case "Pop":
                    questions = _popQuestions;
                    break;
                case "Science":
                    questions = _scienceQuestions;
                    break;
                case "Sports":
                    questions = _sportsQuestions;
                    break;
                default:
                    questions = _rockQuestions;
                    break;
            }

            Console.WriteLine(questions.Pop());
        }

        private String CurrentCategory()
        {
            switch (_players[_currentPlayer].Place)
            {
                case 0:
                case 4:
                case 8:
                    return "Pop";
                case 1:
                case 5:
                case 9:
                    return "Science";
                case 2:
                case 6:
                case 10:
                    return "Sports";
                default:
                    return "Rock";
            }
        }

        public bool WasCorrectlyAnswered()
        {
            if (_players[_currentPlayer].IsInPenaltyBox)
            {
                if (_isGettingOutOfPenaltyBox)
                {
                    Console.WriteLine("Answer was correct!!!!");
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count)
                    {
                        _currentPlayer = 0;
                    }

                    _players[_currentPlayer].Purse++;
                    Console.WriteLine(_players[_currentPlayer].Name
                            + " now has "
                            + _players[_currentPlayer].Purse
                            + " Gold Coins.");

                    var winner = DidPlayerWin();

                    return winner;
                }
                else
                {
                    _currentPlayer++;
                    if (_currentPlayer == _players.Count)
                    {
                        _currentPlayer = 0;
                    }

                    return true;
                }
            }
            else
            {

                Console.WriteLine("Answer was corrent!!!!");
                _players[_currentPlayer].Purse++;
                Console.WriteLine(_players[_currentPlayer].Name
                        + " now has "
                        + _players[_currentPlayer].Purse
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                _currentPlayer++;
                if (_currentPlayer == _players.Count)
                {
                    _currentPlayer = 0;
                }

                return winner;
            }
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(_players[_currentPlayer].Name + " was sent to the penalty box");

            _players[_currentPlayer].IsInPenaltyBox = true;

            _currentPlayer++;
            if (_currentPlayer == _players.Count)
            {
                _currentPlayer = 0;
            }

            return true;
        }

        private bool DidPlayerWin()
        {
            return !(_players[_currentPlayer].Purse == 6);
        }
    }
}
