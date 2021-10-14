using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
            SceneManager.LoadScene(landType.ToString());
        }
    }
}
