using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [Tooltip("Current Wave Manager")]
    private static WaveManager _instance;
    [Tooltip("Current game manager reference")]
    private GameManager gm;

    [Tooltip("do you lose a life when you win a game or not")]
    public bool loseLifeOnWin = false;


    [Header("Start Positions")]
    /**player start position object */
    [SerializeField][Tooltip("player starting position")]private GameObject startPos;
    /**player start position for wave 10*/
    [SerializeField][Tooltip("player starting position for wave 10")]private GameObject wave10StartPos;
    /**player object*/
    [SerializeField][Tooltip("player character object")]private GameObject player;

    [Header("Enemy Fields")]
    /**enemy prefab*/
    [SerializeField][Tooltip("the default enemy to copy")]private GameObject enemyPrefab;
    /**enemy spawn positions*/
    [SerializeField][Tooltip("all possible enemy start positions")]private List<GameObject> enemySpawnPos;
    /**the maximum variation in enemy spawn positions*/
    [SerializeField][Tooltip("the max amount the enemy spawn positions can vary by")][Min(0f)]private float enemySpawnPosVar;
    [Header("enemy fields")]
    /**list of the current enemies*/
    [SerializeField][Tooltip("the list of the current wave's enemies")]private List<GameObject> enemyList;
    ///**the enemies which are currently spawned*/
    // [SerializeField][Tooltip("the list of the spawned enemies")]private List<GameObject> spawnedEnemyList;
    // /**the enemies which have been killed*/
    // [SerializeField][Tooltip("the list of the dead enemies")]private List<GameObject> deadEnemyList;
    /**the max number of zombies that can chase the player at once*/
    [SerializeField][Min(1)]private int maxChasing = 5;
    /**the number of enemies currently chasing the player*/
    private List<GameObject> currentChasing;
    
    
    [Header("Wave Spawning fields")]
    /**enemy spawn count*/
    [SerializeField][Tooltip("the number of enemies that spawn in the wave")]private int enemySpawnCount;
    /**enemy spawn count wave multiplier*/
    [SerializeField][Tooltip("the percentage that the wave spawn counts are increased by every wave")][Min(0)]private float enemySpawnIncrease = .1f;
    /**the number of enemies to be spawned this wave*/
    [SerializeField]private float currentWaveSpawnCount;
    /**current number of enemies that have been spawned*/
    private int enemiesSpawned;
    /**current number of enemies that have been killed*/
    private int enemiesKilled;
    /**enemy spawn delay*/
    [SerializeField][Tooltip("the standard delay between enemies")]private float enemySpawnDelay;
    /**enemy spawn delay variation percentage*/
    [SerializeField][Tooltip("the maximum percentage variation on enemy spawn delays")][Range(0f,1f)]private float enemySpawnDelayVar;
    /**enemy spawn count wave multiplier*/
    [SerializeField][Tooltip("the percentage that the wave spawn counts are decreased by every wave")][Min(0)]private float enemySpawnDelayIncrease = .05f;
    
    /**enemy spawn timer*/
    private float spawnTimer;
    /**current time for enemy spawn */
    private float spawnTime;
    /**wave started */
    [SerializeField][Tooltip("whether or not the wave has started")]public bool waveStarted = false;
    /**Delay before a wave begins*/
    [SerializeField][Tooltip("the delay before a wave begins")]public float WaveStartDelay;
    /**Timer for the wave start*/
    private float waveTimer;
    /**current wave*/
    [SerializeField][Tooltip("the current wave")]public int currentWave = 1;
    

    [Header("Score fields")]
    [SerializeField][Tooltip("points per zombie kill")]public int pointsPerZombieKill = 10;
    [SerializeField][Tooltip("points per wave clear")]public int pointsPerWaveClear = 1000;
    [SerializeField][Tooltip("how drastically the waves affect clear points (waveMult * wave *pointsPerWaveClear")]public int waveMult = 1;
    

    public static WaveManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WaveManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject();
                    _instance = go.AddComponent<WaveManager>();
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
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        enemyPrefab.SetActive(false);
        enemyList = new List<GameObject>();
        Debug.Log(waveStarted);
        waveTimer = WaveStartDelay;
        currentChasing = new List<GameObject>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!gm.Playing()){
            return;
        }
        if(waveStarted){
            if(spawnTimer < 0f){
                SpawnEnemy();
                if(EnemiesLeftToSpawn()){
                    spawnTime = GetSpawnTime();
                    spawnTimer = spawnTime;
                }
            }
            else{
                spawnTimer -= Time.fixedDeltaTime;
            }

            if(!EnemiesLeftAlive() && !EnemiesLeftToSpawn()){
                EndWave();
            }
        }
        else{
            if(waveTimer <= 0f){
                StartWave();
                spawnTime = GetSpawnTime();
                spawnTimer = spawnTime;
            }
            else{
                waveTimer -= Time.fixedDeltaTime;
            }
        }
    }

    //Setup new wave
    void SetupWave(){
        // for(int i = 0; i < enemySpawnCount; i++){
        //     Transform spawn = enemySpawnPos[Random.Range(0,enemySpawnPos.Count)].transform;
        //     Vector3 spawnPos = new Vector3(spawn.position.x +Random.Range(-enemySpawnPosVar,enemySpawnPosVar), spawn.position.y, spawn.position.z + Random.Range(-enemySpawnPosVar,enemySpawnPosVar));
        //     enemyList.Add(Instantiate(enemyPrefab, spawnPos, spawn.rotation));
        // }
        Debug.Log("setup wave");
        waveStarted = false;
        waveTimer = WaveStartDelay;
        currentWaveSpawnCount = (int)(enemySpawnCount * Mathf.Pow(1 + enemySpawnIncrease,currentWave - 1));
        enemiesSpawned = 0;
    }

    void StartWave(){
        Debug.Log("Start wave");
        waveStarted = true;
    }

    void EndWave(){
        ClearWave();
        if(gm.GetLife() < 10){
            SetupWave();
        }
        currentWave++;
        GameManager.Instance.UpdateWave(currentWave);
    }

    void ClearWave(){
        while(enemyList.Count > 0){
            GameObject currEnemy = enemyList[enemyList.Count - 1];
            enemyList.RemoveAt(enemyList.Count - 1);
            Destroy(currEnemy);
        }
        currentChasing = new List<GameObject>();
        //enemyList = new List<GameObject>();
    }

    

    void SpawnEnemy(){
        if(!EnemiesLeftToSpawn()){
            return;
        }
        Transform spawn = enemySpawnPos[Random.Range(0,enemySpawnPos.Count)].transform;
        Vector3 spawnPos = new Vector3(spawn.position.x +Random.Range(-enemySpawnPosVar,enemySpawnPosVar), spawn.position.y, spawn.position.z + Random.Range(-enemySpawnPosVar,enemySpawnPosVar));
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, spawn.rotation);
        enemyList.Add(enemy);
        enemy.SetActive(true);
        enemiesSpawned++;
    }

    public void KillEnemy(GameObject enemy){
        if(enemyList.Contains(enemy)){
            enemyList.Remove(enemy);
            if(currentChasing.Contains(enemy)){
                currentChasing.Remove(enemy);
            }
            Destroy(enemy);
            enemiesKilled++;
            gm.AddPoints(pointsPerZombieKill);
        }
        else{
            Debug.LogWarning("enemy not detected on Kill call");
        }
    }

    float GetSpawnTime(){
        float result = enemySpawnDelay / (Mathf.Pow(1 + enemySpawnDelayIncrease,currentWave-1));
        result += result * Random.Range(-enemySpawnDelayVar,enemySpawnDelayVar);
        return result;
    }

    bool EnemiesLeftToSpawn(){
        //if(gm.GetLife() >= 10){
            //return true;
        //}
        return enemiesSpawned < currentWaveSpawnCount;
    }

    bool EnemiesLeftAlive(){
        return enemiesKilled < currentWaveSpawnCount;
    }

    public void WinWave(){
        if(loseLifeOnWin){
            gm.LoseLife();
        }
        gm.AddPoints(waveMult * currentWave * pointsPerWaveClear);
        EndWave();
    }

    public void LoseWave(){
        gm.LoseLife();
        gm.AddPoints(-1000);
        if(gm.GetLife() < 10){
            SpawnPlayer();
        }
        EndWave();
    }

    public void StartGame(){
        SetupWave();
    }

    public void Reset(){
        ClearWave();
        currentWave = 1;
        SetupWave();
        GameManager.Instance.UpdateWave(currentWave);
        SpawnPlayer();
    }

    public bool CanChase(GameObject enemy){
        //Debug.Log("can chase: " + (currentChasing.Count < maxChasing));
        if(currentChasing.Contains(enemy)){
            return true;
        }
        return currentChasing.Count < maxChasing;
    }

    public void StartChasing(GameObject enemy){
        if(!currentChasing.Contains(enemy)){
            currentChasing.Add(enemy);
        }
    }

    public void SpawnPlayer(){
        if(player){
            player.transform.position = startPos.transform.position;
        }
    }

    public void Life10(){
        if(player){
            player.transform.position = wave10StartPos.transform.position;
        }
    }

    public void DamagePlayer(){
        
        if(currentWave == 10){
            gm.LoseLife();
        }
        else{
            LoseWave();
        }
    }
}
