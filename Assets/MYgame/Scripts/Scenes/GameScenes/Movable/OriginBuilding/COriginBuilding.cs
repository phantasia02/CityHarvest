using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LanKuDot.UnityToolBox;

public class COriginBuilding : CGameObjBas
{
    private readonly int _emissionColor = Shader.PropertyToID("_EmissionColor");

    public override EObjType ObjType() { return EObjType.eOriginBuilding; }

    // ==================== SerializeField ===========================================

    [SerializeField] protected StaticGlobalDel.EBrickColor m_MyBrickColor = StaticGlobalDel.EBrickColor.eBlue;
    public StaticGlobalDel.EBrickColor MyBrickColor => m_MyBrickColor;

    [SerializeField] protected GameObject m_MyCopyBrickObj = null;
    // ==================== SerializeField ===========================================

    protected Color _color;
    protected MaterialPropertyBlock _materialProperty;
    protected Renderer m_MyRendererMesh = null;
    protected Collider[] m_MyAllCollider = null;

    protected Material m_MyMaterial = null;

    protected override void Awake()
    {
        base.Awake();

        m_MyRendererMesh = this.GetComponent<Renderer>();
        m_MyMaterial = m_MyRendererMesh.material;

        m_MyRendererMesh.material.EnableKeyword("_EMISSION");
        _materialProperty = new MaterialPropertyBlock();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagPlayerRoll || other.tag == StaticGlobalDel.TagCompleteBuilding)
        {
           // Debug.Log($"other.tag = {other.tag}");

            GameObject lTempGameObject = GameObject.Instantiate(m_MyCopyBrickObj);

            lTempGameObject.transform.position = this.transform.position;
            lTempGameObject.transform.localScale = this.transform.localScale;
            lTempGameObject.transform.parent = m_MyGameManager.AllBrickObj;
            lTempGameObject.transform.rotation = this.transform.rotation;

            Transform lTempchildTransform = null;
            Renderer lTempTransformRenderer = null;
            CBrickObj lTempBrickObj = null;
            Rigidbody lTempRigidbody = null;
            float lTempTime = 1.0f;
           // Sequence lTempSequence;

            int lTempchildCount = lTempGameObject.transform.childCount;
            for (int i = 0; i < lTempchildCount; i++)
            {
                lTempchildTransform = lTempGameObject.transform.GetChild(i);
                lTempTransformRenderer = lTempchildTransform.GetComponent<Renderer>();
                lTempTransformRenderer.material = m_MyMaterial;
                lTempBrickObj = lTempTransformRenderer.gameObject.AddComponent<CBrickObj>();
                lTempBrickObj.MyBrickColor = MyBrickColor;

                lTempRigidbody = lTempchildTransform.GetComponent<Rigidbody>();

                lTempRigidbody.AddExplosionForce(500.0f, other.transform.position, 1000.0f);

                lTempBrickObj.BrickObjToPlayToStartCoroutine(lTempTime);
            }

            lTempGameObject.SetActive(true);
            this.gameObject.SetActive(false);

            if (other.tag == StaticGlobalDel.TagPlayerRoll)
            {
                Sequence lTempSequence = DOTween.Sequence();
                lTempSequence.AppendInterval(10.0f);
                lTempSequence.AppendCallback(() => { ShowCreate(); });
            }
        }

    }

    public void ShowCreate()
    {
        if (m_MyGameManager.CurState != CGameManager.EState.ePlay)
            return;

        this.gameObject.SetActive(true);

        TweenHDRColorEaseCurve lTempTweenHDRColorEaseCurve = m_MyGameManager.MyTargetBuilding.creatarchitecture;

        DOTween.To(
        () => _color, x => _color = x,
            lTempTweenHDRColorEaseCurve.endValue, lTempTweenHDRColorEaseCurve.duration)
        .SetEase(lTempTweenHDRColorEaseCurve.curve)
        .OnUpdate(UpdateMaterialColor);
    }

    private void UpdateMaterialColor()
    {
        _materialProperty.SetColor(_emissionColor, _color);
        m_MyRendererMesh.SetPropertyBlock(_materialProperty);
    }
}
