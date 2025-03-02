using System;
using System.Collections.Generic;

namespace GeoGuessr.Game
{

    public class Board
    {
        public BoardDefinition Definition { get; }

        Dictionary<Player, int> PlayerPositions { get; } = new();

        public Board(BoardDefinition definition, Player[] players)
        {
            Definition = definition;

            foreach (var player in players)
            {
                PlayerPositions[player] = definition.StartingTileIndex;
            }

        }

        public List<BoardTile> DeterminRollPath(int steps, Player player)
        {
            List<BoardTile> path = new(steps);
            int startingIndex = PlayerPositions[player];

            for (int i = 1; i <= steps; i++)
            {
                path.Add(Definition.BoardTiles[NormalizedIndex(startingIndex + i)]);
            }

            return path;
        }

        private int NormalizedIndex(int i)
        {
            return i % Definition.BoardTiles.Length;
        }

        public BoardTile GetPlayerTile(Player player)
        {
            return Definition.BoardTiles[PlayerPositions[player]];
        }

        public void SetPlayerTile(Player player, BoardTile tile)
        {
            PlayerPositions[player] = tile.Index;
        }
    }
}