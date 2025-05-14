using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSettingsUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider xSensitivitySlider;
    public Slider ySensitivitySlider;
    public TextMeshProUGUI xValueText;
    public TextMeshProUGUI yValueText;

    // PlayerPrefs 키
    private const string XSensitivityKey = "xSensitivity";
    private const string YSensitivityKey = "ySensitivity";

    // 기본 감도(초기값)를 저장할 변수 (초기화 기능용)
    private float defaultXSensitivity;
    private float defaultYSensitivity;

    void Start()
    {
        // MouseLook 인스턴스가 있다면 현재 감도를 불러옴.
        if (MouseLook.instance != null)
        {
            Vector2 currentSensitivity = MouseLook.instance.GetSensitivity();
            // 기본 감도값 저장 (초기화 시 사용)
            defaultXSensitivity = currentSensitivity.x;
            defaultYSensitivity = currentSensitivity.y;
        }
        else
        {
            // MouseLook 인스턴스가 없을 때의 기본값 (필요에 따라 수정)
            defaultXSensitivity = 1f;
            defaultYSensitivity = 1f;
        }

        // 저장된 값이 있으면 불러오고, 없으면 기본값으로 초기화
        float savedX = PlayerPrefs.GetFloat(XSensitivityKey, defaultXSensitivity);
        float savedY = PlayerPrefs.GetFloat(YSensitivityKey, defaultYSensitivity);

        // 슬라이더에 적용 및 텍스트 업데이트
        xSensitivitySlider.value = savedX;
        ySensitivitySlider.value = savedY;
        xValueText.text = savedX.ToString("F1");
        yValueText.text = savedY.ToString("F1");

        // MouseLook 설정 적용
        if (MouseLook.instance != null)
        {
            MouseLook.instance.SetSensitivity(savedX, savedY);
        }

        // 슬라이더 값이 바뀔 때마다 실시간으로 적용
        xSensitivitySlider.onValueChanged.AddListener(delegate { ApplySettings(); });
        ySensitivitySlider.onValueChanged.AddListener(delegate { ApplySettings(); });
    }

    // 슬라이더가 변경되었을 때 호출
    void ApplySettings()
    {
        float newX = xSensitivitySlider.value;
        float newY = ySensitivitySlider.value;

        // UI 텍스트 업데이트
        xValueText.text = newX.ToString("F1");
        yValueText.text = newY.ToString("F1");

        // 적용
        if (MouseLook.instance != null)
        {
            MouseLook.instance.SetSensitivity(newX, newY);
        }
    }

    // Save 버튼에 연결할 함수: 현재 감도값을 PlayerPrefs에 저장
    public void SaveMouseSettings()
    {
        PlayerPrefs.SetFloat(XSensitivityKey, xSensitivitySlider.value);
        PlayerPrefs.SetFloat(YSensitivityKey, ySensitivitySlider.value);
        PlayerPrefs.Save();
        Debug.Log("마우스 감도 설정이 저장되었습니다.");
    }

    // Reset 버튼에 연결할 함수: 저장된 값을 삭제하고 기본값으로 복원
    public void ResetMouseSettings()
    {
        // 저장된 값 삭제
        PlayerPrefs.DeleteKey(XSensitivityKey);
        PlayerPrefs.DeleteKey(YSensitivityKey);
        PlayerPrefs.Save();

        // 기본값으로 다시 설정
        xSensitivitySlider.value = defaultXSensitivity;
        ySensitivitySlider.value = defaultYSensitivity;
        xValueText.text = defaultXSensitivity.ToString("F1");
        yValueText.text = defaultYSensitivity.ToString("F1");

        if (MouseLook.instance != null)
        {
            MouseLook.instance.SetSensitivity(defaultXSensitivity, defaultYSensitivity);
        }

        Debug.Log("마우스 감도 설정이 기본값으로 초기화되었습니다.");
    }
}