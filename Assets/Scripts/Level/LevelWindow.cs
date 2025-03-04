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

        private QuizPopup? _currentQuizPopup = null;

        public void Setup(LevelController levelController)
        {
            _quizPopupsPrefabsMap = _quizPopupPrefabs.ToDictionary();
            _quizResultPopupsPrefabsMap = _quizResultPopupPrefabs.ToDictionary();

            _announcement.Setup();
            _rollPanel.Setup();
        }

        public async UniTask SetupPlayerTurn(Player player)
        {
            await _announcement.Announce($"{player.Name}'s turn");
        }

        public async UniTask SetupPlayerMovement(Player player, IReadOnlyList<BoardTile> path)
        {
            _rollPanel.MoveOut();
            await _announcement.Announce($"Player {player.Index + 1} roll {path.Count}");
        }

        public void ShowQuizPopup(Quiz quiz, DateTime endTime, bool enableUserChoice, Action<Choice>? onChoiceSelected)
        {
            var prefab = _quizPopupsPrefabsMap[quiz.Type];

            _currentQuizPopup = Instantiate(prefab, uiManager.MainTransform());
            _currentQuizPopup.Setup(quiz, endTime, enableUserChoice,
                onChoiceSelected: choice =>
                {

                    onChoiceSelected?.Invoke(choice);
                    _currentQuizPopup.Close();
                });
            uiManager.OpenPopUp(_currentQuizPopup);


        }

        public void ShowQuizResultPopup(Quiz quiz, bool answerWasCorrect, Action onClosed)
        {
            var prefab = _quizResultPopupsPrefabsMap[quiz.Type];
            var popup = Instantiate(prefab, uiManager.MainTransform());
            popup.Setup(quiz, answerWasCorrect, onClosed: onClosed);
            uiManager.OpenPopUp(popup);
        }

        public void CloseQuizPopup()
        {
            if (_currentQuizPopup != null)
            {
                _currentQuizPopup.Close();
            }
        }

        public void ShowRollPanel(Action onRoll)
        {
            _rollPanel.MoveIn(onRoll);
        }
    }
}