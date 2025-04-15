using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BattleShips
{
    class Program
    {
        /// <summary>
        /// --> create a 10 * 10 grid
        /// --> start the game by positioning the ships in to the grid at random position
        /// </summary>
        /// <param name="args"></param>


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Battleships!");
            Console.WriteLine("Enter coordinates like 'A5' to fire at that position.");
            Console.WriteLine("Press 'Q' to quit the game.");
            Console.WriteLine();

            var game = new Game();
            game.Start();

            while (!game.IsGameOver)
            {
                Console.Write("Enter target coordinates: ");
                var input = Console.ReadLine()?.Trim().ToUpper();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Please enter valid coordinates.");
                    continue;
                }

                if (input == "Q")
                {
                    Console.WriteLine("Thanks for playing!");
                    break;
                }

                if (!IsValidCoordinate(input))
                {
                    Console.WriteLine("coordinates are not valid. Please use format like 'A5'.");
                    continue;
                }

                var result = game.FireShot(input);
                Console.WriteLine(result);

                if (game.IsGameOver)
                {
                    Console.WriteLine("Congratulations! You've sunk all ships!");
                    Console.WriteLine($"You won in {game.ShotsFired} shots!");
                    break;
                }
            }
        }

        static bool IsValidCoordinate(string coordinate)
        {
            return Regex.IsMatch(coordinate, @"^[A-J][1-9]|10$");
        }
    }


    public class Position : IEquatable<Position>
    {
        public int Row { get; }
        public int Column { get; }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public bool Equals(Position other)
        {
            if (other == null) return false;
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Position);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    }
}