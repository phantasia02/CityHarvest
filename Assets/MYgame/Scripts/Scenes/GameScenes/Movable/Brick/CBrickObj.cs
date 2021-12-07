using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CBrickObj : CGameObjBas
{
    public override EObjType ObjType() { return EObjType.eBrickObj; }
    // ==================== SerializeField ===========================================

    [SerializeField] protected StaticGlobalDel.EBrickColor m_MyBrickColor = StaticGlobalDel.EBrickColor.eBlue;
    public StaticGlobalDel.EBrickColor MyBrickColor
    {
        set => m_MyBrickColor = value;
        get => m_MyBrickColor;
    }
    // ==================== SerializeField ===========================================

    protected Collider[] m_AllMyCollider = null;


    protected override void Start()
    {
        base.Start();
        m_AllMyCollider = this.GetComponentsInChildren<Collider>();

    }

    public void OpenCollider(bool open)
    {
        for (int i = 0; i < m_AllMyCollider.Length; i++)
            m_AllMyCollider[i].enabled = open;
    }

    public void BrickObjToPlayToStartCoroutine(float delaySecond)
    {
        StartCoroutine(BrickObjToPlay(delaySecond));
    }

    public IEnumerator BrickObjToPlay(float delaySecond)
    {
        yield return new WaitForSeconds(delaySecond);

        Rigidbody lTempRigidbody = this.GetComponent<Rigidbody>();
        if (lTempRigidbody != null)
        {
            lTempRigidbody.useGravity = false;
          //  lTempRigidbody.isKinematic = true;
        }
        OpenCollider(false);

        Vector3 lTempOldV3 = this.transform.position;
        Transform lTempTargetTransform = m_MyGameManager.Player.RecycleBrickObj;
        Vector3 lTempTopPos = Vector3.Lerp(this.transform.position, lTempTargetTransform.position, 0.3f);
        lTempTopPos.y = 40.0f;
        // this.transform.parent = lTempTargetTransform;

        const float CsDoTweenTime = 1.0f;
        Tween lTempTween1 = this.transform.DOScale(Vector3.zero, 1.5f);

        float lTempRatio = 0.0f;
        Tween lTempTween = DOTween.To(() => lTempRatio, x => lTempRatio = x, 1.0f, CsDoTweenTime);

        lTempTween.SetEase( Ease.OutSine);
        lTempTween.OnUpdate(()=>
        {
            Vector3 lTempCurve1 = Vector3.Lerp(lTempOldV3, lTempTopPos, lTempRatio);
            Vector3 lTempCurve2 = Vector3.Lerp(lTempTopPos, lTempTargetTransform.position, lTempRatio);
            this.transform.position = Vector3.Lerp(lTempCurve1, lTempCurve2, lTempRatio);
        });

        lTempTween.onComplete = () =>
        {
            lTempTween1.Kill();
            m_MyGameManager.Player.AddBrickColor(MyBrickColor, 1);
            Destroy(this.gameObject);
        };
    }
}
