using System.Collections;
using TMPro;
using UnityEngine;

public class EcoCoinPickup : MonoBehaviour
{
    private Collider2D coinCollider; // Reference to the coin's collider
    private GardenGameManager gardenManager;
    private SpriteRenderer spriteRenderer; // Reference to the coin's sprite renderer
    private Color originalColor; // To store the original color of the coin

    private void Start()
    {
        // Get the Collider2D and SpriteRenderer components of the coin
        gardenManager = GameObject.Find("GardenGameManager").GetComponent<GardenGameManager>();
        coinCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; // Store the original color

        // Start the coin's lifetime coroutine
        StartCoroutine(CoinLifetime());
    }

    private void Update()
    {
        // Check for touch input on mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Check if the touch is still on the screen
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

                // Check if the touch position collides with the coin's collider
                if (coinCollider.OverlapPoint(touchPosition))
                {
                    PickUpCoin();
                }
            }
        }
        // Simulate touch input with mouse on PC
        else if (Input.GetMouseButton(0)) // Check if the left mouse button is held down
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the mouse position collides with the coin's collider
            if (coinCollider.OverlapPoint(mousePosition))
            {
                PickUpCoin();
            }
        }
    }

    public void PickUpCoin()
    {
        // Optional: Add any additional logic here, like animations or sound effects

        // Destroy the eco coin GameObject
        gardenManager.ecoCoinCount++;
        gardenManager.updateEcoCoinText();
        Destroy(gameObject);
    }

    private IEnumerator CoinLifetime()
    {
        float lifetime = 10f; // Total lifetime in seconds
        float blinkStartTime = 2f; // Start blinking when 5 seconds remain
        float blinkSpeed = 0.2f; // Speed of the blinking effect

        // Wait for the blink start time (5 seconds)
        yield return new WaitForSeconds(lifetime - blinkStartTime);

        // Start blinking
        while (blinkStartTime > 0)
        {
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // Make the sprite invisible
            yield return new WaitForSeconds(blinkSpeed); // Wait for the blink speed
            spriteRenderer.color = originalColor; // Restore original color
            yield return new WaitForSeconds(blinkSpeed); // Wait for the blink speed again
            blinkStartTime -= blinkSpeed * 2; // Decrease the blink time after each full blink cycle
        }

        // Destroy the coin after the full lifetime
        Destroy(gameObject);
    }
}
