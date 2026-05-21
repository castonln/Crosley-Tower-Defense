using UnityEngine;

public class FinancialAdvisor : Student
{
    [SerializeField] protected int moneyPerInterval = 10;
    protected override float GetStrength() => moneyPerInterval;
    protected override void DoAction(float money)
    {
        CurrencyManager.main.IncreaseCurrency((int)moneyPerInterval);
    }
}
