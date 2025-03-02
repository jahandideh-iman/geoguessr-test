namespace GeoGuessr.Game
{
    public class BoardTile
    {
        public BoardTile(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}