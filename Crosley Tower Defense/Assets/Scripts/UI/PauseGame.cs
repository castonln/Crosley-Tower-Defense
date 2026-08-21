using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject quitBox;

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

    public void OpenQuitBox()
    {
        quitBox.SetActive(true);
    }

    public void CloseQuitBox()
    {
        quitBox.SetActive(false);
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title Screen");
    }
}
