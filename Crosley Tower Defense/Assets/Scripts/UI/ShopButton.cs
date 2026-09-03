using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
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
        if (Touchscreen.current == null || !Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            TooltipManager.main.Show(shopEntry.description, shopEntry.cost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.main.HideNonStatic();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!toggle.IsInteractable())
        {
            BuildManager.main.CancelPlacement();
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            TooltipManager.main.ShowStatic(shopEntry.description, shopEntry.cost, gameObject.transform.position);
        else
            TooltipManager.main.Show(shopEntry.description, shopEntry.cost);
    }

    public void HandleClick(string studentFromShop)
    {
        if (BuildManager.main.GetSelectedShopStudent() != shopEntry)
        {   
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
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
                TooltipManager.main.ShowStatic(shopEntry.description, shopEntry.cost, gameObject.transform.position);
            
            BuildManager.main.SetSelectedStudentFromShop(studentFromShop);
        }
    }

}