using UnityEngine;

public class ProjectileFiringStudent : FiringStudent
{
    [SerializeField] private float firingVelocity = 15f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int damage = 10;
    [SerializeField] private float angularVelocity = 174f;
    protected override float GetStrength() => damage;

    private float g = 9.81f;

    protected override void Shoot(float damage)
    {
        float x = Mathf.Abs(firingPoint.position.x - target.position.x);
        float y = target.position.y - firingPoint.position.y;

        float v2 = firingVelocity * firingVelocity;
        float v4 = v2 * v2;
        float x2 = x * x;
        float sq = v4 - g * ((g * x2) + (2f * y * v2));

        if (sq < 0f)
        {
            Debug.Log(sq);
            return;
        }

        float firingAngle = Mathf.Atan2(v2 - Mathf.Sqrt(sq), g * x);

        GameObject projObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        projObj.GetComponent<Projectile>().SetDamage(damage);

        Rigidbody2D rb = projObj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direction = target.position.x > firingPoint.position.x
                ? Vector2.right : Vector2.left;
            rb.linearVelocity = (direction * Mathf.Cos(firingAngle)
                               + Vector2.up * Mathf.Sin(firingAngle))
                               * firingVelocity;
            rb.angularVelocity = angularVelocity;
        }

        print(rb.angularVelocity);
    }

}
