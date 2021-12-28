﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;
using UniRx;
using MYgame.Scripts.Scenes.GameScenes.Data;
using MYgame.Scripts.Window;
using DG.Tweening;

public class CGameManager : MonoBehaviour
{
    public enum EState
    {
        eReady              = 0,
        ePlay               = 1,
        ePlayHold           = 2,
        ePlayOKPerformance  = 3,
        eReadyEnd           = 4,
        eNextEnd            = 5,
        eGameOver           = 6,
        eWinUI              = 7,
        eMax
    };
    
    bool m_bDown = false;

    CChangeScenes m_ChangeScenes = new CChangeScenes();
    protected ResultUI m_MyResultUI = null;
    public ResultUI MyResultUI { get { return m_MyResultUI; } }

    protected Camera m_Camera = null;
    public Camera MainCamera { get { return m_Camera; } }

    protected CPlayer m_Player = null;
    public CPlayer Player { get { return m_Player; } }
    // ==================== SerializeField ===========================================
    [SerializeField] protected CStageType m_StageTypeData = null;
    public CStageType StageTypeData { get { return m_StageTypeData; } }

    //[SerializeField] GameObject m_WinCamera = null;
    //public GameObject WinCamera { get { return m_WinCamera; } }
    [Header("Result OBJ")]
    [SerializeField] protected GameObject   m_WinObjAnima       = null;
    [SerializeField] protected GameObject   m_OverObjAnima      = null;
    [SerializeField] protected StageData    m_MyTargetBuilding  = null;
    // ==================== All ObjData  ===========================================

    protected CGameObjBasListData[]     m_AllGameObjBas     = new CGameObjBasListData[(int)CGameObjBas.EObjType.eMax];
    public CGameObjBasListData GetTypeGameObjBaseListData(CGameObjBas.EObjType type) { return m_AllGameObjBas[(int)type]; }

    protected CMovableBaseListData[]    m_AllMovableBase    = new CMovableBaseListData[(int)CMovableBase.EMovableType.eMax];
    public CMovableBaseListData GetTypeMovableBaseListData(CMovableBase.EMovableType type) { return m_AllMovableBase[(int)type]; }

    protected CActorBaseListData[]      m_AllActorBase      = new CActorBaseListData[(int)CActor.EActorType.eMax];
    public CActorBaseListData GetTypeActorBaseListData(CActor.EActorType type) { return m_AllActorBase[(int)type]; }


    // ==================== All ObjData ===========================================

    [SerializeField] protected Transform    m_AllBrickObj       = null;
    public Transform AllBrickObj => m_AllBrickObj;

    [SerializeField] protected Transform m_AllCompleteBuilding = null;
    public Transform AllCompleteBuilding => m_AllCompleteBuilding;

    public StageData MyTargetBuilding { get { return m_MyTargetBuilding; } }
    // ==================== SerializeField ===========================================

    protected CinemachineTargetGroup m_EndCinemachineTargetGroup = null;
    protected bool isApplicationQuitting = false;
    public bool GetisApplicationQuitting { get { return isApplicationQuitting; } }

    int m_AllFragmentsMax = 100;

    private EState m_eCurState = EState.eReady;
    public EState CurState { get { return m_eCurState; } }
    protected float m_StateTime = 0.0f;
    protected float m_StateUnscaledTime = 0.0f;
    protected int m_StateCount = 0;
    protected Vector3 m_OldInput;
    protected float m_HalfScreenWidth = 600.0f;


    void Awake()
    {
        Application.targetFrameRate = 60;
        const float HWRatioPototype = StaticGlobalDel.g_fcbaseHeight / StaticGlobalDel.g_fcbaseWidth;
        float lTempNewHWRatio = ((float)Screen.height / (float)Screen.width);
        m_HalfScreenWidth = (StaticGlobalDel.g_fcbaseWidth / 2.0f) * (lTempNewHWRatio / HWRatioPototype);

        m_Player = this.GetComponentInChildren<CPlayer>();
        m_MyResultUI = gameObject.GetComponentInChildren<ResultUI>();

        if (m_MyResultUI != null)
        {
            m_MyResultUI.NextButton.onClick.AddListener(OnNext);
            m_MyResultUI.OverButton.onClick.AddListener(OnReset);
        }

        GameObject lTempCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (lTempCameraObj != null)
            m_Camera = lTempCameraObj.GetComponent<Camera>();

        m_EndCinemachineTargetGroup = this.GetComponentInChildren<CinemachineTargetGroup>();
        for (int i = 0; i < m_AllGameObjBas.Length; i++)
            m_AllGameObjBas[i] = new CGameObjBasListData();

        for (int i = 0; i < m_AllMovableBase.Length; i++)
            m_AllMovableBase[i] = new CMovableBaseListData();

        for (int i = 0; i < m_AllActorBase.Length; i++)
            m_AllActorBase[i] = new CActorBaseListData();
    }

    // Start is called before the first frame update
    void Start()
    {


        Observable.EveryUpdate().First(_ => Input.GetMouseButtonDown(0)).Subscribe(
        OnNext =>
        {
            if (m_eCurState == EState.eReady)
            {
                SetState(EState.ePlay);

                CReadyGameWindow lTempCReadyGameWindow = CReadyGameWindow.SharedInstance;
                if (lTempCReadyGameWindow && lTempCReadyGameWindow.GetShow())
                    lTempCReadyGameWindow.CloseShowUI();

                CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;
                if (lTempGameSceneWindow)
                {
                    lTempGameSceneWindow.MyGameStatusUI.StartTimer(TimeOut);
                }
            }
        },
        OnCompleted => { Debug.Log("OK"); }
        ).AddTo(this);


    }

    // Update is called once per frame
    void Update()
    {

        m_StateTime += Time.deltaTime;
        m_StateCount++;
        m_StateUnscaledTime += Time.unscaledDeltaTime;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                  //  UsePlayTick();
                }
                break;
            case EState.ePlay:
                {
                  //  UsePlayTick();
                }
                break;
            case EState.ePlayHold:
                {

                }
                break;
            case EState.ePlayOKPerformance:
                {
                }
                break;

            case EState.eReadyEnd:
                {
                }
                break;


            case EState.eNextEnd:
                {
                }
                break;
            case EState.eGameOver:
                {

                }
                break;
        }
    }

    public void SetState(EState lsetState)
    {
        if (lsetState == m_eCurState)
            return;

        if (m_eCurState == EState.eWinUI || m_eCurState == EState.eGameOver)
            return;

        EState lOldState = m_eCurState;
        m_StateTime = 0.0f;
        m_StateCount = 0;
        m_StateUnscaledTime = 0.0f;
        m_eCurState = lsetState;

        CGameSceneWindow lTempGameSceneWindow = CGameSceneWindow.SharedInstance;

        switch (m_eCurState)
        {
            case EState.eReady:
                {
                }
                break;
            case EState.ePlay:
                {

                }
                break;
            case EState.ePlayHold:
                {

                }
                break;
            case EState.ePlayOKPerformance:
                {
                }
                break;
            case EState.eReadyEnd:
                {
                }
                break;
            case EState.eNextEnd:
                {

                }
                break;
            case EState.eWinUI:
                {
                    if (lTempGameSceneWindow)
                    {
                       // lTempGameSceneWindow.ShowObj(false);
                    }

                  //  m_AllGroupQuestionHole[0].ChangeQuestionHoleState( CQuestionHole.ECurShowState.eFinish);
                  //  ChangeQuestionHoleState
                    m_MyResultUI.ShowSuccessUI(2.0f);
                }
                break;
            case EState.eGameOver:
                {
                    //if (lTempGameSceneWindow)
                    //    lTempGameSceneWindow.ShowObj(false);
                  
                    m_MyResultUI.ShowFailedUI(0.0f);
                }
                break;
        }
    }

    public void UsePlayTick()
    {
//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            m_bDown = true;
            m_OldInput = Input.mousePosition;
            //InputRay();
           // OKAllGroupQuestionHole(0);
        }
        else if (Input.GetMouseButton(0))
        {
            //float moveX = (Input.mousePosition.x - m_OldInput.x) / m_HalfScreenWidth;
            //m_Player.SetXMove(moveX);
            //m_OldInput = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (m_bDown)
            {
                m_OldInput = Vector3.zero;
                m_bDown = false;
            }
        }

    }
    private GUIStyle guiStyle = new GUIStyle(); //create a new variable
    List<string> testbugtext = new List<string>();
    private void OnGUI()
    {
        string test = "";
        for (int i = testbugtext.Count - 1; i >= 0; i--)
            test += $"{testbugtext[i]}\n";

        guiStyle.fontSize = 60; //change the font size
        GUI.Label(new Rect(10, 10, 1000, 2000), test, guiStyle);
    }

    public void InputRay()
    {
        
    }

    public void OnNext()
    {
        m_ChangeScenes.LoadGameScenes();
    }

    public void OnReset()
    {
        m_ChangeScenes.ResetScene();
    }

    void OnApplicationQuit() { isApplicationQuitting = true; }

    public void SetWinUI()
    {
        SetState(EState.eWinUI);
    }

    public void SetLoseUI()
    {
        SetState(EState.eGameOver);
    }

    public void TimeOut()
    {
        m_Player.SetChangState(CMovableStatePototype.EMovableState.eWait, 1);

        if (m_AllCompleteBuilding.childCount == 0)
            SetState(EState.eGameOver);
        else
        {
            //CinemachineVirtualCamera lTempCinemachineVirtualCamera = m_WinObjAnima.GetComponent<CinemachineVirtualCamera>();
            //lTempCinemachineVirtualCamera.Follow = m_AllCompleteBuilding.GetChild(m_AllCompleteBuilding.childCount - 1);

            for (int i = 0; i < m_AllCompleteBuilding.childCount; i++)
                m_EndCinemachineTargetGroup.AddMember(m_AllCompleteBuilding.GetChild(i), 0.1f, 2.0f);

            Physics.gravity = new Vector3(0.0f, -9.81f, 0.0f);
            m_WinObjAnima.SetActive(true);
            SetState(EState.eWinUI);
        }

        
    }
    
    // ==================== All ObjData  ===========================================

    public void AddGameObjBasListData(CGameObjBas addGameObjBas)
    {
        if (isApplicationQuitting)
            return;

        if (addGameObjBas == null)
            return;

        int lTempTypeIndex = (int)addGameObjBas.ObjType();

        addGameObjBas.GameObjBasIndex = m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData.Count;
        m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData.Add(addGameObjBas);
        m_AllGameObjBas[lTempTypeIndex].m_GameObjBasHashtable.Add(addGameObjBas.GetInstanceID(), addGameObjBas);
    }

    public void RemoveGameObjBasListData(CGameObjBas addGameObjBas)
    {
        if (isApplicationQuitting)
            return;

        if (addGameObjBas == null)
            return;

        int lTempTypeIndex = (int)addGameObjBas.ObjType();
        List<CGameObjBas> lTempGameObjBasList = m_AllGameObjBas[lTempTypeIndex].m_GameObjBasListData;

        lTempGameObjBasList.Remove(addGameObjBas);
        for (int i = 0; i < lTempGameObjBasList.Count; i++)
            lTempGameObjBasList[i].GameObjBasIndex = i;

        m_AllGameObjBas[lTempTypeIndex].m_GameObjBasHashtable.Remove(addGameObjBas.GetInstanceID());
    }

    public void AddMovableBaseListData(CMovableBase addMovableBase)
    {
        if (addMovableBase == null)
            return;

        int lTempTypeIndex = (int)addMovableBase.MyMovableType();
        m_AllMovableBase[lTempTypeIndex].m_MovableBaseListData.Add(addMovableBase);
    }

    public void RemoveMovableBaseListData(CMovableBase removeMovableBase)
    {
        if (isApplicationQuitting)
            return;

        if (removeMovableBase == null)
            return;

        int lTempTypeIndex = (int)removeMovableBase.MyMovableType();
        List<CMovableBase> lTempMovableBaseList = m_AllMovableBase[lTempTypeIndex].m_MovableBaseListData;
        lTempMovableBaseList.Remove(removeMovableBase);
    }

    public void AddActorBaseListData(CActor addActorBase)
    {
        if (addActorBase == null)
            return;

        int lTempTypeIndex = (int)addActorBase.MyActorType();
        m_AllActorBase[lTempTypeIndex].m_ActorBaseListData.Add(addActorBase);
    }

    public void RemoveActorBaseListData(CActor removeActorBase)
    {
        if (isApplicationQuitting)
            return;

        if (removeActorBase == null)
            return;

        int lTempTypeIndex = (int)removeActorBase.MyActorType();
        List<CActor> lTempActorBaseList = m_AllActorBase[lTempTypeIndex].m_ActorBaseListData;
        lTempActorBaseList.Remove(removeActorBase);
    }

    // ==================== All ObjData  ===========================================
}
