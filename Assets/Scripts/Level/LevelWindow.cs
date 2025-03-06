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
        [SerializeField] PlayerScorePresenter _playerScorePresenter;
        [SerializeField] List<SerializableDictionaryKeyValue<QuizType, QuizPopup>> _quizPopupPrefabs;
        [SerializeField] List<SerializableDictionaryKeyValue<QuizType, QuizResultPopup>> _quizResultPopupPrefabs;

        private Dictionary<QuizType, QuizPopup> _quizPopupsPrefabsMap = null!;
        private Dictionary<QuizType, QuizResultPopup> _quizResultPopupsPrefabsMap = null!;

        private GameTransitionManager _gameTransitionManager;

        public void Setup(GameTransitionManager gameTransitionManager)
        {
            _gameTransitionManager = gameTransitionManager;
            _quizPopupsPrefabsMap = _quizPopupPrefabs.ToDictionary();
            _quizResultPopupsPrefabsMap = _quizResultPopupPrefabs.ToDictionary();

            _announcement.Setup();
            _rollPanel.Setup();
            _playerScorePresenter.Setup(0);
        }

        public async UniTask SetupPlayerTurn(Player player)
        {
            _playerScorePresenter.Setup(player.Score);
            await _announcement.Announce($"{player.Name}'s turn");
        }

        public async UniTask SetupPlayerMovement(Player player, IReadOnlyList<BoardTile> path)
        {
            _rollPanel.MoveOut();
            await _announcement.Announce($"{player.Name} rolled {path.Count}");
        }

        public QuizPopup ShowQuizPopup(Quiz quiz, DateTime endTime, bool enableUserChoice, Action<Choice>? onChoiceSelected)
        {
            var prefab = _quizPopupsPrefabsMap[quiz.Type];

            var popup = Instantiate(prefab, uiManager.MainTransform());
            popup.Setup(quiz, endTime, enableUserChoice,
                onChoiceSelected: choice =>
                {
                    onChoiceSelected?.Invoke(choice);
                });
            uiManager.OpenPopUp(popup);
            return popup;
        }

        public QuizResultPopup ShowQuizResultPopup(Quiz quiz, bool answerWasCorrect, bool enableUserInput, Action? onClosed = null)
        {
            var prefab = _quizResultPopupsPrefabsMap[quiz.Type];
            var popup = Instantiate(prefab, uiManager.MainTransform());
            popup.Setup(quiz, answerWasCorrect, enableUserInput, onClosed: onClosed);
            uiManager.OpenPopUp(popup);
            return popup;
        }

        public void ShowRollPanel(Action onRoll)
        {
            _rollPanel.MoveIn(onRoll);
        }

        public void UpdatePlayerScore(int score, int change)
        {
            _playerScorePresenter.UpdateScore(score, change);
        }

        public void ExitLevel()
        {
            _gameTransitionManager.GoToMainMenu();
        }
    }
}