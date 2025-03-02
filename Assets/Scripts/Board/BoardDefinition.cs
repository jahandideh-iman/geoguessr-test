using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Game
{

    public class BoardDefinition
    {
        public BoardTile[] BoardTiles { get; }

        public int StartingTileIndex { get; }

        public BoardDefinition(IEnumerable<BoardTile> boardTiles, int startingTileIndex)
        {
            BoardTiles = boardTiles.ToArray();
            StartingTileIndex = startingTileIndex;
        }
    }
}