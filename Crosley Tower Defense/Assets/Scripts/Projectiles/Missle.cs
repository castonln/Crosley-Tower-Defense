using UnityEngine;

public class Missle : Bullet
{
    // I KNOW ITS NOT HOW YOU SPELL IT OKAY I'M WORKING ON BORROWED TIME HERE
    [Header("References")]
    [SerializeField] private Animator animator;

    protected override void HandleDestroy()
    {
        FloorsAndGround.main.ShakeHorizontal();
        animator.SetTrigger("IsExploded");
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
