using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBrickObj : CGameObjBas
{
    public override EObjType ObjType() { return EObjType.eBrickObj; }
    // ==================== SerializeField ===========================================

    [SerializeField] protected StaticGlobalDel.EBrickColor m_MyBrickColor = StaticGlobalDel.EBrickColor.eBlue;
    public StaticGlobalDel.EBrickColor MyBrickColor => m_MyBrickColor;

    // ==================== SerializeField ===========================================



}
