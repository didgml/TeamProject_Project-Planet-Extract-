using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameTime : MonoBehaviour
{
    public static GameTime Instance;
    public TextMeshProUGUI TimeText;

    public TextMeshProUGUI TIMETEXT
    {
        get
        {
            if (TimeText == null)
            {
                TimeText = GameObject.Find("TimeUI").GetComponentInChildren<TextMeshProUGUI>();
            }

            return TimeText;
        }
        set
        {
            TimeText = value;
        }
    }

    public int hours = 8; // �ʱ� �ð� (08:00)
    public int minutes = 0;
    private float timer = 0f;

    private int previousHour = 8; // ���� �ð� ���� ����

    [SerializeField] private Material skyboxNight;
    [SerializeField] private Material skyboxSunrise;
    [SerializeField] private Material skyboxDay;
    [SerializeField] private Material skyboxSunset;

    [SerializeField] private Gradient gradientSunriseToDay;
    [SerializeField] private Gradient gradientDayToSunset;
    [SerializeField] private Gradient gradientSunsetToNight;

    [SerializeField] private Light globalLight;

    public bool isTimeRunning = false; // �ð� ���� ����

    // �̺�Ʈ ���� (�� ĳ������ UI ������Ʈ��)
    public event Action<string> OnTimeUpdated;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (Instance != null)
        {
            Instance.OnTimeUpdated += UpdateTimeDisplay;
            UpdateTimeDisplay(string.Format("{0 : 00}:{1 : 00}", GameTime.Instance.hours, GameTime.Instance.minutes));
        }
    }

    private void ChangeSkybox(Material newSkybox)
    {
        RenderSettings.skybox = newSkybox;
        DynamicGI.UpdateEnvironment(); // ���� ��� �ݿ�
    }

    void UpdateTimeDisplay(string formattedTime)
    {
        TIMETEXT.text = formattedTime;
    }

    private void OnDestroy()
    {
        if (GameTime.Instance != null)
        {
            GameTime.Instance.OnTimeUpdated -= UpdateTimeDisplay;
        }
    }

    void Update()
    {
        if (!isTimeRunning) return;

        timer += Time.deltaTime;

        // 4�ʸ��� 5�о� ����
        if (timer >= 4f)
        {
            timer = 0f;
            AddMinutes(5);
        }
    }

    void AddMinutes(int mins)
    {
        minutes += mins;

        if (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }

        if (previousHour != hours)
        {
            previousHour = hours; // ���� �ð� ������Ʈ
            CheckSkyboxChange();
        }

        // 24:00�� 4�� ���� �����ְ� ���� ƽ�� �ʱ�ȭ
        if (hours == 24 && minutes == 0)
        {
            UpdateTimeText(); // 24:00 ǥ��
            timer = 0f;       // ���� 4�� ��ٸ�
            return;
        }

        if (hours > 24 || (hours == 24 && minutes > 0))
        {
            StartNewDay();     // 08:00���� �ʱ�ȭ
        }

        UpdateTimeText();
    }

    void CheckSkyboxChange()
    {
        if (hours == 8)
        {
            StartCoroutine(LerpSkybox(skyboxNight, skyboxSunrise, 0f));
        }
        else if (hours == 12)
        {
            StartCoroutine(LerpSkybox(skyboxSunrise, skyboxDay, 0f));
        }
        else if (hours == 18)
        {
            StartCoroutine(LerpSkybox(skyboxDay, skyboxSunset, 0f));
        }
        else if (hours == 22)
        {
            StartCoroutine(LerpSkybox(skyboxSunset, skyboxNight, 0f));
        }
    }

    public void StartNewDay()
    {
        hours = 8;
        minutes = 0;

        SceneManager.LoadScene("1.SpaceShip");
    }


    public void ResetCurrentTime()
    {
        hours = 8;
        minutes = 0;
        StartTimer();
    }

    void UpdateTimeText()
    {
        string formattedTime = string.Format("{0 : 00}:{1 : 00}", hours, minutes);
        OnTimeUpdated?.Invoke(formattedTime);
    }

    public void StartTimer()
    {
        isTimeRunning = true;
    }

    private void ToggleisTimeRunning()
    {
        isTimeRunning = !isTimeRunning;
    }

    public void StopTimer()
    {
        isTimeRunning = false;
    }

    private IEnumerator LerpSkybox(Material a, Material b, float time)
    {
        if (RenderSettings.skybox == null || RenderSettings.skybox != a)
        {
            RenderSettings.skybox = a;
            DynamicGI.UpdateEnvironment();
        }

        Debug.Log("��ī�̹ڽ� ���� ����: " + a.name + " �� " + b.name);

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            float blend = i / time;
            RenderSettings.skybox.Lerp(a, b, blend);
            DynamicGI.UpdateEnvironment();
            yield return null;
        }

        RenderSettings.skybox = b;
        DynamicGI.UpdateEnvironment();

        Debug.Log("��ī�̹ڽ� ���� �Ϸ�: " + b.name);
    }

    private IEnumerator LerpLight(Gradient lightGradient, float time)
    {
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            globalLight.color = lightGradient.Evaluate(i / time);
            RenderSettings.fogColor = globalLight.color;
            yield return null;
        }
    }
}
