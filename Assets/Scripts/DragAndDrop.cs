using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Vector3 offset;
    private Rigidbody2D rb;
    private TargetJoint2D targetJoint;
    public bool isDragging = false;
    public float damping = 10.0f;
    public float frequency = 5.0f;
    public bool drawDragLine = true;
    public Color lineColor = Color.red;
    public string originalTag;

    private static Dictionary<int, DragAndDrop> activeDrags = new Dictionary<int, DragAndDrop>();

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        originalTag = gameObject.tag;
        gameObject.tag = "Untagged";
    }

    void Update()
    {
        // Update dragging for all active drags
        foreach (var kvp in activeDrags)
        {
            int pointerId = kvp.Key;
            DragAndDrop drag = kvp.Value;

            if (drag.isDragging)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.fingerId == pointerId)
                    {
                        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(touch.position);
                        drag.targetJoint.target = worldMousePosition;

                        if (drag.drawDragLine)
                        {
                            Debug.DrawLine(drag.targetJoint.transform.TransformPoint(drag.targetJoint.anchor), worldMousePosition, drag.lineColor);
                        }
                        break;
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        gameObject.tag = originalTag;

        Color currentColor = spriteRenderer.color;
        currentColor.a = 0.5f;
        spriteRenderer.color = currentColor;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - worldMousePosition;

        // Create the joint
        if (targetJoint == null)
        {
            targetJoint = gameObject.AddComponent<TargetJoint2D>();
            targetJoint.dampingRatio = damping;
            targetJoint.frequency = frequency;
            targetJoint.autoConfigureTarget = false;
            targetJoint.target = worldMousePosition;
        }

        targetJoint.anchor = targetJoint.transform.InverseTransformPoint(worldMousePosition);

        isDragging = true;
        activeDrags[eventData.pointerId] = this; // Track drag instance by pointerId
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        targetJoint.target = worldMousePosition;

        if (drawDragLine)
        {
            Debug.DrawLine(targetJoint.transform.TransformPoint(targetJoint.anchor), worldMousePosition, lineColor);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = 1f;
        spriteRenderer.color = currentColor;
        isDragging = false;

        // Remove Target Joint component when dragging ends
        if (targetJoint != null)
        {
            Destroy(targetJoint);
            targetJoint = null;
        }

        activeDrags.Remove(eventData.pointerId); // Remove drag instance by pointerId
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Deadly")
        {
            gameManager.LoseLife(1);
            Destroy(gameObject);
        }
    }

}
