using DG.Tweening;
using LanKuDot.UnityToolBox;
using UnityEngine;

namespace MYgame.Scripts.Effect
{
    public class ShineEffect : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] _renderers;
        [SerializeField]
        [ColorUsage(true, true)]
        private Color _startColor;
        [SerializeField]
        private TweenHDRColorEaseCurve _shineCurve;

        private MaterialPropertyBlock _materialProperty;
        private readonly int _emissionColor = Shader.PropertyToID("_EmissionColor");
        private Color _color;

        private void Awake()
        {
            _materialProperty ??= new MaterialPropertyBlock();
            foreach (var renderer in _renderers)
                renderer.material.EnableKeyword("_EMISSION");
        }

        public void Shine()
        {
            _color = _startColor;
            DOTween.To(
                    () => _color, x => _color = x,
                    _shineCurve.endValue, _shineCurve.duration)
                .SetEase(_shineCurve.curve)
                .OnUpdate(UpdateMaterialColor);
        }

        private void UpdateMaterialColor()
        {
            _materialProperty.SetColor(_emissionColor, _color);
            foreach (var renderer in _renderers)
                renderer.SetPropertyBlock(_materialProperty);
        }
    }
}
