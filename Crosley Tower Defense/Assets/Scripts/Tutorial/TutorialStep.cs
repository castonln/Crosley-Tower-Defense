using UnityEngine;

public class TutorialStep : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private bool pauseGame = false;

    private float timeScaleBeforePause = 1.0f;
    protected virtual void Start()
    {
        if (pauseGame) PauseGame();
    }

    protected void PauseGame()
    {
        timeScaleBeforePause = Time.timeScale;
        Time.timeScale = 0f;
    }

    // SHOULD HAVE NAMED IT FINISH STEP BUT WHATEVER
    public virtual void StepFinished()
    {
        if (pauseGame) Time.timeScale = timeScaleBeforePause;

        TutorialManager.main.NextStep();
        gameObject.SetActive(false);
    }
}
