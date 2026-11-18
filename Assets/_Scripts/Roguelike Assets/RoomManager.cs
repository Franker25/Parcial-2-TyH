using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] roomCenters;
    [SerializeField]
    private Transform[] playerStarts;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform spawnTopLeft, spawnTopRight, spawnBottomLeft, spawnBottomRight;
    [SerializeField]
    private Vector2 spawnOffset = new Vector2(9f, 4.5f);

    void Awake()
    {
        int pick = Random.Range(0, roomCenters.Length);
        while (pick == RunData.LastRoom && roomCenters.Length > 1)
        {
            pick = Random.Range(0, roomCenters.Length);
        }
        RunData.LastRoom = pick;

        Vector3 center = roomCenters[pick].position;
        Vector3 start = playerStarts[pick].position;

        player.position = new Vector3(start.x, start.y, player.position.z);

        Transform cam = Camera.main.transform;
        cam.position = new Vector3(center.x, center.y, cam.position.z);

        spawnTopLeft.position = new Vector3(center.x - spawnOffset.x, center.y + spawnOffset.y, 0f);
        spawnTopRight.position = new Vector3(center.x + spawnOffset.x, center.y + spawnOffset.y, 0f);
        spawnBottomLeft.position = new Vector3(center.x - spawnOffset.x, center.y - spawnOffset.y, 0f);
        spawnBottomRight.position = new Vector3(center.x + spawnOffset.x, center.y - spawnOffset.y, 0f);

        Physics2D.SyncTransforms();
    }
}
