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
        SceneManager.LoadScene("Tutorial");
    }

    public void LeaderboardButton()
    {
        SceneManager.LoadScene("Leaderboard");
    }
}

