using System.Collections;
using UnityEngine;
using UnityEngine.LightTransport;
using static UnityEngine.GraphicsBuffer;

public class Triceracopter : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeReference] private SpriteRenderer sr;
    [SerializeField] private Animator triceracopterAnimator;
    [SerializeField] private PolygonCollider2D polyCol;

    [Header("Attributes")]
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float secondsBetweenFiring = 5f;
    [SerializeField] private float health = 25;
    [SerializeField] private int cashOnKill;

    private Color originalColor;

    private Transform shootTarget;
    private Lane lane;

    private bool isReprogrammed = false;
    private float selfDestructTimer = 10f;

    private float timeSinceFiring = 0f;

    private void Start()
    {
        originalColor = sr.color;
    }

    private void Update()
    {
        timeSinceFiring += Time.deltaTime;

        if (timeSinceFiring >= secondsBetweenFiring)
        {
            Shoot();
            timeSinceFiring = 0f;
        }

        if (isReprogrammed)
        {
            selfDestructTimer -= Time.deltaTime;
            if (selfDestructTimer <= 0f) HandleDeath();
        }
    }

    public void SetTarget(Transform _shootTarget)
    {
        shootTarget = _shootTarget;
    }

    public void SetLane(Lane _lane)
    {
        lane = _lane;
    }

    public Lane GetLane()
    {
        return lane;
    }

    private void Shoot()
    {
        if (!shootTarget) return;

        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(shootTarget);
    }

    private IEnumerator FlashDamage()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        sr.color = originalColor;
    }

    public void TakeDamage(float damage)
    {
        if (isReprogrammed) return;

        if (damage >= health)
        {
            HandleDeath();
        } else
        {
            health -= damage;
            StopAllCoroutines();
            StartCoroutine(FlashDamage());
        }
    }

    private void HandleDeath()
    {
        lane.RemoveEnemy(gameObject);
        CurrencyManager.main.IncreaseCurrency(cashOnKill);
        Destroy(polyCol);
        triceracopterAnimator.SetTrigger("IsDead");
    }

    public void OnDeathAnimationComplete()
    {
        Destroy(gameObject);
    }

    public float GetHealth()
    {
        return health;
    }

    // turns enemies into friends
    public void Reprogram(GameObject reprogrammedBullet, float _selfDestructTimer)
    {
        isReprogrammed = true;
        shootTarget = lane.GetSpawnPoint();
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        bulletPrefab = reprogrammedBullet;
        gameObject.GetComponent<EnemyMovement>().Reprogram();
        lane.RemoveEnemy(gameObject);
        selfDestructTimer = _selfDestructTimer;


        gameObject.layer = LayerMask.NameToLayer("Student Allies");
        SetLayerAllChildren(gameObject.transform, gameObject.layer);
        void SetLayerAllChildren(Transform root, int layer)
        {
            var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
            foreach (var child in children)
            {
                child.gameObject.layer = layer;
            }
        }
    }

    public bool GetIsReprogrammed()
    {
        return isReprogrammed;
    }
}
