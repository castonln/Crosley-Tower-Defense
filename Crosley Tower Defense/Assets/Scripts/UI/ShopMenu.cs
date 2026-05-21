using TMPro;
using UnityEngine;

public class ShopMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI currencyUI;

    private void OnGUI()
    {
        currencyUI.text = CurrencyManager.main.GetCurrency().ToString();
    }

    private void EnableShopMenu()
    {
        gameObject.SetActive(true);
    }

    private void DisableShopMenu()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        BuildManager.OnSelectMoveStudent += DisableShopMenu;
        BuildManager.OnDeselectMoveStudent += EnableShopMenu;
    }

    private void OnDestroy()
    {
        BuildManager.OnSelectMoveStudent -= DisableShopMenu;
        BuildManager.OnDeselectMoveStudent -= EnableShopMenu;

    }
}
