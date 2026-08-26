using UnityEngine;
using UnityEngine.UI;

public class TutorialStep4 : TutorialStep
{
    private Button startWaveButton;

    protected override void Start()
    {
        base.Start();

        startWaveButton = TutorialManager.main.references.startWaveButton;

        startWaveButton.interactable = true;
    }

    void Update()
    {
        if (EnemySpawner.main.IsWaveActive()) StepFinished();
    }

    public override void StepFinished()
    {
        startWaveButton.interactable = false;

        base.StepFinished();
    }
}
