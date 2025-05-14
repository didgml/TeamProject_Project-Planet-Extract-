using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource outsideBGM;
    public AudioSource dungeonBGM;

    public GameObject outsideSoundGroup;  // 야외 사운드 그룹
    public GameObject dungeonSoundGroup;  // 던전 사운드 그룹

    private AudioSource currentBGM;

    private void Awake()
    {
        // 싱글톤 설정
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
        // 게임 시작 시 야외 BGM 및 사운드 활성화
        PlayOutsideBGM();
    }

    public void PlayOutsideBGM()
    {
       
        SetSoundGroup(true);  // 야외 사운드 활성화
        SwitchBGM(outsideBGM);
    }

    public void PlayDungeonBGM()
    {
        SetSoundGroup(false); // 던전 사운드 활성화
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
