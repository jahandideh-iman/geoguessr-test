﻿using Arman.UIManagement;
using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class QuizResultPopup : PopupWindow
    {
        [SerializeField] TextMeshProUGUI _resultText;
        [SerializeField] TextMeshProUGUI _answerText;
        [SerializeField] Image _answerImage;

        Action _onClosed;

        public void Setup(Quiz quiz, bool wasCorrectAnswer, Action onClosed)
        {
            _onClosed = onClosed;
            _resultText.text = wasCorrectAnswer ? "Well Done!" : "You'll get it right next time!";

            _answerText.text = quiz.Answer.Text;

            if (quiz.Answer.ImageID != null)
            {
            }
            else
            {
                _answerImage.gameObject.SetActive(false);
            }
        }

        protected override void InternalOnDestroy()
        {
            base.InternalOnDestroy();
            _onClosed.Invoke();
        }

    }
}