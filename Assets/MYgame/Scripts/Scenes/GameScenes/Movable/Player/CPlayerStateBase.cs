using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MYgame.Scripts.Scenes.GameScenes.Data;

public abstract class CPlayerStateBase : CStateActor
{
    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;


    public CPlayerStateBase(CMovableBase pamMovableBase) : base(pamMovableBase)
    {
        m_MyPlayerMemoryShare = (CPlayerMemoryShare)m_MyMemoryShare;
    }

    protected override void InState()
    {
    }


    public override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == (int)StaticGlobalDel.ELayerIndex.eWater)
            SetHitWater(collision.contacts[0].point);
       // SetHitWater(collision.contacts[0].point);
    }


    public void SetHitWater(Vector3 hitpoint)
    {
        m_MyPlayerMemoryShare.m_HitWaterPoint = hitpoint;
        m_MyPlayerMemoryShare.m_MyPlayer.LockChangState = StaticGlobalDel.EMovableState.eHit;
        m_MyPlayerMemoryShare.m_MyPlayer.ChangState = StaticGlobalDel.EMovableState.eHit;
    }

    public void UpdateSpeed()
    {
        if (m_MyPlayerMemoryShare.m_TotleSpeed.Value != m_MyPlayerMemoryShare.m_TargetTotleSpeed)
        {
            m_MyMemoryShare.m_TotleSpeed.Value = Mathf.MoveTowards(m_MyPlayerMemoryShare.m_TotleSpeed.Value, m_MyPlayerMemoryShare.m_TargetTotleSpeed, m_MyPlayerMemoryShare.m_AddSpeedSecond * Time.deltaTime);

            if (Mathf.Abs(m_MyPlayerMemoryShare.m_TotleSpeed.Value - m_MyPlayerMemoryShare.m_TargetTotleSpeed) < 0.1f)
                m_MyMemoryShare.m_TotleSpeed.Value = m_MyMemoryShare.m_TargetTotleSpeed;
        }
    }
}
