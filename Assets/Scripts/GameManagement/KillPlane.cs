using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class KillPlane : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new();
    private bool gameIsOver = false;

    [Header("Temp")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text gameOverText;

    [SerializeField] private int levelLoadTime = 3;
    [SerializeField] private string[] levels;

    // TEMP
    private void Awake()
    {
        Time.timeScale = 1.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameIsOver) return;

        if(other.TryGetComponent<PlayerStats>(out PlayerStats player))
        {
            player.lives--;

            if (player.lives <= 0)
            {
                player.lives = 0;
            }

            if(player.alive)
            {
                // Respawn
                Vector3 respawnPos = Vector3.zero;
                try
                {
                    int rand = Random.Range(0, spawnPoints.Count);
                    respawnPos = spawnPoints[rand].position;
                }
                catch
                {
                    Debug.LogError("No Respawn Points Listed, spawns player at 0,0,0");
                }

                player.transform.position = respawnPos;
                Rigidbody rb = player.GetComponent<Rigidbody>();
                if(rb != null) rb.velocity = Vector3.zero;
            }
            else
            {
                // Check how many players are remaining
                PlayerStats[] allPlayers = (PlayerStats[])FindObjectsOfType(typeof(PlayerStats));
                List<PlayerStats> alivePlayers = new();
                foreach (PlayerStats p in allPlayers)
                {
                    if (p.alive) alivePlayers.Add(p);
                }

                if(alivePlayers.Count < 1)
                {
                    // TODO: Tie
                    gameIsOver = true;
                    Debug.LogError("No implementation for a TIED game");
                    
                    StartCoroutine(ChangeScene());
                }
                else if (alivePlayers.Count == 1)
                {
                    // Winner
                    EndGame(alivePlayers[0]);
                }
            }
        }
    }

    private void EndGame(PlayerStats winner)
    {
        if(gameOverScreen == null) return;
        gameIsOver = true;
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        gameOverText.text = $"Player {winner.playerIndex} WINS";
        
        StartCoroutine(ChangeScene());
    }
    
    private IEnumerator ChangeScene()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        List<string> levelList = new List<string>(levels);
        levelList.Remove(currentLevel);
        
        Debug.Log("Loading next level in 3 seconds...");
        yield return new WaitForSeconds(levelLoadTime);
        
        int randomLevel = Random.Range(0, levelList.Count);
        SceneManager.LoadScene(levelList[randomLevel]);
    }
}
