using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CHitStatePlayer : CPlayerStateBase
{
    public override EMovableState StateType() { return EMovableState.eHit; }
    public override int Priority => 2;
    protected Vector3 m_HitDir = Vector3.one;

    public CHitStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eIdle);
        m_HitDir = m_MyPlayerMemoryShare.m_MyMovable.transform.position - m_MyPlayerMemoryShare.m_HitWaterPoint;
        m_HitDir.y = 0.0f;
        m_HitDir.Normalize();

        m_MyPlayerMemoryShare.m_MyPlayer.SetMoveBuff(CMovableBase.ESpeedBuff.eHit, 2.0f, true);

        m_MyPlayerMemoryShare.m_AllObj.transform.DOShakePosition(2.3f, 1.0f, 10, 90);
    }

    protected override void updataState()
    {
        if (m_MyGameManager.CurState != CGameManager.EState.ePlay)
            return;

        if (m_StateTime < 0.5f)
        {
            UpdateSpeed();
            m_MyPlayerMemoryShare.m_MyRigidbody.position += m_HitDir * m_MyPlayerMemoryShare.m_MyPlayer.TotleSpeed * 0.01f * Time.deltaTime;
        }

        if (MomentinTime(0.5f))
        {
            m_MyPlayerMemoryShare.m_MyPlayer.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eHit);
        }
        else if (MomentinTime(2.0f))
        {
            m_MyPlayerMemoryShare.m_MyPlayer.SetChangState(EMovableState.eWait);
        } 
    }

    protected override void OutState()
    {
        m_HitDir = Vector3.one;
        m_MyPlayerMemoryShare.m_HitWaterPoint = Vector3.zero;
        
    }
}
