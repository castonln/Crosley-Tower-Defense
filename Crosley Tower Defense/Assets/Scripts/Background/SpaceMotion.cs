using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceMotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject originalLastTile;
    [SerializeField] private GameObject spawnTriggerPoint;
    [SerializeField] private GameObject despawnTriggerPoint;
    [SerializeField] private FloorsAndGround floorsAndGround;

    private Stack<GameObject> tileStack = new Stack<GameObject>();

    private void Start()
    {
        tileStack.Push(originalLastTile);
    }

    private void Update()
    {
        if (!floorsAndGround.IsMoving()) return;

        if (tileStack.Peek().transform.position.y < spawnTriggerPoint.transform.position.y)
        {
            GameObject lastTile = tileStack.Peek();
            Vector3 newPostion = new Vector3(lastTile.transform.position.x, lastTile.transform.position.y + 16, lastTile.transform.position.z);
            GameObject newTile = Instantiate(tilePrefab, newPostion, lastTile.transform.rotation, transform);
            tileStack.Push(newTile);
        }
        else if (tileStack.Peek().transform.position.y > despawnTriggerPoint.transform.position.y && tileStack.Peek() != originalLastTile) {
            Destroy(tileStack.Pop());
        }
    }
}
