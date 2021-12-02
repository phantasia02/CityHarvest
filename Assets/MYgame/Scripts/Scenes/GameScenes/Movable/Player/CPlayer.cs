﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UniRx;

public class CPlayerMemoryShare : CActorMemoryShare
{
    public bool                             m_bDown                     = false;
    public Vector3                          m_OldMouseDownPos           = Vector3.zero;
    public Vector3                          m_OldMouseDragDirNormal     = Vector3.zero;
    public CPlayer                          m_MyPlayer                  = null;
    public Vector3[]                        m_AllPathPoint              = new Vector3[8];
    public int                              m_CurStandPointindex        = 0;
    public Vector3                          m_TargetStandPoint          = Vector3.zero;
    public Collider                         m_SwordeCollider            = null;
    public UniRx.ReactiveProperty<float>    m_AnimationVal              = new ReactiveProperty<float>(0.5f);
    public float                            m_AddSpeedSecond            = 5.0f;

    public bool                             m_UpdateUI                  = false;
   // public UniRx.ReactiveProperty<int>      m_UpdateFeverScore          = new ReactiveProperty<int>(StaticGlobalDel.g_InitScoreFever);
    public int                              m_EndIndex                  = 0;

};

public class CPlayer : CActor
{
    public override EMovableType MyMovableType() { return EMovableType.ePlayer; }

    protected float m_MaxMoveDirSize = 5.0f;

    protected CPlayerMemoryShare m_MyPlayerMemoryShare = null;

    // ==================== SerializeField ===========================================

    // ==================== SerializeField ===========================================



    public float AnimationVal
    {
        set {
                float lTempValue = Mathf.Clamp(value, 0.0f, 1.0f);
                m_MyPlayerMemoryShare.m_AnimationVal.Value = lTempValue;
            }
        get { return m_MyPlayerMemoryShare.m_AnimationVal.Value; }
    }


    protected Vector3 m_OldMouseDragDir = Vector3.zero;

    int m_MoveingHash = 0;
 

    protected override void AddInitState()
    {
        m_AllState[(int)StaticGlobalDel.EMovableState.eWait].AllThisState.Add(new CWaitStatePlayer(this));
        m_AllState[(int)StaticGlobalDel.EMovableState.eMove].AllThisState.Add(new CMoveStatePlayer(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eDeath].AllThisState.Add(new CDeathStatePlayer(this));

        m_AllState[(int)StaticGlobalDel.EMovableState.eWin].AllThisState.Add(new CWinStatePlayer(this));
    }

    protected override void CreateMemoryShare()
    {
        m_MyPlayerMemoryShare = new CPlayerMemoryShare();
        m_MyMemoryShare = m_MyPlayerMemoryShare;

        m_MyPlayerMemoryShare.m_MyPlayer                    = this;

        base.CreateMemoryShare();

        m_MaxMoveDirSize = Screen.width > Screen.height ? (float)Screen.width : (float)Screen.height;
        m_MaxMoveDirSize = m_MaxMoveDirSize / 5.0f;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
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
        m_OldMouseDragDir += lTempMouseDrag * 2.0f;
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

    // ===================== UniRx ======================


    //public UniRx.ReactiveProperty<int> UpdateFeverScoreVal()
    //{
    //    return m_MyPlayerMemoryShare.m_UpdateFeverScore ?? (m_MyPlayerMemoryShare.m_UpdateFeverScore = new ReactiveProperty<int>(StaticGlobalDel.g_InitScoreFever));
    //}
    // ===================== UniRx ======================
}
