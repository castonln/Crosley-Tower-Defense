using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class StudentHover : MonoBehaviour
{
    [SerializeField] private Transform offScreenHolding;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (BuildManager.main.IsPlacingStudent())
        {
            spriteRenderer.sprite = BuildManager.main.GetSelectedStudentSprite();
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.y += 0.25f;
            transform.position = mousePos;
        }
        else if (transform.position != offScreenHolding.position) {
            transform.position = offScreenHolding.position;
        }

        //idfk dude.
    }
}
