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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider) {

        
        if(isBiodegradable){

        if(collider.tag != "Biodegradable"){
            Debug.Log("Wrong");
            PlayWrongParticle();
            Destroy(collider.gameObject);
            
        }else{
            Debug.Log("Correct");
            Destroy(collider.gameObject);
            PlayParticle();
            
        }
        }


        if(isNotBiodegradable){

        if(collider.tag != "NonBiodegradable"){
            PlayWrongParticle();
            Debug.Log("Wrong");
            Destroy(collider.gameObject);
            
        }else{
            Debug.Log("Correct");
            Destroy(collider.gameObject);
            PlayParticle();
            
        }
        }

        if(isToxic){

        if(collider.tag != "Toxic"){
            PlayWrongParticle();
            Debug.Log("Wrong");
            Destroy(collider.gameObject);
            
        }else{
            Debug.Log("Correct");
            Destroy(collider.gameObject);
            PlayParticle();
            
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
