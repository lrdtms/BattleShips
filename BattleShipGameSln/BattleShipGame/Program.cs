namespace BattleShipGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var renderer = new BoardRenderer();
            var player = new Player();
            var computer = new Computer();

            player.PlaceBoats();
            computer.PlaceBoats(); // randomly set boats

            Random rand = new Random();
            HashSet<string> computerGuesses = new HashSet<string>();

            while (true)
            {
                Console.Clear();
                char[,] playerView = player.GetVisibleBoard(); // visible board for player
                char[,] computerView = computer.GetVisibleBoard(); // hides computer boats
                renderer.DrawBoard(playerView, "Player Board", computerView, "Enemy Board");

                Console.WriteLine("Your turn to guess!");

                int row, col;
                while (true)
                {
                    try
                    {
                        Console.Write("Enter row (0-9): ");
                        row = int.Parse(Console.ReadLine());
                        Console.Write("Enter column (0-9): ");
                        col = int.Parse(Console.ReadLine());

                        if (row >= 0 && row < 10 && col >= 0 && col < 10)
                            break;

                        Console.WriteLine("Invalid input. Try again.");
                    }
                    catch
                    {
                        Console.WriteLine("Invalid input. Please enter numbers.");
                    }
                }

                bool playerHit = computer.ReceiveGuess(row, col);
                Console.WriteLine(playerHit ? "You hit a ship!" : "You missed.");
                Console.ReadKey();

                if (computer.AllBoatsSunk())
                {
                    Console.WriteLine("🎉 You win!");
                    break;
                }

                // --- Computer Turn ---
                Console.WriteLine("Computer's turn...");
                int guessRow, guessCol;
                do
                {
                    guessRow = rand.Next(10);
                    guessCol = rand.Next(10);
                }
                while (!computerGuesses.Add($"{guessRow},{guessCol}")); // prevents duplicate guesses

                bool computerHit = player.ReceiveGuess(guessRow, guessCol);
                Console.WriteLine($"Computer guessed ({guessRow}, {guessCol}) and " +
                                  (computerHit ? "hit your ship!" : "missed."));
                Console.ReadKey();

                if (player.AllBoatsSunk())
                {
                    Console.WriteLine("💥 The computer wins!");
                    break;
                }
            }
            Console.WriteLine("Game over! Press any key to exit...");
            Console.ReadKey();

            char[,] playerboard = new char[10, 10];
                char[,] enemyboard = new char[10, 10];
            // Initialize board with water (e.g., '~')
            for (int row = 0; row < 10; row++)
            { 
                    for (int col = 0; col < 10; col++)
                { 
                    playerboard[row, col] = '~';
                    enemyboard[row, col] = '~';
                }
            }
            renderer.DrawBoard(playerboard, "Player Board", enemyboard, "Enemy Board"); 
            // Show result
            player.DisplayHitsOnly();
                Console.ReadKey();
        }
    }
}

