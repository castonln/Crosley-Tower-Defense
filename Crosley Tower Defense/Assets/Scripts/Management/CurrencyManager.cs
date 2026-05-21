using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager main;

    public static Action OnChangeCurrency;

    [Header("Attributes")]
    [SerializeField] private int currency;

    private void Awake()
    {
        main = this;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
        OnChangeCurrency?.Invoke();
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            OnChangeCurrency?.Invoke();
            return true;
        }
        else
        {
            Debug.Log("Can't buy");
            return false;
        }
    }

    public int GetCurrency()
    {
        return currency;
    }
}
