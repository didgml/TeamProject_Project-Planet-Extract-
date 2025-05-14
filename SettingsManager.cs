using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private float defaultVolume = 1.0f;  // 기본 볼륨 값
    private bool defaultFullscreen = true; // 기본 전체 화면 값

    private float tempVolume;
    private bool tempFullscreen;

    void Start()
    {
        LoadSettings();
    }

    public void OpenSettings()
    {
        // 설정 창을 열 때 현재 값을 저장 (취소 대비)
        tempVolume = volumeSlider.value;
        tempFullscreen = fullscreenToggle.isOn;
    }

    public void ApplySettings()
    {
        // 적용 (실제 저장 X, 변경된 값만 반영)
        Debug.Log("설정이 적용됨: 볼륨 " + volumeSlider.value + ", 전체 화면 " + fullscreenToggle.isOn);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("설정 저장됨!");
    }

    public void LoadSettings()
    {
        // 저장된 설정 불러오기 (없으면 기본값 사용)
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", defaultVolume);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", defaultFullscreen ? 1 : 0) == 1;
    }

    public void ResetToDefault()
    {
        PlayerPrefs.DeleteAll(); // 모든 설정 초기화
        LoadSettings(); // 기본값 불러오기
        Debug.Log("모든 설정이 기본값으로 초기화됨!");
    }

    public void CancelSettings()
    {
        // 취소 시 기존 값으로 복원
        volumeSlider.value = tempVolume;
        fullscreenToggle.isOn = tempFullscreen;
        Debug.Log("설정 취소됨!");
    }
}
