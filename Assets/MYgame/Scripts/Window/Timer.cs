using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace MYgame.Scripts.Window
{
    /// <summary>
    /// The timer
    /// </summary>
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private int _totalTime = 60;
        [SerializeField]
        private int _warningTime = 10;
        [SerializeField]
        private TextMeshProUGUI _timeText;

        private Action _onTimesUp;
        private float _timeRemaining;
        private bool _isWarning;
        private const float _timeStep = 0.2f;

        public void StartTimer(Action onTimesUp)
        {
            _onTimesUp = onTimesUp;
            _timeRemaining = _totalTime;
            DOTween.Sequence()
                .AppendInterval(_timeStep)
                .AppendCallback(UpdateTimer)
                .SetLoops(-1)
                .SetId(this);
        }

        public void StopTimer()
        {
            DOTween.Kill(this);
        }

        private void UpdateTimer()
        {
            _timeRemaining -= _timeStep;

            if (_timeRemaining < _warningTime && !_isWarning) {
                _isWarning = true;
                _timeText.color = Color.red;
            }

            _timeText.text = Mathf.CeilToInt(_timeRemaining).ToString();

            if (_timeRemaining > 0)
                return;

            _onTimesUp.Invoke();
            DOTween.Kill(this);
        }
    }
}
