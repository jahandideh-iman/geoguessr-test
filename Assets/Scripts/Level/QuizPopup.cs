using Arman.UIManagement;
using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class QuizPopup : PopupWindow
    {
        [SerializeField] TextMeshProUGUI _questionText;
        [SerializeField] TextMeshProUGUI _timerText;
        [SerializeField] Image _questionImage;
        [SerializeField] ChoicePresenter[] _choicePresenters;

        Action<Choice> _onChoiceSelected;
        DateTime _endTime;

        public void Setup(Quiz quiz, DateTime endTime, Action<Choice> onChoiceSelected)
        {
            _endTime = endTime;
            _onChoiceSelected = onChoiceSelected;
            _questionText.text = quiz.Question;
            
            if (quiz.CustomImageID != null )
            {

            }
            else
            {
                _questionImage.gameObject.SetActive(false);
            }

            Debug.Assert(_choicePresenters.Length == quiz.Choices.Length);

            for (int i = 0; i < quiz.Choices.Length; i++)
            {
                var choice = quiz.Choices[i];
                _choicePresenters[i].Setup(choice, onSelected: () =>
                {
                    _onChoiceSelected.Invoke(choice); 
                    _onChoiceSelected = null;
                });
            }

        }

        public void Update()
        {
            int remainingSeconds = (int)(_endTime - DateTime.UtcNow).TotalSeconds;
            _timerText.text = remainingSeconds.ToString();
        }
    }
}