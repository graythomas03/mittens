using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    /**the wave counter text box*/
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI scoreText;
    /**The life the cat is currently on. Starts at 1 and ends at 10*/
    [SerializeField]private int currentLife;
    private int currentHealth = 9;
    private int totalLives = 9;
    /**the current wave. Updated by waveMan*/
    private int currentWave = 1;
    /**if the game is currently started or not*/
    [SerializeField]private bool gameStarted;
    /**if the game is currently paused*/
    [SerializeField] private bool paused;
    private bool canPause = true;
    private int score = 0;

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
        if (currentLife < 9)
        {
            // 9 lives mode
            currentLife++;
            GameObject heartsPanel = inGameUI.transform.GetChild(0).gameObject;

            for (int i = 0; i < totalLives; i++)
            {
                GameObject image = heartsPanel.transform.GetChild(i).gameObject;
                Animator animator = image.GetComponent<Animator>();

                if (i + 1 == currentLife)
                {
                    // Heart corresponds to the current life lost
                    animator.SetTrigger("Dies");
                }
                else if (currentLife == 3 || currentLife == 6)
                {
                    // All hearts must proceed to next decay state
                    animator.SetTrigger("Decays");
                }

                if (currentLife == 9)
                {
                    // Give the last heart time to decay (0.45sec) and wait a moment before starting to refill health bar
                    canPause = false;
                    StartCoroutine(ZombifyHearts());
                }
            }
        }
        else
        {
            //10th life mode
            currentHealth--;

            if (currentHealth > -1)
            {
                GameObject heartsPanel = inGameUI.transform.GetChild(0).gameObject;
                GameObject image = heartsPanel.transform.GetChild(currentHealth).gameObject;
                Animator animator = image.GetComponent<Animator>();
                animator.SetTrigger("Dies");

                if (currentHealth == 0)
                {
                    // Lose game
                }
            }
        }
    }

    IEnumerator ZombifyHearts()
    {
        // Pause to give the 9th heart time to die; disable pausing during this
        yield return new WaitForSeconds(1f);
        GameObject heartsPanel = inGameUI.transform.GetChild(0).gameObject;
        for (int i = totalLives - 1; i > -1; i--)
        {
            GameObject image = heartsPanel.transform.GetChild(i).gameObject;
            Animator animator = image.GetComponent<Animator>();

            animator.SetTrigger("Zombifies");
            yield return new WaitForSeconds(.2f);
        }

        StartTenthLife();
        canPause = true;
        StopCoroutine(ZombifyHearts());
    }

    // Called from the ZombifyHearts coroutine, which itself runs after LoseLife()
    public void StartTenthLife()
    {

    }

    public void AddPoints(int val)
    {
        score += val;
        scoreText.text = "Score: " + score;
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
        SoundManager.Instance.ToggleTitle(false);
        SoundManager.Instance.Toggle(true, 0);
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
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
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        SoundManager.Instance.Toggle(false, 0, 1, 2);
        paused = false;
        gameStarted = false;
        ResetGame();
        pauseUI.SetActive(false);
        mainMenuUI.SetActive(true);
        inGameUI.SetActive(false);
    }

    public void PauseGame(){
        if (canPause)
        {
            SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
            paused = true;
            pauseUI.SetActive(true);

            GameObject heartsPanel = inGameUI.transform.GetChild(0).gameObject;

            for (int i = 0; i < totalLives; i++)
            {
                GameObject image = heartsPanel.transform.GetChild(i).gameObject;
                image.GetComponent<Animator>().enabled = false;
            }
        }
    }

    public void UnpauseGame(){
        SoundManager.Instance.PlayOnce(SoundFX.SFXButton);
        paused = false;
        pauseUI.SetActive(false);

        GameObject heartsPanel = inGameUI.transform.GetChild(0).gameObject;

        for (int i = 0; i < heartsPanel.transform.childCount; i++)
        {
            GameObject image = heartsPanel.transform.GetChild(i).gameObject;
            image.GetComponent<Animator>().enabled = true;
        }
    }

    public void UpdateWave(int wave){
        currentWave = wave;
        waveText.text = "Wave " + currentWave;
    }

    public int GetLife(){
        return currentLife;
    }

    
}
