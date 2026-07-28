using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameOverScreen gameOverScreen;

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
        GameSession.FinalWave = EnemySpawner.main.GetWaveTotalUI() - 1;
        GameSession.Currency = CurrencyManager.main.GetHighestCurrency();
        GameSession.TowerHeight = Tower.main.GetHighestFloorHeight();
        gameOverScreen.Enable();
    }
}
