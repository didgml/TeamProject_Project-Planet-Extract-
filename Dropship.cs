using UnityEngine;

public class Dropship : MonoBehaviour
{
    public AudioClip landingSound;  // 착륙 소리 클립
    private AudioSource audioSource;  // 오디오 소스 컴포넌트
    private Animation anim;  // Animation 컴포넌트

    void Awake()
    {
        // AudioSource가 없으면 추가
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Animation 컴포넌트 찾기
        anim = GetComponent<Animation>();
    }

    void Start()
    {
        // 애니메이션 재생
        if (anim != null)
        {
            anim.Play("landing");  // "landing" 애니메이션 재생
            Invoke("PlayLandingSound", 2f);  // 2초 뒤에 소리 재생 (애니메이션 타이밍에 맞춰서 조정)
        }
    }

    void PlayLandingSound()
    {
        // 소리 재생 전에 디버깅 로그 출력
        Debug.Log("Landing sound should play now.");

        if (landingSound != null)
        {
            audioSource.PlayOneShot(landingSound);  // 소리 재생
        }
        else
        {
            Debug.LogWarning("Landing sound is not assigned!");
        }
    }
}
