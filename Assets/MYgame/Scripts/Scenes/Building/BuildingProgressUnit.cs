using DG.Tweening;
using LanKuDot.UnityToolBox;
using UnityEngine;

namespace MYgame.Scripts.Scenes.Building
{
    /// <summary>
    /// The unit of the single building progress
    /// </summary>
    public class BuildingProgressUnit : MonoBehaviour
    {
        private Renderer _renderer;
        private MaterialPropertyBlock _materialProperty;
        private readonly int _emissionColor = Shader.PropertyToID("_EmissionColor");
        private Color _color;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _renderer.material.EnableKeyword("_EMISSION");
            _materialProperty = new MaterialPropertyBlock();
            _color = Color.black;
        }

        public void Activate(TweenHDRColorEaseCurve easeCurve)
        {
            gameObject.SetActive(true);
            DOTween.To(
                    () => _color, x => _color = x,
                    easeCurve.endValue, easeCurve.duration)
                .SetEase(easeCurve.curve)
                .OnUpdate(UpdateMaterialColor);
        }

        public void Inactivate()
        {
            gameObject.SetActive(false);
        }

        private void UpdateMaterialColor()
        {
            _materialProperty.SetColor(_emissionColor, _color);
            _renderer.SetPropertyBlock(_materialProperty);
        }
    }
}
