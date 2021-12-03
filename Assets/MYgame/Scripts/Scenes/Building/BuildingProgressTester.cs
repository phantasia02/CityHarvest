using DG.Tweening;
using UnityEngine;

namespace MYgame.Scripts.Scenes.Building
{
    public class BuildingProgressTester : MonoBehaviour
    {
        [SerializeField]
        private BuildingProgress _buildingProgress;

        private float _currentProgress;

        private void Start()
        {
            DOTween.Sequence()
                .AppendInterval(0.2f)
                .AppendCallback(UpdateProgress)
                .SetLoops(-1);
        }

        private void UpdateProgress()
        {
            _currentProgress += 0.05f;
            _buildingProgress.UpdateProgress(_currentProgress);
        }
    }
}
