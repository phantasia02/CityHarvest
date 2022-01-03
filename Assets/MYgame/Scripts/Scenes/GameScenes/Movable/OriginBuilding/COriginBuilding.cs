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
    public StaticGlobalDel.EBrickColor MyBrickColor{get { return m_MyBrickColor; }}

    [SerializeField] protected GameObject m_MyCopyBrickObj = null;
    // ==================== SerializeField ===========================================

    protected Color _color;
    protected MaterialPropertyBlock _materialProperty;
    protected Renderer m_MyRendererMesh = null;
    protected BoxCollider m_MyBoxCollider = null;
    protected Vector3 m_MyBoxColliderSizehalf = Vector3.one;

    protected Material m_MyMaterial = null;

    protected override void Awake()
    {
        base.Awake();

        m_MyRendererMesh = this.GetComponent<Renderer>();
        m_MyMaterial = m_MyRendererMesh.material;
        m_MyBoxCollider = this.GetComponent<BoxCollider>();
        m_MyBoxColliderSizehalf = m_MyBoxCollider.size;
        m_MyBoxColliderSizehalf.x *= this.transform.localScale.x;
        m_MyBoxColliderSizehalf.y *= this.transform.localScale.y;
        m_MyBoxColliderSizehalf.z *= this.transform.localScale.z;
        m_MyBoxColliderSizehalf *= 0.5f;

        m_MyRendererMesh.material.EnableKeyword("_EMISSION");
        _materialProperty = new MaterialPropertyBlock();
    }

    public void SetDateBrick(CDateBrick lTempDateBrick)
    {
        m_MyRendererMesh = this.GetComponent<Renderer>();
        m_MyMaterial = m_MyRendererMesh.material = lTempDateBrick.m_ColorMat;
        m_MyBrickColor = lTempDateBrick.m_Type;
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
           // BoxCollider lTempBoxCollider = null;
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

                lTempchildTransform.gameObject.tag = StaticGlobalDel.TagBrickObj;

                //lTempBoxCollider = lTempchildTransform.gameObject.AddComponent<BoxCollider>();
                //lTempBoxCollider.isTrigger = true;

                lTempRigidbody = lTempchildTransform.gameObject.GetComponent<Rigidbody>();
                lTempRigidbody.AddExplosionForce(1000.0f, other.transform.position, 1000.0f);

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

        Collider[] lTempCollider = Physics.OverlapBox(this.transform.position + m_MyBoxCollider.center, m_MyBoxColliderSizehalf, this.transform.rotation, StaticGlobalDel.g_CompleteBuildingMask | StaticGlobalDel.g_PlayerMask);
        if (lTempCollider.Length != 0)
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
