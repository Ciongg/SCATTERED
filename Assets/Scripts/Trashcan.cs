using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : MonoBehaviour
{


    public bool isBiodegradable;
    public bool isNotBiodegradable;
    public bool isToxic;
    
    public GameObject particlePrefab;
    public GameObject wrongParticlePrefab;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WrongAnswer(Collider2D collider){
        Debug.Log("Wrong");
            gameManager.LoseLife(1);
            PlayWrongParticle();
            Destroy(collider.gameObject);
        
    }

    void CorrectAnswer(Collider2D collider){
         Debug.Log("Correct");
            gameManager.AddScore(Random.Range(10, 16));   
            Destroy(collider.gameObject);
            PlayParticle();
    }

    void OnTriggerEnter2D(Collider2D collider) {

        
        if(isBiodegradable){

        if(collider.tag != "Biodegradable"){
            WrongAnswer(collider);
            
        }else{
           
            CorrectAnswer(collider);
        }
        }


        if(isNotBiodegradable){

        if(collider.tag != "NonBiodegradable"){
            WrongAnswer(collider);
            
        }else{
             CorrectAnswer(collider);
            
        }
        }

        if(isToxic){

        if(collider.tag != "Toxic"){
            WrongAnswer(collider);
            
        }else{
            CorrectAnswer(collider);
            
        }
        }



    }


    void PlayParticle(){
        GameObject particleSystemInstance = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
                ps.Play();
                
                // Destroy the particle system after it has played to clean up
                Destroy(particleSystemInstance, ps.main.duration + ps.main.startLifetime.constantMax);
    }


    void PlayWrongParticle(){
        GameObject particleSystemInstance = Instantiate(wrongParticlePrefab, transform.position, Quaternion.identity);
                ParticleSystem ps = particleSystemInstance.GetComponent<ParticleSystem>();
                ps.Play();
                
                // Destroy the particle system after it has played to clean up
                Destroy(particleSystemInstance, ps.main.duration + ps.main.startLifetime.constantMax);

    }
}
