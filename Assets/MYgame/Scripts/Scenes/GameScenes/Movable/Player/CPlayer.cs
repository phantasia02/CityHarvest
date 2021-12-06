using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;
using MYgame.Scripts.Scenes.GameScenes.Data;
using MYgame.Scripts.Scenes.Building;

public class CPlayerMemoryShare : CActorMemoryShare
{
    public bool                             m_bDown                     = false;
    public Vector3                          m_OldMouseDownPos           = Vector3.zero;
    public Vector3                          m_OldMouseDragDirNormal     = Vector3.zero;
    public CPlayer                          m_MyPlayer                  = null;

    public UniRx.ReactiveProperty<float>    m_AnimationVal              = new ReactiveProperty<float>(0.5f);
    public float                            m_AddSpeedSecond            = 5.0f;

   // public UniRx.ReactiveProperty<int>      m_UpdateFeverScore          = new ReactiveProperty<int>(StaticGlobalDel.g_InitScoreFever);
    public int                              m_EndIndex                  = 0;
    public StageData                        m_CurStageData              = null;
    //public BuildingRecipeData               m_CurBuildingRecipeData     = null;
    //public BuildingRecipeData               m_NextBuildingRecipeData    = null;
    public int                              m_BuildingRecipeDataIndex   = 0;
    public BrickAmount[]                    m_CurBrickAmount            = new BrickAmount[(int)StaticGlobalDel.EBrickColor.eMax];
    public Transform                        m_RecycleBrickObj           = null;
    public Transform                        m_BuildingPos               = null;
    public BuildingProgress                 m_CurBuildingProgress       = null;
};

public class CPlayer : CActor
{
    public override EMovableType MyMovableType() { return EMovableType.ePlayer; }

    protected float m_MaxMoveDirSize = 5.0f;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    // ==================== SerializeField ===========================================

    [SerializeField] protected Transform m_RecycleBrickObj  = null;
    [SerializeField] protected Transform m_BuildingPos      = null;

    // ==================== SerializeField ===========================================

    public Transform RecycleBrickObj => m_MyPlayerMemoryShare.m_RecycleBrickObj;
    public Transform BuildingPos => m_MyPlayerMemoryShare.m_BuildingPos;

    public float AnimationVal
    {
        set {
                float lTempValue = Mathf.Clamp(value, 0.0f, 1.0f);
                m_MyPlayerMemoryShare.m_AnimationVal.Value = lTempValue;
            }
        get { return m_MyPlayerMemoryShare.m_AnimationVal.Value; }
    }


    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    public override float DefSpeed { get { return 1000.0f; } }

    int m_MoveingHash = 0;


    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayer(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWinStateBase(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayer(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_MyPlayer = this;
        m_MyPlayerMemoryShare.m_RecycleBrickObj     = m_RecycleBrickObj;
        m_MyPlayerMemoryShare.m_BuildingPos         = m_BuildingPos;


        base.CreateMemoryShare();

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 5.0f;

        for (int i = 0; i < m_MyPlayerMemoryShare.m_CurBrickAmount.Length; i++)
        {
            m_MyPlayerMemoryShare.m_CurBrickAmount[i] = new BrickAmount();
            m_MyPlayerMemoryShare.m_CurBrickAmount[i].color = (StaticGlobalDel.EBrickColor)i;
        }


    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


        m_MyPlayerMemoryShare.m_CurStageData = m_MyGameManager.MyTargetBuilding;

        CGameSceneWindow lTempCGameSceneWindow = CGameSceneWindow.SharedInstance;
        foreach (StaticGlobalDel.EBrickColor lTempColor in m_MyPlayerMemoryShare.m_CurStageData.brickColors)
            lTempCGameSceneWindow.MyGameStatusUI.UpdateTotalBricksNumber(lTempColor, m_MyPlayerMemoryShare.m_CurBrickAmount[(int)lTempColor].amount);

        UpdateInitSetUIMode();

        SetCurState(StaticGlobalDel.EMovableState.eWait);
        //UpdateAnimationVal().Subscribe(_ => {
        //    UpdateAnimationChangVal();
        //}).AddTo(this.gameObject);


    }

    public void UpdateAnimationChangVal()
    {
       // if (m_MyPlayerMemoryShare.m_isupdateAnimation)
            m_AnimatorStateCtl.SetFloat(m_MoveingHash, m_MyPlayerMemoryShare.m_AnimationVal.Value);
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //if (m_MyGameManager.CurState == CGameManager.EState.ePlay || m_MyGameManager.CurState == CGameManager.EState.eReady)
        InputUpdata();


    }

    public override void InputUpdata()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayerMouseDown();
        }
        else if (Input.GetMouseButton(0))
        {
            PlayerMouseDrag();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            PlayerMouseUp();
        }
    }

    public void PlayerMouseDown()
    {
        //if (!PlayerCtrl())
        //{
        //    if (m_CurState != StaticGlobalDel.EMovableState.eNull && m_AllState[(int)m_CurState] != null)
        //    {
        //        m_AllState[(int)m_CurState].MouseDown();
        //    }
        //}

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDown();

        m_MyPlayerMemoryShare.m_bDown = true;
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseDrag()
    {
        //if (!PlayerCtrl())
        //    return;
        if (!m_MyPlayerMemoryShare.m_bDown)
            return;

        DataState lTempDataState = m_AllState[(int)CurState];
        if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
            lTempDataState.AllThisState[lTempDataState.index].MouseDrag();

        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;
    }

    public void PlayerMouseUp()
    {
        if (m_MyPlayerMemoryShare.m_bDown)
        {
            DataState lTempDataState = m_AllState[(int)CurState];
            if (m_CurState != StaticGlobalDel.EMovableState.eNull && lTempDataState != null && lTempDataState.AllThisState[lTempDataState.index] != null)
                lTempDataState.AllThisState[lTempDataState.index].MouseUp();

            m_MyPlayerMemoryShare.m_bDown = false;
            m_MyPlayerMemoryShare.m_OldMouseDownPos = Vector3.zero;
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public void GameOver()
    {
     //   float lTempResultPercent = 1.0f - (float)m_MyPlayerMemoryShare.m_PlayerFollwer.result.percent;
        //float lTempFeverScoreRatio = (float)m_MyPlayerMemoryShare.m_UpdateFeverScore.Value / (float)StaticGlobalDel.g_MaxFever;
        //float lTempResult = lTempFeverScoreRatio;

        //CAllScoringBox lTempAllScoringBox = CAllScoringBox.SharedInstance;
        //m_MyPlayerMemoryShare.m_EndIndex = (int)(lTempResult * (float)lTempAllScoringBox.AllScoringBox.Count - 1);

        //m_MyPlayerMemoryShare.m_MyMovable.ChangState = StaticGlobalDel.EMovableState.eWin;
    }

    public void UpdateDrag()
    {
        if (!m_MyPlayerMemoryShare.m_bDown)
            return;


        Vector3 lTempMouseDrag = Input.mousePosition - m_MyPlayerMemoryShare.m_OldMouseDownPos;
        lTempMouseDrag.z = lTempMouseDrag.y;
        lTempMouseDrag.y = 0.0f;
        //  float lTempScreenDragProportion = lTempMouseDrag.magnitude / m_MinScreenSize;
        //if (lTempScreenDragProportion >= 0.01f)
        // {
        ChangState = StaticGlobalDel.EMovableState.eMove;
        m_OldMouseDragDir += lTempMouseDrag * 3.0f;
        m_OldMouseDragDir.y = 0.0f;

        m_OldMouseDragDir = Vector3.ClampMagnitude(m_OldMouseDragDir, m_MaxMoveDirSize);
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal = m_OldMouseDragDir;
        m_MyPlayerMemoryShare.m_OldMouseDragDirNormal.Normalize();
        m_MyPlayerMemoryShare.m_OldMouseDownPos = Input.mousePosition;

        if (m_MyPlayerMemoryShare.m_OldMouseDragDirNormal == Vector3.zero)
            return;

        m_MyPlayerMemoryShare.m_MyTransform.forward = m_MyPlayerMemoryShare.m_OldMouseDragDirNormal;
      //  m_MyPlayerMemoryShare.m_MyRigidbody.velocity = m_MyPlayerMemoryShare.m_OldMouseDragDirNormal;
    }


    public void AddBrickColor(StaticGlobalDel.EBrickColor setBrickColor, int Amount)
    {
        int lTempindex = (int)setBrickColor;
        m_MyPlayerMemoryShare.m_CurBrickAmount[lTempindex].amount += Amount;

        CGameSceneWindow lTempCGameSceneWindow = CGameSceneWindow.SharedInstance;
        lTempCGameSceneWindow.MyGameStatusUI.UpdateTotalBricksNumber(setBrickColor, m_MyPlayerMemoryShare.m_CurBrickAmount[lTempindex].amount);
        lTempCGameSceneWindow.MyGameStatusUI.IncreaseBrickNumber(setBrickColor);

        CheckBrickIsTarget();

    }

    public bool CheckBrickIsTarget()
    {
        int lTempCurIndex = m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex;
        BuildingRecipeData lTempCurBuildingRecipeData = m_MyPlayerMemoryShare.m_CurStageData.buildings[lTempCurIndex];
        int lTempColorIndex = 0;
        bool lTempbCheckOK = true;
        int TotalBuildingRecipeDataCount = 0;
        int CurBuildingRecipeDataCount = 0;

        for (int i = 0; i < lTempCurBuildingRecipeData.brickAmounts.Length; i++)
        {
            lTempColorIndex = (int)lTempCurBuildingRecipeData.brickAmounts[i].color;
            if (m_MyPlayerMemoryShare.m_CurBrickAmount[lTempColorIndex].amount < lTempCurBuildingRecipeData.brickAmounts[i].amount)
            {
                CurBuildingRecipeDataCount += m_MyPlayerMemoryShare.m_CurBrickAmount[lTempColorIndex].amount;
                TotalBuildingRecipeDataCount += lTempCurBuildingRecipeData.brickAmounts[i].amount;
                lTempbCheckOK = false;
            }
        }

        float lTempRatio = (float)CurBuildingRecipeDataCount / (float)TotalBuildingRecipeDataCount;
        m_MyPlayerMemoryShare.m_CurBuildingProgress.UpdateProgress(lTempRatio);

        if (lTempbCheckOK)
            SetNextBuildings();

        return lTempbCheckOK;
    }

    public void SetNextBuildings()
    {
        for (int i = 0; i < m_MyPlayerMemoryShare.m_CurBrickAmount.Length; i++)
            m_MyPlayerMemoryShare.m_CurBrickAmount[i].amount = 0;

        Vector3 lTemppoint = Vector3.zero;
        Vector3 lTempTargetpos = Vector3.zero;
        Collider[] lTempAllCollider = null;
        for (int i = 0; i < 100; i++)
        {
            lTempAllCollider = null;
            lTemppoint = Random.insideUnitSphere * Random.Range(25.0f, 30.0f);
            lTemppoint.y = 0.0f;
            lTemppoint = this.transform.position + lTemppoint + Vector3.up * 10.0f;

            if (Physics.Raycast(lTemppoint, Vector3.down, out RaycastHit hit, 20.0f, StaticGlobalDel.g_FloorMask))
            {
                lTempAllCollider = Physics.OverlapSphere(hit.point, 3.0f, StaticGlobalDel.g_CompleteBuildingMask);
                if (lTempAllCollider.Length > 0)
                    continue;

                lTempTargetpos = hit.point;
                break;
            }

        }

        m_MyPlayerMemoryShare.m_CurBuildingProgress.transform.parent = m_MyGameManager.AllCompleteBuilding;
        Tween lTempTween = m_MyPlayerMemoryShare.m_CurBuildingProgress.transform.DOJump(lTempTargetpos, 20.0f, 1, 1.0f);
        m_MyPlayerMemoryShare.m_CurBuildingProgress.transform.DOScale(Vector3.one * 0.2f, 3.0f).SetEase( Ease.OutBounce);

        int lTempCurIndex = m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex;
        lTempCurIndex++;
        if (m_MyPlayerMemoryShare.m_CurStageData.buildings.Length <= lTempCurIndex)
            lTempCurIndex = 0;

        m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex = lTempCurIndex;

        UpdateInitSetUIMode();
    }

    public void UpdateInitSetUIMode()
    {
        int lTempCurIndex = m_MyPlayerMemoryShare.m_BuildingRecipeDataIndex;
        BuildingRecipeData lTempCurBuildingRecipeData = m_MyPlayerMemoryShare.m_CurStageData.buildings[lTempCurIndex];

        int lTempNextIndex = lTempCurIndex + 1;
        if (m_MyPlayerMemoryShare.m_CurStageData.buildings.Length == lTempNextIndex)
            lTempNextIndex = 0;

        BuildingRecipeData lTempNextBuildingRecipeData = m_MyPlayerMemoryShare.m_CurStageData.buildings[lTempNextIndex];

        CGameSceneWindow lTempCGameSceneWindow = CGameSceneWindow.SharedInstance;
        lTempCGameSceneWindow.MyGameStatusUI.SetBuildingRecipe(lTempCurBuildingRecipeData.buildingSprite, lTempCurBuildingRecipeData.brickAmounts, lTempNextBuildingRecipeData.buildingSprite);
        for (int i = 0; i < lTempCurBuildingRecipeData.brickAmounts.Length; i++)
            lTempCGameSceneWindow.MyGameStatusUI.UpdateTotalBricksNumber(lTempCurBuildingRecipeData.brickAmounts[i].color, 0);

        GameObject lTempCurBuilding =  GameObject.Instantiate(lTempCurBuildingRecipeData.Prefab3DMode);
        lTempCurBuilding.transform.parent = m_MyPlayerMemoryShare.m_BuildingPos;
        lTempCurBuilding.transform.localPosition = Vector3.zero ;

        m_MyPlayerMemoryShare.m_CurBuildingProgress = lTempCurBuilding.GetComponent<BuildingProgress>();
    }

    //public void UpdateBrickAmount()
    //{

    //}

    // ===================== UniRx ======================


    //public UniRx.ReactiveProperty<int> UpdateFeverScoreVal()
    //{
    //    return m_MyPlayerMemoryShare.m_UpdateFeverScore ?? (m_MyPlayerMemoryShare.m_UpdateFeverScore = new ReactiveProperty<int>(StaticGlobalDel.g_InitScoreFever));
    //}
    // ===================== UniRx ======================
}
