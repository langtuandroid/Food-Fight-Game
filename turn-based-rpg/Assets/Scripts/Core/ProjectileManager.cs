using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject projectile_pf;
    [SerializeField] private GameObject hit_ps;
    [SerializeField] private Transform launchPoint; // the point where a projectile is launched from.
    [SerializeField] private Transform controlPoint; // used to calculate the curve of a launched projectile.
    private void Awake()
    {
        // This may need to reference 
        //GetComponent<Character_Base>().OnShootProjectile += ShootProjectile;
    }

    public void ShootProjectile(CharacterBattle attacker, CharacterBattle targetCharacterBattle, Action onHit)
    {
        Transform projectileTransform = Instantiate(projectile_pf, attacker.GetPosition(), Quaternion.identity).transform;
        Vector3 shootDir = targetCharacterBattle.GetPosition() - attacker.GetPosition();//.normalized;

        projectileTransform.GetComponent<Projectile>().Setup(attacker, shootDir, hit_ps, targetCharacterBattle, onHit);
    }

    // need to know when this projectile hit's somehting

    public Vector2 GetLaunchPoint()
    {
        return launchPoint.position;
    }

    public Vector2 GetControlPoint()
    {
        return controlPoint.position;
    }
}
