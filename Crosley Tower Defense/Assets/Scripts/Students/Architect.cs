using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Architect : Student
{
    [SerializeField] private int healthPerInterval = 10;
    protected override float GetStrength() => healthPerInterval;
    protected override void DoAction(float health)
    {
        Tower.main.HealDamage((int)health);
    }
    protected override void Update()
    {
        if (!EnemySpawner.main.IsWaveActive()) return;

        timeSinceAction += Time.deltaTime;

        float interval = GetInterval() / speedMultiplier;
        float t = EnemySpawner.main.GetTimeSinceWaveStart();
        float previousT = t - Time.deltaTime;

        if (Mathf.FloorToInt(t / interval) > Mathf.FloorToInt(previousT / interval))
        {
            DoAction(GetStrength() * strengthMultiplier);
            if (CanFlashAction())
                StartCoroutine(FlashActionSprite());
            timeSinceAction = 0f;
        }
    }
}