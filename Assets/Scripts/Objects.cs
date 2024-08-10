
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] prefabs;
    public float timeBetweenSpawn;
    public float minForce = 300f; // Minimum force
    public float maxForce = 700f; // Maximum force
    public float baseTimeBetweenSpawn = 2f;
    public int phase1ScoreThreshold = 50;
    public int phase2ScoreThreshold = 100;
    float nextSpawnTime;
    private GameObject trash;
    public GameManager gameManager;

    public GameObject phasestart;
    public GameObject phase1;
    public GameObject phase2;

    int currentPhase;

    // Update is called once per frame

    void Start(){
        nextSpawnTime = Time.time + baseTimeBetweenSpawn;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetPhase(0);
        
    }

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnTrash();
            AdjustDifficulty();
            nextSpawnTime = Time.time + baseTimeBetweenSpawn;
        }
    }

    void SpawnTrash()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        int maxIndex = GetMaxPrefabIndex();
        trash = Instantiate(prefabs[Random.Range(0, maxIndex)], spawnPoint.position, randomRotation);
        Rigidbody2D rb = trash.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomForce = Random.Range(minForce, maxForce);
            Vector2 forceDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1).normalized;
            rb.AddForce(forceDirection * randomForce);
        }
    }

    void SetPhase(int phase){
        currentPhase = phase;
        UpdateMap(currentPhase);
    }

    void UpdateMap(int currentPhase){
        // Ensure phaseStart is always active at the start

        

        phasestart.SetActive(currentPhase == 0);

        // Manage phase 1 visibility
        phase1.SetActive(currentPhase == 1);    

        // Manage phase 2 visibility
        phase2.SetActive(currentPhase == 2);

        
    }

    void AdjustDifficulty(){
        int score = gameManager.GetScore();

        if (score >= phase2ScoreThreshold){
            SetPhase(2);
            baseTimeBetweenSpawn = 1f;
        }else if (score >= phase1ScoreThreshold){
            SetPhase(1);
            baseTimeBetweenSpawn = 1.5f;
        }
    }

    int GetMaxPrefabIndex(){
        int score = gameManager.GetScore();

        if (score >= phase2ScoreThreshold){
            return 5;
        }else if(score >= phase1ScoreThreshold){
            return 4;
        }else{
            return 2;
        }
    
    }
}
