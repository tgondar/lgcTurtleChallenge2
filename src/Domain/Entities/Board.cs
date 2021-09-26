using System.Collections.Generic;

namespace Domain.Entities
{
    public class Board
    {
        public Board()
        {
            Settings = new();
            Movements = new();
            GameInfo = new();
        }

        public Settings Settings { get; set; }
        public List<string> Movements { get; set; }
        public List<string> GameInfo { get; set; }
    }
}
