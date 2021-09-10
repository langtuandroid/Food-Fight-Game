// Author: Keith Goodman 5/18/2021
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PreventClickThru : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
