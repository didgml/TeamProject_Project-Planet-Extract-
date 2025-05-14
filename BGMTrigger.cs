using UnityEngine;

public class BGMTrigger : MonoBehaviour
{
    public AudioSource bgmAudioSource;
    public AudioClip outsideBGM;
    public AudioClip dungeonBGM;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (CompareTag("OutsideZone"))
        {
            if (bgmAudioSource.clip != outsideBGM)
            {
                bgmAudioSource.clip = outsideBGM;
                bgmAudioSource.Play();
            }
        }
        else if (CompareTag("DungeonZone"))
        {
            if (bgmAudioSource.clip != dungeonBGM)
            {
                bgmAudioSource.clip = dungeonBGM;
                bgmAudioSource.Play();
            }
        }
    }
}