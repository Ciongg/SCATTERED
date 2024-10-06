

using UnityEngine;

[System.Serializable]
public class Phase
{
    public int scoreThreshold;
    public float timeBetweenSpawn;
    public int maxNonBioPrefabIndex;
    public int maxBioPrefabIndex;
    public int maxInfectiousPrefabIndex;
    public int maxElectronicPrefabIndex;
    public int maxHazardousPrefabIndex;
}


public class Objects : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] nonBioPrefabs;
    public GameObject[] bioPrefabs;
    public GameObject[] infectiousPrefabs;
    public GameObject[] electronicPrefabs;
    public GameObject[] hazardousPrefabs;
    public float minForce = 300f; // Minimum force
    public float maxForce = 700f; // Maximum force
    public float baseTimeBetweenSpawn = 4f;
    float nextSpawnTime;
    private GameObject trash;
    public GameManager gameManager;

    public int score;


    int currentPhase;
    public Phase[] phases;
    public GameObject[] phaseGameObjects;
    

    // Update is called once per frame

    void Start(){
        nextSpawnTime = Time.time + phases[0].timeBetweenSpawn;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        SetPhase(0);
        
    }

    void Update()
    {
        score = gameManager.GetScore();
        AdjustDifficulty(score);
        
        if (Time.time > nextSpawnTime)
        {
            SpawnTrash();
            nextSpawnTime = Time.time + phases[currentPhase].timeBetweenSpawn;
        }
    }

    void SpawnTrash()
    {
        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //pick one random spawnpoint from array and get the pos
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f)); // random rotation


        
        GameObject prefabToSpawn = null;
        bool prefabFound = false;

         while (!prefabFound)
        {
            int categoryIndex = Random.Range(0, 5); // Random category index

            switch (categoryIndex)
            {
                case 0: // Non-biodegradable
                    if (phases[currentPhase].maxNonBioPrefabIndex >= 0)
                    {
                        prefabToSpawn = nonBioPrefabs[Random.Range(0, phases[currentPhase].maxNonBioPrefabIndex)];
                        prefabFound = true;
                    }
                    break;
                case 1: // Biodegradable
                    if (phases[currentPhase].maxBioPrefabIndex >= 0)
                    {
                        prefabToSpawn = bioPrefabs[Random.Range(0, phases[currentPhase].maxBioPrefabIndex)];
                        prefabFound = true;
                    }
                    break;
                case 2: // Infectious
                    if (phases[currentPhase].maxInfectiousPrefabIndex >= 0)
                    {
                        prefabToSpawn = infectiousPrefabs[Random.Range(0, phases[currentPhase].maxInfectiousPrefabIndex)];
                        prefabFound = true;
                    }
                    break;
                case 3: // Electronic
                    if (phases[currentPhase].maxElectronicPrefabIndex >= 0)
                    {
                        prefabToSpawn = electronicPrefabs[Random.Range(0, phases[currentPhase].maxElectronicPrefabIndex)];
                        prefabFound = true;
                    }
                    break;
                case 4: // Hazardous
                    if (phases[currentPhase].maxHazardousPrefabIndex >= 0)
                    {
                        prefabToSpawn = hazardousPrefabs[Random.Range(0, phases[currentPhase].maxHazardousPrefabIndex)];
                        prefabFound = true;
                    }
                    break;
            }
        }


        trash = Instantiate(prefabToSpawn, spawnPoint.position, randomRotation);

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

        for (int i = 0; i < phases.Length; i++)
        {
            phaseGameObjects[i].SetActive(i == currentPhase);
        }

        
    }

    // void AdjustDifficulty(int score){
        

    //     if (score >= phase2ScoreThreshold){
    //         SetPhase(2);
    //         baseTimeBetweenSpawn = 3f;
    //     }else if (score >= phase1ScoreThreshold){
    //         SetPhase(1);
    //         baseTimeBetweenSpawn = 3.5f;
    //     }
    // }


    void AdjustDifficulty(int score){
        
        for (int i = phases.Length - 1; i >= 0; i--)
        {
                
            if(score >= phases[i].scoreThreshold){
                
                if(currentPhase != i){
                    SetPhase(i);
                    Debug.Log("Setting phase to " + i);
                }

                break;
            }
        }
       
    }

   

    // int GetMaxPrefabIndex(){
    //     return phases[currentPhase].maxPrefabIndex;
    
    // }
}
