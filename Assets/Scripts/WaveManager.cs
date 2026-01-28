using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveManager : MonoBehaviour
{
    public static bool StopAllShooting = false;

    public Transform player;

    [Header("Group Settings")]
    public int groupSize = 3;

    public float groupDuration = 6f;     // how long each group stays active
    public float delayBetweenGroups = 0.3f;

    private List<GameObject> enemies = new List<GameObject>();

    private void Start()
    {
        StopAllShooting = false;

        // Find all shooters (make sure enemies are active ONCE so they can be found)
        EnemyShooter[] shooters = FindObjectsByType<EnemyShooter>(FindObjectsSortMode.None);

        foreach (var s in shooters)
        {
            s.player = player;
            enemies.Add(s.gameObject);
        }

        // Sort by name: enemy, enemy (1), enemy (2)...
        enemies.Sort((a, b) => string.Compare(a.name, b.name));

        // Turn all off initially
        for (int i = 0; i < enemies.Count; i++)
            enemies[i].SetActive(false);

        StartCoroutine(CycleGroupsForever());
    }

    private IEnumerator CycleGroupsForever()
    {
        int index = 0;

        while (!StopAllShooting && enemies.Count > 0)
        {
            // Enable next group
            List<GameObject> current = new List<GameObject>();

            for (int i = 0; i < groupSize; i++)
            {
                GameObject e = enemies[index];
                e.SetActive(true);
                current.Add(e);

                index = (index + 1) % enemies.Count; // wrap around forever
            }

            // Keep them active & shooting for groupDuration
            float t = 0f;
            while (t < groupDuration && !StopAllShooting)
            {
                t += Time.deltaTime;
                yield return null;
            }

            // Disable current group
            for (int i = 0; i < current.Count; i++)
                current[i].SetActive(false);

            // small gap before next group
            float gap = 0f;
            while (gap < delayBetweenGroups && !StopAllShooting)
            {
                gap += Time.deltaTime;
                yield return null;
            }
        }

        // If stopped: disable everyone (optional)
        for (int i = 0; i < enemies.Count; i++)
            if (enemies[i] != null) enemies[i].SetActive(false);
    }

    public void StopShootingNow()
    {
        StopAllShooting = true;
    }
}