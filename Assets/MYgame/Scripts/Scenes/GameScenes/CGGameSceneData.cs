using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MYgame.Scripts.Scenes.GameScenes.Data;

[System.Serializable]
public class CDateBrick
{
    public Material m_ColorMat = null;
    public StaticGlobalDel.EBrickColor m_Type = StaticGlobalDel.EBrickColor.eRed;
}

public class CGGameSceneData : CSingletonMonoBehaviour<CGGameSceneData>
{

    public enum EAllFXType
    {
        eExplodePos         = 0,

        eMax,
    };

    public enum EOtherObj
    {
        eNpcCharacter       = 0,
        eFragments          = 1,
        eMax,
    };

    [SerializeField]  public GameObject[]    m_AllFX                = null;
    [SerializeField]  public GameObject[]    m_AllOtherObj          = null;
    [SerializeField]  public StageData       m_CurStageData         = null;
    [SerializeField]  public GameObject      m_PrefabEventSystem    = null;


    [VarRename(new string[] { "Red", "Orange", "Yellow", "Green", "Blue", "White" })]
    [SerializeField]  public CDateBrick[]    m_AllDateBrick         = new CDateBrick[(int)StaticGlobalDel.EBrickColor.eMax];

    private void Awake()
    {
        //for (int i = 0; i < (int)EArmsType.eMax; i++)
        //{
        //    m_CurNewArmsCount = i;
        //    m_AllArmsPool[i] = new CObjPool<GameObject>();
        //    m_AllArmsPool[i].NewObjFunc = NewArms;
        //    m_AllArmsPool[i].RemoveObjFunc = RemoveArms;
        //    m_AllArmsPool[i].InitDefPool(10);
        //}
    }
}
