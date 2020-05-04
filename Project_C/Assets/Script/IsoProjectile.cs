using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ProjectileHitObjectType
{
    E_All,
    E_Player,
    E_Monster
}

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class IsoProjectile : MonoBehaviour
{

    public static IsoProjectile CreateProjectile(string projectileName, Vector3 position, Vector3 direction, float speed, float lifeTime = -1f)
    {
        GameObject projectile = ResourceManager.GetResource<GameObject>("Projectiles/" + projectileName);

        if (projectile == null || projectile.GetComponent<IsoParticle>() == null)
        {
            return null;
        }

        IsoProjectile iprojectile = Instantiate(projectile, position, Quaternion.identity).GetComponent<IsoProjectile>();
        iprojectile.Direction = direction;
        iprojectile.Speed = speed;
        iprojectile.LifeTime = lifeTime;
        return iprojectile;
    }

    public Vector3 Direction { get; set; }
    public RenderTransform RenderChild { get; set; }
    public Animator Anim { get; set; }
    public Rigidbody Body { get; set; }
    public float LifeTime { get; set; }
    public float Speed { get; set; }
    public float Damage { get; set; }
    public ProjectileHitObjectType HitObjectType { get; set; }
    public Action<Vector3, GameObject> HitAction { get; set; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        RenderChild = GetComponentInChildren<RenderTransform>();
        Anim = GetComponentInChildren<Animator>();
        Body = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        Vector3 rotatedDir = Isometric.IsometricToWorldRotation * Direction;
        rotatedDir.z = 0f;
        rotatedDir.Normalize();

        float angle = Quaternion.FromToRotation(Vector3.right, rotatedDir).eulerAngles.z;
        RenderChild.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        Body.MovePosition(transform.position + Direction * Speed * Isometric.IsometricGridSize * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        Monster monster = collision.gameObject.GetComponent<Monster>();

        if (Player.CurrentPlayer != null && collision.gameObject == Player.CurrentPlayer.gameObject)
        {
            if (HitObjectType != ProjectileHitObjectType.E_Monster)
            {
                Player.CurrentPlayer.AddState(new CharacterHitState(Player.CurrentPlayer, Damage, 0.1f).Init());
                if(Damage > 0f) HitAction?.Invoke(collision.contacts[0].point, collision.gameObject);
                Destroy(gameObject);
                return;
            }
        }
        else if (monster != null)
        {
            if (HitObjectType != ProjectileHitObjectType.E_Player)
            {
                monster.AddState(new CharacterHitState(monster, Damage, 0.1f).Init());
                if (Damage > 0f) HitAction?.Invoke(collision.contacts[0].point, collision.gameObject);
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            if (HitObjectType == ProjectileHitObjectType.E_All)
            {
                HitAction?.Invoke(collision.contacts[0].point, collision.gameObject);
            }

            Destroy(gameObject);
            return;
        }
    }
}
