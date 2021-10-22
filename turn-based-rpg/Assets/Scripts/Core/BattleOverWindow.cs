using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BattleOverWindow : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winnerField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(string winnerName, bool isPlayerTheWinner)
    {
        if (!isPlayerTheWinner)
        {
            winnerField.text = "You Lose!";
        }
        else
        {
            winnerField.text = "You Win!";
        }
        //winnerField.text = winnerName;
    }


    public void Done()
    {
        SceneManager.LoadScene(WorldMapManager.LandsTypes.home.ToString());
    }
}
