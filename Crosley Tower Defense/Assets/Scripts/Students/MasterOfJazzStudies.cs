using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MasterOfJazzStudies : VaryingFloatingProjectileFiringStudent
{
    [Header("Attributes")]
    [SerializeField] private int randomLow = 1;
    [SerializeField] private int randomHigh = 7;

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
        randInterval = Random.Range(randomLow, randomHigh);
    }

}
