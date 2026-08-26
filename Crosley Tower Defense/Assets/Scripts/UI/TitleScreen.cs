using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void TutorialButton()
    {
        StartCoroutine(LoadTutorialRoutine());
    }

    public void LeaderboardButton()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    private IEnumerator LoadTutorialRoutine()
    {
        AsyncOperation loadScene1 = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        AsyncOperation loadScene2 = SceneManager.LoadSceneAsync("Tutorial", LoadSceneMode.Additive);

        while (!loadScene1.isDone || !loadScene2.isDone)
        {
            yield return null;
        }

        Scene nextActiveScene = SceneManager.GetSceneByName("Game");
        if (nextActiveScene.IsValid())
        {
            SceneManager.SetActiveScene(nextActiveScene);
        }

        yield return SceneManager.UnloadSceneAsync("Title Screen");
    }
}

