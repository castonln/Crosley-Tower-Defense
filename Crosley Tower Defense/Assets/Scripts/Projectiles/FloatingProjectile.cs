using System.Net;
using TMPro;
using UnityEngine;

public class FloatingProjectile : Projectile
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float projectileSpeed = 5f;

    private Vector2 direction;
    
    public void SetDirection(Vector2 targetPosition)
    {
        direction = (targetPosition - (Vector2)transform.position).normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * projectileSpeed;
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        Triceracopter triceracopter = collision.transform.parent?.gameObject.GetComponent<Triceracopter>();
        if (triceracopter != null) //|| triceracopter.GetIsReprogrammed())
        {
            triceracopter.TakeDamage(damage * Time.deltaTime);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
