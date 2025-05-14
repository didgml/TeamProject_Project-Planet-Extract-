using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class AudioSettingsUI : MonoBehaviour
{
    [Header("Audio Settings UI")]
    public Slider bgmSlider;               // 배경음악 슬라이더
    public Slider sfxSlider;               // 효과음 슬라이더
    public TextMeshProUGUI bgmValueText;     // 배경음악 값 표시 텍스트
    public TextMeshProUGUI sfxValueText;     // 효과음 값 표시 텍스트
    public AudioMixer audioMixer;          // Inspector에서 할당할 AudioMixer

    // AudioMixer의 노출 파라미터 이름 (AudioMixer에서 Expose 시 지정한 이름)
    private const string BGMMixerParam = "Music";
    private const string SFXMixerParam = "Sfx";

    // 기본값 (초기화 시 사용 - 필요에 따라 수정)
    private float defaultBGMVolume = 1f;
    private float defaultSFXVolume = 1f;

    void Start()
    {
        // 저장된 값 불러오기, 없으면 기본값 사용 (PlayerPrefs 키로 저장)
        float savedBGM = PlayerPrefs.GetFloat(BGMMixerParam, defaultBGMVolume);
        float savedSFX = PlayerPrefs.GetFloat(SFXMixerParam, defaultSFXVolume);

        bgmSlider.value = savedBGM;
        sfxSlider.value = savedSFX;
        bgmValueText.text = savedBGM.ToString("F2");
        sfxValueText.text = savedSFX.ToString("F2");

        // AudioMixer에 적용
        SetBGMVolume(savedBGM);
        SetSFXVolume(savedSFX);

        // 슬라이더 값이 바뀔 때마다 실시간 적용
        bgmSlider.onValueChanged.AddListener(delegate { ApplyAudioSettings(); });
        sfxSlider.onValueChanged.AddListener(delegate { ApplyAudioSettings(); });
    }

    // 슬라이더 값 변경 시 호출 – UI 표시와 AudioMixer에 적용
    public void ApplyAudioSettings()
    {
        float newBGM = bgmSlider.value;
        float newSFX = sfxSlider.value;
        bgmValueText.text = newBGM.ToString("F2");
        sfxValueText.text = newSFX.ToString("F2");

        SetBGMVolume(newBGM);
        SetSFXVolume(newSFX);
    }

    // 배경음악 볼륨 적용 (슬라이더 값 0~1를 dB로 변환)
    private void SetBGMVolume(float volume)
    {
        // 0인 경우 0에 가까운 값으로 대체하여 로그 변환 에러 방지
        if (volume <= 0f)
            volume = 0.0001f;
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(BGMMixerParam, dB);
    }

    // 효과음 볼륨 적용
    private void SetSFXVolume(float volume)
    {
        if (volume <= 0f)
            volume = 0.0001f;
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat(SFXMixerParam, dB);
    }

    // Save 버튼에 연결할 함수: 현재 슬라이더 값을 PlayerPrefs에 저장
    public void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(BGMMixerParam, bgmSlider.value);
        PlayerPrefs.SetFloat(SFXMixerParam, sfxSlider.value);
        PlayerPrefs.Save();
        Debug.Log("오디오 설정이 저장되었습니다.");
    }

    // Reset 버튼에 연결할 함수: 저장된 값을 삭제하고 기본값으로 복원
    public void ResetAudioSettings()
    {
        PlayerPrefs.DeleteKey(BGMMixerParam);
        PlayerPrefs.DeleteKey(SFXMixerParam);
        PlayerPrefs.Save();

        bgmSlider.value = defaultBGMVolume;
        sfxSlider.value = defaultSFXVolume;
        bgmValueText.text = defaultBGMVolume.ToString("F2");
        sfxValueText.text = defaultSFXVolume.ToString("F2");

        SetBGMVolume(defaultBGMVolume);
        SetSFXVolume(defaultSFXVolume);

        Debug.Log("오디오 설정이 기본값으로 초기화되었습니다.");
    }
}