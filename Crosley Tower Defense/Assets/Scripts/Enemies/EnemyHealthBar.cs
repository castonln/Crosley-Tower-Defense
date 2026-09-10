using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Slider slider;

    [Header("Attributes")]
    [SerializeField] private float temporarilyActiveTime = 3f;

    private bool isMouseHovering = false;

    public void SetIsMouseHovering(bool _isMouseHovering)
    {
        isMouseHovering = _isMouseHovering;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        slider.value = currentHealth / maxHealth;
    }

    public void ShowHealthBarTemporarily()
    {
        gameObject.SetActive(true);
        StartCoroutine(SetInactiveAfterDelay(temporarilyActiveTime));
    }

    IEnumerator SetInactiveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!isMouseHovering) gameObject.SetActive(false);
    }

}
