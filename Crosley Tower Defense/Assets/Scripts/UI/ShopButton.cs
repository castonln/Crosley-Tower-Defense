using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Toggle toggle;
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
        TooltipManager.main.Hide();
    }

    public void HandleClick(string studentFromShop)
    {
        if (BuildManager.main.GetSelectedShopStudent() != shopEntry && toggle.IsInteractable())
        {
            TooltipManager.main.Show(shopEntry.description, shopEntry.cost, gameObject.transform.position);
            BuildManager.main.SetSelectedStudentFromShop(studentFromShop);
        }
        else
        {
            BuildManager.main.CancelPlacement();
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void HandleDragStart(string studentFromShop)
    {
        if (toggle.IsInteractable())
        {
            TooltipManager.main.Show(shopEntry.description, shopEntry.cost, gameObject.transform.position);
            BuildManager.main.SetSelectedStudentFromShop(studentFromShop);
        }
    }

}