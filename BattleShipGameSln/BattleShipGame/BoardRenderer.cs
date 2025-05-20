using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipGame
{
    public class BoardRenderer
    {
        private const int BoardSize = 10;

        public void DrawBoard(char[,] board1, string title1, char[,] board2, string title2)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            // Titles
            Console.WriteLine($"   {title1.PadRight(BoardSize * 3)}       {title2}");

            // Draw column headers
            Console.Write("   ");
            for (int col = 0; col < BoardSize; col++)
                Console.Write($" {col} ");
            Console.Write("       "); // space between boards
            for (int col = 0; col < BoardSize; col++)
                Console.Write($" {col} ");
            Console.WriteLine();

            // Draw rows
            for (int row = 0; row < BoardSize; row++)
            {
                // Left board row
                Console.Write($" {row} ");
                for (int col = 0; col < BoardSize; col++)
                    Console.Write($"[{board1[row, col]}]");

                Console.Write("    "); // space between boards

                // Right board row
                Console.Write($" {row} ");
                for (int col = 0; col < BoardSize; col++)
                    Console.Write($"[{board2[row, col]}]");
                Console.WriteLine();
            }

            Console.ResetColor();
        }
    }
    public class Player
    {
        private const int BoardSize = 10;
        private char[,] board = new char[BoardSize, BoardSize];
        private int boatsToPlace = 3;

        public void DisplayPlacementBoard()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Board Reference (Rows & Columns):");

            Console.Write("   ");
            for (int col = 0; col < BoardSize; col++)
                Console.Write($" {col} ");
            Console.WriteLine();

            for (int row = 0; row < BoardSize; row++)
            {
                Console.Write($" {row} ");
                for (int col = 0; col < BoardSize; col++)
                {
                    Console.Write("[ ]"); // Empty cells
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }
        public Player()
        {
            // Initialize the board with water (~)
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    board[row, col] = '~';
        }

        // Player places 3 boats secretly
        public void PlaceBoats()
        {
            Console.Clear();
            DisplayPlacementBoard(); // show blank grid first

            Console.WriteLine("Place your 3 boats (hidden).");
            int placed = 0;

            while (placed < boatsToPlace)
            {
                try
                {
                    Console.Write($"Enter row for boat #{placed + 1}: ");
                    int row = int.Parse(Console.ReadLine());

                    Console.Write($"Enter column for boat #{placed + 1}: ");
                    int col = int.Parse(Console.ReadLine());

                    if (IsValid(row, col))
                    {
                        if (board[row, col] == 'S')
                        {
                            Console.WriteLine("You already placed a boat there. Try again.");
                        }
                        else
                        {
                            board[row, col] = 'S';
                            placed++;
                            Console.WriteLine("Boat placed!\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid coordinates. Must be between 0 and 9.");
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 9.");
                }
            }

            Console.Clear();
            Console.WriteLine("All boats placed. Press any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }

        private bool IsValid(int row, int col)
        {
            return row >= 0 && row < BoardSize && col >= 0 && col < BoardSize;
        }

        // Method to process a computer guess and reveal the hit
        public bool ReceiveGuess(int row, int col)
        {
            if (!IsValid(row, col))
                return false;

            if (board[row, col] == 'S')
            {
                board[row, col] = 'H'; // mark hit
                return true;
            }
            else if (board[row, col] == '~')
            {
                board[row, col] = 'M'; // mark miss
            }

            return false;
        }

        // Display the player's board showing only hits and misses
        public void DisplayHitsOnly()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Your board (Hits = H, Misses = M):");

            Console.Write("   ");
            for (int col = 0; col < BoardSize; col++)
                Console.Write($" {col}");
            Console.WriteLine();

            for (int row = 0; row < BoardSize; row++)
            {
                Console.Write($" {row} ");
                for (int col = 0; col < BoardSize; col++)
                {
                    char cell = board[row, col];
                    if (cell == 'H')
                        Console.Write("[H]");
                    else
                        Console.Write("[~]");
                }
                Console.WriteLine();
            }

            Console.ResetColor();
        }
        public char[,] GetVisibleBoard()
        {
            char[,] visible = new char[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == 'H')
                        visible[row, col] = 'H';
                    else if (board[row, col] == 'M')
                        visible[row, col] = 'M';
                    else
                        visible[row, col] = '~';
                }
            }
            return visible;
        }
        public bool AllBoatsSunk()
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (board[row, col] == 'S') // if any boat is not hit
                        return false;
                }
            }
            return true;
        }
    }
    public class Computer
    {
        private const int BoardSize = 10;
        private char[,] board = new char[BoardSize, BoardSize];
        private int boatsToPlace = 3;

        private Random random = new Random();

        public Computer()
        {
            // Fill board with water (~)
            for (int row = 0; row < BoardSize; row++)
                for (int col = 0; col < BoardSize; col++)
                    board[row, col] = '~';
        }

        public void PlaceBoats()
        {
            int placed = 0;

            while (placed < boatsToPlace)
            {
                int row = random.Next(BoardSize); // random number between 0 and 9
                int col = random.Next(BoardSize);

                if (board[row, col] != 'S')
                {
                    board[row, col] = 'S';
                    placed++;
                }
            }
        }

        public bool ReceiveGuess(int row, int col)
        {
            if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
                return false;

            if (board[row, col] == 'S')
            {
                board[row, col] = 'H'; // hit
                return true;
            }
            else if (board[row, col] == '~')
            {
                board[row, col] = 'M'; // miss
            }

            return false;
        }

        public char[,] GetVisibleBoard()
        {
            char[,] visible = new char[BoardSize, BoardSize];

            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == 'H')
                        visible[row, col] = 'H';
                    else if (board[row, col] == 'M')
                        visible[row, col] = 'M';
                    else
                        visible[row, col] = '~'; // hide ships
                }
            }

            return visible;
        }
        public bool AllBoatsSunk()
        {
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (board[row, col] == 'S') // if any boat is not hit
                        return false;
                }
            }
            return true;
        }
    }
}

