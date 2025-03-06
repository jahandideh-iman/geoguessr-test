using Cysharp.Threading.Tasks;
using DG.Tweening;
using GeoGuessr.Game;
using System;
using TMPro;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class Announcement : MonoBehaviour
    {
        [SerializeField] int _outOfScreenOffset;
        [SerializeField] TextMeshProUGUI _text;

        Vector2 _initialPosition;
        RectTransform _rectTransform;

        public void Setup()
        {
            _rectTransform = (transform as RectTransform);
            _initialPosition = _rectTransform.anchoredPosition;
            _rectTransform.anchoredPosition = _initialPosition + (Vector2.left * _outOfScreenOffset);
            gameObject.SetActive(false);
        }

        public async UniTask Announce(string message)
        {
            gameObject.SetActive(true);
            _text.text = message;
            _rectTransform.anchoredPosition = _initialPosition + (Vector2.left * _outOfScreenOffset);
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(_rectTransform.DOAnchorPos(_initialPosition, 1));
            mySequence.AppendInterval(0.5f);
            mySequence.Append(_rectTransform.DOAnchorPos(_initialPosition + (Vector2.right * _outOfScreenOffset), 1));

            await mySequence.AsyncWaitForCompletion().AsUniTask();
            gameObject.SetActive(false);
        }
    }
}