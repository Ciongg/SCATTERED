using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specials : MonoBehaviour
{
    GameManager gameManager;
    public GameObject heartParticles;
    public GameObject timeParticles;
    public float slowTimeLength;
    public float smogActiveLength;
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

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "NonBiodegradable" || collider.tag == "Biodegradable" || collider.tag == "Toxic" ){
            
            Destroy(gameObject);
            switch(this.tag){
                case "extralife":
                    
                    
                    if(gameManager.life < 5){
                        ExtraLife();
                        characterDialogue.ShowHealthDialogue();
                    }else{
                        
                        GameObject heartInstance = Instantiate(heartParticles, transform.position, Quaternion.identity);
                        Destroy(heartInstance, 1f);
                        characterDialogue.ShowHealthFullDialogue();
                    }

                    
                
                break;


                case "timeslow":
                        gameManager.StartSlowTime(slowTimeLength);
                        characterDialogue.ShowTimeDialogue();
                break;

                case "smog":
                        gameManager.ActivateSmog(smogActiveLength);
                        characterDialogue.ShowSmogDialogue();
                        
                break;
            }
            
        
            
        }
    }


    void ExtraLife(){
        gameManager.life++;
        gameManager.UpdateLifeText();
        GameObject heartInstance = Instantiate(heartParticles, transform.position, Quaternion.identity);
        Destroy(heartInstance, 1f);
    }

    

    
}
