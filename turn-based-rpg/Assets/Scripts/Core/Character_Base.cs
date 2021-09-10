using System;
using UnityEngine;
using V_AnimationSystem;
using CodeMonkey.Utils;
using UnityEngine.Events;

/*
 * Character Base Class
 * */
public class Character_Base : MonoBehaviour {

    #region BaseSetup
    private V_UnitSkeleton unitSkeleton;
    private V_UnitAnimation unitAnimation;
    private AnimatedWalker animatedWalker;
    private UnitAnimType attackUnitAnim;
    private Color materialTintColor;

    private UnityAction AttackAniDone;
    //public UnityAction<Vector3> OnShootProjectile;

    private void Awake()
    {
        //Transform bodyTransform = transform.Find("Body");
        //transform.Find("Body").GetComponent<MeshRenderer>().material = new Material(GetMaterial());
        //unitSkeleton = new V_UnitSkeleton(1f, bodyTransform.TransformPoint, (Mesh mesh) => bodyTransform.GetComponent<MeshFilter>().mesh = mesh);
        //unitAnimation = new V_UnitAnimation(unitSkeleton);

        //UnitAnimType idleUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle");
        //UnitAnimType walkUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk");
        //UnitAnimType hitUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_Hit");
        //attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");

        //animatedWalker = new AnimatedWalker(unitAnimation, idleUnitAnim, walkUnitAnim, 1f, 1f);
    }

    private void Update() {
        //unitSkeleton.Update(Time.deltaTime);

        //if (materialTintColor.a > 0) {
        //    float tintFadeSpeed = 6f;
        //    materialTintColor.a -= tintFadeSpeed * Time.deltaTime;
        //    GetMaterial().SetColor("_Tint", materialTintColor);
        //}
    }

    //public V_UnitAnimation GetUnitAnimation() {
    //    return unitAnimation;
    //}

    //public AnimatedWalker GetAnimatedWalker() {
    //    return animatedWalker;
    //}
    #endregion

    //public Material GetMaterial()
    //{
    //    //return transform.Find("Body").GetComponent<MeshRenderer>().material;
    //    //return GetComponent<MeshRenderer>().material;
    //}

    //public void SetColorTint(Color color) {
    //    materialTintColor = color;
    //}

    public void PlayAnimMove(Vector3 moveDir)
    {
        print("PlayAnimMove");

        //animatedWalker.SetMoveVector(moveDir);
    }

    public void PlayAnimIdle()
    {
        print("PlayAnimIdle");

        //animatedWalker.SetMoveVector(Vector3.zero);
    }

    public void PlayAnimIdle(Vector3 animDir)
    {
        print("PlayAnimIdle");

        //animatedWalker.PlayIdleAnim(animDir);
    }

    public void PlayAnimSlideRight()
    {
        print("PlayAnimSlideRight");

        //unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideRight"), 1f, null);
    }

    public void PlayAnimSlideLeft()
    {
        print("PlayAnimSlideLeft");

        //unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("dBareHands_SlideLeft"), 1f, null);
    }

    public void PlayAnimLyingUp()
    {
        print("PlayAnimLyingUp");
        //unitAnimation.PlayAnimForced(UnitAnim.GetUnitAnim("LyingUp"), 1f, null);
    }

    // Keith 

    public void PlayAnimAttack(CharacterBattle attacker, CharacterBattle target, Action onHit, Action onComplete)
    {

        if (attacker.GetAttackType() == CharacterBattle.AttackType.Range)
        {
            ProjectileManager projectileManager = GetComponent<ProjectileManager>();
            projectileManager.ShootProjectile(attacker, target, onHit);
        }
        else
        {

        }
        print("PlayAnimAttack");
        if (onHit != null && attacker.GetAttackType() == CharacterBattle.AttackType.Melee)
        {
            // The animation is at it's hit point, like throwing punch. (could happen multiple times)
            // this is for melee. Range attacks should call this when the projectile hits the target.
            onHit();
        } 
        //unitAnimation.PlayAnimForced(attackUnitAnim, attackDir, 1f, (UnitAnim unitAnim) =>
        //{
        //    if (onComplete != null) onComplete();
        //}, (string trigger) =>
        //{
        //    if (onHit != null) onHit();
        //}, null);

        if (onComplete != null)
        {
            print("Attack ani done");
            // This will probably be called when the animation is done.
            // How can I call it on a delay for now?
            // Maybe a custom timer for each char?



            // Keith, figure out what type of attack this character is doing (projectile, magic, melee...)
            // for melee this can be called when the animation is done.
            // For range it should be called after the last projectile has hit it's target
            onComplete();
        }
        else
        {
            print("Attack ani done but onComplete action is null");
        }

    }

    public void SetAnimsBareHands()
    {
        print("SetAnimsBareHands");

        //animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dBareHands_Idle"), UnitAnimType.GetUnitAnimType("dBareHands_Walk"), 1f, 1f);
        //attackUnitAnim = UnitAnimType.GetUnitAnimType("dBareHands_PunchQuickAttack");
    }

    public void SetAnimsSwordTwoHandedBack()
    {
        print("SetAnimsSwordTwoHandedBack");

        //animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Idle"), UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Walk"), 1f, 1f);
        //attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordTwoHandedBack_Sword");
    }

    public void SetAnimsSwordShield()
    {
        print("SetAnimsSwordShield");

        //animatedWalker.SetAnimations(UnitAnimType.GetUnitAnimType("dSwordShield_Idle"), UnitAnimType.GetUnitAnimType("dSwordShield_Walk"), 1f, 1f);
        //attackUnitAnim = UnitAnimType.GetUnitAnimType("dSwordShield_Attack");
    }

    //public Vector3 GetHandLPosition() {
    //    return unitSkeleton.GetBodyPartPosition("HandL");
    //}

    //public Vector3 GetHandRPosition()
    //{
    //    return unitSkeleton.GetBodyPartPosition("HandR");
    //}
}
