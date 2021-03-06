using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using CodeMonkey.Utils;


public class Projectile : MonoBehaviour
{
    protected Vector3 shootDir = new Vector3(1.1f,1.1f,1.1f);
    protected CharacterBattle targetCharacterBattle;
    protected Action OnHitAction;
    protected GameObject hit_ps;
    [SerializeField] protected AudioSource collisionSnd_as;
    protected SpriteRenderer spriteRenderer;
    private Color spriteColor;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = spriteRenderer.color;
    }

    public virtual void Setup(CharacterBattle attacker, Vector3 shootDir, GameObject hit_ps, CharacterBattle targetCharacterBattle, Action OnHit)
    {
        this.shootDir = shootDir;
        this.targetCharacterBattle = targetCharacterBattle;
        this.hit_ps = hit_ps;
        OnHitAction = OnHit;
        transform.eulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(shootDir));
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        float moveSpeed = 1.5f;
        transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
        CharacterBattle characterBattle = collision.gameObject.GetComponent<CharacterBattle>();
        if (characterBattle == targetCharacterBattle)
        {
            if (collisionSnd_as)
            {
                print("!!!!!!!!!!PLAY HIT SOUND");

                collisionSnd_as.Play();
            }
            else
            {
                print("CANT PLAY HIT SOUND");
            }

            if(hit_ps)
            {
                Instantiate(hit_ps, collision.gameObject.transform.position, Quaternion.identity);
            }
            OnHitAction();

            spriteColor.a = 0;
            spriteRenderer.color = spriteColor;
            //Destroy(gameObject);
        }
    }
}
