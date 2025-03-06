using Cysharp.Threading.Tasks;
using GeoGuessr.Game;
using System;
using System.Collections.Generic;

namespace GeoGuessr.Presentation
{
    public class LevelViewAdapter : ILevelViewPort, IPlayerControllerViewPort
    {
        private readonly LevelWindow _levelWindow;
        private readonly LevelPresenter _levelPresenter;
        private readonly FollowCamera _followCamera;

        private QuizPopup? _currentQuizPopup = null;

        public LevelViewAdapter(
            LevelWindow levelWindow,
            LevelPresenter levelPresenter,
            FollowCamera followCamera)
        {
            _levelWindow = levelWindow;
            _levelPresenter = levelPresenter;
            _followCamera = followCamera;
        }
        public async UniTask StartTurn(Player player)
        {
            await UniTask.WaitForSeconds(1);
            var playerPresenter = _levelPresenter.GetPlayerPresenter(player);

            _followCamera.SetTarget(playerPresenter.transform);
            await _levelWindow.SetupPlayerTurn(player);
            _followCamera.ClearTarget();
        }

        public IEnumerable<UniTask<BoardTile>> MovePlayer(Player player, IReadOnlyList<BoardTile> path)
        {
            var playerPresenter = _levelPresenter.GetPlayerPresenter(player);

            _followCamera.SetTarget(playerPresenter.transform);

            for (int i = 0; i < path.Count; i++)
            {
                int indexCapture = i;
                yield return MovePlayerTo(indexCapture);
            }

            async UniTask<BoardTile> MovePlayerTo(int tileIndex)
            {
                var tile = path[tileIndex];
                if(tileIndex == 0)
                {
                    await _levelWindow.SetupPlayerMovement(player, path);
                }

                var tilePresenter = _levelPresenter.BoardPresenter.GetTilePresenter(tile);
                await playerPresenter.MoveTo(tilePresenter);

                if(tileIndex == path.Count - 1)
                {
                    _followCamera.ClearTarget();
                }
                return tile;
            }
        }

        public void UpdatePlayerScore(int score, int change)
        {
            _levelWindow.UpdatePlayerScore(score, change);
        }

        public void CloseQuiz()
        {
            _currentQuizPopup.Close();
            _currentQuizPopup = null;
        }

        public async UniTask ShowRollingOption()
        {
            bool playerRolled = false;
            _levelWindow.ShowRollPanel(onRoll: () => playerRolled = true);

            await UniTask.WaitUntil(() => playerRolled);
        }

        public async UniTask ShowAiQuiz(Quiz quiz, DateTime endTime)
        {
            _currentQuizPopup = _levelWindow.ShowQuizPopup(quiz, endTime, enableUserChoice: false, onChoiceSelected: null);
            await UniTask.WaitForSeconds(2);
            _currentQuizPopup?.Close();
        }

        public async UniTask ShowAiQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            var popup = _levelWindow.ShowQuizResultPopup(quiz, answerWasCorrect, enableUserInput: false);
            await UniTask.WaitForSeconds(2);
            popup.Close();
        }

        public async UniTask<Choice> ShowPlayerQuiz(Quiz quiz, DateTime endTime)
        {
            Choice? selectedChoice = null;
            _currentQuizPopup = _levelWindow.ShowQuizPopup(quiz, endTime, enableUserChoice: true, onChoiceSelected: choice => selectedChoice = choice);
            await UniTask.WaitUntil(() => selectedChoice != null);
            _currentQuizPopup?.Close();
            return selectedChoice;
        }

        public async UniTask ShowPlayerQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            bool resultIsClosed = false;
            _levelWindow.ShowQuizResultPopup(quiz, answerWasCorrect, enableUserInput: true, onClosed: () => resultIsClosed = true);
            await UniTask.WaitUntil(() => resultIsClosed);
            return;
        }

    }
}