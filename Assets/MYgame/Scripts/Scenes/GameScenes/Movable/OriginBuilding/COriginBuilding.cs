using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COriginBuilding : CGameObjBas
{
    public override EObjType ObjType() { return EObjType.eOriginBuilding; }

    // ==================== SerializeField ===========================================

    [SerializeField] protected StaticGlobalDel.EBrickColor m_MyBrickColor = StaticGlobalDel.EBrickColor.eBlue;
    public StaticGlobalDel.EBrickColor MyBrickColor => m_MyBrickColor;

    // ==================== SerializeField ===========================================
    protected Transform m_AllBrickObj = null;


    protected override void Awake()
    {
        base.Awake();
        m_AllBrickObj = this.transform.GetChild(0);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagPlayerRoll)
        {
            Debug.Log($"other.tag = {other.tag}");
            m_AllBrickObj.parent = m_MyGameManager.AllBrickObj;
            m_AllBrickObj.gameObject.SetActive(true);
            this.gameObject.SetActive(false);

        }
    }
}
