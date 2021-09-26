using System.Collections.Generic;

namespace Domain.Entities
{
    public class Settings
    {
        public Settings()
        {
            Mines = new List<Mine>();
        }

        public Tile BoardSize { get; set; }

        public List<Mine> Mines { get; set; }

        public Turtle TurtlePosition { get; set; }

        public Tile Exit { get; set; }
    }
}
