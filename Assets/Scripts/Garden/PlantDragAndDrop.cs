using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantDragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private SpriteRenderer spriteRenderer;
    private Vector3 offset;
    private Rigidbody2D rb;
    private TargetJoint2D targetJoint;
    public bool isDragging = false;
    public float damping = 10.0f;
    public float frequency = 5.0f;
    public bool drawDragLine = true;
    public Color lineColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Access SpriteRenderer from child
        rb = GetComponent<Rigidbody2D>();
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        Color currentColor = spriteRenderer.color;
        currentColor.a = 0.5f;
        spriteRenderer.color = currentColor;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - worldMousePosition;
        
            targetJoint = gameObject.AddComponent<TargetJoint2D>();
            targetJoint.dampingRatio = damping;
            targetJoint.frequency = frequency;
            targetJoint.autoConfigureTarget = false;
            targetJoint.target = worldMousePosition;
        

        targetJoint.anchor = targetJoint.transform.InverseTransformPoint(worldMousePosition);

        isDragging = true;
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
        Destroy(targetJoint);
        targetJoint = null;
        
    }
}
