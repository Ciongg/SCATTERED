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
    GameManager gameManager;
    public Collider2D trashBinCollider; 

    public CharacterDialogue characterDialogue;

    

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        characterDialogue = GameObject.Find("CharacterPanel").GetComponent<CharacterDialogue>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WrongAnswer(Collider2D collider){
        int dialogueToShowIndex = Random.Range(0, characterDialogue.wrongAnswersText.Length);

        string dialogueToShow = characterDialogue.wrongAnswersText[dialogueToShowIndex];

        characterDialogue.ShowAssuranceDialogue(dialogueToShow);
        
            gameManager.LoseLife(1);
            PlayWrongParticle();
            Destroy(collider.gameObject);
        
    }

    void CorrectAnswer(Collider2D collider){
         int dialogueToShowIndex = Random.Range(0, characterDialogue.rightAnswersText.Length);
        
        string dialogueToShow = characterDialogue.rightAnswersText[dialogueToShowIndex];

        characterDialogue.ShowSmileDialogue(dialogueToShow);
        
            gameManager.AddScore(Random.Range(10, 20));   
            Destroy(collider.gameObject);
            PlayParticle();
    }

    
    void OnTriggerEnter2D(Collider2D collider) {

        if(collider.tag == "Leaf"){
            return;
        }

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


                Vector3 colliderBounds = trashBinCollider.bounds.size;
                Vector3 topPosition = trashBinCollider.bounds.center + new Vector3(0, colliderBounds.y / 2, 0);

                GameObject particleSystemInstance = Instantiate(particlePrefab, topPosition, Quaternion.Euler(-90f, 0f, 0f));

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
