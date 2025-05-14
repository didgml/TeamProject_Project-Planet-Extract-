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

    // PlayerPrefs Ű
    private const string XSensitivityKey = "xSensitivity";
    private const string YSensitivityKey = "ySensitivity";

    // �⺻ ����(�ʱⰪ)�� ������ ���� (�ʱ�ȭ ��ɿ�)
    private float defaultXSensitivity;
    private float defaultYSensitivity;

    void Start()
    {
        // MouseLook �ν��Ͻ��� �ִٸ� ���� ������ �ҷ���.
        if (MouseLook.instance != null)
        {
            Vector2 currentSensitivity = MouseLook.instance.GetSensitivity();
            // �⺻ ������ ���� (�ʱ�ȭ �� ���)
            defaultXSensitivity = currentSensitivity.x;
            defaultYSensitivity = currentSensitivity.y;
        }
        else
        {
            // MouseLook �ν��Ͻ��� ���� ���� �⺻�� (�ʿ信 ���� ����)
            defaultXSensitivity = 1f;
            defaultYSensitivity = 1f;
        }

        // ����� ���� ������ �ҷ�����, ������ �⺻������ �ʱ�ȭ
        float savedX = PlayerPrefs.GetFloat(XSensitivityKey, defaultXSensitivity);
        float savedY = PlayerPrefs.GetFloat(YSensitivityKey, defaultYSensitivity);

        // �����̴��� ���� �� �ؽ�Ʈ ������Ʈ
        xSensitivitySlider.value = savedX;
        ySensitivitySlider.value = savedY;
        xValueText.text = savedX.ToString("F1");
        yValueText.text = savedY.ToString("F1");

        // MouseLook ���� ����
        if (MouseLook.instance != null)
        {
            MouseLook.instance.SetSensitivity(savedX, savedY);
        }

        // �����̴� ���� �ٲ� ������ �ǽð����� ����
        xSensitivitySlider.onValueChanged.AddListener(delegate { ApplySettings(); });
        ySensitivitySlider.onValueChanged.AddListener(delegate { ApplySettings(); });
    }

    // �����̴��� ����Ǿ��� �� ȣ��
    void ApplySettings()
    {
        float newX = xSensitivitySlider.value;
        float newY = ySensitivitySlider.value;

        // UI �ؽ�Ʈ ������Ʈ
        xValueText.text = newX.ToString("F1");
        yValueText.text = newY.ToString("F1");

        // ����
        if (MouseLook.instance != null)
        {
            MouseLook.instance.SetSensitivity(newX, newY);
        }
    }

    // Save ��ư�� ������ �Լ�: ���� �������� PlayerPrefs�� ����
    public void SaveMouseSettings()
    {
        PlayerPrefs.SetFloat(XSensitivityKey, xSensitivitySlider.value);
        PlayerPrefs.SetFloat(YSensitivityKey, ySensitivitySlider.value);
        PlayerPrefs.Save();
        Debug.Log("���콺 ���� ������ ����Ǿ����ϴ�.");
    }

    // Reset ��ư�� ������ �Լ�: ����� ���� �����ϰ� �⺻������ ����
    public void ResetMouseSettings()
    {
        // ����� �� ����
        PlayerPrefs.DeleteKey(XSensitivityKey);
        PlayerPrefs.DeleteKey(YSensitivityKey);
        PlayerPrefs.Save();

        // �⺻������ �ٽ� ����
        xSensitivitySlider.value = defaultXSensitivity;
        ySensitivitySlider.value = defaultYSensitivity;
        xValueText.text = defaultXSensitivity.ToString("F1");
        yValueText.text = defaultYSensitivity.ToString("F1");

        if (MouseLook.instance != null)
        {
            MouseLook.instance.SetSensitivity(defaultXSensitivity, defaultYSensitivity);
        }

        Debug.Log("���콺 ���� ������ �⺻������ �ʱ�ȭ�Ǿ����ϴ�.");
    }
}