using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using MoreMountains.Feedbacks;

public class DropYummy : MonoBehaviour
{
    private CharacterBattle targetCharacterBattle;
    private CharacterBattle attackerCharacterBattle;
    private Action OnHitAction;
    private GameObject hit_ps;
    [SerializeField] protected AudioSource collisionSnd_as;


    private void Awake()
    {
        print("Yummy awake!");

    }

    private void Start()
    {
        print("Yummy Starg!");
    }

    public void Setup(CharacterBattle attacker, GameObject hit_ps, CharacterBattle targetCharacterBattle, Action onHit)
    {
        attackerCharacterBattle = attacker;
        this.targetCharacterBattle = targetCharacterBattle;
        this.hit_ps = hit_ps;
        OnHitAction = onHit;
        print("Drop me!!!!!!!!!!!!!!");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("yummie collision!!!!!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Droppable yummy hit somethingt ");

        CharacterBattle characterBattle = collision.gameObject.GetComponent<CharacterBattle>();
        if (characterBattle == targetCharacterBattle)
        {
            print("Bam!");
            collisionSnd_as.Play();
            OnHitAction();
            Destroy(gameObject, 2f);
        }
        else
        {
            print("Droppable yummy hit something other than the intended target");
        }
    }
}
