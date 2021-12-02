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


    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == StaticGlobalDel.TagBrickObj)
        {
            CBrickObj lTempBrickObj = other.GetComponent<CBrickObj>();

            lTempBrickObj.gameObject.SetActive(false);
            AddBrickColor(lTempBrickObj.MyBrickColor, 1);
            CheckBrickIsTarget();
            //Debug.Log($"===========================================");
            //for (int i = 0; i < m_MyPlayerMemoryShare.m_CurBrickAmount.Length; i++)
            //    Debug.Log($"m_MyPlayerMemoryShare.m_CurBrickAmount[i].amount = {m_MyPlayerMemoryShare.m_CurBrickAmount[i].amount}");
        }
      //  base.OnTriggerEnter(other);
    }

    public void AddBrickColor(StaticGlobalDel.EBrickColor setBrickColor, int Amount)
    {
        int lTempindex = (int)setBrickColor;
        m_MyPlayerMemoryShare.m_CurBrickAmount[lTempindex].amount += Amount;
    }

    public bool CheckBrickIsTarget()
    {
        int lTempCurIndex = m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex;
        BuildingRecipeData lTempCurBuildingRecipeData = m_MyPlayerMemoryShare.m_CurStageData.buildings[lTempCurIndex];
        int lTempColorIndex = 0;
        bool lTempbCheckOK = true;

        for (int i = 0; i < lTempCurBuildingRecipeData.brickAmounts.Length; i++)
        {
            lTempColorIndex = (int)lTempCurBuildingRecipeData.brickAmounts[i].color;
            if (m_MyPlayerMemoryShare.m_CurBrickAmount[lTempColorIndex].amount < lTempCurBuildingRecipeData.brickAmounts[i].amount)
            {
                lTempbCheckOK = false;
                break;
            }
        }

        if (lTempbCheckOK)
            SetNextBuildings();

        return lTempbCheckOK;
    }

    public void SetNextBuildings()
    {
        int lTempCurIndex = m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex;
        BuildingRecipeData lTempCurBuildingRecipeData = m_MyPlayerMemoryShare.m_CurStageData.buildings[lTempCurIndex];
        int lTempColorIndex = 0;

        Debug.Log($"SetNextBuildings OK = {lTempCurIndex}");

        for (int i = 0; i < lTempCurBuildingRecipeData.brickAmounts.Length; i++)
        {
            lTempColorIndex = (int)lTempCurBuildingRecipeData.brickAmounts[i].color;
            m_MyPlayerMemoryShare.m_CurBrickAmount[lTempColorIndex].amount -= lTempCurBuildingRecipeData.brickAmounts[i].amount;
        }

        lTempCurIndex++;
        if (m_MyPlayerMemoryShare.m_CurStageData.buildings.Length <= lTempCurIndex)
            lTempCurIndex = 0;

        m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex = lTempCurIndex;
    }
}
