using UnityEngine;

public class Swipe : MonoBehaviour
{
    public GameObject phonePhase; // Phone panel or placeholder to show/hide
    public float swipeThreshold = 50f; // Minimum distance required for a swipe
    public Collider2D swipeTrigger; // The Collider2D that will be used to detect swipe start

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);

            if (touch.phase == TouchPhase.Began)
            {
                // Check if touch position is inside the trigger collider
                if (swipeTrigger.OverlapPoint(touchPosition))
                {
                    startTouchPosition = touchPosition;
                    Debug.Log("Touch began within collider at: " + startTouchPosition);
                }
                else
                {
                    return; // Ignore touches outside the collider
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touchPosition;

                // Calculate the vertical distance of the swipe
                float verticalDistance = endTouchPosition.y - startTouchPosition.y;
                Debug.Log("Vertical distance: " + verticalDistance);

                // Detect swipe direction if it exceeds the threshold
                if (Mathf.Abs(verticalDistance) > swipeThreshold)
                {
                    if (verticalDistance > 0) // Swipe down
                    {
                        phonePhase.SetActive(false); // Hide phone
                        Debug.Log("Swipe down detected");
                    }
                    else // Swipe up
                    {
                        phonePhase.SetActive(true); // Show phone
                        Debug.Log("Swipe up detected");
                    }
                }
            }
        }
    }
}
