using UnityEngine;

public class GameplayIntermission2 : WaitForFirstTriceracopterInLaneToDieIntermission
{
    protected override void Start()
    {
        base.Start();

        base.SetLaneToSearch(TutorialManager.main.references.topRightLane);
    }
}
