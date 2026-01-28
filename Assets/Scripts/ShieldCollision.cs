using System.Collections.Generic;
using UnityEngine;

public class ShieldCollision : MonoBehaviour
{
    [Tooltip("Tags that should be reflected by the shield")]
    public string[] collisionTags;

    [Header("Bounce Tuning")]
    public float speedMultiplier = 1.0f;

    public float pushOut = 0.05f;

    private HashSet<string> tagSet;

    private void OnEnable()
    {
        tagSet = new HashSet<string>();

        if (collisionTags == null) return;

        for (int i = 0; i < collisionTags.Length; i++)
        {
            string tag = collisionTags[i];
            if (!string.IsNullOrEmpty(tag))
                tagSet.Add(tag);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!MatchesTag(other.gameObject)) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb == null) return;

        Vector3 v = rb.linearVelocity;

        if (v.sqrMagnitude < 0.01f) return;

        Vector3 n = (other.transform.position - transform.position).normalized;

        Vector3 reflected = Vector3.Reflect(v, n) * speedMultiplier;

        rb.linearVelocity = reflected;

        other.transform.position += n * pushOut;
    }

    private bool MatchesTag(GameObject go)
    {
        if (tagSet == null || tagSet.Count == 0) return false;

        // Still use CompareTag (fast + validated by Unity)
        foreach (var tag in tagSet)
            if (go.CompareTag(tag))
                return true;

        return false;
    }
}