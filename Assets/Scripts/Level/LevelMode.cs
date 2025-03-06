#nullable enable
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace GeoGuessr.Game
{
    public class LevelMode
    {
        BoardDefinition? _boardDefinition;

        List<string> _localPlayerNames = new();
        List<string> _aiPlayerNames = new();

        public LevelMode()
        {
        }

        public LevelMode SetBoard(BoardDefinition boardDefinition)
        {
            _boardDefinition = boardDefinition;
            return this;
        }

        public LevelMode AddLocalPlayer(string name)
        {
            _localPlayerNames.Add(name);
            return this;
        }

        public LevelMode AddAiPlayer()
        {
            _aiPlayerNames.Add($"AiPlayer{_aiPlayerNames.Count + 1}");
            return this;
        }

        public LevelController BuildLevelController(
            QuizDatabase quizDatabase,
            ILevelViewPort levelViewPort,
            IPlayerControllerViewPort playerViewPort)
        {
            UnityEngine.Debug.Assert(_boardDefinition != null, "No board is set for the level");
            var players = _localPlayerNames
                .Select(name => new Player(name, new LocalPlayerController(playerViewPort)))
                .Concat(_aiPlayerNames.Select(name => new Player(name, new AiPlayerController(playerViewPort))));

            return new LevelController(players, _boardDefinition, quizDatabase, levelViewPort);
        }
    }
}