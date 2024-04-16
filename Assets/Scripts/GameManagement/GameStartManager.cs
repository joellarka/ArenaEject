using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        SpawnPlayers();
    }


    private void SpawnPlayers()
    {
        if (playerPrefab == null) return;

        Dictionary<int, int> playerControllerMappings = PlayerData.playerToControllerBinding;
        if(playerControllerMappings == null)
        {
            Debug.Log("Player Controller Bindings not found, constructing 2 player controller bindings");
            playerControllerMappings = new Dictionary<int, int>()
            {
                {1,1},
                {2,2}
            };
        }

        foreach (int playerIndex in playerControllerMappings.Keys)
        {
            if(playerControllerMappings.TryGetValue(playerIndex, out int controllerIndex))
            {
                Vector3 spawnAt = transform.position;
                try
                {
                    spawnAt = spawnPoints[playerIndex - 1].position;
                }
                catch
                {
                    Debug.LogError("Appropriate spawnpoint not found, spawning on controller");
                }
                GameObject playerObj = Instantiate(playerPrefab, spawnAt, Quaternion.identity);
                Movement playerMovement = playerObj.GetComponent<Movement>();
                if(playerMovement == null) {
                    Debug.LogError("Player prefab missing movement script");
                    return;
                }
                playerMovement.playerIndex = playerIndex;
                playerMovement.controllerIndex = controllerIndex;

            }
            else
            {
                Debug.LogError("Unexpected state mutation in playerControllerMappings - see GameStartManager");
                return;
            }
        }
    }
}
