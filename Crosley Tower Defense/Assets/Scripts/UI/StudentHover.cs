using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class StudentHover : MonoBehaviour
{
    public static StudentHover main;

    [SerializeField] private Transform offScreenHolding;

    private SpriteRenderer spriteRenderer;
    private Plot hoveringPlotWhileMoving = null;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        spriteRenderer.sprite = BuildManager.main.GetSelectedStudentSprite();

        if (BuildManager.main.IsPlacingStudent() && !BuildManager.main.IsMovingStudent())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.y += 0.25f;
            transform.position = mousePos;
        }
        else if (hoveringPlotWhileMoving)
        {
            transform.position = hoveringPlotWhileMoving.transform.position + Vector3.up * 0.5f;
        }
        else if (transform.position != offScreenHolding.position) {
            transform.position = offScreenHolding.position;
        }

        //idfk dude.
    }

    public void SetHoveringPlotWhileMoving(Plot plot)
    {
        hoveringPlotWhileMoving = plot;
    }
}
