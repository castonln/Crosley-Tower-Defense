using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController main;

    [Header("References")]
    [SerializeField] private GameOverScreen gameOverScreen;

    private bool isGameOver = false;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        GameSession.Reset();
    }

    private void OnEnable()
    {
        Tower.OnFloorStackEmpty += GameOver;
    }

    private void OnDisable()
    {
        Tower.OnFloorStackEmpty -= GameOver;
    }

    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 1f;
        GameSession.FinalWave = EnemySpawner.main.GetWaveTotalUI() - 1;
        GameSession.Currency = CurrencyManager.main.GetHighestCurrency();
        GameSession.TowerHeight = Tower.main.GetHighestFloorHeight();
        gameOverScreen.Enable();
    }

    public bool IsGameOver() { 
        return isGameOver;
    }
}
