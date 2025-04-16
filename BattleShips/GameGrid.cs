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

        public string IsCellPositionOccupied(Position position)
        {
            try
            {
                //gets the value in the grid cell position, returns 1 if its occupied and 0 if it's empty
                var isOccupied = _grid[position.Row, position.Column];
                if (isOccupied == 1)
                {
                    return "1";
                }
                else { return "2"; }
            }
            catch (Exception ex)
            {
               Console.WriteLine("The provided coordinate is out of bounds of the array");
               return "0";
            }
          
           
        }

        public void PlaceShip(Position position)
        {
            //this assign value 1 into the cell position to depiect a ship is in that cell
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
