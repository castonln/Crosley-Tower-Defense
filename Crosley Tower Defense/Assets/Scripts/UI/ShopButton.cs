using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Toggle toggle;
    [SerializeField] private ShopEntry shopEntry;

    private void Start()
    {
       CurrencyManager.OnChangeCurrency += CheckButtonAffordability;
       CheckButtonAffordability();
    }

    public void OnDestroy()
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
        toggle.interactable = true;
    }

    private void DisableButton()
    {
        toggle.interactable = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.main.Show(shopEntry.description, shopEntry.cost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData is ExtendedPointerEventData extended && extended.pointerType == UIPointerType.Touch)
            return;

        TooltipManager.main.Hide();
    }

    public void HandleClick(string studentFromShop)
    {
        if (BuildManager.main.GetSelectedShopStudent() != shopEntry && toggle.IsInteractable())
        {
            TooltipManager.main.ShowStatic(shopEntry.description, shopEntry.cost, gameObject.transform.position);
            BuildManager.main.SetSelectedStudentFromShop(studentFromShop);
        }
        else
        {
            TooltipManager.main.Hide();
            BuildManager.main.CancelPlacement();
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void HandleDragStart(string studentFromShop)
    {
        if (toggle.IsInteractable())
        {
            TooltipManager.main.ShowStatic(shopEntry.description, shopEntry.cost, gameObject.transform.position);
            BuildManager.main.SetSelectedStudentFromShop(studentFromShop);
        }
    }

}