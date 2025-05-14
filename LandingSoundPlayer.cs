using UnityEngine;

public class LandingSoundPlayer : MonoBehaviour
{
    public AudioClip landingClip;  // 재생할 사운드 클립
    public AnimationClip landingAnimationClip; // 직접 할당할 애니메이션 클립

    private AudioSource audioSource;
    private Animation animationComponent;
    private bool hasPlayedSound = false;  // 첫 번째 재생 여부 체크
    private float soundDuration;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animationComponent = GetComponent<Animation>(); // Animation 컴포넌트 참조

        // 애니메이션 클립을 Animation 컴포넌트에 할당
        if (landingAnimationClip != null)
        {
            animationComponent.AddClip(landingAnimationClip, "Landing"); // "Landing" 이름으로 애니메이션 추가
        }

        // 소리 길이를 클립에서 추출
        if (landingClip != null)
        {
            soundDuration = landingClip.length;
        }

        // 씬 간 이동 시에도 이 오브젝트가 삭제되지 않도록
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // 애니메이션이 끝났고, 소리가 한 번도 재생되지 않았다면 소리 재생
        if (!animationComponent.isPlaying && !hasPlayedSound)
        {
            PlayLandingSound();  // 애니메이션이 끝난 후 소리 재생
        }
    }

    void PlayLandingSound()
    {
        if (landingClip != null && !hasPlayedSound)  // 소리가 아직 재생되지 않은 경우에만 실행
        {
            audioSource.clip = landingClip;  // 소리 클립 할당
            audioSource.Play(); // 소리 재생
            audioSource.loop = false;  // 소리가 반복되지 않도록 설정
            hasPlayedSound = true;  // 소리가 이미 재생되었음을 기록

            // 소리 길이를 애니메이션 길이에 맞게 맞추기 위해, 애니메이션이 끝나기 전에 소리도 끝나게 설정
            Invoke("StopSound", landingAnimationClip.length); // 애니메이션 길이만큼 후에 소리 중지
        }
    }

    // 소리 중지 함수
    void StopSound()
    {
        audioSource.Stop();  // 소리 중지
    }
}
