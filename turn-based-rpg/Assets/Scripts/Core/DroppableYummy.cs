using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.Feedbacks;

public class DroppableYummy : MonoBehaviour
{
    private CharacterBattle targetCharacterBattle;
    private CharacterBattle attackerCharacterBattle;
    private Action OnHitAction;
    private GameObject hit_ps;
    [SerializeField] protected AudioSource collisionSnd_as;
    [SerializeField] MMFeedbacks scaleFeedBack;

    private void Start()
    {
        scaleFeedBack.PlayFeedbacks();
    }

    public void Setup(CharacterBattle attacker, GameObject hit_ps, CharacterBattle target, Action onHit)
    {
        attackerCharacterBattle = attacker;
        targetCharacterBattle = target;
        this.hit_ps = hit_ps;
        OnHitAction = onHit;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        CharacterBattle characterBattle = collision.gameObject.GetComponent<CharacterBattle>();
        if (characterBattle == targetCharacterBattle)
        {
            //collisionSnd_as.Play();
            OnHitAction();
            Destroy(gameObject);
        }
    }
}
