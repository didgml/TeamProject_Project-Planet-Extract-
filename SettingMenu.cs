using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;
    public TMPro.TMP_Dropdown qualityDropdown;

    // PlayerPrefs 키 상수
    private const string QualityKey = "QualitySetting";
    private const string FullScreenKey = "FullScreenSetting";
    private const string ResolutionKey = "ResolutionSetting";

    // 초기 기본값 (필요에 따라 수정)
    private int defaultQuality;
    private bool defaultFullScreen;
    private int defaultResolutionIndex;

    public void Start()
    {
        // 기본값 설정 (현재 값들을 기본값으로 사용)
        defaultQuality = QualitySettings.GetQualityLevel();
        defaultFullScreen = Screen.fullScreen;

        // 해상도 설정
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        defaultResolutionIndex = 0; // 현재 해상도 기준

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                defaultResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);

        // 저장된 해상도 불러오기
        int savedResolution = PlayerPrefs.GetInt(ResolutionKey, defaultResolutionIndex);
        resolutionDropdown.value = savedResolution;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedResolution);

        // 저장된 품질 설정 불러오기 (품질 Dropdown이 있다면)
        int savedQuality = PlayerPrefs.GetInt(QualityKey, defaultQuality);
        SetQuality(savedQuality);
        if (qualityDropdown != null)
        {
            qualityDropdown.value = savedQuality;
            qualityDropdown.RefreshShownValue();
        }

        // 저장된 전체화면 설정 불러오기
        bool savedFullscreen = PlayerPrefs.GetInt(FullScreenKey, defaultFullScreen ? 1 : 0) == 1;
        SetFullScreen(savedFullscreen);
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = savedFullscreen;
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex, true);
        PlayerPrefs.SetInt(QualityKey, qualityIndex);
        PlayerPrefs.Save();  // 저장된 설정을 즉시 디스크에 쓰기
        Debug.Log("퀄리티 변경됨: " + QualitySettings.names[qualityIndex]);
        StartCoroutine(ApplyQualitySettings());
    }

    private IEnumerator ApplyQualitySettings()
    {
        yield return null;
        QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel(), true);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt(FullScreenKey, isFullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(ResolutionKey, resolutionIndex);
        PlayerPrefs.Save();
    }

    // 저장 버튼을 위한 함수 (여기서는 추가 저장 명령)
    public void SaveSettings()
    {
        PlayerPrefs.Save();
        Debug.Log("설정이 저장되었습니다.");
    }

    // 설정 초기화(리셋) 기능: 저장된 모든 설정을 기본값으로 변경
    public void ResetSettings()
    {
        // PlayerPrefs 초기화
        PlayerPrefs.DeleteKey(QualityKey);
        PlayerPrefs.DeleteKey(FullScreenKey);
        PlayerPrefs.DeleteKey(ResolutionKey);
        PlayerPrefs.Save();

        // 기본값으로 다시 설정
        SetQuality(defaultQuality);
        if (qualityDropdown != null)
        {
            qualityDropdown.value = defaultQuality;
            qualityDropdown.RefreshShownValue();
        }

        SetFullScreen(defaultFullScreen);
        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = defaultFullScreen;
        }

        SetResolution(defaultResolutionIndex);
        resolutionDropdown.value = defaultResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        Debug.Log("설정이 기본값으로 초기화되었습니다.");
    }
}