using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Game
{
    public class BoardDefinition
    {
        public BoardTile[] BoardTiles { get; }
        public int StartingTileIndex { get; }
        public string Name { get; }

        public BoardDefinition(IEnumerable<BoardTile> boardTiles, int startingTileIndex, string name)
        {
            BoardTiles = boardTiles.ToArray();
            StartingTileIndex = startingTileIndex;
            Name = name;
        }
    }
}