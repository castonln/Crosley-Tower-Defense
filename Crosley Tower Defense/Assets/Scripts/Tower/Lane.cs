using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Lane : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform targetPoint;
    [SerializeField] private LayerMask laneMask;
    [SerializeField] private SpriteRenderer laneBuffSprite;

    private LaneMultipliers multipliers = new LaneMultipliers { strength = 1f, speed = 1f };

    private List<GameObject> enemies = new List<GameObject>();

    public int GetNumberOfEnemies()
    {
        return enemies.Count;
    }

    public Transform SpawnPoint => spawnPoint;
    public void SetMultipliers(LaneMultipliers _multipliers)
    {
        multipliers = _multipliers;
        SetStudentMultipliers();
        DisplayLaneBuff();
    }

    public void ResetMultipliers()
    {
        multipliers = new LaneMultipliers { strength = 1f, speed = 1f };
        SetStudentMultipliers();
        HideLaneBuff();
    }

    private void SetStudentMultipliers()
    {
        Student[] students = GetComponentsInChildren<Student>();
        foreach (Student student in students)
        {
            student.SetMultipliers(multipliers);
        }
    }

    public Transform GetTargetPoint(GameObject enemy)
    {
        int index = enemies.IndexOf(enemy);

        if (index <= 0) return targetPoint;

        return enemies[index - 1].transform;
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
        int layer = (int)Mathf.Log(laneMask.value, 2);

        foreach (Transform child in enemy.GetComponentsInChildren<Transform>())
        {
            child.gameObject.layer = layer;
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);

        int curSortingOrder = 0;

        foreach (GameObject remainingEnemy in enemies)
        {
            remainingEnemy.GetComponent<EnemyMovement>().SetTarget(GetTargetPoint(remainingEnemy));
            remainingEnemy.GetComponentInChildren<SpriteRenderer>().sortingOrder = curSortingOrder;
            curSortingOrder++;
        }
    }

    public LayerMask GetLaneMask()
    {
        return laneMask;
    }

    public bool AreEnemiesAlive()
    {
        return enemies.Count > 0;
    }

    private void DisplayLaneBuff()
    {
        laneBuffSprite.enabled = true;
    }

    private void HideLaneBuff()
    {
        laneBuffSprite.enabled = false;
    }
}
