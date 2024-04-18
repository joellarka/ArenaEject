using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class KillPlane : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints = new();
    private bool gameIsOver = false;

    [Header("Temp")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text gameOverWinnerText;
    [SerializeField] private TMP_Text gameOverResultsText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameOverLoadingText;
    
    [SerializeField] private int levelLoadTime = 3;

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

                    EndGameTie();
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
        Time.timeScale = 0.2f;
        
        ShowEndGamePanel(winner.playerIndex);
    }
    
    private void EndGameTie()
    {
        if(gameOverScreen == null) return;
        gameIsOver = true;
        Time.timeScale = 0.2f;

        ShowEndGamePanel(0);
    }

    private void ShowEndGamePanel(int winnerIndex)
    {
        gameOverScreen.SetActive(true);
        gameOverText.transform.DOScale(1, 0.2f).SetEase(Ease.OutElastic);
        DOVirtual.DelayedCall(0.4f, () =>
        {
            gameOverPanel.transform.DOScale(1, 0.2f).SetEase(Ease.OutQuart).onComplete += () =>
            {
                if (winnerIndex == 0)
                {
                    // gameOverWinnerText.text = "Tie!";
                    gameOverWinnerText.text = "Player " + winnerIndex + " Wins!";
                }
                else
                {
                    gameOverWinnerText.text = "Player " + winnerIndex + " Wins!";
                }
                
                gameOverWinnerText.transform.DOScale(1, 0.2f).SetEase(Ease.OutElastic);
                
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    gameOverResultsText.transform.DOScale(1, 0.2f).SetEase(Ease.OutElastic);
                });
                
                DOVirtual.DelayedCall(1f, () =>
                {
                    StartCoroutine(ChangeLevel(new List<int> {0, 1}));
                });
            };
        });
    }
    
    private IEnumerator ChangeLevel(List<int> avoidedSceneIndex)
    {
        int sceneIndex = SceneManager.sceneCountInBuildSettings;
        
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex;

        int counter = 0;
        do
        {
            nextSceneIndex = Random.Range(0, sceneIndex);
            counter++;
            if (counter > 20)
            {
                Debug.Log("This level is the only one available in the builds settings. Reloading current level in " + levelLoadTime + " seconds...");

                StartCoroutine(Countdown(levelLoadTime + 2));
                
                DOVirtual.DelayedCall(2f, () =>
                {
                    gameOverLoadingText.transform.DOScale(1, 0.2f).SetEase(Ease.OutExpo);
                });
                
                yield return new WaitForSecondsRealtime(levelLoadTime + 2);
                SceneManager.LoadScene(currentLevelIndex);
                yield break;
            }
        } while (nextSceneIndex == currentLevelIndex || avoidedSceneIndex.Contains(nextSceneIndex));
        
        Debug.Log("Loading next level in " + levelLoadTime + " seconds...");
        yield return new WaitForSecondsRealtime(levelLoadTime + 2);
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator Countdown(int countdownTime)
    {
        while (countdownTime > 0)
        {
            gameOverLoadingText.GetComponent<TMP_Text>().text = "Loading next level in " + countdownTime;
            yield return new WaitForSecondsRealtime(1);
            countdownTime--;
        }
    }
}