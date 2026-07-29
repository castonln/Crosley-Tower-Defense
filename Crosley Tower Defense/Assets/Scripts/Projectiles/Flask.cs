using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Flask : CollidingProjectile
{
    [Header("References")]
    [SerializeField] private Transform collisionPoint;
    [SerializeField] private GameObject particlesPrefab;

    [Header("Attributes")]
    [SerializeField] private float splashRadius = 10f;
    [SerializeField] private float shatterAngularVelocity = 100f;
    [SerializeField] private GameObject brokenFlaskPrefab;
    [SerializeField] private float breakingForce = 4f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, splashRadius);
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided) return;
        hasCollided = true;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, splashRadius);

        foreach (Collider2D col in colliders)
        {
            Triceracopter triceracopter = col.gameObject.GetComponentInParent<Triceracopter>();
            if (triceracopter != null)
            {
                triceracopter.TakeDamage(damage);
            }
        }

        HandleBreak();
    }

    private void HandleBreak()
    {
        GameObject brokenFlask = Instantiate(brokenFlaskPrefab, transform.position, Quaternion.identity);
        Rigidbody2D[] brokenFlaskRbs = brokenFlask.GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D rb in brokenFlaskRbs)
        {
            Vector2 direction = (rb.transform.position - collisionPoint.position).normalized;
            Vector2 push = direction * breakingForce;
            rb.AddForce(push, ForceMode2D.Impulse);
            rb.angularVelocity = shatterAngularVelocity;
        }

        Instantiate(particlesPrefab, collisionPoint.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
