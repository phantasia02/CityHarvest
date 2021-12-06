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

        OpenCollider(false);

        Transform lTempTargetTransform = m_MyGameManager.Player.RecycleBrickObj;
        this.transform.parent = lTempTargetTransform;

        const float CsDoTweenTime = 1.0f;
        Tween lTempTween = this.transform.DOLocalJump(Vector3.zero, 0.1f, 1, CsDoTweenTime);
        Tween lTempTween1 = this.transform.DOScale(Vector3.zero, 1.5f);

        lTempTween.onComplete = () =>
        {
            lTempTween1.Kill();
            m_MyGameManager.Player.AddBrickColor(MyBrickColor, 1);
            Destroy(this.gameObject);
        };
    }
}
