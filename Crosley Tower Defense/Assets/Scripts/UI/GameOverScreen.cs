using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject shopMenu;
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private GameObject levelInfo;

    [SerializeField] private TextMeshProUGUI wavesSurvived;
    [SerializeField] private TextMeshProUGUI highestFloor;
    [SerializeField] private TextMeshProUGUI highestCurrency;

    private string playerName = string.Empty;

    public void Enable()
    {
        Invoke("PrepareGameOverScreen", 3.0f);
    }

    private void PrepareGameOverScreen()
    {

        gameObject.SetActive(true);
        shopMenu.SetActive(false);
        upgradeMenu.SetActive(false);
        levelInfo.SetActive(false);

        wavesSurvived.text = GameSession.FinalWave.ToString();
        highestFloor.text = GameSession.TowerHeight.ToString();
        highestCurrency.text = GameSession.Currency.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void ReadName(string input)
    {
        playerName = input;
    }

    public void SaveToLeaderboardButton()
    {
        GameSession.PlayerName = playerName;
        GameSession.IsDataReadyToBeEnteredToLeaderboard = true;
        SceneManager.LoadScene("Leaderboard");
    }
}
