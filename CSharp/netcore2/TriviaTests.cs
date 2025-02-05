using System;
using System.IO;
using System.Text;
using Xunit;
using Assent;
using Assent.Reporters;
using Assent.Reporters.DiffPrograms;

namespace Trivia
{
    public class TriviaTests
    {
        [Fact]
        public void RefactoringTests()
        {
            var output = new StringBuilder();
            Console.SetOut(new StringWriter(output));

            Game aGame = new Game("Chet", "Pat", "Sue");

            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);
            aGame.Roll(1);

            aGame.WasCorrectlyAnswered();
            aGame.WrongAnswer();

            aGame.Roll(2);

            aGame.Roll(6);

            aGame.WrongAnswer();

            aGame.Roll(2);

            aGame.Roll(2);


            aGame.WrongAnswer();

            aGame.WasCorrectlyAnswered();
            aGame.Roll(1);
            aGame.WasCorrectlyAnswered();

            var configuration = BuildConfiguration();
            this.Assent(output.ToString(), configuration);
        }

        [Fact]
        public void Can_get_out_of_prison()
        {
            var output = new StringBuilder();
            Console.SetOut(new StringWriter(output));

            Game aGame = new Game("Chet", "Pat");
            aGame.Roll(1);
            aGame.WrongAnswer();
            aGame.Roll(1);
            aGame.WrongAnswer();
            aGame.Roll(1);
            aGame.WasCorrectlyAnswered();
            Console.WriteLine(aGame.CurrentPlayer.IsInPenaltyBox);

            var configuration = BuildConfiguration();
            this.Assent(output.ToString(), configuration);
        }

        private static Configuration BuildConfiguration()
        {
            return 
                new Configuration()
                
            // Uncomment this block if an exception 
            // « Could not find a diff program to use »
            // is thrown and if you have VsCode installed.
            // Otherwise, use other DiffProgram with its full path
            // as parameter.
            // See  https://github.com/droyad/Assent/wiki/Reporting
                   .UsingReporter(
                       new DiffReporter(
                           new []
                           {
                                // For linux
                            //    new VsCodeDiffProgram(new []
                            //    {
                            //        "/usr/bin/code"
                            //    })
                
                               // For Windows
                               new VsCodeDiffProgram(new [] {@"C:\Users\Ildar\AppData\Local\Programs\Microsoft VS Code\Code.exe"}), 
                           }))
                ;
        }
    }
}
