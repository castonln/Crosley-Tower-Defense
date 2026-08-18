using UnityEngine;
using UnityEngine.EventSystems;

public class TestErrorReportButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.main.Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.main.Hide();
    }

    public void SendTestError()
    {
        Debug.LogError("This is an automatic Sentry test error via Debug.LogError");
    }
}
