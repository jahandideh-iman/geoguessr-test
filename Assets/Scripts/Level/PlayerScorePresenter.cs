using TMPro;
using UnityEngine;

namespace GeoGuessr.Presentation
{
    public class PlayerScorePresenter : MonoBehaviour
    {
        [SerializeField] int scoreChangeRate;
        [SerializeField] TextMeshProUGUI _scoreText;

        int _targetScore = 0;
        int _currentScore = 0;
        public void Setup(int score)
        {
            _currentScore = score;
            _targetScore = score;
            _scoreText.text = _currentScore.ToString();
        }

        public void UpdateScore(int score, int change)
        {
            _targetScore = score;
            // TODO: Spawn a object for the score change
        }

        public void Update()
        {
            if(_currentScore < _targetScore)
            {
                _currentScore = (int) Mathf.Min(_targetScore, _currentScore + Time.deltaTime *  scoreChangeRate);
                _scoreText.text = _currentScore.ToString();
            }
        }
    }
}