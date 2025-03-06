using GeoGuessr.Game;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Configuration
{
    public class BoardJsonConfiguration
    {
        [JsonObject]
        public struct Board
        {
            [JsonProperty] public List<int[]> tiles;
            [JsonProperty] public int startingTileIndex;
            [JsonProperty] public int[] questionQuizTiles;
            [JsonProperty] public int[] flagQuizTiles;
            [JsonProperty] public string name;
        }

        [JsonObject]
        public struct Tile
        {
            public int x; public int y;
        }

        public static BoardDefinition Load(string boardJson)
        {
            var boardConfig = JsonConvert.DeserializeObject<Board>(boardJson);
            var boardTiles = new List<BoardTile>();

            for (int i = 0; i < boardConfig.tiles.Count; i++)
            {
                int[] tile = boardConfig.tiles[i];
                boardTiles.Add(new BoardTile(tile[0], tile[1], i, QuizTypeForTile(boardConfig, i)));
            }
            return new BoardDefinition(boardTiles, boardConfig.startingTileIndex, boardConfig.name);
        }

        private static QuizType? QuizTypeForTile(Board boardConfig, int i)
        {
            if (boardConfig.questionQuizTiles != null && boardConfig.questionQuizTiles.Contains(i))
            {
                return QuizType.Question;
            }
            else if (boardConfig.flagQuizTiles != null && boardConfig.flagQuizTiles.Contains(i))
            {
                return QuizType.Flag;
            }

            return null;
        }
    }
}