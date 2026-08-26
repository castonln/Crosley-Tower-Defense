using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[Serializable]
public enum UpgradePathSelection
{
    path1,
    path2,
}

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    public enum PlacementMode { None, PlacingFromShop, MovingFromPlot, Upgrading }
    private PlacementMode placementMode = PlacementMode.None;

    [Header("References")]
    [SerializeField] private ShopEntry[] studentShopEntries;
    [SerializeField] private LayerMask plotMask;

    private Dictionary<string, ShopEntry> studentDict = new();
    private string selectedShopStudentKey;

    private GameObject selectedMoveStudent;
    private Plot sourcePlot;

    private bool isPlacingStudent = false;

    public static event Action OnSelectMoveStudent;
    public static event Action OnDeselectMoveStudent;

    private void Awake()
    {
        main = this;
        foreach (var student in studentShopEntries)
        {
            studentDict[student.name] = student;
        }
    }

    private void Update()
    {
        if (!isPlacingStudent) return;
        if (!Mouse.current.leftButton.wasPressedThisFrame) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (Physics2D.OverlapPoint(mousePos, plotMask) != null) return;

        CancelPlacement();
    }

    public GameObject SpawnStudent(Plot plot)
    {
        TooltipManager.main.Hide();
        switch (placementMode)
        {
            case PlacementMode.PlacingFromShop: return PlaceFromShop(plot);
            case PlacementMode.MovingFromPlot: return MoveFromPlot(plot);
            default: return null;
        }
    }

    private GameObject PlaceFromShop(Plot plot)
    {
        ShopEntry studentToPlace = GetSelectedShopStudent();
        if (!CurrencyManager.main.SpendCurrency(studentToPlace.cost))
        {
            CancelPlacement();
            return null;
        }

        GameObject studentObj = Instantiate(
            studentToPlace.prefab,
            plot.StudentSpawnPoint().position + Vector3.up * 0.5f,
            plot.StudentSpawnPoint().rotation,
            plot.transform
        );
        studentObj.name = studentToPlace.prefab.name;
        AlignSpriteDirection(studentObj, plot);
        PutSpriteInSortingLayer(studentObj, plot);
        FinishPlacement();
        return studentObj;
    }

    private GameObject MoveFromPlot(Plot plot)
    {
        void SpawnStudentInPlotFromGameObject(GameObject student, Plot plot)
        {
            student.transform.position = plot.StudentSpawnPoint().position + Vector3.up * 0.5f;
            student.transform.rotation = plot.StudentSpawnPoint().rotation;
            student.transform.SetParent(plot.transform);

            AlignSpriteDirection(student, plot);
            PutSpriteInSortingLayer(student, plot);
            plot.SetStudentInPlot(student);
        }

        sourcePlot.ClearStudent();

        // Swap
        if (plot.GetStudentInPlot() != null)
        {
            Plot plotA = sourcePlot;
            GameObject studentB = plot.GetStudentInPlot();
            Plot plotB = plot;

            SpawnStudentInPlotFromGameObject(studentB, plotA);
            plotB.ClearStudent();
        }

        SpawnStudentInPlotFromGameObject(selectedMoveStudent, plot);

        GameObject studentObj = selectedMoveStudent;
        selectedMoveStudent = null;
        sourcePlot = null;
        OnDeselectMoveStudent?.Invoke();
        FinishPlacement();
        return studentObj;
    }

    private GameObject PlaceUpgrade(Plot plot, UpgradePath pendingUpgrade)
    {
        if (!CurrencyManager.main.SpendCurrency(pendingUpgrade.pathCost))
        {
            CancelPlacement();
            return null;
        }

        // Detach BEFORE destroying so OnDestroy can't find the Lane
        selectedMoveStudent.transform.SetParent(null);
        DespawnStudent(selectedMoveStudent);

        GameObject studentObj = Instantiate(
            pendingUpgrade.pathPrefab,
            plot.StudentSpawnPoint().position + Vector3.up * 0.5f,
            plot.StudentSpawnPoint().rotation,
            plot.transform
        );
        studentObj.name = pendingUpgrade.pathPrefab.name;
        pendingUpgrade = null;
        AlignSpriteDirection(studentObj, plot);
        PutSpriteInSortingLayer(studentObj, plot);
        plot.SetStudentInPlot(studentObj);
        FinishPlacement();
        return studentObj;
    }

    private void FinishPlacement()
    {
        placementMode = PlacementMode.None;
        isPlacingStudent = false;
        selectedShopStudentKey = "";
    }

    public void SellSelectedStudent()
    {
        CurrencyManager.main.IncreaseCurrency(GetSelectedStudentSellValue());
        DespawnStudent(selectedMoveStudent);
    }

    public void UpgradePath1() => UpgradeSelectedStudent(GetSelectedStudentUpgradePaths()[0]);
    public void UpgradePath2() => UpgradeSelectedStudent(GetSelectedStudentUpgradePaths()[1]);

    private void UpgradeSelectedStudent(UpgradePath upgradePath)
    {
        placementMode = PlacementMode.Upgrading;
        PlaceUpgrade(sourcePlot, upgradePath);
    }

    public void DespawnStudent(GameObject student)
    {
        Destroy(student);
        CancelPlacement();
    }

    public bool IsPlacingStudent()
    {
        return isPlacingStudent;
    }

    public bool IsMovingStudent()
    {
        return selectedMoveStudent != null;
    }

    public ShopEntry GetSelectedShopStudent()
    {
        if (string.IsNullOrEmpty(selectedShopStudentKey))
            return null;
        studentDict.TryGetValue(selectedShopStudentKey, out ShopEntry student);
        return student;
    }

    public void SetSelectedStudentFromShop(string _selectedStudentKey)
    {
        selectedShopStudentKey = _selectedStudentKey;
        placementMode = PlacementMode.PlacingFromShop;
        isPlacingStudent = true;
    }

    public void SetSelectedStudentFromPlot(Plot plot)
    {
        sourcePlot = plot;
        selectedMoveStudent = plot.GetStudentInPlot();
        placementMode = PlacementMode.MovingFromPlot;
        isPlacingStudent = true;
        OnSelectMoveStudent?.Invoke();
    }

    public void CancelPlacement()
    {
        if (IsMovingStudent())
        {
            sourcePlot = null;
            selectedMoveStudent = null;
            OnDeselectMoveStudent?.Invoke();
        }
        placementMode = PlacementMode.None;
        isPlacingStudent = false;
        selectedShopStudentKey = "";
        TooltipManager.main.Hide();
    }

    public string GetSelectedStudentName()
    {
        return selectedMoveStudent.name;
    }

    public int GetSelectedStudentCost()
    {
        foreach (var shopEntry in studentShopEntries)
        {
            if (shopEntry.name == selectedMoveStudent.name)
                return shopEntry.cost;
        }
        return 0;
    }

    public int GetSelectedStudentSellValue()
    {
        foreach (var shopEntry in studentShopEntries)
        {
            if (shopEntry.name == selectedMoveStudent.name
                || shopEntry.path1.pathTitle == selectedMoveStudent.name
                || shopEntry.path2.pathTitle == selectedMoveStudent.name)
                return shopEntry.cost / 2;
        }
        return 0;
    }

    public UpgradePath[] GetSelectedStudentUpgradePaths()
    {
        foreach (var shopEntry in studentShopEntries)
        {
            if (shopEntry.name == selectedMoveStudent?.name)
                return new UpgradePath[] { shopEntry.path1, shopEntry.path2 };
        }
        return null;
    }

    public Sprite GetSelectedStudentSprite()
    {
        if (placementMode == PlacementMode.PlacingFromShop)
        {
            return studentDict[selectedShopStudentKey].prefab.GetComponent<SpriteRenderer>().sprite;
        
        } else if (placementMode == PlacementMode.MovingFromPlot)
        {
            return selectedMoveStudent.GetComponent<SpriteRenderer>().sprite;
        }

        return null;
    }

    public Plot GetSourcePlot()
    {
        return sourcePlot;
    }

    private void AlignSpriteDirection(GameObject student, Plot plot)
    {
        Vector3 scale = student.transform.localScale;
        scale.x = plot.IsRightSide() ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        student.transform.localScale = scale;
    }

    private void PutSpriteInSortingLayer(GameObject student, Plot plot)
    {
        student.GetComponent<SpriteRenderer>().sortingLayerName = plot.GetPlotSortingLayer().ToDisplayName();
    }
}