using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    [Tooltip("Current Wave Manager")]
    private static WaveManager _instance;
    [Tooltip("Current game manager reference")]
    private GameManager gm;


    [Header("Start Positions")]
    /**player start position object */
    [SerializeField][Tooltip("player starting position")]private GameObject startPos;
    /**player start position for wave 10*/
    [SerializeField][Tooltip("player starting position for wave 10")]private GameObject wave10StartPos;

    [Header("Enemy Fields")]
    /**enemy prefab*/
    [SerializeField][Tooltip("the default enemy to copy")]private GameObject enemyPrefab;
    /**enemy spawn positions*/
    [SerializeField][Tooltip("all possible enemy start positions")]private List<GameObject> enemySpawnPos;
    /**the maximum variation in enemy spawn positions*/
    [SerializeField][Tooltip("the max amount the enemy spawn positions can vary by")][Min(0f)]private float enemySpawnPosVar;
    /**enemy spawn count*/
    [SerializeField][Tooltip("the number of enemies that spawn in the wave")]private int enemySpawnCount;
    /**current number of enemies that have been spawned*/
    private int enemiesSpawned;
    /**current number of enemies that have been killed*/
    private int enemiesKilled;
    /**enemy spawn delay*/
    [SerializeField][Tooltip("the standard delay between enemies")]private float enemySpawnDelay;
    /**enemy spawn delay variation percentage*/
    [SerializeField][Tooltip("the maximum percentage variation on enemy spawn delays")][Range(0f,1f)]private float enemySpawnDelayVar;
    /**enemy spawn timer*/
    private float spawnTimer;
    /**current time for enemy spawn */
    private float spawnTime;

    [Header("enemy fields")]
    /**list of the current enemies*/
    [SerializeField][Tooltip("the list of the current wave's enemies")]private List<GameObject> enemyList;
    ///**the enemies which are currently spawned*/
    // [SerializeField][Tooltip("the list of the spawned enemies")]private List<GameObject> spawnedEnemyList;
    // /**the enemies which have been killed*/
    // [SerializeField][Tooltip("the list of the dead enemies")]private List<GameObject> deadEnemyList;

    /**wave started */
    [SerializeField][Tooltip("whether or not the wave has started")]public bool waveStarted = false;
    /**Delay before a wave begins*/
    [SerializeField][Tooltip("the delay before a wave begins")]public float WaveStartDelay;
    /**Timer for the wave start*/
    private float waveTimer;
    /**current wave*/
    [SerializeField][Tooltip("the current wave")]public int currentWave = 1;

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
    }

    void StartWave(){
        Debug.Log("Start wave");
        waveStarted = true;
    }

    void EndWave(){
        ClearWave();
        SetupWave();
        currentWave++;
    }

    void ClearWave(){
        while(enemyList.Count > 0){
            GameObject currEnemy = enemyList[enemyList.Count - 1];
            enemyList.RemoveAt(enemyList.Count - 1);
            Destroy(currEnemy);
        }
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

    void KillEnemy(GameObject enemy){
        if(enemyList.Contains(enemy)){
            enemyList.Remove(enemy);
            Destroy(enemy);
            enemiesKilled++;
        }
        else{
            Debug.LogWarning("enemy not detected on Kill call");
        }
    }

    float GetSpawnTime(){
        float result = enemySpawnDelay;
        result += result * Random.Range(-enemySpawnDelayVar,enemySpawnDelayVar);
        return result;
    }

    bool EnemiesLeftToSpawn(){
        return enemiesSpawned < enemySpawnCount;
    }

    bool EnemiesLeftAlive(){
        return enemiesKilled < enemySpawnCount;
    }

    public void LoseLife(){
        gm.LoseLife();
        EndWave();
    }

    public void StartGame(){
        SetupWave();
    }

    public void Reset(){
        ClearWave();
        currentWave = 1;
        SetupWave();
    }
}
