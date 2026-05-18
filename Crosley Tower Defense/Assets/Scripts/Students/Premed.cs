using UnityEngine;

public class Premed : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LaneMultipliers multipliers;

    private Lane lane;
    private void Start()
    {
        SetLaneMultipliers();
        print("Multi set.");
    }
    private void OnTransformParentChanged()
    {
        SetLaneMultipliers();
    }
    private void OnBeforeTransformParentChanged()
    {
        ResetLaneMultipliers();
    }

    private void OnDestroy()
    {
        ResetLaneMultipliers();
        print("Multi reset. Despawn finished.");
    }

    private void SetLaneMultipliers()
    {
        lane = GetComponentInParent<Lane>();
        if (lane == null) return;
        lane.SetMultipliers(multipliers);
    }

    private void ResetLaneMultipliers()
    {
        lane = GetComponentInParent<Lane>();
        if (lane == null) return;
        lane.ResetMultipliers();
    }
}
