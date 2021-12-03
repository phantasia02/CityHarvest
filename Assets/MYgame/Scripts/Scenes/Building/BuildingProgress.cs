using UnityEngine;

namespace MYgame.Scripts.Scenes.Building
{
    /// <summary>
    /// The progress of the constructed building
    /// </summary>
    public class BuildingProgress : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _bulidingPieces;
        [SerializeField]
        private GameObject _completedPiece;

        private const float _progressStep = 0.25f;
        private const float _progressComplete = 1f;

        private int _currentStep = 0;

        private void Awake()
        {
            foreach (var piece in _bulidingPieces)
                piece.SetActive(false);

            _completedPiece.SetActive(false);
        }

        /// <summary>
        /// Update the building progress
        /// </summary>
        /// <param name="progress">The progress of the building</param>
        public void UpdateProgress(float progress)
        {
            progress = Mathf.Min(progress, _progressComplete);

            var step = (int)(progress / _progressStep) - 1;

            if (step < _currentStep)
                return;

            ++_currentStep;

            if (progress >= _progressComplete) {
                ShowCompletedPiece();
                return;
            }

            ShowPiece(_bulidingPieces[step]);
        }

        /// <summary>
        /// Show the pieces
        /// </summary>
        /// <param name="targetPiece">The target piece to be shown</param>
        private void ShowPiece(GameObject targetPiece)
        {
            targetPiece.SetActive(true);
        }

        /// <summary>
        /// Show the completed piece
        /// </summary>
        private void ShowCompletedPiece()
        {
            foreach (var piece in _bulidingPieces)
                piece.SetActive(false);

            _completedPiece.SetActive(true);
        }
    }
}
