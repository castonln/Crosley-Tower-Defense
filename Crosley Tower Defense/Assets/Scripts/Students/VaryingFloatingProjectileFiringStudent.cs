using UnityEngine;

public class VaryingFloatingProjectileFiringStudent : FloatingProjectileFiringStudent
{
    [Header("References")]
    [SerializeField] private GameObject[] floatingProjectilePrefabVariants;

    protected override void Shoot(float damage)
    {
        int randomIndex = Random.Range(0, floatingProjectilePrefabVariants.Length);
        floatingProjectilePrefab = floatingProjectilePrefabVariants[randomIndex];
        
        base.Shoot(damage);
    }
}
