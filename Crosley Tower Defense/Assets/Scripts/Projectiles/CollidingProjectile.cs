using UnityEngine;

public class CollidingProjectile : Projectile
{
    protected bool hasCollided = false;

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasCollided) return;

        Triceracopter triceracopter = collision.gameObject.GetComponent<Triceracopter>();
        if (triceracopter != null)
        {
            triceracopter.TakeDamage(damage);
            hasCollided = true;
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
