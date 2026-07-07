using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform centerPoint;

    [Header("Attributes")]
    [SerializeField] private float baseSpeed = 1.5f;
    [SerializeField] private float enemyDetectionRange = 2f;

    private Transform moveTarget;

    private float moveSpeed;

    private void Start()
    {
        moveSpeed = baseSpeed;
        moveTarget = gameObject.GetComponent<Triceracopter>().GetLane().GetTargetPoint(gameObject);
    }

    private void FixedUpdate()
    {
        if (GetXDistanceToTarget() < enemyDetectionRange)
        {
            moveSpeed = 0;
        } else
        {
            moveSpeed = baseSpeed;
        }

        Vector2 direction = new Vector2(moveTarget.position.x - centerPoint.position.x, 0f).normalized;

        rb.linearVelocity = direction * moveSpeed;

    }

    private float GetXDistanceToTarget()
    {
        return Mathf.Abs(moveTarget.position.x - centerPoint.position.x);
    }

    public void SetTarget(Transform targetPoint)
    {
        moveTarget = targetPoint;
    }

    public void Reprogram()
    {
        Transform reprogrammedTarget = new GameObject("reprogrammedTargetGameObj").transform;
        // left lane
        if (moveTarget.position.x < 0)
        {
            reprogrammedTarget.position = new Vector3(
                moveTarget.position.x + 1.83f, 
                moveTarget.position.y, 
                moveTarget.position.z
            );
        }
        // right lane
        else
        {
            reprogrammedTarget.position = new Vector3(
                moveTarget.position.x - 1.83f,
                moveTarget.position.y,
                moveTarget.position.z
            );
        }

        SetTarget(reprogrammedTarget);
    }
}
