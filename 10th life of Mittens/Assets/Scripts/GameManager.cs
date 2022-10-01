using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /**the wave manager */
    [SerializeField]private WaveManager waveMan;
    /**the main menu UI*/
    [SerializeField]private GameObject mainMenuUI;
    /**the in game UI panels*/
    [SerializeField]private GameObject inGameUI;
    /**the pause menu UI*/
     [SerializeField]private GameObject pauseUI;
    /**The life the cat is currently on. Starts at 1 and ends at 10*/
    [SerializeField]private int currentLife;
    /**if the game is currently started or not*/
    [SerializeField]private bool gameStarted;
    /**if the game is currently paused*/
    [SerializeField]private bool paused;
    [Tooltip("Current Game Manager")]
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<GameManager>();
                }
            }
            return _instance;
        }
    }

    void Awake(){
        if (Instance != this)
        {
            Destroy(this.gameObject);
            Destroy(this);
            return;
        }
        _instance = this;
        gameStarted = false;
        paused = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        waveMan = WaveManager.Instance;
        mainMenuUI.SetActive(true);
        pauseUI.SetActive(false);
        inGameUI.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void LoseLife(){
        currentLife++;
    }

    public bool Paused(){
        return paused;
    }

    public bool GameStarted(){
        return gameStarted;
    }

    public bool Playing(){
        return gameStarted && !paused;
    }

    public void StartGame(){
        mainMenuUI.SetActive(false);
        inGameUI.SetActive(true);
        gameStarted = true;
        waveMan.StartGame();
    }

    public void ResetGame(){
        currentLife = 1;
        waveMan.Reset();
    }

    public void MainMenu(){
        paused = false;
        gameStarted = false;
        ResetGame();
        pauseUI.SetActive(false);
        mainMenuUI.SetActive(true);
        inGameUI.SetActive(false);
    }

    public void PauseGame(){
        paused = true;
        pauseUI.SetActive(true);
        inGameUI.SetActive(false);
    }

    public void UnpauseGame(){
        paused = false;
        pauseUI.SetActive(false);
        inGameUI.SetActive(true);
    }

    
}
