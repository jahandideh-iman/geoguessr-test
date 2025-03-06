#nullable enable
using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class ChoicePresenter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _choiceText = null!;
        [SerializeField] Button _button = null!;
        [SerializeField] Image _choiceImage = null!;

        private Action? _onSelected = null;

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