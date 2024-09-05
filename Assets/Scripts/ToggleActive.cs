using UnityEngine;
using UnityEngine.UI;

public class ToggleActive : MonoBehaviour
{
    public Canvas targetObject; 

    public void ToggleGameObject()
    {
       
        targetObject.gameObject.SetActive(!targetObject.gameObject.activeSelf);
    }
}
