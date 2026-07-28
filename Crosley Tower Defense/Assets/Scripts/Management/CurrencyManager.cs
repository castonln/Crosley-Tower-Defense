using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager main;

    public static Action OnChangeCurrency;

    [Header("Attributes")]
    [SerializeField] private int currency;

    private int highestCurrency = 0;

    private void Awake()
    {
        main = this;
        OnChangeCurrency += CheckIfHighestCurrency;
    }

    private void Start()
    {
        highestCurrency = currency;
    }

    private void OnDestroy()
    {
        OnChangeCurrency -= CheckIfHighestCurrency;

    }
    private void CheckIfHighestCurrency()
    {
        highestCurrency = currency > highestCurrency ? currency : highestCurrency;
    }

    public int GetHighestCurrency()
    {
        return highestCurrency;
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
