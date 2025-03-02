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
        [SerializeField] TextMeshProUGUI text;

        public void Setup()
        {
            gameObject.SetActive(false);
        }

        public async UniTask Announce(string message)
        {
            gameObject.SetActive(true);
            text.text = message;
            transform.localPosition = Vector3.left * 300;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOLocalMove(Vector3.zero, 1));
            mySequence.AppendInterval(0.5f);
            mySequence.Append(transform.DOLocalMove(Vector3.right*300, 1));

            await mySequence.AsyncWaitForCompletion().AsUniTask();
            gameObject.SetActive(false);
        }
    }
}