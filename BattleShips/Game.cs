using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    public class Game
    {
        private readonly GameGrid _gameGrid;
        private readonly List<Ship> _ships;
        private readonly Dictionary<string, int> _shotHistory;

        public int ShotsFired 
        {
            get
            {
                return _shotHistory.Count;
            }
        }
        public bool IsGameOver => _ships.All(s => s.IsSunk);

        public Game()
        {
            _gameGrid = new GameGrid(10, 10);
            _ships = new List<Ship>();
            _shotHistory = new Dictionary<string, int>();
        }

        public void Start()
        {
            PositionShipsIntoGrids();

            _gameGrid.PrintGrid();
        }

        private void PositionShipsIntoGrids()
        {
            //in-memory db for ships to be setup on the grid
            var shipTypes = new List<ShipType> 
            {
                new ShipType() { Name = "Battleship", Size=5, CountOfShipType=1},
                new ShipType() { Name = "Destroyer", Size=4, CountOfShipType=2},
                
            };

            var random = new Random();

            foreach (var shipType in shipTypes)
            {
                //loops through the countofshiptype
                for (int i = 0; i < shipType.CountOfShipType; i++)
                {
                    var ship = new Ship(shipType.Name, shipType.Size);
                    bool placed = false;

                    // Try to place the ship until successful
                    while (!placed)
                    {
                        // Randomly decide horizontal or vertical orientation
                        bool isHorizontal;
                        if(random.Next(2) == 0)
                        {
                            isHorizontal = true;
                        }
                        else
                        {
                            isHorizontal = false;
                        }

                        // Calculate max starting position based on orientation of the grid (added 1 to prevent the ship position to go outside the grid boundary)
                        int maxRow;
                        int maxCol;
                        if(isHorizontal == true)
                        {
                            maxRow = _gameGrid.Rows;
                            maxCol = (_gameGrid.Columns - ship.Size)+ 1;
                        }
                        else
                        {
                            maxRow = (_gameGrid.Rows - ship.Size)+ 1;
                            maxCol = _gameGrid.Columns;

                        }

                        // Get random starting position
                        int startRow = random.Next(maxRow);
                        int startCol = random.Next(maxCol);

                        // Try to place the ship
                        var positions = new List<Position>();
                        bool canPlace = true;

                        //loops through the size of the ship i.e each cell the ship is made up to be placed into their position
                        for (int j = 0; j < ship.Size; j++)
                        {
                            int row;
                            int col;
                            if(isHorizontal == true)
                            {
                                row = startRow;

                                //move to the next column to ship cell will occupy an horizontally-placed ship
                                col = startCol + j;
                            }
                            else
                            {
                                //move to the next row to ship cell will occupy for a vertically-placed ship
                                row = startRow + j;
                                col= startCol;

                            }

                            var position = new Position(row, col);

                            // Check if cell position is already occupied
                            if (_gameGrid.IsCellPositionOccupied(position))
                            {
                                canPlace = false;
                                break;
                            }

                            //curates the list of ship position i.e. add the posiotion of each ship to the list
                            positions.Add(position);
                        }

                        if (canPlace)
                        {
                            // Place the ship
                            foreach (var position in positions)
                            {
                                _gameGrid.PlaceShip(position);
                                ship.AddPosition(position);
                            }
                            _ships.Add(ship);
                            placed = true;
                        }
                    }
                }
            }

        }

        public string FireShot(string coordinate)
        {
            int hitCount = 0;
            var position = ConvertCoordinateToPosition(coordinate);

            // Check if this position was already fired at
            if (_shotHistory.ContainsKey(coordinate))
            {
                return "You have fired a shot at this position already. Try again";
            }

            _shotHistory.Add(coordinate, hitCount+1);

            // Check if hit a ship
            if (_gameGrid.IsCellPositionOccupied(position))
            {
                // Register hit with the appropriate ship
                var hitShip = _ships.FirstOrDefault(s => s.Positions.Any(p => p.Equals(position)));
                if (hitShip != null)
                {
                    hitShip.AddHitToInMemoryDB(position);

                    if (hitShip.IsSunk)
                    {
                        return $"Hit! You've sunk the {hitShip.Name}!";
                    }

                    return "Hit!";
                }

                return "Hit!"; // Fallback, should not reach here
            }

            return "Miss!";
        }

        private Position ConvertCoordinateToPosition(string coordinate)
        {
            int column = coordinate[0] - 'A';
            int row = int.Parse(coordinate.Substring(1)) - 1;
            return new Position(row, column);
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
