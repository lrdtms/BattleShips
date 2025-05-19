namespace BattleShipGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
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
            // Place a ship for testing (optional)
            //playerboard[2, 3] = 'S';
            //playerboard[4, 5] = 'S';
            //enemyboard[1, 2] = 'S';

            // Render the board
            var renderer = new BoardRenderer();
            renderer.DrawBoard(playerboard, "Player Board", enemyboard, "Enemy Board"); ;
            
            var player = new Player();
            player.PlaceBoats();

            // Example computer guesses
            Console.WriteLine("Computer guessing (3,4)...");
            bool hit = player.ReceiveGuess(3, 4);
            //bool hit2 = player.ReceiveGuess(4, 4);

            // Show result
            player.DisplayHitsOnly();
            // Reveal only hits/misses
            //player.DisplayHitsAndMisses();
            // Wait for user input
            Console.SetCursorPosition(0, 22);
                Console.Write("Press any key to exit...");
                Console.ReadKey();
        }
    }
}

