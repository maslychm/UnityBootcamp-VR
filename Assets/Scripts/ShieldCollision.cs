using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    [Tooltip("Tag that should be reflected by the shield")]
    [SerializeField] private string reflectThisTag;

    [Header("Bounce Tuning")]
    public float speedMultiplier = 1.0f;

    public float pushOut = 0.05f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(reflectThisTag)) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 v = rb.linearVelocity;

        if (v.sqrMagnitude < 0.01f) return;

        Vector3 n = (other.transform.position - transform.position).normalized;

        Vector3 reflected = Vector3.Reflect(v, n) * speedMultiplier;

        rb.linearVelocity = reflected;

        other.transform.position += n * pushOut;
    }
}