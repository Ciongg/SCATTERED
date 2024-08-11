using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LeafSystem : MonoBehaviour
{

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

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "NonBiodegradable" || collider.tag == "Biodegradable" || collider.tag == "Toxic" ){
            gameManager.AddLeaf(1);
            Destroy(gameObject);
        }

    }
}
