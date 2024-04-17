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

        // Control player-controller-binding
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

        // Spawning
        foreach (int playerIndex in playerControllerMappings.Keys)
        {
            if(playerControllerMappings.TryGetValue(playerIndex, out int controllerIndex))
            {
                // Get Sapwn location
                Vector3 spawnAt = transform.position;
                try
                {
                    spawnAt = spawnPoints[playerIndex - 1].position;
                }
                catch
                {
                    Debug.LogError("Appropriate spawnpoint not found, spawning on controller");
                }

                // Instansiate
                GameObject playerObj = Instantiate(playerPrefab, spawnAt, Quaternion.identity);
                PlayerStats playerStats = playerObj.GetComponent<PlayerStats>();
                Movement playerMovement = playerObj.GetComponent<Movement>();
                Aiming playerAim = playerObj.GetComponent<Aiming>();
                WeaponUser weaponUser = playerObj.GetComponent<WeaponUser>();
                
                // Stats set-up
                if(playerStats == null) {
                    Debug.LogError("Player prefab missing PlayerStats script");
                }
                else
                {
                    playerStats.playerIndex = playerIndex;
                }

                // Movement setup
                if(playerMovement == null) {
                    Debug.LogError("Player prefab missing movement script");
                }
                else
                {
                    playerMovement.appropriatlySpawned = true;
                    playerMovement.controllerIndex = controllerIndex;
                }

                // Aiming setup
                if (playerAim == null) {
                    Debug.LogError("Player prefab missing aiming script");
                }
                else
                {
                    playerAim.appropriatlySpawned = true;
                    playerAim.controllerIndex = controllerIndex;
                }

                // Weapon Usage
                if (weaponUser == null)
                {
                    Debug.LogError("Player prefab missing Weapon User script");
                }
                else
                {
                    weaponUser.appropriatlySpawned = true;
                    weaponUser.controllerIndex = controllerIndex;
                }

            }
            else
            {
                Debug.LogError("Unexpected state mutation in playerControllerMappings - see GameStartManager");
                return;
            }
        }
    }
}
