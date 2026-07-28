using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private int floorHealth = 100;

    [Header("References")]
    [SerializeField] private Floors floors;
    [SerializeField] private Transform floorsTransform;
    [SerializeField] private FloorsAndGround floorsAndGround;
    [SerializeField] private TowerHealSprites towerHealSprites;
    [SerializeField] private GameObject towerShadow;

    [Header("Damage Sprites")]
    [SerializeField] private GameObject noDamageSprite;
    [SerializeField] private GameObject loDamageSprite;
    [SerializeField] private GameObject meDamageSprite;
    [SerializeField] private GameObject hiDamageSprite;

    private GameObject curDamageSprite;
    private Stack<GameObject> floorsStack;

    private int highestFloorHeight = 0;

    public static event Action OnFloorStackEmpty;
    public static event Action OnFloorsStackChange;
    public static Tower main;
    private void Awake()
    {
        main = this;
        OnFloorsStackChange += CheckIfHighestFloorHeight;
        OnFloorStackEmpty += DestroyTowerShadow;
    }

    private void OnDestroy()
    {
        OnFloorsStackChange -= CheckIfHighestFloorHeight;
        OnFloorStackEmpty -= DestroyTowerShadow;
    }

    // sprite bottom outline shenangins smh
    private void DestroyTowerShadow()
    {
        Destroy(towerShadow);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        floorsStack = new Stack<GameObject>();
        foreach (Transform childTransform in floorsTransform)
        {
            floorsStack.Push(childTransform.gameObject);
            OnFloorsStackChange?.Invoke();
        } 
        curDamageSprite = noDamageSprite;
        CheckIfHighestFloorHeight();
    }

    public int GetFloorsStackLength()
    {
        return floorsStack.Count;
    }

    public void CheckIfHighestFloorHeight()
    {
        int floorHeight = GetFloorsStackLength();
        highestFloorHeight = floorHeight > highestFloorHeight ? floorHeight : highestFloorHeight; 
    }

    public void TakeDamage(int damage)
    {
        if (floorsAndGround.IsMoving()) return;

        if (floorHealth <= damage)
        {
            DestroyTopFloor();
            floorHealth = 100;
            if (floorsStack.Count == 0)
                OnFloorStackEmpty?.Invoke();
        }
        else
        {
            floorHealth -= damage;
        }
        UpdateDamageSprite();
    }

    public void HealDamage(int health)
    {
        if (floorsAndGround.IsMoving()) return;

        floorHealth += health;

        if (floorHealth >= 100)
        {
            CreateNewFloor();
            floorHealth = 100;
        } 
        UpdateDamageSprite();
        towerHealSprites.TriggerTowerHealSprites();
    }

    private int damageLevel = 0;

    private void UpdateDamageSprite()
    {
        curDamageSprite.GetComponent<SpriteRenderer>().enabled = false;

        int newDamageLevel;
        GameObject newSprite;

        if (floorHealth > 75)
        {
            newDamageLevel = 0;
            newSprite = noDamageSprite;
        }
        else if (floorHealth > 50)
        {
            newDamageLevel = 1;
            newSprite = loDamageSprite;
        }
        else if (floorHealth > 25)
        {
            newDamageLevel = 2;
            newSprite = meDamageSprite;
        }
        else
        {
            newDamageLevel = 3;
            newSprite = hiDamageSprite;
        }

        if (newDamageLevel > damageLevel)
            floorsAndGround.ShakeHorizontal();

        damageLevel = newDamageLevel;
        curDamageSprite = newSprite;
        curDamageSprite.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void DestroyTopFloor()
    {
        GameObject destroyedFloor = floorsStack.Pop();
        OnFloorsStackChange?.Invoke();
        Destroy(destroyedFloor);
        floorsAndGround.UpdateFloorsAndGroundFalling();
    }

    private void CreateNewFloor()
    {
        GameObject previousTopFloor = floorsStack.Peek();
        GameObject newTopFloor = Instantiate(
                previousTopFloor,
                previousTopFloor.transform.position + Vector3.up,
                previousTopFloor.transform.rotation,
                floors.transform
            );
        newTopFloor.GetComponent<SpriteRenderer>().sortingOrder = floorsStack.Count + 1;
        floorsStack.Push(newTopFloor);
        OnFloorsStackChange?.Invoke();
        floorsAndGround.UpdateFloorsAndGroundRising();
    }

    public int GetHighestFloorHeight()
    {
        return highestFloorHeight;
    }
}