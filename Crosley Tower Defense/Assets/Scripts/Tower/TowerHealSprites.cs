using UnityEngine;

public class TowerHealSprites : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator leftHealSpriteAnimator;
    [SerializeField] private Animator rightHealSpriteAnimator;

    public void TriggerTowerHealSprites()
    {
        leftHealSpriteAnimator.SetTrigger("TowerHeal");
        rightHealSpriteAnimator.SetTrigger("TowerHeal");
    }
}
