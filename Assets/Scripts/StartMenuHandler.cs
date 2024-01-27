using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuHandler : MonoBehaviour
{
    private InputField txtPlayerName;

    // Start is called before the first frame update
    void Start()
    {
        txtPlayerName = GetComponentInChildren<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Player.Instance.PlayerName = GetPlayerName();

        SceneManager.LoadScene(1);
    }

    private string GetPlayerName()
    {
        
        string name = txtPlayerName.text;
        Debug.Log($"Name = {name}");
        return name;
    }
}
