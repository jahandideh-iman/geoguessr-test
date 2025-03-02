using Arman.UIManagement;
using Cysharp.Threading.Tasks;
using GeoGuessr.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using static GeoGuessr.Presentation.SerializableDictionaryUtilities;

namespace GeoGuessr.Presentation
{
    public class LevelWindow : MainWindow
    {
        [SerializeField] Announcement _announcement;
        [SerializeField] RollPanel _rollPanel;
        [SerializeField] List<SerializableDictionaryKeyValue<QuizType, QuizPopup>> _quizPopupPrefabs;
        [SerializeField] List<SerializableDictionaryKeyValue<QuizType, QuizResultPopup>> _quizResultPopupPrefabs;

        private Dictionary<QuizType, QuizPopup> _quizPopupsPrefabsMap;
        private Dictionary<QuizType, QuizResultPopup> _quizResultPopupsPrefabsMap;
        internal void Setup(LevelController levelController)
        {
            _quizPopupsPrefabsMap = _quizPopupPrefabs.ToDictionary();
            _quizResultPopupsPrefabsMap = _quizResultPopupPrefabs.ToDictionary();

            _announcement.Setup();
            _rollPanel.Setup(onRoll: levelController.Roll);
        }

        public async UniTask SetupPlayerTurn(Player player)
        {
            _rollPanel.MoveIn();
            await _announcement.Announce($"Player {player.Index+1} turn");
        }

        public async UniTask SetupPlayerMovement(Player player, IReadOnlyList<BoardTile> path)
        {
            _rollPanel.MoveOut();
            await _announcement.Announce($"Player {player.Index+1} roll {path.Count}");
        }

        public async UniTask<Choice> ShowQuizPopup(Quiz quiz)
        {
            var prefab = _quizPopupsPrefabsMap[quiz.Type];

            Choice? selectedChoice = null;
            var popup = Instantiate(prefab, uiManager.MainTransform());
            popup.Setup(quiz, onChoiceSelected: choice => selectedChoice = choice);
            uiManager.OpenPopUp(popup);

            await UniTask.WaitUntil(() => selectedChoice != null);

            return selectedChoice;
        }

        public async UniTask ShowQuizResultPopup(Quiz quiz, bool answerWasCorrect)
        {
            var prefab = _quizResultPopupsPrefabsMap[quiz.Type];
            var popup = Instantiate(prefab, uiManager.MainTransform());
            bool resultClosed = false;
            popup.Setup(quiz, answerWasCorrect, onClosed: () => resultClosed = true);
            uiManager.OpenPopUp(popup);

            await UniTask.WaitUntil(() => resultClosed);
        }
    }
}