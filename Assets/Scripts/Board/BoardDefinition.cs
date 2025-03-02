using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Game
{
    public class BoardDefinition
    {
        public BoardTile[] BoardTiles { get; }

        public BoardDefinition(IEnumerable<BoardTile> boardTiles)
        {
            BoardTiles = boardTiles.ToArray();
        }
    }
}