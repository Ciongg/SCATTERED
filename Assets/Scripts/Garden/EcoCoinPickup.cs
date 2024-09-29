using System.Collections;
using TMPro;
using UnityEngine;

public class EcoCoinPickup : MonoBehaviour
{
    private Collider2D coinCollider; 
    private GardenGameManager gardenManager;
    private SpriteRenderer spriteRenderer; 
    private Color originalColor;
    private float lifetime;  // Randomized lifetime for each coin

    private void Start()
    {
        gardenManager = GameObject.Find("GardenGameManager").GetComponent<GardenGameManager>();
        coinCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; 

        // Randomize the lifetime (between 5 and 10 seconds, for example)
        lifetime = Random.Range(0.5f, 3f);

        gardenManager.ecoCoinCount++;
        gardenManager.updateEcoCoinText();

        // Start the transparency fade effect
        StartCoroutine(FadeOutCoin());
    }

    private IEnumerator FadeOutCoin()
    {
        float elapsedTime = 0f; // How long the coin has been alive

        while (elapsedTime < lifetime)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the alpha based on the elapsed time and lifetime
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / lifetime);
            
            // Apply the new color with the fading alpha
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null; // Wait for the next frame
        }

        // Destroy the coin after its lifetime is over
        Destroy(gameObject);
    }
    private void Update()
    {
        
        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);

        //     // Check if the touch is still on the screen
        //     if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //     {
        //         Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

        //         // Check if the touch position collides with the coin's collider
        //         if (coinCollider.OverlapPoint(touchPosition))
        //         {
        //             PickUpCoin();
        //         }
        //     }
        // }
       
        // else if (Input.GetMouseButton(0)) // Check if the left mouse button is held down
        // {
        //     Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //     // Check if the mouse position collides with the coin's collider
        //     if (coinCollider.OverlapPoint(mousePosition))
        //     {
        //         PickUpCoin();
        //     }
        // }
    }

    

    // private IEnumerator CoinLifetime()
    // {
    //     float lifetime = 3f; 
    //     float blinkStartTime = 2f; 
    //     float blinkSpeed = 0.2f; 

    //     // Wait for the blink start time (5 seconds)
    //     yield return new WaitForSeconds(lifetime - blinkStartTime);

    //     // Start blinking
    //     while (blinkStartTime > 0)
    //     {
    //         spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); 
    //         yield return new WaitForSeconds(blinkSpeed); 
    //         spriteRenderer.color = originalColor; 
    //         yield return new WaitForSeconds(blinkSpeed); 
    //         blinkStartTime -= blinkSpeed * 2; 
    //     }

    //     // Destroy the coin after the full lifetime
    //     Destroy(gameObject);
    // }
}
