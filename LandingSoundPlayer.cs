using UnityEngine;

public class LandingSoundPlayer : MonoBehaviour
{
    public AudioClip landingClip;  // ����� ���� Ŭ��
    public AnimationClip landingAnimationClip; // ���� �Ҵ��� �ִϸ��̼� Ŭ��

    private AudioSource audioSource;
    private Animation animationComponent;
    private bool hasPlayedSound = false;  // ù ��° ��� ���� üũ
    private float soundDuration;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animationComponent = GetComponent<Animation>(); // Animation ������Ʈ ����

        // �ִϸ��̼� Ŭ���� Animation ������Ʈ�� �Ҵ�
        if (landingAnimationClip != null)
        {
            animationComponent.AddClip(landingAnimationClip, "Landing"); // "Landing" �̸����� �ִϸ��̼� �߰�
        }

        // �Ҹ� ���̸� Ŭ������ ����
        if (landingClip != null)
        {
            soundDuration = landingClip.length;
        }

        // �� �� �̵� �ÿ��� �� ������Ʈ�� �������� �ʵ���
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        // �ִϸ��̼��� ������, �Ҹ��� �� ���� ������� �ʾҴٸ� �Ҹ� ���
        if (!animationComponent.isPlaying && !hasPlayedSound)
        {
            PlayLandingSound();  // �ִϸ��̼��� ���� �� �Ҹ� ���
        }
    }

    void PlayLandingSound()
    {
        if (landingClip != null && !hasPlayedSound)  // �Ҹ��� ���� ������� ���� ��쿡�� ����
        {
            audioSource.clip = landingClip;  // �Ҹ� Ŭ�� �Ҵ�
            audioSource.Play(); // �Ҹ� ���
            audioSource.loop = false;  // �Ҹ��� �ݺ����� �ʵ��� ����
            hasPlayedSound = true;  // �Ҹ��� �̹� ����Ǿ����� ���

            // �Ҹ� ���̸� �ִϸ��̼� ���̿� �°� ���߱� ����, �ִϸ��̼��� ������ ���� �Ҹ��� ������ ����
            Invoke("StopSound", landingAnimationClip.length); // �ִϸ��̼� ���̸�ŭ �Ŀ� �Ҹ� ����
        }
    }

    // �Ҹ� ���� �Լ�
    void StopSound()
    {
        audioSource.Stop();  // �Ҹ� ����
    }
}
