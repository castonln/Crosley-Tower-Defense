using UnityEngine;

public class TutorialStep3 : TutorialStep
{
    [Header("References")]
    [SerializeField] private Color leftColor;
    [SerializeField] private Color rightColor;
    [SerializeField] private Color topLeftColor;
    [SerializeField] private Color topRightColor;


    protected override void Start()
    {
        base.Start();

        TutorialManager.main.references.leftLane.DisplayLaneBuff(leftColor);
        TutorialManager.main.references.rightLane.DisplayLaneBuff(rightColor);
        TutorialManager.main.references.topLeftLane.DisplayLaneBuff(topLeftColor);
        TutorialManager.main.references.topRightLane.DisplayLaneBuff(topRightColor);

    }

    public override void StepFinished()
    {
        TutorialManager.main.references.leftLane.HideLaneBuff();
        TutorialManager.main.references.rightLane.HideLaneBuff();
        TutorialManager.main.references.topLeftLane.HideLaneBuff();
        TutorialManager.main.references.topRightLane.HideLaneBuff();

        base.StepFinished();
    }
}
