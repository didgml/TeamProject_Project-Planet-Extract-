using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    private float defaultVolume = 1.0f;  // �⺻ ���� ��
    private bool defaultFullscreen = true; // �⺻ ��ü ȭ�� ��

    private float tempVolume;
    private bool tempFullscreen;

    void Start()
    {
        LoadSettings();
    }

    public void OpenSettings()
    {
        // ���� â�� �� �� ���� ���� ���� (��� ���)
        tempVolume = volumeSlider.value;
        tempFullscreen = fullscreenToggle.isOn;
    }

    public void ApplySettings()
    {
        // ���� (���� ���� X, ����� ���� �ݿ�)
        Debug.Log("������ �����: ���� " + volumeSlider.value + ", ��ü ȭ�� " + fullscreenToggle.isOn);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("���� �����!");
    }

    public void LoadSettings()
    {
        // ����� ���� �ҷ����� (������ �⺻�� ���)
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", defaultVolume);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", defaultFullscreen ? 1 : 0) == 1;
    }

    public void ResetToDefault()
    {
        PlayerPrefs.DeleteAll(); // ��� ���� �ʱ�ȭ
        LoadSettings(); // �⺻�� �ҷ�����
        Debug.Log("��� ������ �⺻������ �ʱ�ȭ��!");
    }

    public void CancelSettings()
    {
        // ��� �� ���� ������ ����
        volumeSlider.value = tempVolume;
        fullscreenToggle.isOn = tempFullscreen;
        Debug.Log("���� ��ҵ�!");
    }
}
