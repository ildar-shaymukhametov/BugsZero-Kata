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
        public bool IsGettingOutOfPenaltyBox { get; set; }
    }

    public enum Category
    {
        Pop,
        Science,
        Sports,
        Rock
    }

    public class Question
    {
        public Question(Category category, string text)
        {
            Category = category;
            Text = text;
        }

        public Category Category { get; set; }
        public string Text { get; set; }
    }

    public class Game
    {
        private readonly List<Player> _players = new List<Player>();

        private readonly Stack<string> _popQuestions = new Stack<string>();
        private readonly Stack<string> _scienceQuestions = new Stack<string>();
        private readonly Stack<string> _sportsQuestions = new Stack<string>();
        private readonly Stack<string> _rockQuestions = new Stack<string>();
        private readonly Dictionary<Category, Stack<string>> _questions;

        public Player CurrentPlayer { get; private set; }

        public Game(string player1, string player2)
        {
            Add(player1);
            Add(player2);

            CurrentPlayer = _players[0];

            _questions = new Dictionary<Category, Stack<string>>
            {
                [Category.Pop] = _popQuestions,
                [Category.Science] = _scienceQuestions,
                [Category.Sports] = _sportsQuestions,
                [Category.Rock] = _rockQuestions
            };

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

        private bool Add(String playerName)
        {

            _players.Add(new Player(playerName));

            Console.WriteLine(playerName + " was Added");
            Console.WriteLine("They are player number " + _players.Count);

            return true;
        }

        public void Roll(int roll)
        {
            Console.WriteLine(CurrentPlayer.Name + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (!CurrentPlayer.IsInPenaltyBox)
            {
                AskQuestion(roll);
            }
            else if (roll % 2 != 0)
            {
                CurrentPlayer.IsGettingOutOfPenaltyBox = true;
                Console.WriteLine(CurrentPlayer.Name + " is getting out of the penalty box");
                AskQuestion(roll);
            }
            else
            {
                Console.WriteLine(CurrentPlayer.Name + " is not getting out of the penalty box");
                CurrentPlayer.IsGettingOutOfPenaltyBox = false;
            }
        }

        private void AskQuestion(int roll)
        {
            CurrentPlayer.Place = CurrentPlayer.Place + roll;
            if (CurrentPlayer.Place > 11)
            {
                CurrentPlayer.Place = CurrentPlayer.Place - 12;
            }

            Console.WriteLine(CurrentPlayer.Name
                    + "'s new location is "
                    + CurrentPlayer.Place);

            var currentCategory = CurrentCategory();

            Console.WriteLine("The category is " + currentCategory);

            AskQuestion();
        }

        private void AskQuestion()
        {
            var category = CurrentCategory();
            var questions = _questions[category];
            Console.WriteLine(questions.Pop());
        }

        private Category CurrentCategory()
        {
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>().ToArray();
            var index = CurrentPlayer.Place % categories.Length;
            return categories[index];
        }

        public bool WasCorrectlyAnswered()
        {
            if (CurrentPlayer.IsInPenaltyBox)
            {
                CurrentPlayer = GetNextPlayer();
                return true;
            }
            else
            {
                return CorrectAnswer();
            }
        }

        private bool CorrectAnswer()
        {
            Console.WriteLine("Answer was corrent!!!!");

            CurrentPlayer.Purse++;

            Console.WriteLine(CurrentPlayer.Name
                    + " now has "
                    + CurrentPlayer.Purse
                    + " Gold Coins.");

            var winner = DidPlayerWin();

            CurrentPlayer = GetNextPlayer();

            return winner;
        }

        private Player GetNextPlayer()
        {
            var index = _players.IndexOf(CurrentPlayer) + 1;
            if (index == _players.Count)
            {
                index = 0;
            }

            return _players[index];
        }

        public bool WrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(CurrentPlayer.Name + " was sent to the penalty box");

            CurrentPlayer.IsInPenaltyBox = true;
            CurrentPlayer = GetNextPlayer();

            return true;
        }

        private bool DidPlayerWin()
        {
            return !(CurrentPlayer.Purse == 6);
        }
    }
}
