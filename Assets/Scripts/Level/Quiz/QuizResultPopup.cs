#nullable enable
using Arman.UIManagement;
using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class QuizResultPopup : PopupWindow
    {
        [SerializeField] TextMeshProUGUI _resultText = null!;
        [SerializeField] TextMeshProUGUI _answerText = null!;
        [SerializeField] Image _answerImage = null!;
        [SerializeField] Button _closeButton = null!;

        Action? _onClosed;

        public void Setup(Quiz quiz, bool wasCorrectAnswer, bool enableUserInput, Action? onClosed)
        {
            _onClosed = onClosed;
            _resultText.text = wasCorrectAnswer ? "Well Done!" : "You'll get it right next time!";
            _closeButton.interactable = enableUserInput;

            _answerText.text = quiz.Answer.Text;

            if (quiz.Answer.ImageID != null)
            {
                _answerImage.sprite = Resources.Load<Sprite>(quiz.Answer.ImageID);
                _answerImage.gameObject.SetActive(true);
            }
            else
            {
                _answerImage.gameObject.SetActive(false);
            }
        }

        protected override void InternalOnDestroy()
        {
            base.InternalOnDestroy();
            _onClosed?.Invoke();
        }

    }
}