using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private ShopEntry shopEntry;
    private void Start()
    {
       CheckButtonAffordability();
    }

    public void OnEnable()
    {
        CurrencyManager.OnChangeCurrency += CheckButtonAffordability;
    }

    public void OnDisable()
    {
        CurrencyManager.OnChangeCurrency -= CheckButtonAffordability;
    }

    private void CheckButtonAffordability()
    {
        if (shopEntry.cost > CurrencyManager.main.GetCurrency())
        {
            DisableButton();
        } else
        {
            EnableButton();
        }
    }

    private void EnableButton()
    {
        button.interactable = true;
    }

    private void DisableButton()
    {
        button.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.main.Show(shopEntry.description, shopEntry.cost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.main.Hide();
    }

}