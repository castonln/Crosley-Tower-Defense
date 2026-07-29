using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MasterOfJazzStudies : VaryingFloatingProjectileFiringStudent
{
    private int randInterval = 4;

    protected override float GetInterval() { 
        return randInterval;
    }

    protected override void DoAction(float damage)
    {
        base.DoAction(damage);
        SetRandInterval();
    }

    private void SetRandInterval()
    {
        randInterval = Random.Range(1, 7);
    }

}
