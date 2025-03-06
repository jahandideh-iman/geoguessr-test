using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{

    public class RollPanel : MonoBehaviour
    {
        [SerializeField] Vector2 _outOfScreenPositionOffset;
        [SerializeField] Button _button;

        Vector2 _initialPosition;
        Action? _onRoll;
        RectTransform _rectTransform;

        public void Setup()
        {
            _rectTransform = (transform as RectTransform);
            _initialPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _initialPosition + _outOfScreenPositionOffset;
            _button.interactable = false;

        }

        public void OnRoll()
        {
            _onRoll?.Invoke();
            _onRoll = null;
        }

        public void MoveIn(Action onRoll)
        {
            _onRoll = onRoll;
            _button.interactable = true;
            _rectTransform.DOAnchorPos(_initialPosition, 0.5f);
        }

        public void MoveOut()
        {
            _button.interactable = false;
            _rectTransform.DOAnchorPos(_initialPosition + _outOfScreenPositionOffset, 0.5f);
        }
    }
}