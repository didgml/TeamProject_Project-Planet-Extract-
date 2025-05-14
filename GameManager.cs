using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using SM = UnityEngine.SceneManagement.SceneManager;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ���� ��¥
    public int CurrentDay = 0;

    /// <summary>
    /// INT ������ �Ķ����
    /// ��ǥ �Ҵ緮 ��ġ (������)
    /// </summary>
    public int quota = 0;
    /// <summary>
    /// INT ������ �Ķ����
    /// ���� ������ �Ҵ緮 ��ġ
    /// </summary>
    public int currentQuota = 0;

    // UI ���۷��� (���� ��ġ�� UI Text�� Report UI ������Ʈ)
    [Header("UI Animation")]
    public CanvasGroup FadeUI;
    private string loadSceneName;

    /*
    // UI ���۷��� (���� ��ġ�� UI Text�� Report UI ������Ʈ)
    [Header("In-Game UI")]
    public TMP_Text dateUIText;     // ���� ���� �� ��� �÷��̾�� ǥ�õǴ� ��¥ UI
    public TMP_Text targetUIText1;   // ��ǥ�� 1�� ���� UI
    public TMP_Text targetUIText2;   // ��ǥ�� 2�� ���� UI
    */

    [Header("Report UI")]
    private GameObject reportUI;            // ���� ���� �� ����Ʈ UI
    private GameObject txt_Goal;            // ����Ʈ UI - Goal
    private GameObject txt_Remain;          // ����Ʈ UI - Remain
    private GameObject txt_Result;          // ����Ʈ UI - Result

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
            // �� ��ȯ �� �����ؾ� �Ѵٸ� DontDestroyOnLoad(this.gameObject); �߰� ����
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
            // 3������ ��� �Ҵ緮 ����
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
    /// �÷��̾� ���� �ʱ�ȭ
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
    /// �÷��̾� ���� �ʱ�ȭ
    /// </summary>
    public void PlayerSetup(Scene scene, LoadSceneMode mode) {
        // �÷��̾ ���� ��� ĳ���� ����
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
        // �÷��̾� �ʱ�ȭ
        player.Setup();
        reportUI = GameObject.Find("Report_UI");
        txt_Goal = reportUI.transform.GetChild(0).gameObject;
        txt_Remain = reportUI.transform.GetChild(1).gameObject;
        txt_Result = reportUI.transform.GetChild(2).gameObject;
    }

    /// <summary>
    /// �� ������ ������ �ʱ�ȭ
    /// </summary>
    public void StartNewGameSession(Scene scene, LoadSceneMode mode)
    {
        // ��¥ ����
        CurrentDay++;
        GameSetup(CurrentDay);
        Debug.Log($"���ο� ���� ���۵�: {CurrentDay}����");

        // ����Ʈ UI - Goal & Remain ���
        //StartCoroutine(ReportUISetup());
    }

    IEnumerator ReportUISetup()
    {
        txt_Goal.SetActive(true);
        txt_Goal.GetComponentInChildren<TMP_Text>().text = $"<color=#ffffff>{CurrentDay}</color> ���� <br>" +
            $"<size=64>��ǥ �Ҵ緮 : {quota} $ </size><br>";
        yield return new WaitForSeconds(1.5f);
        txt_Goal.SetActive(false);
        txt_Remain.SetActive(true);
        txt_Remain.GetComponentInChildren<TMP_Text>().text =
            $"<b><size=128><color=#ffffff>{3 - CurrentDay}</color></size> �� ���ҽ��ϴ�</b><br>" +
            $"<size=56>��ǥ �Ҵ緮 ��������</size>";
        yield return new WaitForSeconds(1.5f);
        txt_Remain.SetActive(false);

    }

    /// <summary>
    /// N �������� ��ǥ�� üũ
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
    /// ���̵� �ƿ�, �ε� ȭ��, ���̵� ��, �κ� �̵� �� ����Ʈ UI ��� ������
    /// </summary>
    IEnumerator LoadingSceneWithProduction(string sceneName)
    {
        float delay = 0f;
        yield return StartCoroutine(FadeInOut(true));

        // Scene ȣ�� 
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
    /// ������ üũ
    /// </summary>
    IEnumerator HandleMissionCheck()
    {
        Debug.Log("Misson Check");
        // ReportUI - Result ���
        txt_Result.SetActive(true);
        txt_Result.GetComponentInChildren<TMP_Text>().text =
            $"<color=#ffffff>{CurrentDay}</color> ���� ����Ʈ <br>" +
            $"<size=64>���� �Ҵ緮 : {quota} $</size><br>";

        yield return null;

        yield return new WaitForSeconds(3f);
        // 3�������� üũ
        if (quota >= currentQuota)
        // ������ �������� & ��ǥ���� �Ҵ緮 ���� ���� ���
        {
            Debug.Log("Clear");
            ChangeCurrentState(SceneState.Select);
        }
        else
        // ������ �������� & ��ǥ���� �Ҵ緮 ���� ���� ���
        {
            //reportUI.SetActive(false);
            yield return new WaitForSeconds(3f);
            // �ӹ� ���� 
            Debug.Log("Fail");
            ChangeCurrentState(SceneState.End);
        }
    }
}