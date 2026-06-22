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
        if (BuildManager.main.IsPlacingStudent() && studentInPlot) return;
        sr.enabled = true;
    }

    public void OnPointerClick(PointerEventData eventData) => TryPlaceStudent();
    public void OnDrag(PointerEventData eventData) => TryPlaceStudent();
    public void OnDrop(PointerEventData eventData) => TryPlaceStudent();

    private void TryPlaceStudent()
    {
        if (BuildManager.main.IsPlacingStudent())
        {
            if (studentInPlot) return;
            GameObject _studentInPlot = BuildManager.main.SpawnStudent(this);

            Vector3 scale = _studentInPlot.transform.localScale;
            scale.x = studentSpawns.IsRightSide() ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            _studentInPlot.transform.localScale = scale;

            studentInPlot = _studentInPlot;
            sr.enabled = false;
        }
        else
        {
            if (!studentInPlot) return;
            BuildManager.main.SetSelectedStudentFromPlot(this);
        }
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
