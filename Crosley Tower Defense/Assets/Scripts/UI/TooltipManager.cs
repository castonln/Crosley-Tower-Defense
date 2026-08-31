using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // for LayoutRebuilder

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager main;
    [Header("References")]
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipDescription;
    [SerializeField] private TextMeshProUGUI tooltipCost;
    [SerializeField] private RectTransform tooltipRect;
    [Header("Attributes")]
    [SerializeField] private Vector2 offset = new Vector2(10f, -10f);
    [SerializeField] private Vector2 tapOffset = new Vector2(2f, 1f);
    [SerializeField] private Vector2 tooltipBoundaryBox = new Vector2(5, 5);
    private Canvas canvas;
    private RectTransform canvasRect;
    private bool displayedFromTap = false;

    private void Awake()
    {
        main = this;
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.transform as RectTransform;
        Hide();
    }

    private void Update()
    {
        if (tooltipObject.activeSelf && !displayedFromTap)
            FollowMouse();
    }

    public void ShowStatic(string text, int cost, Vector2 position)
    {
        // No clamping, no mouse follow
        tooltipObject.SetActive(true);
        tooltipDescription.text = text;
        tooltipCost.text = cost.ToString();
        displayedFromTap = true;
        tooltipRect.position = position + tapOffset;
    }

    public void Show(string text, int cost)
    {
        tooltipObject.SetActive(true);
        tooltipDescription.text = text;
        tooltipCost.text = cost.ToString();
        displayedFromTap = false;
        FollowMouse();
    }

    public void Show(string text)
    {
        tooltipObject.SetActive(true);
        tooltipDescription.text = text;
        displayedFromTap = false;
        FollowMouse();
    }

    public void Show()
    {
        tooltipObject.SetActive(true);
        displayedFromTap = false;
        FollowMouse();
    }

    public void Hide()
    {
        tooltipObject.SetActive(false);
        displayedFromTap = false;
    }

    private void FollowMouse()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRect);

        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            Mouse.current.position.ReadValue(),
            canvas.worldCamera,
            out mousePos
        );

        Vector2 desiredTooltipLocation = mousePos + offset;
        tooltipRect.anchoredPosition = ClampToCanvas(desiredTooltipLocation);
    }

    private Vector2 ClampToCanvas(Vector2 desiredAnchoredPosition)
    {
        Rect canvasBounds = canvasRect.rect;
        Vector2 size = tooltipRect.rect.size;
        Vector2 pivot = tooltipRect.pivot;

        float minX = canvasBounds.xMin + size.x * pivot.x + tooltipBoundaryBox.x;
        float maxX = canvasBounds.xMax - size.x * (1f - pivot.x) - tooltipBoundaryBox.x;
        float minY = canvasBounds.yMin + size.y * pivot.y + tooltipBoundaryBox.y;
        float maxY = canvasBounds.yMax - size.y * (1f - pivot.y) - tooltipBoundaryBox.y;

        float x = minX <= maxX ? Mathf.Clamp(desiredAnchoredPosition.x, minX, maxX) : canvasBounds.center.x;
        float y = minY <= maxY ? Mathf.Clamp(desiredAnchoredPosition.y, minY, maxY) : canvasBounds.center.y;

        return new Vector2(x, y);
    }

    public string GetTooltipDescription()
    {
        return tooltipDescription.text;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, tooltipBoundaryBox);
    }
}