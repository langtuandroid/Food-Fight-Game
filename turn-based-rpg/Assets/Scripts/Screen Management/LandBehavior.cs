using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LandBehavior : MonoBehaviour
{

    public UnityAction<LandBehavior> LandClicked;
    public WorldMapManager.LandsTypes landType;
    public string sceneName = "no name yet";
    public bool isLocked;

    void Start()
    {
        
    }

    private void OnMouseDown()
    {
        if (!isLocked)
        {
            LandClicked?.Invoke(this);
        }
    }
}
