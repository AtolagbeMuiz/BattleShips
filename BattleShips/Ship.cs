using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    public class Ship
    {
        public string Name { get; }
        public int Size { get; }
        public List<Position> Positions { get; }
        private readonly Dictionary<Position, int> _hits;
        public int hitCount = 0;

        public bool IsSunk => _hits.Count == Size;
       

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Positions = new List<Position>();
            _hits = new Dictionary<Position, int>();
        }

        public void AddPosition(Position position)
        {
            Positions.Add(position);
        }

        public void AddHitToInMemoryDB(Position position)
        {
            if (Positions.Any(p => p.Equals(position)))
            {
                //update the hit count by 1
                 hitCount = hitCount + 1;
                _hits.Add(position, hitCount);
            }
        }
    }
}
