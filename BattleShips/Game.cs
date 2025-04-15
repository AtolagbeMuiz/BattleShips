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

            var shipTypes = new List<ShipType> 
            {
                new ShipType() { Name = "Battleship", Size=5, CountOfShipType=1},
                new ShipType() { Name = "Destroyer", Size=4, CountOfShipType=2},
                
            };

            var random = new Random();

            foreach (var shipType in shipTypes)
            {
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

                        // Calculate max starting position based on orientation
                        int maxRow = isHorizontal ? _gameGrid.Rows : _gameGrid.Rows - ship.Size + 1;
                        int maxCol = isHorizontal ? _gameGrid.Columns - ship.Size + 1 : _gameGrid.Columns;

                        // Get random starting position
                        int startRow = random.Next(maxRow);
                        int startCol = random.Next(maxCol);

                        // Try to place the ship
                        var positions = new List<Position>();
                        bool canPlace = true;

                        for (int j = 0; j < ship.Size; j++)
                        {
                            int row = isHorizontal ? startRow : startRow + j;
                            int col = isHorizontal ? startCol + j : startCol;
                            var position = new Position(row, col);

                            // Check if position is already occupied
                            if (_gameGrid.IsOccupied(position))
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
            if (_gameGrid.IsOccupied(position))
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

}
