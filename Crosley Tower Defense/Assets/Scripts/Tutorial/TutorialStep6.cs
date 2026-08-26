using UnityEngine;

public class TutorialStep6 : TutorialStep
{
    [Header("Attribute")]
    [SerializeField] private Color topRightLaneColor;

    private GameObject topRightLaneStudentSpawns;
    private GameObject leftLaneStudentSpawns;
    private Plot[] topRightLaneStudentPlots;
    private Lane topRightLane;

    protected override void Start()
    {
        base.Start();

        leftLaneStudentSpawns = TutorialManager.main.references.leftLaneStudentSpawns;
        topRightLaneStudentSpawns = TutorialManager.main.references.topRightLaneStudentSpawns;
        topRightLaneStudentPlots = topRightLaneStudentSpawns.GetComponentsInChildren<Plot>();
        topRightLane = TutorialManager.main.references.topRightLane;

        topRightLane.DisplayLaneBuff(topRightLaneColor);
        leftLaneStudentSpawns.SetActive(true);
        topRightLaneStudentSpawns.SetActive(true);
    }

    void Update()
    {
        foreach (Plot plot in topRightLaneStudentPlots) 
        {
            if (plot.GetStudentInPlot() != null) StepFinished();
        }
    }

    public override void StepFinished()
    {
        leftLaneStudentSpawns.SetActive(false);
        topRightLane.HideLaneBuff();

        base.StepFinished();
    }
}
