using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI studentName;
    [SerializeField] private TextMeshProUGUI path1Title;
    [SerializeField] private TextMeshProUGUI path2Title;

    [SerializeField] private TextMeshProUGUI path1Description;
    [SerializeField] private TextMeshProUGUI path2Description;

    [SerializeField] private TextMeshProUGUI path1Cost;
    [SerializeField] private TextMeshProUGUI path2Cost;

    [SerializeField] private TextMeshProUGUI sellAmount;

    [SerializeField] private TextMeshProUGUI currencyUI;

    [SerializeField] private Button path1Button;
    [SerializeField] private Button path2Button;

    private void DisablePath1Button()
    {
        path1Button.interactable = false;
    }

    private void EnablePath1Button()
    {
        path1Button.interactable = true;
    }

    private void DisablePath2Button()
    {
        path2Button.interactable = false;
    }

    private void EnablePath2Button()
    {
        path2Button.interactable = true;
    }

    private void EnableUpgradeMenu()
    {
        gameObject.SetActive(true);
        studentName.text = BuildManager.main.GetSelectedStudentName();
        sellAmount.text = BuildManager.main.GetSelectedStudentSellValue().ToString();
        currencyUI.text = CurrencyManager.main.GetCurrency().ToString();

        EnablePath1Button();
        EnablePath2Button();

        UpgradePath[] paths = BuildManager.main.GetSelectedStudentUpgradePaths();

        if (paths == null)
        {
            path1Title.text = "Path complete";
            path2Title.text = "Path complete";

            path1Description.text = "";
            path2Description.text = "";

            path1Cost.text = "";
            path2Cost.text = "";

            DisablePath1Button();
            DisablePath2Button();
            
            return;
        }

        path1Title.text = paths[0].pathTitle;
        path2Title.text = paths[1].pathTitle;

        path1Description.text = paths[0].pathDescription;
        path2Description.text = paths[1].pathDescription;

        path1Cost.text = paths[0].pathCost.ToString();
        path2Cost.text = paths[1].pathCost.ToString();

        if (paths[0].pathCost > CurrencyManager.main.GetCurrency())
        {
            DisablePath1Button();
        }

        if (paths[1].pathCost > CurrencyManager.main.GetCurrency())
        {
            DisablePath2Button();
        }
    }

    private void DisableUpdgradeMenu()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        BuildManager.OnSelectMoveStudent += EnableUpgradeMenu;
        BuildManager.OnDeselectMoveStudent += DisableUpdgradeMenu;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BuildManager.OnSelectMoveStudent -= EnableUpgradeMenu;
        BuildManager.OnDeselectMoveStudent -= DisableUpdgradeMenu;

    }
}
