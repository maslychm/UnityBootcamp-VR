using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 4f;

    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();

    public void Launch(Vector3 velocity)
    {
        rb.linearVelocity = velocity;
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision c)
    {
        Destroy(gameObject);
    }
}