using System.Collections;
using Cinemachine;
using UnityEngine;
//TODO find solution for duplicate GameManagers ( namespace ) topKek
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    [SerializeField] private Vector3Int currentPlayerChunkPosition;
    private Vector3Int currentChunkCenter = Vector3Int.zero;

    [SerializeField] private World world;

    private float detectionTime = 1.0f;
    [SerializeField] private CinemachineVirtualCamera cameraVM;

    public void SpawnPlayer()
    {
        if (player != null)
            return;
        Vector3Int raycastStartPosition = new Vector3Int(world.ChunkSize / 2, 100, world.ChunkSize / 2);
        RaycastHit hit;
        if (Physics.Raycast(raycastStartPosition, Vector3.down, out hit, 120))
        {
            player = Instantiate(playerPrefab, hit.point + Vector3.up, Quaternion.identity);
            cameraVM.Follow = player.transform.GetChild(0);
            StartCheckingTheMap();
        }
    }

    public void StartCheckingTheMap()
    {
        SetCurrentChunkCoordinates();
        StopAllCoroutines();
        StartCoroutine(CheckIfLoadRequired());
    }

    private IEnumerator CheckIfLoadRequired()
    {
        yield return new WaitForSeconds(detectionTime);
        if (Mathf.Abs(currentChunkCenter.x - player.transform.position.x) > world.ChunkSize ||
            Mathf.Abs(currentChunkCenter.z - player.transform.position.z) > world.ChunkSize ||
            (Mathf.Abs(currentPlayerChunkPosition.y - player.transform.position.y) > world.ChunkHeight))
        {
            world.ChunkLoadRequest(player);
        }
        else
        {
            StartCoroutine(CheckIfLoadRequired());
        }
    }

private void SetCurrentChunkCoordinates()
{
    currentPlayerChunkPosition = WorldDataHelper.ChunkPositionFromBlockCoords(world, Vector3Int.RoundToInt(player.transform.position));
    currentChunkCenter.x = currentPlayerChunkPosition.x + world.ChunkSize / 2;
    currentChunkCenter.z = currentPlayerChunkPosition.z + world.ChunkSize / 2;
}
}