
namespace GeoGuessr.Game
{
    public class LevelController
    {
        public Board Board { get; }
        public LevelController(BoardDefinition boardDefinition)
        {
            Board = new Board(boardDefinition);
        }
    }
}