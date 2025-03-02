using GeoGuessr.Game;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GeoGuessr.Configuration
{
    [JsonObject]
    public class BoardJsonConfiguration
    {
        [JsonObject]
        public struct Tile
        {
            public int x; public int y;
        }

        [JsonProperty]
        private List<int[]> tiles;

        [JsonProperty]
        private int startingTileIndex;


        public BoardDefinition ToBoardDefinition()
        {
            var boardTiles = new List<BoardTile>();
            for (int i = 0; i < tiles.Count; i++)
            {
                int[] tile = tiles[i];
                boardTiles.Add(new BoardTile(tiles[i][0], tiles[i][1], i));
            }
            return new BoardDefinition(boardTiles, startingTileIndex);
        }

    }
}