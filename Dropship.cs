using UnityEngine;

public class Dropship : MonoBehaviour
{
    public AudioClip landingSound;  // ���� �Ҹ� Ŭ��
    private AudioSource audioSource;  // ����� �ҽ� ������Ʈ
    private Animation anim;  // Animation ������Ʈ

    void Awake()
    {
        // AudioSource�� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Animation ������Ʈ ã��
        anim = GetComponent<Animation>();
    }

    void Start()
    {
        // �ִϸ��̼� ���
        if (anim != null)
        {
            anim.Play("landing");  // "landing" �ִϸ��̼� ���
            Invoke("PlayLandingSound", 2f);  // 2�� �ڿ� �Ҹ� ��� (�ִϸ��̼� Ÿ�ֿ̹� ���缭 ����)
        }
    }

    void PlayLandingSound()
    {
        // �Ҹ� ��� ���� ����� �α� ���
        Debug.Log("Landing sound should play now.");

        if (landingSound != null)
        {
            audioSource.PlayOneShot(landingSound);  // �Ҹ� ���
        }
        else
        {
            Debug.LogWarning("Landing sound is not assigned!");
        }
    }
}
