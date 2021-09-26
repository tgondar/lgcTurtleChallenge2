using Domain.Enums;

namespace Domain.Entities
{
    public class Turtle : Tile
    {
        public DirectionEnum Direction { get; set; }

        public override string ToString()
        {
            return X + "," + Y;
        }
    }
}
