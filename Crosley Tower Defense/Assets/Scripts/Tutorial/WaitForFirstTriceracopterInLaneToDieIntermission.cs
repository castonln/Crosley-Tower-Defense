using UnityEngine;

public class WaitForFirstTriceracopterInLaneToDieIntermission : TutorialStep
{
    private Lane laneToSearch;
    private GameObject enemies;

    private Triceracopter targetedTriceracopter = null;
    private bool triceracopterFound = false;

    protected override void Start()
    {
        base.Start();

        enemies = TutorialManager.main.references.enemies;
    }

    protected void SetLaneToSearch(Lane lane)
    {
        laneToSearch = lane;
    }


    void Update()
    {
        if (!laneToSearch) return;

        if (!triceracopterFound)
        {
            Triceracopter[] triceracopters = enemies.GetComponentsInChildren<Triceracopter>();

            foreach (Triceracopter triceracopter in triceracopters)
            {
                if (triceracopter.GetLane() == laneToSearch)
                {
                    targetedTriceracopter = triceracopter;
                    triceracopterFound = true;
                }
            }
        }
        else if (!targetedTriceracopter)
        {
            StepFinished();
        }

    }
}
