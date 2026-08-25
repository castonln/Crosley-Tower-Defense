using UnityEngine;
using UnityEngine.UI;

public class FullscreenButton : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite fullscreenIcon;
    [SerializeField] private Sprite minimizeIcon;

    private bool isFullscreen = false;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void OnClick()
    {
        if (isFullscreen)
        {
            image.sprite = fullscreenIcon;
            isFullscreen = false;
        } else
        {
            image.sprite = minimizeIcon;
            isFullscreen = true;
        }

        Screen.fullScreen = !Screen.fullScreen;
    }
}
