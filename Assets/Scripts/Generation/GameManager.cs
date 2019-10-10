using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager boardScript;

    void Awake()
    {
        /*
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
        */

        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupBoard(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
