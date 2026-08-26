using UnityEngine;
using UnityEngine.UI;

public class TutorialStep7 : TutorialStep
{

    protected override void Start()
    {
        base.Start();

        Plot[] topRightLaneStudentPlots = TutorialManager.main.references.topRightLaneStudentSpawns.GetComponentsInChildren<Plot>();

        Plot selectedPlot = null;

        foreach (Plot plot in topRightLaneStudentPlots)
        {
            if (plot.GetStudentInPlot() != null) selectedPlot = plot;
        }

        if (selectedPlot == null)
        {
            print("Selected plot was marked as null in TutorialStep7 script.");
            return;
        }

        BuildManager.main.SetSelectedStudentFromPlot(selectedPlot);

        Button[] upgradePathButtons = TutorialManager.main.references.upgradePathButtons;
        foreach (Button button in upgradePathButtons) button.onClick.AddListener(StepFinished);
    }

}
