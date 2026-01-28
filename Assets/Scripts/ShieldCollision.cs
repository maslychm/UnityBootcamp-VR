using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    [Tooltip("Tags that should be reflected by the shield (ex: Laser)")]
    public string[] collisionTags;

    [Header("Bounce Tuning")]
    public float speedMultiplier = 1.0f;   // 1 = same speed, >1 faster after bounce

    public float pushOut = 0.05f;          // prevents instant re-trigger inside shield

    private void OnTriggerEnter(Collider other)
    {
        if (!MatchesTag(other.gameObject)) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 v = rb.linearVelocity;
        if (v.sqrMagnitude < 0.01f) return;

        // Approx collision normal for trigger: from shield center -> bullet
        Vector3 n = (other.transform.position - transform.position).normalized;

        // Reflect velocity
        Vector3 reflected = Vector3.Reflect(v, n) * speedMultiplier;
        rb.linearVelocity = reflected;

        // Push bullet slightly outside the shield so it doesn't re-trigger immediately
        other.transform.position += n * pushOut;
    }

    private bool MatchesTag(GameObject go)
    {
        if (collisionTags == null) return false;
        for (int i = 0; i < collisionTags.Length; i++)
            if (!string.IsNullOrEmpty(collisionTags[i]) && go.CompareTag(collisionTags[i]))
                return true;
        return false;
    }
}