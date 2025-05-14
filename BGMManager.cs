using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource outsideBGM;
    public AudioSource dungeonBGM;

    public GameObject outsideSoundGroup;  // �߿� ���� �׷�
    public GameObject dungeonSoundGroup;  // ���� ���� �׷�

    private AudioSource currentBGM;

    private void Awake()
    {
        // �̱��� ����
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // ���� ���� �� �߿� BGM �� ���� Ȱ��ȭ
        PlayOutsideBGM();
    }

    public void PlayOutsideBGM()
    {
       
        SetSoundGroup(true);  // �߿� ���� Ȱ��ȭ
        SwitchBGM(outsideBGM);
    }

    public void PlayDungeonBGM()
    {
        SetSoundGroup(false); // ���� ���� Ȱ��ȭ
        SwitchBGM(dungeonBGM);
    }

    private void SwitchBGM(AudioSource newBGM)
    {
        if (currentBGM == newBGM) return;

        if (currentBGM != null)
            currentBGM.Stop();

        currentBGM = newBGM;
        currentBGM.Play();
    }

    private void SetSoundGroup(bool isOutside)
    {
        if (outsideSoundGroup != null)
            outsideSoundGroup.SetActive(isOutside);

        if (dungeonSoundGroup != null)
            dungeonSoundGroup.SetActive(!isOutside);
    }
}
