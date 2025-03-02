using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class ChoicePresenter : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI choiceText;
        [SerializeField] Image choiceImage;

        private Action _onSelected;

        public void Setup(Choice choice, Action onSelected)
        {
            _onSelected = onSelected;

            if(choice.Text != null)
            {
                choiceText.text = choice.Text;
            }
            else
            {
                choiceText.gameObject.SetActive(false);
            }

            if(choice.ImageID != null)
            {
                
            }
            else
            {
                choiceImage.gameObject.SetActive(false);
            }    
            
        }

        public void OnSelected()
        {
            _onSelected.Invoke();
        }
    }
}