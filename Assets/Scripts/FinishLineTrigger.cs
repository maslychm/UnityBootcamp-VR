using System.Collections;
using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    private AudioSource audioSource;
    private bool triggered = false;

    // Optional: if you have a manager reference, drag it in. Otherwise we’ll just use the static flag.
    public WaveManager waveManager;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        // Stop all enemy shooting
        if (waveManager != null) waveManager.StopShootingNow();
        else WaveManager.StopAllShooting = true;

        // Stop ALL other audios except this one
        StopAllOtherAudioSources();

        // Play finish sound
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.Play();
            StartCoroutine(QuitAfterAudio());
        }
        else
        {
            // No clip? Just end immediately
            QuitNow();
        }
    }

    private void StopAllOtherAudioSources()
    {
        // Includes inactive objects too
        var sources = FindObjectsByType<AudioSource>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var src in sources)
        {
            if (src == null) continue;
            if (src == audioSource) continue;
            src.Stop();
        }
    }

    private IEnumerator QuitAfterAudio()
    {
        // Wait until audio finishes (unscaled so it works even if timeScale changes)
        while (audioSource != null && audioSource.isPlaying)
            yield return null;

        QuitNow();
    }

    private void QuitNow()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}