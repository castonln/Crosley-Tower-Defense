using UnityEngine;

public class GameplayIntermission1 : WaitForFirstTriceracopterInLaneToDieIntermission
{

    protected override void Start()
    {
        base.Start();

        base.SetLaneToSearch(TutorialManager.main.references.leftLane);
    }

}
