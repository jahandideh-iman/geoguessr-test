using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GeoGuessr.Configuration
{
    [Serializable]
    public class BoardJsonConfiguration
    {
        [Serializable]
        public struct Tile
        {
            public int x; public int y;
        }

        [SerializeField]
        public List<int[]> tiles;


        public BoardDefinition ToBoardDefinition()
        {

            return new BoardDefinition(tiles.Select(pos => new BoardTile(pos[0], pos[1])));

        }

    }
}