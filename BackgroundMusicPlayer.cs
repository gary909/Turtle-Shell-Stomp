using UnityEngine;
using MoreMountains.Tools;

public class BackgroundMusicPlayer : MonoBehaviour
{
    public AudioClip backgroundMusic;

    void Start()
    {
        MMSoundManagerSoundPlayEvent.Trigger(
            backgroundMusic,
            MMSoundManager.MMSoundManagerTracks.Music, // Track ID
            loop: true,
            volume: 1f,
            pitch: 1f,
            panStereo: 0f,
            spatialBlend: 0f,
            priority: 128,
            fade: true,
            fadeDuration: 2f,
            recycleAudioSource: true,
            persistent: true // This keeps it across scene loads if needed
        );
    }
}
