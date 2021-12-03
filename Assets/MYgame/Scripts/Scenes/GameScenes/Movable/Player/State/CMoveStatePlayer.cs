using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMoveStatePlayer : CPlayerStateBase
{
    public override StaticGlobalDel.EMovableState StateType() { return StaticGlobalDel.EMovableState.eMove; }

    public CMoveStatePlayer(CMovableBase pamMovableBase) : base(pamMovableBase)
    {

    }

    protected override void InState()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.AnimatorStateCtl.SetCurState(CAnimatorStateCtl.EState.eRun);
    }

    protected override void updataState()
    {

        m_MyPlayerMemoryShare.m_MyTransform.Translate(new Vector3(0.0f, 0.0f, Time.deltaTime * m_MyPlayerMemoryShare.m_MyMovable.TotleSpeed));
    }

    protected override void OutState()
    {

    }

    public override void MouseDown() { }
    public override void MouseDrag()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.UpdateDrag();
    }
    public override void MouseUp()
    {
        m_MyPlayerMemoryShare.m_MyPlayer.ChangState = StaticGlobalDel.EMovableState.eWait;
    }
}
