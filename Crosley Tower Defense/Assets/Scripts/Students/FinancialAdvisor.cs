using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FinancialAdvisor : Student
{
    [Header("References")]
    [SerializeField] protected int moneyPerInterval = 10;

    [Header("Money Attributes")]
    [SerializeField] private Transform moneySpawnPoint;
    [SerializeField] private GameObject moneyPrefab;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifespan = 1f;

    private GameObject money;
    private float currentLifetime = 0f;

    protected override float GetStrength() => moneyPerInterval;
    protected override void DoAction(float money)
    {
        print("Money gained");
        CurrencyManager.main.IncreaseCurrency((int)money);
        SpawnMoney(); // could override FlashActionSprite but i dont want to figure out coroutine nonsense
    }

    private void SpawnMoney()
    {
        print("Money Spawned");
        Vector3 moneyPosition = moneySpawnPoint.position;
        moneyPosition.x += Random.Range(-0.222f, 0.222f);
        print("Money Position: " + moneyPosition.x);
        money = Instantiate(moneyPrefab, moneyPosition, moneySpawnPoint.rotation);
    }

    protected override void Update()
    {
        base.Update();

        if (money != null) { 
            money.transform.Translate(Vector3.up * speed * Time.deltaTime);
            currentLifetime += Time.deltaTime;
        }

        if (currentLifetime >= lifespan) {
            Destroy(money);
            print("Money Destroyed");
            currentLifetime = 0f;
        }
    }
}
