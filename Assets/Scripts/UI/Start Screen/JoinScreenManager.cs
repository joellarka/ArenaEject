using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinScreenManager : MonoBehaviour
{
    private static JoinScreenManager instance;
    public static JoinScreenManager Instance { get => instance; }
    [SerializeField] private int maxPlayersAllowed = 4;
    private List<string> playerControllersJoined = new List<string>();
    private Dictionary<string, string> controllerToPlayerBinding= new Dictionary<string, string>();
    [SerializeField] private GameObject playerDisplayPrefab;


    private void OnEnable()
    {
        if(instance == null || instance == this)
        {
            instance = this;
            playerControllersJoined = new List<string>();
            controllerToPlayerBinding = new Dictionary<string, string>();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Update()
    {
        CheckForJoiningPlayers();
    }

    private void CheckForJoiningPlayers()
    {
        for (int i = 1; i < maxPlayersAllowed + 1; i++)
        {
            if (Input.GetButtonDown($"P{i}_Join_Duo"))
            {
                string inputDevice = $"InputDevice_{i}";
                if (playerControllersJoined.Contains(inputDevice)) return;

                playerControllersJoined.Add(inputDevice);
            }
        }
    }

    private void GeneratePrefabsForPlayers()
    {

    }
}
