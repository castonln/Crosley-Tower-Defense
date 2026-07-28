using UnityEngine;

public class CloudMotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject leftPoint;
    [SerializeField] private GameObject rightPoint;
    [SerializeField] private GameObject tile1;
    [SerializeField] private GameObject tile2;

    [Header("Attributes")]
    [SerializeField] private float speed = 0.2f;

    private void Update()
    {
        gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
        CheckIfTileMustBeTeleported(tile1);
        CheckIfTileMustBeTeleported(tile2);
    }

    private void CheckIfTileMustBeTeleported(GameObject tile)
    {
        if (tile.transform.position.x > rightPoint.transform.position.x)
        {
            Vector3 newPosition = new Vector3(leftPoint.transform.position.x, tile.transform.position.y, tile.transform.position.z);
            tile.transform.position = newPosition;
        }
    }
}
