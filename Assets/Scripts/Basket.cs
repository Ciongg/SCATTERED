using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public float slideDuration = 1.0f; // Duration of the slide animation in seconds
    public Vector2 startOffScreenPosition; // Position off-screen to the right
    public Vector2 endOnScreenPosition; // Target position on-screen

    private RectTransform rectTransform;
    public BoxCollider2D boxcollider;
    public GardenGameManager gameManager;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Start the basket off-screen
        rectTransform.anchoredPosition = startOffScreenPosition;
    }

    public void OnTriggerEnter2D(Collider2D collider){
        gameManager.isInitialized = false;
        switch(collider.tag){
            case "gaollium":
                Debug.Log("I got an" + collider.tag);
                Destroy(collider.gameObject);

                SlideOut();
            break;

            case "sunflower":
                Debug.Log("I got an" + collider.tag);
                Destroy(collider.gameObject);

                SlideOut();
            break;

        }
    }

    public void SlideIn()
    {
        
        gameManager.countText.enabled = false;
        // Start sliding animation
        StartCoroutine(SlideToPosition(endOnScreenPosition, slideDuration));
    }

    public void SlideOut(){
        gameManager.isAlreadyPlanted = false;
        StartCoroutine(SlideToPosition(startOffScreenPosition, slideDuration));
    }

    private IEnumerator SlideToPosition(Vector2 targetPosition, float duration)
    {
        Vector2 initialPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate position
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure the basket reaches the target position
        rectTransform.anchoredPosition = targetPosition;
    }
}
