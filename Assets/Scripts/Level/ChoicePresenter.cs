using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GeoGuessr.Configuration.QuizJsonConfiguration;

namespace GeoGuessr.Presentation
{
    public class ChoicePresenter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _choiceText;
        [SerializeField] Button _button;
        [SerializeField] Image _choiceImage;

        private Action? _onSelected;

        public void Setup(Choice choice, bool enableUserSelection, Action? onSelected)
        {
            if (enableUserSelection)
            {
                _onSelected = onSelected;
            }

            _button.interactable = enableUserSelection;

            if (choice.Text != null)
            {
                _choiceText.text = choice.Text;
            }
            else
            {
                _choiceText.gameObject.SetActive(false);
            }

            if (choice.ImageID != null)
            {
                _choiceImage.sprite = Resources.Load<Sprite>(choice.ImageID);
                _choiceImage.gameObject.SetActive(true);
            }
            else
            {
                _choiceImage.gameObject.SetActive(false);
            }

        }

        public void OnSelected()
        {
            _onSelected?.Invoke();
        }
    }
}