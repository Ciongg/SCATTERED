// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;
// using System;
// using Unity.VisualScripting;

// public class DragAndDrop : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
// {
//     // private RectTransform rectTransform;
//      private SpriteRenderer spriteRenderer;
//      private Vector3 offset;
//      public Rigidbody2D rb;
//      public bool isDragging = false;
//      public float maxSpeed = 5f;



//     public void OnBeginDrag(PointerEventData eventData)
//     {
//         ;
//         spriteRenderer.color=new Color(200, 0, 0, 1f);
        
//         Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
//         offset = transform.position - worldMousePosition;


        
//         rb.simulated = false;
//         isDragging = true;
//     }



//     public void OnDrag(PointerEventData eventData)
//     {
        
//         rb.gravityScale = 0;

//         Vector3 screenPoint = Input.mousePosition;
//         screenPoint.z = Camera.main.WorldToScreenPoint(transform.position).z; // Preserve the Z position
//         Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(screenPoint);

//         Vector3 targetPosition = worldMousePosition + offset;

//         rb.simulated = true;

//         rb.MovePosition(targetPosition);
        
        
//         // transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
//     }



//     public void OnEndDrag(PointerEventData eventData)
//     {
        


//         if(isDragging){
//         rb.gravityScale = 0;

//         Vector2 dragStartPosition = eventData.pressPosition;
//         Vector2 dragEndPosition = eventData.position;
//         Vector2 dragDelta = (dragEndPosition - dragStartPosition) / Time.deltaTime;

//         if(dragDelta.magnitude > maxSpeed){
//             dragDelta = dragDelta.normalized * maxSpeed;
//         }

        
//         rb.gravityScale = 1;
//         rb.AddForce(dragDelta, ForceMode2D.Impulse);

//         rb.simulated = true;
//         rb.drag = 0.5f;
        
//         }

//         spriteRenderer.color = Color.red;
//         isDragging = false;

//     }

//     void Start()
//     {
//         // rectTransform = GetComponent<Transform>();
        
//         spriteRenderer = GetComponent<SpriteRenderer>();
//     }


//     void OnCollisionEnter2D(Collision2D collision){
//             if(collision.collider.tag == "Deadly"){
//                 Destroy(gameObject);
//             }
//     }

//     void Update() {
//         Debug.Log(rb.gravityScale);
//     }


    
   
// }







using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Color currentColor = spriteRenderer.color;
        currentColor.a = 0.5f;
        spriteRenderer.color = currentColor;


        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        offset = transform.position - worldMousePosition;

        // Create tThe joint
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
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
        // Vector3 targetPosition = worldMousePosition + offset;

        // Move the Rigidbody using the targetjoint
        targetJoint.target = worldMousePosition;

        if (drawDragLine)
        {
            //parameters start, end, color
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Deadly"))
        {
            Destroy(gameObject);
        }
    }
}
