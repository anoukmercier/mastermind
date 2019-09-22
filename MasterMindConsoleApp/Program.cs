using System;

namespace MasterMindConsoleApp
{
    class MasterMindProgram
    {
        public const int NB_OF_DIGITS = 4;
        public const int MAX_NB_OF_ATTEMPTS = 10;
        static void Main(string[] args)
        {
            // Display title as the C# MasterMind console app.
            Console.WriteLine("Console MasterMind in C#\r");
            Console.WriteLine("------------------------\n");

            Console.WriteLine("Instructions: We have a secret key that you must guess.");
            Console.WriteLine("To play the game, you will enter a " + NB_OF_DIGITS.ToString() + "- digit number, where each digit is between 1 and 6, in order to guess the correct key..\r");
            Console.WriteLine("You will win if you can guess the number within " + MAX_NB_OF_ATTEMPTS.ToString() + " attempts.\r");
            Console.WriteLine("Type a " + NB_OF_DIGITS.ToString() + "- digit number, where each digit is between 1 and 6, and then press Enter");
            Console.WriteLine("Each '+' signals a digit that is both correct and in the correct position");
            Console.WriteLine("Each '-' signals a digit that is correct but in the wrong position");
            Console.WriteLine("Type QUIT to end the game\r");

            // create a secret key that user needs to guess
            MasterMind newGame = new MasterMind();
            Console.WriteLine("The answer is: " + newGame.AnswerKeyString);

            int nbOfAttempts = 0;
            bool? results;
            string userResponse;
            while (nbOfAttempts < MAX_NB_OF_ATTEMPTS)
            {
                results = newGame.ProcessUserGuess(++nbOfAttempts);
                if (!results.HasValue)
                    break;

                while (results.Value == true || (results.Value == false && nbOfAttempts == MAX_NB_OF_ATTEMPTS))
                {
                    if (results.Value == true)
                        Console.WriteLine("Great game!");
                    else if (results.Value == false && nbOfAttempts == MAX_NB_OF_ATTEMPTS)
                        Console.WriteLine("You lose, better luck next time.");

                    Console.WriteLine("Would you like to play again?");
                    userResponse = Console.ReadLine();
                    if (userResponse != null && (userResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase) ||
                        userResponse.Equals("yes", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        newGame = new MasterMind();
                        nbOfAttempts = 0;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }


            }

            // Wait for the user to respond before closing.
            Console.Write("Press any key to close the MasterMind console app...");
            Console.ReadKey();
        }

        public class MasterMind
        {
            private int[] answerKey;
            public int[] AnswerKey
            {
                get
                {
                    if (answerKey == null)
                    {
                        answerKey = new int[NB_OF_DIGITS];
                        Random rnd = new Random();
                        for (int i = 0; i < NB_OF_DIGITS; i++)
                        {
                            int dice = rnd.Next(1, 7);   // creates a number between 1 and 6
                            answerKey[i] = dice;
                        }
                    }
                    return answerKey;
                }
            }
            private string answerKeyString;
            public string AnswerKeyString
            {
                get
                {
                    if (answerKeyString == null)
                        answerKeyString = String.Join("", AnswerKey);
;                    return answerKeyString;
                }
            }
            public bool? ProcessUserGuess(int nbAttempts)
            {
                string userGuess;
                string clues = String.Empty;

                // Ask the user to type their guess
                Console.Write("Attempt #" + nbAttempts.ToString() + " : ");
                userGuess = Console.ReadLine();

                if (userGuess != null && userGuess.Trim().Contains("quit", StringComparison.InvariantCultureIgnoreCase))
                    return null;

                if (userGuess == null || userGuess.Trim().Length != NB_OF_DIGITS)
                {
                    Console.WriteLine("Invalid response. Please make sure your guess has " + NB_OF_DIGITS.ToString() + " digits, where each digit is a number between 1 and 6: ");
                    return false;
                }


                int guessInt;
                string leftoverAnswer = AnswerKeyString;
                string leftoverGuess = String.Empty;
                int leftoverIndex;
                if (userGuess != null && userGuess.Length > 0)
                {
                    int position = 0;
                    // let's go once over the response to get digits in correct position
                    foreach (char guess in userGuess)
                    {
                        if (position > 3)
                            break;

                        // make sure we have a valid character
                        if (!Int32.TryParse(guess.ToString(), out guessInt) || guessInt < 1 || guessInt > 6)
                        {
                            Console.WriteLine("Invalid response. Please make sure your guess has " + NB_OF_DIGITS.ToString() + " digits, where each digit is a number between 1 and 6: ");
                            return false;
                        }
                        if (guessInt == AnswerKey[position])
                        {
                            clues += '+';
                            leftoverIndex = leftoverAnswer.IndexOf(guess);
                            leftoverAnswer = (leftoverIndex > 0 ? leftoverAnswer.Substring(0, leftoverIndex) : String.Empty) +
                                (leftoverIndex < 4 ?  leftoverAnswer.Substring(leftoverIndex + 1) : String.Empty);
                        }
                        else
                        {
                            leftoverGuess += guess;
                        }
                        position++;
                    }
                    if (clues == "++++")
                    {
                        Console.WriteLine("You WIN! You have correctly guessed the key.");
                        return true;
                    }

                    // let's go over the response second time to find digits in incorrect position
                    foreach (char guess in leftoverGuess)
                    {
                        if (leftoverAnswer.Contains(guess))
                        {
                            clues += '-';
                            leftoverIndex = leftoverAnswer.IndexOf(guess);
                            leftoverAnswer = (leftoverIndex > 0 ? leftoverAnswer.Substring(0, leftoverIndex) : String.Empty) +
                                (leftoverIndex < leftoverAnswer.Length ? leftoverAnswer.Substring(leftoverIndex + 1) : String.Empty);
                        }
                    }

                }

                Console.WriteLine("Your guess yields these results : " + clues);

                return false;
            }
        }
    }
}
