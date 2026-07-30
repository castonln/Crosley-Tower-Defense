using UnityEngine;

public class FloatingProjectileFiringStudent : FiringStudent
{
    [Header("Atttributes")]
    [SerializeField] private int damagePerSecond = 10;
    [SerializeField] private int downwardCorrection = 0;
    protected override float GetStrength() => damagePerSecond;

    [Header("References")]
    [SerializeField] protected GameObject floatingProjectilePrefab;

    protected override void Shoot(float damage)
    {
        FloatingProjectile projectile = Instantiate(floatingProjectilePrefab, firingPoint.position, Quaternion.identity)
            .GetComponent<FloatingProjectile>();
        projectile.SetDamage(damage);

        Vector3 direction = target.position;
        direction.y -= downwardCorrection;

        projectile.SetDirection(direction);
    }
}