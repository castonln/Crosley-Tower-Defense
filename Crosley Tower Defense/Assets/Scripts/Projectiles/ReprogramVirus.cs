using UnityEngine;

public class ReprogramVirus : FloatingProjectile
{
    [Header("References")]
    [SerializeField] private GameObject reprogrammedBullet;
    [SerializeField] private float selfDestructTimer;

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        Triceracopter triceracopter = collision.transform.parent.gameObject.GetComponent<Triceracopter>();

        if (triceracopter == null || triceracopter.GetIsReprogrammed()) return;
        
        if (triceracopter.GetHealth() - damage * Time.deltaTime > 0)
            triceracopter.TakeDamage(damage * Time.deltaTime);
        else
            triceracopter.Reprogram(reprogrammedBullet, selfDestructTimer);
    }
}
