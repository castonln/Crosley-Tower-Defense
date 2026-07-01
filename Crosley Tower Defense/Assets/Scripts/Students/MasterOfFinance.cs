using UnityEngine;

public class MasterOfFinance : FinancialAdvisor
{
    [SerializeField] private float interestMultiplier;

    protected override float GetStrength() => Mathf.CeilToInt(CurrencyManager.main.GetCurrency() * interestMultiplier);

    protected override void DoAction(float _money)
    {
        base.DoAction(_money);
    }
}