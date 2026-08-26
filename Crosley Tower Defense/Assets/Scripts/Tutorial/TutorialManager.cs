using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager main;

    [Header("References")]
    [SerializeField] private GameObject[] tutorialSteps;

    [Header("Attributes")]
    [SerializeField] private WaveData tutorialWave;

    private int currentStep = 0;

    [HideInInspector] public TutorialReferences references;

    private void Awake()
    {
        main = this;
    }

    void Start()
    {
        references = FindFirstObjectByType<TutorialReferences>();

        references.startWaveButton.interactable = false;
        references.sellButton.interactable = false;

        references.leftLaneStudentSpawns.SetActive(false);
        references.rightLaneStudentSpawns.SetActive(false);
        references.topLeftLaneStudentSpawns.SetActive(false);
        references.topRightLaneStudentSpawns.SetActive(false);
        references.middleLaneStudentSpawns.SetActive(false);

        references.pauseButton.onClick.AddListener(DeactivateStep);
        references.resumeButton.onClick.AddListener(ActivateStep);

        references.enemySpawner.InjectWaves(new[] { tutorialWave });

        ActivateStep();
    }

    private void DeactivateStep()
    {
        tutorialSteps[currentStep].SetActive(false);
    }

    private void ActivateStep()
    {
        if (tutorialSteps.Length > currentStep) tutorialSteps[currentStep].SetActive(true);
        else EndTutorial();
    }

    public void NextStep()
    {
        currentStep++;
        ActivateStep();
    }

    private void EndTutorial()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
