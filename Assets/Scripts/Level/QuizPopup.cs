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
        [SerializeField] TextMeshProUGUI questionText;
        [SerializeField] Image questionImage;
        [SerializeField] ChoicePresenter[] choicePresenters;

        Action<Choice> _onChoiceSelected;

        public void Setup(Quiz quiz, Action<Choice> onChoiceSelected)
        {
            _onChoiceSelected = onChoiceSelected;
            questionText.text = quiz.Question;
            
            if (quiz.CustomImageID != null )
            {

            }
            else
            {
                questionImage.gameObject.SetActive(false);
            }

            Debug.Assert(choicePresenters.Length == quiz.Choices.Length);

            for (int i = 0; i < quiz.Choices.Length; i++)
            {
                var choice = quiz.Choices[i];
                choicePresenters[i].Setup(choice, onSelected: () =>
                {
                    _onChoiceSelected.Invoke(choice); _onChoiceSelected = null;
                    Close();
                });
            }

        }
    }
}