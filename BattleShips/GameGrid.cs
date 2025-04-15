using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    public class GameGrid
    {
        public readonly int[,] _grid;

        public int Rows { get; }
        public int Columns { get; }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _grid = new int[rows, columns];
        }

        public bool IsCellPositionOccupied(Position position)
        {
            var isOccupied = _grid[position.Row, position.Column];
            if(isOccupied == 1)
            {
                return true;
            }
            else { return false; }
           
        }

        public void PlaceShip(Position position)
        {
            _grid[position.Row, position.Column] = 1;

        }

        public void PrintGrid()
        {
            int rows = _grid.GetLength(0);  // number of rows
            int cols = _grid.GetLength(1);  // number of columns

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    Console.Write(_grid[row, col] + " ");
                }
                Console.WriteLine(); // move to the next line after each row
            }
        }
    }

}
