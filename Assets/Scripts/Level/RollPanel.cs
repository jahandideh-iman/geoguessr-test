using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GeoGuessr.Presentation
{
    public class RollPanel : MonoBehaviour
    {
        [SerializeField] Button button;


        Vector2 initialPosition;
        Vector2 outOfScreenPositionOffset = new Vector2 (200, -200);

        Action? _onRoll;

        public void Setup()
        {
            initialPosition = transform.localPosition;
            transform.localPosition = initialPosition + outOfScreenPositionOffset;
            button.interactable = false;

        }

        public void OnRoll()
        {
            _onRoll?.Invoke();
            _onRoll = null;
        }

        public void MoveIn(Action onRoll)
        {
            _onRoll = onRoll;
            button.interactable = true;
            transform.DOLocalMove(initialPosition, 0.5f);
        }

        public void MoveOut()
        {
            button.interactable = false;
            transform.DOLocalMove(initialPosition + outOfScreenPositionOffset, 0.5f);
        }
    }
}