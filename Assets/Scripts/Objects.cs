
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] prefabs;
    public float minForce = 300f; // Minimum force
    public float maxForce = 700f; // Maximum force
    public float baseTimeBetweenSpawn = 4f;
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
        AdjustDifficulty();
        
        if (Time.time > nextSpawnTime)
        {
            SpawnTrash();
            nextSpawnTime = Time.time + baseTimeBetweenSpawn;
        }
    }

    void SpawnTrash()
    {
        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //pick one random spawnpoint from array and get the pos
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f)); // random rotation

        int maxIndex = GetMaxPrefabIndex(); //para dun sa phases
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
        

        //sets phase if currentphase == phase

        phasestart.SetActive(currentPhase == 0);

        
        phase1.SetActive(currentPhase == 1);    

        
        phase2.SetActive(currentPhase == 2);

        
    }

    void AdjustDifficulty(){
        int score = gameManager.GetScore();

        if (score >= phase2ScoreThreshold){
            SetPhase(2);
            baseTimeBetweenSpawn = 3f;
        }else if (score >= phase1ScoreThreshold){
            SetPhase(1);
            baseTimeBetweenSpawn = 3.5f;
        }
    }

    int GetMaxPrefabIndex(){
        int score = gameManager.GetScore();
        //returns the maxindex taken to go over the arrays in trash prefabs
        if (score >= phase2ScoreThreshold){
            return 5;
        }else if(score >= phase1ScoreThreshold){
            return 4;
        }else{
            return 2;
        }
    
    }
}
