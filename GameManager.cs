using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 현재 날짜
    public int CurrentDay = 0;

    /// <summary>
    /// INT 형식의 파라미터
    /// 목표 할당량 수치 (일차별)
    /// </summary>
    public int quota = 0;
    /// <summary>
    /// INT 형식의 파라미터
    /// 현재 제출한 할당량 수치
    /// </summary>
    public int currentQuota = 0;

    // UI 레퍼런스 (씬에 배치된 UI Text와 Report UI 오브젝트)
    [Header("UI Animation")]
    public CanvasGroup FadeUI;
    private string loadSceneName;

    /*
    // UI 레퍼런스 (씬에 배치된 UI Text와 Report UI 오브젝트)
    [Header("In-Game UI")]
    public TMP_Text dateUIText;     // 게임 입장 시 모든 플레이어에게 표시되는 날짜 UI
    public TMP_Text targetUIText1;   // 목표물 1의 수량 UI
    public TMP_Text targetUIText2;   // 목표물 2의 수량 UI
    */

    [Header("Report UI")]
    private GameObject reportUI;            // 게임 종료 후 리포트 UI
    private GameObject txt_Goal;            // 리포트 UI - Goal
    private GameObject txt_Remain;          // 리포트 UI - Remain
    private GameObject txt_Result;          // 리포트 UI - Result

    [Header("Player")]
    public GameObject playerPrefab;
    private Character player;
    private GameObject playerObject;
    private Vector3 spawnPos;

    [Header("Code Test")]
    public bool isTesting = false;

    private StateMachine stateMachine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            // 씬 전환 시 유지해야 한다면 DontDestroyOnLoad(this.gameObject); 추가 가능
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        stateMachine = new StateMachine();

        stateMachine.Setup(isTesting);
    }

    public void ChangeCurrentState(SceneState state)
    {
        stateMachine.ChangeState(state);
    }
    public void GameSetup(int day)
    {
        if ((day-1) % 3 == 0)
            // 3일차인 경우 할당량 증가
        {
            quota = Mathf.RoundToInt((quota * 0.8f) * 2f);
        }
        
        if (day == 1)
        {
            quota = 100;
            currentQuota = 0;
        }
    }

    /// <summary>
    /// 플레이어 상태 초기화
    /// </summary>
    public void SpawnPointSetup(Scene scene, LoadSceneMode mode) {
        var pos = GameObject.Find("SpawnPoint");
        if (pos != null)
        {
            spawnPos = pos.transform.position;
        }
        else
        {
            spawnPos = Vector3.zero;
        }
    }


    /// <summary>
    /// 플레이어 상태 초기화
    /// </summary>
    public void PlayerSetup(Scene scene, LoadSceneMode mode) {
        // 플레이어가 죽은 경우 캐릭터 생성
        if (playerObject == null)
        {
            playerObject = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            player = playerObject.GetComponentInChildren<Character>();
        }
        if (player.isDead)
        {
            playerObject = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            player = playerObject.GetComponentInChildren<Character>();
        }
        // 플레이어 초기화
        player.Setup();
        reportUI = GameObject.Find("Report_UI");
        txt_Goal = reportUI.transform.GetChild(0).gameObject;
        txt_Remain = reportUI.transform.GetChild(1).gameObject;
        txt_Result = reportUI.transform.GetChild(2).gameObject;
    }

    /// <summary>
    /// 각 일차별 데이터 초기화
    /// </summary>
    public void StartNewGameSession(Scene scene, LoadSceneMode mode)
    {
        // 날짜 증가
        CurrentDay++;
        GameSetup(CurrentDay);
        Debug.Log($"새로운 날이 시작됨: {CurrentDay}일차");

        // 리포트 UI - Goal & Remain 출력
        //StartCoroutine(ReportUISetup());
    }

    IEnumerator ReportUISetup()
    {
        txt_Goal.SetActive(true);
        txt_Goal.GetComponentInChildren<TMP_Text>().text = $"<color=#ffffff>{CurrentDay}</color> 일차 <br>" +
            $"<size=64>목표 할당량 : {quota} $ </size><br>";
        yield return new WaitForSeconds(1.5f);
        txt_Goal.SetActive(false);
        txt_Remain.SetActive(true);
        txt_Remain.GetComponentInChildren<TMP_Text>().text =
            $"<b><size=128><color=#ffffff>{3 - CurrentDay}</color></size> 일 남았습니다</b><br>" +
            $"<size=56>목표 할당량 충족까지</size>";
        yield return new WaitForSeconds(1.5f);
        txt_Remain.SetActive(false);

    }

    /// <summary>
    /// N 일차마다 목표량 체크
    /// </summary>
    public void MissionCheck()
    {
        StartCoroutine(HandleMissionCheck());
    }

    public void LoadingProduction(string sceneName)
    {
        FadeUI.gameObject.SetActive(true);
        SM.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;
        StartCoroutine(LoadingSceneWithProduction(sceneName));
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == loadSceneName)
        {
            StartCoroutine(FadeInOut(false));
            SM.sceneLoaded -= OnSceneLoaded;
        }
    }

    /// <summary>
    /// 페이드 아웃, 로딩 화면, 페이드 인, 로비 이동 후 리포트 UI 출력 시퀀스
    /// </summary>
    IEnumerator LoadingSceneWithProduction(string sceneName)
    {
        float delay = 0f;
        yield return StartCoroutine(FadeInOut(true));

        // Scene 호출 
        AsyncOperation async = SM.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        float timer = 0f;
        while (!async.isDone)
        {
            yield return null;
            if (async.progress < .9f)
            {
                delay = async.progress;
            } else
            {
                timer += Time.unscaledDeltaTime;
                delay = Mathf.Lerp(.9f, 1f, timer);

                if (delay >= 1f)
                {
                    async.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    IEnumerator FadeInOut(bool isActive)
    {
        float timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            FadeUI.alpha = isActive ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }
        
        if (!isActive)
        {
            FadeUI.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 마감일 체크
    /// </summary>
    IEnumerator HandleMissionCheck()
    {
        Debug.Log("Misson Check");
        // ReportUI - Result 출력
        txt_Result.SetActive(true);
        txt_Result.GetComponentInChildren<TMP_Text>().text =
            $"<color=#ffffff>{CurrentDay}</color> 일차 리포트 <br>" +
            $"<size=64>남은 할당량 : {quota} $</size><br>";

        yield return null;

        yield return new WaitForSeconds(3f);
        // 3일차마다 체크
        if (quota >= currentQuota)
        // 수집한 연구일지 & 목표물이 할당량 보다 많은 경우
        {
            Debug.Log("Clear");
            ChangeCurrentState(SceneState.Select);
        }
        else
        // 수집한 연구일지 & 목표물이 할당량 보다 적은 경우
        {
            //reportUI.SetActive(false);
            yield return new WaitForSeconds(3f);
            // 임무 실패 
            Debug.Log("Fail");
            ChangeCurrentState(SceneState.End);
        }
    }
}