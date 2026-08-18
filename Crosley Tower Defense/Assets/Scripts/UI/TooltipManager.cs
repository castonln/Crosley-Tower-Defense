using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class TooltipManager : MonoBehaviour
{
    public static TooltipManager main;

    [Header("References")]
    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipDescription;
    [SerializeField] private TextMeshProUGUI tooltipCost;
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private Vector2 offset = new Vector2(10f, -10f);

    private Canvas canvas;

    private void Awake()
    {
        main = this;
        canvas = GetComponentInParent<Canvas>();
        Hide();
    }

    private void Update()
    {
        if (tooltipObject.activeSelf)
            FollowMouse();
    }

    public void Show(string text, int cost)
    {
        tooltipObject.SetActive(true);
        tooltipDescription.text = text;
        tooltipCost.text = cost.ToString();
        FollowMouse();
    }

    public void Show(string text)
    {
        tooltipObject.SetActive(true);
        tooltipDescription.text = text;
        FollowMouse();
    }

    public void Show()
    {
        tooltipObject.SetActive(true);
        FollowMouse();
    }

    public void Hide()
    {
        tooltipObject.SetActive(false);
    }

    private void FollowMouse()
    {
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            Mouse.current.position.ReadValue(),
            canvas.worldCamera,
            out mousePos
        );
        tooltipRect.anchoredPosition = mousePos + offset;
    }

    public string GetTooltipDescription()
    {
        return tooltipDescription.text;
    }
}