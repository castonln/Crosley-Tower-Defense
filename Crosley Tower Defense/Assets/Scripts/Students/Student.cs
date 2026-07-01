using UnityEngine;
using System.Collections;

public abstract class Student : MonoBehaviour
{
    [SerializeField] protected float secondsPerInterval = 1f;
    [SerializeField] private Sprite actionSprite;               // the "acting" sprite
    [SerializeField] private float actionSpriteDuration = 0.1f; // how long it shows

    private float timeSinceAction = 0f;
    protected float speedMultiplier = 1f;
    protected float strengthMultiplier = 1f;

    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            defaultSprite = spriteRenderer.sprite;
    }
    protected abstract float GetStrength();
    protected virtual float GetInterval() => secondsPerInterval;
    protected virtual bool CanFlashAction() => true;

    private void Update()
    {
        if (!EnemySpawner.main.IsWaveActive()) return;

        timeSinceAction += Time.deltaTime;
        if (timeSinceAction >= GetInterval() / speedMultiplier)
        {
            DoAction(GetStrength() * strengthMultiplier);

            if (CanFlashAction())
                StartCoroutine(FlashActionSprite());

            timeSinceAction = 0f;
        }
    }

    private IEnumerator FlashActionSprite()
    {
        if (spriteRenderer == null || actionSprite == null) yield break;

        spriteRenderer.sprite = actionSprite;
        yield return new WaitForSeconds(actionSpriteDuration);
        spriteRenderer.sprite = defaultSprite;
    }

    public void SetMultipliers(LaneMultipliers _multipliers)
    {
        strengthMultiplier = _multipliers.strength;
        speedMultiplier = _multipliers.speed;
        Debug.Log(gameObject.name + " Multi: " + strengthMultiplier);
    }

    protected abstract void DoAction(float strength);
}