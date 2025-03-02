namespace GeoGuessr.Game
{
    public class Board
    {
        public BoardDefinition Definition { get; }
        public Board(BoardDefinition definition)
        {
            Definition = definition;
        }
    }
}