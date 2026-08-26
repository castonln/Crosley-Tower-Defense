using UnityEngine;

public class TutorialStep2 : TutorialStep
{
    [Header("Attributes")]
    [SerializeField] private Color leftLaneColor;

    private GameObject leftLaneStudentSpawns;
    private Plot[] studentPlots;
    private Lane leftLane;

    protected override void Start()
    {
        base.Start();

        leftLaneStudentSpawns = TutorialManager.main.references.leftLaneStudentSpawns;
        studentPlots = leftLaneStudentSpawns.GetComponentsInChildren<Plot>();
        leftLane = TutorialManager.main.references.leftLane;

        leftLane.DisplayLaneBuff(leftLaneColor);
        leftLaneStudentSpawns.SetActive(true); // this stays active
    }

    void Update()
    {
        foreach (Plot plot in studentPlots) 
        {
            if (plot.GetStudentInPlot() != null) StepFinished();
        }
    }

    public override void StepFinished()
    {
        leftLane.HideLaneBuff();
        base.StepFinished();
    }
}
