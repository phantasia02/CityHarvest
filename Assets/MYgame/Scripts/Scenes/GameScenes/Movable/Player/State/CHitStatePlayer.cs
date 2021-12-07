using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CHitStatePlayer : CPlayerStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eHit; }

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
    }

    protected override void updataState()
    {
        if (m_StateTime < 0.5f)
        {
            UpdateSpeed();
            m_MyPlayerMemoryShare.m_MyRigidbody.position += m_HitDir * m_MyPlayerMemoryShare.m_MyPlayer.TotleSpeed * 0.01f * Time.fixedDeltaTime;
        }

        if (MomentinTime(0.5f))
        {
            m_MyPlayerMemoryShare.m_MyPlayer.LockChangState = StaticGlobalDel.EMovableState.eMax;
            m_MyPlayerMemoryShare.m_MyPlayer.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eHit);
        }
        else if (MomentinTime(2.0f))
        {
            
            m_MyPlayerMemoryShare.m_MyPlayer.ChangState = StaticGlobalDel.EMovableState.eWait;
        } 
    }

    protected override void OutState()
    {
        m_HitDir = Vector3.one;
        m_MyPlayerMemoryShare.m_HitWaterPoint = Vector3.zero;
        
    }
}
