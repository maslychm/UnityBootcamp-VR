using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public Transform player;
    public GameObject bulletPrefab;

    public float bulletSpeed = 18f;
    public float aimHeight = 1.0f;
    public float shootInterval = 1.0f;

    private float nextShotTime;

    private void OnEnable()
    {
        // shoot immediately (optional)
        nextShotTime = Time.time + Random.Range(0f, 0.25f);
    }

    private void Update()
    {
        if (waveManager.StopAllShooting) return;
        if (!isActiveAndEnabled) return;

        if (player == null || bulletPrefab == null) return;

        if (Time.time >= nextShotTime)
        {
            nextShotTime = Time.time + shootInterval;
            FireOnce();
        }
    }

    private void FireOnce()
    {
        Vector3 firePos = transform.position + Vector3.up * 1.0f + transform.forward * 1f;
        Vector3 targetPos = player.position + Vector3.up * aimHeight;

        Vector3 dir = (targetPos - firePos).normalized;

        GameObject b = Instantiate(bulletPrefab, firePos, Quaternion.LookRotation(dir));
        var rb = b.GetComponent<Rigidbody>();
        if (rb != null) rb.linearVelocity = dir * bulletSpeed;
    }
}