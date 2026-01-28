using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastBlaster : MonoBehaviour
{
    public float range = 100f;

    public void Blast(ActivateEventArgs _)
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, -transform.up, range);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.Log("Hit enemy: " + hit.collider.name);
                hit.transform.GetComponent<EnemyShooter>()?.TakeDamage();
            }
        }
    }
}