namespace GeoGuessr.Game
{
    public class BoardTile
    {
        public Position Position { get; }
        public int Index { get; }
        public int X => Position.x;
        public int Y => Position.y;


        public BoardTile(int x, int y, int index)
        {
            Position = new Position { x = x, y =y };
            Index = index;
        }
    }
}