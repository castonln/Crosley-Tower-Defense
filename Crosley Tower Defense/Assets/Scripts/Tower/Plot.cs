using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class Plot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDragHandler, IDropHandler
{
    [Header("References")]
    [SerializeField] private Transform studentSpawnPoint;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private StudentSpawns studentSpawns;

    private GameObject studentInPlot = null;
    private LayerMask laneMask;

    private void Start()
    {
        laneMask = gameObject.transform.parent.parent.GetComponent<Lane>().GetLaneMask();
    }

    public GameObject GetStudentInPlot()
    {
        return studentInPlot;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!BuildManager.main.IsPlacingStudent() && !studentInPlot) return;
        sr.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData) => TryPlaceStudent();
    public void OnDrag(PointerEventData eventData)
    {
        if (!studentInPlot) return;
        if (BuildManager.main.GetSourcePlot() == this) return; // already dragging this one, don't re-toggle
        if (!BuildManager.main.GetSourcePlot())
            BuildManager.main.SetSelectedStudentFromPlot(this);
    }
    public void OnDrop(PointerEventData eventData) => TryPlaceStudent();

    private void TryPlaceStudent()
    {
        if (studentInPlot)
        {
            if (!BuildManager.main.GetSourcePlot())
                BuildManager.main.SetSelectedStudentFromPlot(this);
            else if (BuildManager.main.GetSourcePlot() != this)
                BuildManager.main.SwapStudentsFromPlotAndSourcePlot(this);
            else
                BuildManager.main.CancelPlacement();
            return;
        }

        GameObject _studentInPlot = BuildManager.main.SpawnStudent(this);

        Vector3 scale = _studentInPlot.transform.localScale;
        scale.x = studentSpawns.IsRightSide() ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        _studentInPlot.transform.localScale = scale;

        studentInPlot = _studentInPlot;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!BuildManager.main.IsPlacingStudent() && !studentInPlot) return;
        sr.enabled = false;
    }
    
    public Transform StudentSpawnPoint()
    {
        return studentSpawnPoint;
    }

    public void ClearStudent()
    {
        studentInPlot = null;
    }

    public void SetStudentInPlot(GameObject _studentInPlot)
    {
        studentInPlot = _studentInPlot;
    }

    public LayerMask GetLaneMask()
    {
        return laneMask;
    }
}
