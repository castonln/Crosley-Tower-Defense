using UnityEngine;
using UnityEngine.UI;

public class TutorialReferences : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Button startWaveButton;
    [SerializeField] public Button pauseButton;
    [SerializeField] public Button resumeButton;

    [SerializeField] public Button sellButton;

    [SerializeField] public GameObject leftLaneStudentSpawns;
    [SerializeField] public GameObject rightLaneStudentSpawns;
    [SerializeField] public GameObject topLeftLaneStudentSpawns;
    [SerializeField] public GameObject topRightLaneStudentSpawns;
    [SerializeField] public GameObject middleLaneStudentSpawns;

    [SerializeField] public Button[] upgradePathButtons;

    [SerializeField] public GameObject enemies;

    [SerializeField] public Lane leftLane;
    [SerializeField] public Lane topRightLane;
    [SerializeField] public Lane topLeftLane;
    [SerializeField] public Lane rightLane;

    [SerializeField] public EnemySpawner enemySpawner; // i know it's static but idk this is how i've been doing it
}
