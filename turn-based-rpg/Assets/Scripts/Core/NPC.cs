using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using VikingCrew.Tools.UI;


public class NPC : MonoBehaviour
{
    [TextArea] public string speech= "There's nothing like \n a good food fight!";
    [SerializeField] float speachScreenTime = 3f;
    [SerializeField] bool triggerFight;
    private bool isTalking;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Speak();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Speak();
    }

    private void Speak()
    {
        if (isTalking) return;
        isTalking = true;
        SpeechBubbleManager.Instance.AddSpeechBubble(transform, speech, SpeechBubbleManager.SpeechbubbleType.NORMAL, speachScreenTime);
        Invoke("DoneTalking", speachScreenTime);
    }

    private void DoneTalking()
    {
        isTalking = false;
        if(triggerFight)
        {
            //
            SceneManager.LoadScene("Battle Dev");

        }

    }

}
