using UnityEngine;

public class StudentSpawns : MonoBehaviour
{
    [SerializeField] private bool isRightSide;

    public bool IsRightSide()
    {
        return isRightSide;
    }
}
