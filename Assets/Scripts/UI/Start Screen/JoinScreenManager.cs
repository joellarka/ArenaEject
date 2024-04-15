using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinScreenManager : MonoBehaviour
{
    private static JoinScreenManager instance;
    public static JoinScreenManager Instance { get => instance; }
    [SerializeField] [Range(2, 8)] private int maxPlayersAllowed = 4;
    private List<string> playerControllersJoined = new List<string>();
    private Dictionary<string, int> controllerToPlayerBinding= new Dictionary<string, int>();
    [SerializeField] private GameObject playerDisplayPrefab;
    [SerializeField] private Transform displayParent;
    [SerializeField] private Button startButton;
    [SerializeField] private List<Color> playerColors = new List<Color>();


    private void OnEnable()
    {
        if(instance == null || instance == this)
        {
            instance = this;
            playerControllersJoined = new List<string>();
            controllerToPlayerBinding = new Dictionary<string, int>();
            maxPlayersAllowed = Math.Clamp(maxPlayersAllowed, 2, 8);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void Update()
    {
        CheckForJoiningPlayers();
        GeneratePrefabsForPlayers();
        DetermineIfStartConditionMet();
    }

    private void CheckForJoiningPlayers()
    {
        for (int i = 1; i < maxPlayersAllowed + 1; i++)
        {
            if (Input.GetButtonDown($"P{i}_Join_Duo"))
            {
                string inputDevice = $"InputDevice_{i}";
                if (playerControllersJoined.Contains(inputDevice)) return;

                controllerToPlayerBinding.Add(inputDevice, playerControllersJoined.Count+1);
                playerControllersJoined.Add(inputDevice);
            }
        }
    }

    private void GeneratePrefabsForPlayers()
    {
        if (displayParent == null) return;
        if (displayParent.childCount == playerControllersJoined.Count) return;

        // Clear display
        // TODO: If players get to select avatars/colors/hats, then this needs to be saved/reworked
        List<GameObject> displayParentChildren = new List<GameObject>();
        for (int i = 0; i < displayParent.childCount; i++)
        {
            displayParentChildren.Add(displayParent.GetChild(i).gameObject);
        }
        foreach (GameObject child in displayParentChildren)
        {
            Destroy(child);
        }

        // Generate new display
        for (int i = 0; i < playerControllersJoined.Count; i++)
        {
            GameObject displayObj = Instantiate(playerDisplayPrefab, Vector3.zero, Quaternion.identity, displayParent);
            JoinDisplay displayComponents = displayObj.GetComponent<JoinDisplay>();
           
            if (!controllerToPlayerBinding.TryGetValue(playerControllersJoined[i], out int playerNumber))
            {
                Debug.LogError(playerControllersJoined[i] + "not included in dictonary, state mutated unexpectantly");
                return;
            }
            
            displayComponents.playerText.text = $"Player {playerNumber}";
            displayComponents.controllerText.text = $"Controller {(playerControllersJoined[i])[^1]}";
            if (playerColors[i] != null) {
                displayComponents.displayImage.color = playerColors[i];
            }
        }
    }

    private void DetermineIfStartConditionMet()
    {
        if (startButton == null) return;

        if(playerControllersJoined.Count >= 2)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
}
