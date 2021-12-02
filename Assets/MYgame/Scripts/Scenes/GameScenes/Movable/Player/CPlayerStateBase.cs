using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagBrickObj)
        {
            CBrickObj lTempBrickObj = other.GetComponent<CBrickObj>();

            lTempBrickObj.gameObject.SetActive(false);
            m_MyPlayerMemoryShare.m_MyPlayer.AddBrickColor(lTempBrickObj.MyBrickColor, 1);

            Debug.Log($"===========================================");
            for (int i = 0; i < m_MyPlayerMemoryShare.m_CurBrickAmount.Length; i++)
                Debug.Log($"m_MyPlayerMemoryShare.m_CurBrickAmount[i].amount = {m_MyPlayerMemoryShare.m_CurBrickAmount[i].amount}");
        }

      //  base.OnTriggerEnter(other);
    }
}
