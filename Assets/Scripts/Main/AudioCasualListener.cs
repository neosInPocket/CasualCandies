using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioCasualListener : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        List<AudioCasualListener> audioCasualListeners = FindObjectsByType<AudioCasualListener>(sortMode: FindObjectsSortMode.None).ToList();

        if (audioCasualListeners.Count > 1 || audioCasualListeners.Count < 1)
        {
            var audioCasualListener = audioCasualListeners.FirstOrDefault(x => x.gameObject.scene.name != "DontDestroyOnLoad");
            Destroy(audioCasualListener.gameObject);

        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetListener(int enabledMusic)
    {
        Conserver.musicEffects = enabledMusic;
        Conserver.Save();

        musicSource.volume = enabledMusic != 1 ? 0f : 1f;
    }
}
