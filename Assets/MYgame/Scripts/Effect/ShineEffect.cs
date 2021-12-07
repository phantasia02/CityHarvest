using DG.Tweening;
using LanKuDot.UnityToolBox;
using UnityEngine;

namespace MYgame.Scripts.Effect
{
    public class ShineEffect : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;
        [SerializeField]
        [ColorUsage(true, true)]
        private Color _startColor;
        [SerializeField]
        private TweenHDRColorEaseCurve _shineCurve;

        private MaterialPropertyBlock _materialProperty;
        private readonly int _emissionColor = Shader.PropertyToID("_EmissionColor");
        private Color _color;

        private void OnEnable()
        {
            _materialProperty ??= new MaterialPropertyBlock();
            _renderer.material.EnableKeyword("_EMISSION");
            _color = _startColor;
            DOTween.To(
                    () => _color, x => _color = x,
                    _shineCurve.endValue, _shineCurve.duration)
                .SetEase(_shineCurve.curve)
                .OnUpdate(UpdateMaterialColor)
                .OnComplete(() => enabled = false);
        }

        private void UpdateMaterialColor()
        {
            _materialProperty.SetColor(_emissionColor, _color);
            _renderer.SetPropertyBlock(_materialProperty);
        }
    }
}
