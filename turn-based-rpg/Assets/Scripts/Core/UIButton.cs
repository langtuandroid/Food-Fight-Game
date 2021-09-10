using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIButton : MonoBehaviour
{
    public UnityAction OnButtonClicked;

    private void OnMouseDown()
    {
        OnButtonClicked?.Invoke();
    }
}
