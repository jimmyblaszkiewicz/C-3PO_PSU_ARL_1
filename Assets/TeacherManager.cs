using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class TeacherManager : MonoBehaviourPunCallbacks
{

    public PlayerList playerList;
    public GameObject buttonPrefab;
    public GameObject buttonListContent;
    public Button monitorButton;
    public Button backButton;

    private Dictionary<string, GameObject> buttonDictionary;
    private string selectedPlayer;
    public CameraMovement cameraMovement;
    public GameObject teacherUI;

    public Text timespent;
    public Text name;
    public Text scenariosCompleted;

    // Start is called before the first frame update
    void Start()
    {
        buttonDictionary = new Dictionary<string, GameObject>();
        monitorButton.onClick.AddListener(SpectatePlayer);
        monitorButton.gameObject.SetActive(false);
        backButton.onClick.AddListener(BackToLobby);
        backButton.gameObject.SetActive(false);
        selectedPlayer = null;
        
     
    }

    // Update is called once per frame
    void Update()
    {

        updatePlayerList();

        if (selectedPlayer != null)
        {
            PlayerStatistics activePlayer = playerList.getPlayerList()[selectedPlayer];
            timespent.text = "Total Time Spent: " + string.Format("{0:.##}",activePlayer.timeSpent) + " s";
            name.text = "Student : " + activePlayer.name;
            scenariosCompleted.text = "Scenarios Completed: " + activePlayer.scenariosCompleted.ToString();

        }
            


    }


    private void updatePlayerList()
    {
        foreach(KeyValuePair<string,PlayerStatistics> entry in playerList.getPlayerList())
        {
            if (!buttonDictionary.ContainsKey(entry.Key))
            {
                AddButton(entry.Value.name);
            }
        }

        foreach (KeyValuePair<string, GameObject> entry in buttonDictionary.Reverse())
        {
            if (!playerList.getPlayerList().ContainsKey(entry.Key))
            {
                if (entry.Key == selectedPlayer)
                {
                    selectedPlayer = null;
                    HideMonitorButton();
                }
                RemoveButton(entry.Key);
            }
        }


    }


    private void HideMonitorButton()
    {
        monitorButton.gameObject.SetActive(false);
    }

    private void ShowMonitorButton()
    {
        monitorButton.gameObject.SetActive(true);
    }

    private void AddButton(string name)
    {
        Debug.Log("Creating Button for player: " + name);
        GameObject button = (GameObject)Instantiate(buttonPrefab);
        button.transform.SetParent(buttonListContent.transform);
        button.GetComponent<Button>().onClick.AddListener(delegate { onClick(name); });
        button.transform.GetChild(0).GetComponent<Text>().text = name;
        buttonDictionary.Add(name, button);
    }

    private void RemoveButton(string name)
    {
        GameObject button = buttonDictionary[name];
        buttonDictionary.Remove(name);
        Destroy(button);

    }


    void onClick(string name)
    {
        Debug.Log("Button Clicked");
        selectedPlayer = name;
        ShowMonitorButton();

    }

    void BackToLobby()
    {
        this.teacherUI.SetActive(true);
        backButton.gameObject.SetActive(false);

    }

    private void SpectatePlayer()
    {

        cameraMovement.setPlayer(playerList.getPlayerList()[selectedPlayer].gameObject.GetComponent<Transform>());
        this.teacherUI.SetActive(false);
        backButton.gameObject.SetActive(true);

    }

}
