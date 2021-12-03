using LanKuDot.UnityToolBox;
using UnityEngine;

namespace MYgame.Scripts.Scenes.Building
{
    /// <summary>
    /// The progress of the constructed building
    /// </summary>
    public class BuildingProgress : MonoBehaviour
    {
        [SerializeField]
        private BuildingProgressUnit[] _bulidingPieces;
        [SerializeField]
        private BuildingProgressUnit _completedPiece;
        [SerializeField]
        private TweenHDRColorEaseCurve _showUpEmissionCurve;

        private const float _progressStep = 0.25f;
        private const float _progressComplete = 1f;

        private int _currentStep = 0;

        private void Awake()
        {
            foreach (var piece in _bulidingPieces)
                piece.Inactivate();

            _completedPiece.Inactivate();
        }

        /// <summary>
        /// Update the building progress
        /// </summary>
        /// <param name="progress">The progress of the building</param>
        public void UpdateProgress(float progress)
        {
            progress = Mathf.Min(progress, _progressComplete);

            var step = (int)(progress / _progressStep);

            if (step <= _currentStep)
                return;

            if (progress >= _progressComplete) {
                _currentStep = 100;
                ShowCompletedPiece();
                return;
            }

            for (; _currentStep < step; ++_currentStep)
                ShowPiece(_bulidingPieces[_currentStep]);
        }

        /// <summary>
        /// Show the pieces
        /// </summary>
        /// <param name="targetPiece">The target piece to be shown</param>
        private void ShowPiece(BuildingProgressUnit targetPiece)
        {
            targetPiece.Activate(_showUpEmissionCurve);
        }

        /// <summary>
        /// Show the completed piece
        /// </summary>
        private void ShowCompletedPiece()
        {
            foreach (var piece in _bulidingPieces)
                piece.Inactivate();

            _completedPiece.Activate(_showUpEmissionCurve);
        }
    }
}
