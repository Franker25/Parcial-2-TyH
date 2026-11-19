using UnityEngine;
using System.Collections.Generic;

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
        List<int> candidates = new List<int>();
        for (int i = 0; i < roomCenters.Length; i++)
        {
            if (!RunData.VisitedRooms.Contains(i))
            {
                candidates.Add(i);
            }
        }

        if (candidates.Count == 0)
        {
            RunData.VisitedRooms.Clear();
            for (int i = 0; i < roomCenters.Length; i++)
            {
                candidates.Add(i);
            }
        }

        int pick = candidates[Random.Range(0, candidates.Count)];
        RunData.VisitedRooms.Add(pick);

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
