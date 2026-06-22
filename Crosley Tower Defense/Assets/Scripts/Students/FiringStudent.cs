using UnityEngine;

public abstract class FiringStudent : Student
{
    [Header("Attributes")]
    [SerializeField] protected Transform firingPoint;

    protected LayerMask laneMask;
    protected Transform target;
    protected float timeSinceFire = 0f;

    private void Start()
    {
        SetLaneMask(gameObject.GetComponentInParent<Plot>().GetLaneMask());
    }

    private void OnTransformParentChanged()
    {
        if (transform.parent == null) return; 

        SetLaneMask(gameObject.GetComponentInParent<Plot>().GetLaneMask());
    }

    public void SetLaneMask(LayerMask _laneMask)
    {
        laneMask = _laneMask;
        FindTarget();
    }

    public LayerMask GetLaneMask() { return laneMask; }

    public void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 15f, (Vector2)transform.position, 0f, laneMask);
        target = hits.Length > 0 ? hits[0].transform : null;
    }

    public Transform GetTarget() { return target; }

    protected override void DoAction(float damage)
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        Shoot(damage);
    }

    protected abstract void Shoot(float damage);
}