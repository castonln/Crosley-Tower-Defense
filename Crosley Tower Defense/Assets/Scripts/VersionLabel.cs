using UnityEngine;
using TMPro;

public class VersionLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    private void Awake()
    {
        label.text = $"v{Application.version} ({Application.buildGUID.Substring(0, 8)})";
    }
}