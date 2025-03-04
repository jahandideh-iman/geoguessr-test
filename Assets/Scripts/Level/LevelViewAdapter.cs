using Cysharp.Threading.Tasks;
using GeoGuessr.Game;
using System;
using System.Collections.Generic;

namespace GeoGuessr.Presentation
{

    public class LevelViewAdapter : ILevelViewPort, ILocalPlayerControllerViewPort, IAiPlayerControllerViewPort
    {
        private readonly LevelWindow _levelWindow;
        private readonly LevelPresenter _levelPresenter;
        private readonly FollowCamera _followCamera;

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
            var playerPresenter = _levelPresenter.GetPlayerPresenter(player);

            _followCamera.SetTarget(playerPresenter.transform);
            await _levelWindow.SetupPlayerTurn(player);
            _followCamera.ClearTarget();
        }

        public async UniTask MovePlayer(Player player, IReadOnlyList<BoardTile> path)
        {
            var playerPresenter = _levelPresenter.GetPlayerPresenter(player);

            _followCamera.SetTarget(playerPresenter.transform);
            await _levelWindow.SetupPlayerMovement(player, path);

            foreach (var tile in path)
            {
                var tilePresenter = _levelPresenter.BoardPresenter.GetTilePresenter(tile);
                await playerPresenter.MoveTo(tilePresenter);
            }

            _followCamera.ClearTarget();
        }


        public void CloseQuiz()
        {
            _levelWindow.CloseQuizPopup();
        }

        public async UniTask ShowQuizResult(Quiz quiz, bool answerWasCorrect)
        {
            bool resultIsClosed = false;
            _levelWindow.ShowQuizResultPopup(quiz, answerWasCorrect, onClosed: () => resultIsClosed = true);
            await UniTask.WaitUntil(() => resultIsClosed);
            return;
        }

        public async UniTask ShowRollingOption()
        {
            bool playerRolled = false;
            _levelWindow.ShowRollPanel(onRoll: () => playerRolled = true);

            await UniTask.WaitUntil(() => playerRolled);
        }

        void IAiPlayerControllerViewPort.ShowQuiz(Quiz quiz, DateTime endTime)
        {
            _levelWindow.ShowQuizPopup(quiz, endTime, enableUserChoice: false, onChoiceSelected: null);
        }

        async UniTask<Choice> ILocalPlayerControllerViewPort.ShowQuiz(Quiz quiz, DateTime endTime)
        {
            Choice? selectedChoice = null;
            _levelWindow.ShowQuizPopup(quiz, endTime, enableUserChoice: true, onChoiceSelected: choice => selectedChoice = choice);
            await UniTask.WaitUntil(() => selectedChoice != null);
            return selectedChoice;
        }
    }
}