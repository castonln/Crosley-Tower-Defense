using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;

    private float timeScaleBeforePause = 1f;

    public void Pause()
    {
        pausePanel.SetActive(true);
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = timeScaleBeforePause;
    }
}
