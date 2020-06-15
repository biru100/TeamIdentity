using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using System.Reflection;
using System.Linq;

public static class NodeUtil
{
    public static void MoveDelta(Character owner, Vector3 targetPosition)
    {
        owner.NavAgent.Move(targetPosition);
    }

    public static void MoveToPlayer(Character owner)
    {
        owner.NavAgent.destination = Player.CurrentPlayer.transform.position;
    }
    public static void MoveToPpsition(Character owner, Vector3 position)
    {
        owner.NavAgent.destination = position;
    }

    public static float Random(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static bool IsDestination(Character owner)
    {
        return owner.NavAgent.isStopped;
    }

    public static bool IsActivateAbility(Character owner, int wantToAbilityIndex)
    {
        return EntityUtil.IsActivateAbility(owner, (CharacterAbilityType)wantToAbilityIndex);
    }

    public static void StopMovement(Character owner)
    {
        owner.NavAgent.isStopped = true;
        owner.NavAgent.ResetPath();
    }

    public static void DestroyEntity(Character target)
    {
        UnityEngine.Object.Destroy(target.gameObject);
    }

    public static bool IsGoalDestination(Character owner)
    {
        return owner.NavAgent.isStopped;
    }

    public static Vector3 GetNavMoveDirection(Character owner)
    {
        return owner.NavAgent.velocity;
    }

    public static void AvoidFormPlayer(Character owner)
    {
        Quaternion avoidDir = Quaternion.LookRotation(Player.CurrentPlayer.transform.position - owner.transform.position);
        NavMeshHit hit;

        float distance = (Player.CurrentPlayer.transform.position - owner.transform.position).magnitude;

        if (distance > 3f)
            return;

        Vector3 currentTarget = owner.transform.position + avoidDir * -Vector3.forward * (3f - distance) * Isometric.IsometricTileSize.x;

        if (NavMesh.Raycast(owner.transform.position, 
            currentTarget, 
            out hit, 0))
        {
            currentTarget = owner.transform.position + avoidDir * Quaternion.Euler(0f, 60f, 0f) * -Vector3.forward * (3f - distance) * Isometric.IsometricTileSize.x;
            if (NavMesh.Raycast(owner.transform.position,
            currentTarget,
            out hit, 0))
            {
                currentTarget = owner.transform.position + avoidDir * Quaternion.Euler(0f, 120f, 0f) * -Vector3.forward * (3f - distance) * Isometric.IsometricTileSize.x;
                if (NavMesh.Raycast(owner.transform.position,
                    currentTarget,
                    out hit, 0))
                {
                    owner.NavAgent.SetDestination(hit.position);
                }
                else
                {
                    owner.NavAgent.SetDestination(currentTarget);
                }
            }
            else
            {
                owner.NavAgent.SetDestination(currentTarget);
            }
        }
        else
        {
            owner.NavAgent.SetDestination(currentTarget);
        }    
    }

    public static void ShootProjectile(Character owner, string projectileName, Vector3 direction, float speed)
    {
        IsoProjectile projectile = IsoProjectile.CreateProjectile(projectileName, owner.transform.position + owner.transform.forward * Isometric.IsometricGridSize,
            direction, speed, 2f);
        projectile.Damage = owner.Status.CurrentDamage;
    }

    public static void LookPlayer(Character owner)
    {
        owner.transform.rotation = Quaternion.LookRotation((Player.CurrentPlayer.transform.position - owner.transform.position).normalized
            , Vector3.up);
    }

    public static Character CreateEntity(string name, Vector3 position)
    {
        Character character = UnityEngine.Object.Instantiate(ResourceManager.GetResource<GameObject>("Tiles/" + name), position,
            Quaternion.Euler(0f, 135f, 0f)).GetComponent<Character>();
        RoomManager.Instance.CurrentRoom.RoomAllEntitys.Add(character);
        return character;
    }

    public static void TakeDamageToPlayer(float damage)
    {
        Player.CurrentPlayer.AddState(new CharacterHitState(Player.CurrentPlayer, damage, 0.1f).Init());
    }

    public static void TakeDamage(Character target, float damage)
    {
        target.AddState(new CharacterHitState(target, damage, 0.1f).Init());
    }

    public static void TakeDamageBoth(List<Character> target, float damage)
    {
        foreach (var c in target)
        {
            c.AddState(new CharacterHitState(c, damage, 0.1f).Init());
        }
    }

    public static float GetDamage(Character owner)
    {
        return owner.Status.CurrentDamage;
    }

    public static float GetMosterParameter(Character owner, int index)
    {
        return (owner as Monster).Data._Parameter[index];
    }

    public static List<Character> GetCharactersInRange(Character owner, bool containPlayer, bool ignoreMine, float length)
    {
        Character[] enemys = GameObject.FindObjectsOfType<Character>();
        return enemys.Where((c) =>
        {
            if (!containPlayer && c is Player)
                return false;
            if (ignoreMine && c == owner)
                return false;

            if((owner.transform.position - c.transform.position).magnitude < length * Isometric.IsometricGridSize)
                return true;

            return false;
        }).ToList();
    }

    public static void AddCardToHand(int cardIndex)
    {
        CardInterface ci = CardInterface.CreateCard(InGameInterface.Instance.transform);
        ci.CardData = new Card(cardIndex);
        ci.CurrentAction = HandCardAction.GetInstance();
        InGameInterface.Instance.AddToHand(ci);
    }

    public static void DrawCard()
    {
        if (!PlayerStatus.CurrentStatus.CurrentStates.Contains(CharacterStateType.E_Invincibility))
            InGameInterface.Instance.DrawCard(1);
    }

    public static void BurnCard()
    {
        if (!PlayerStatus.CurrentStatus.CurrentStates.Contains(CharacterStateType.E_Invincibility))
            InGameInterface.Instance.DestroyCard();
    }

    public static void GiveStun(Character target, float time)
    {
        target.AddState(new CharacterState(CharacterStateType.E_Stun, target, time).Init());
    }

    public static void GiveStunBoth(List<Character> target, float time)
    {
        foreach (var c in target)
        {
            c.AddState(new CharacterState(CharacterStateType.E_Stun, c, time).Init());
        }
    }

    public static void GiveHold(Character target, float time)
    {
        target.AddState(new CharacterState(CharacterStateType.E_Hold, target, time).Init());
    }

    public static void GiveHoldBoth(List<Character> target, float time)
    {
        foreach (var c in target)
        {
            c.AddState(new CharacterState(CharacterStateType.E_Hold, c, time).Init());
        }
    }

    public static void GiveSilence(Character target, float time)
    {
        target.AddState(new CharacterSilenceState(target, time).Init());
    }

    public static void GiveSilenceBoth(List<Character> target, float time)
    {
        foreach (var c in target)
        {
            c.AddState(new CharacterSilenceState(c, time).Init());
        }
    }

    public static bool PlayerInRange(Character owner, float range)
    {
        Player player = Player.CurrentPlayer;
        return (owner.transform.position - player.transform.position).magnitude <= Isometric.IsometricTileSize.x * range;
    }

    public static bool PlayerInSight(Character owner, float range, float angle)
    {
        Player player = Player.CurrentPlayer;
        return (owner.transform.position - player.transform.position).magnitude <= Isometric.IsometricTileSize.x * range &&
                Mathf.Acos(Vector3.Dot((player.transform.position - owner.transform.position).normalized, owner.transform.forward)) < Mathf.Deg2Rad * angle;
    }

    public static bool SweepInRange(Vector3 pos1, Vector3 pos2, float Range, Vector3 target)
    {
        Vector3 v1 = pos2 - pos1;
        Vector3 right = Quaternion.FromToRotation(Vector3.forward, v1.normalized) * Vector3.right;

        return (target - pos1).magnitude < v1.magnitude && Vector3.Cross(v1.normalized, (target - pos1 + right * Isometric.IsometricGridSize * Range).normalized).y > 0f &&
            Vector3.Cross((target - pos1 - right * Isometric.IsometricGridSize * Range).normalized, v1.normalized).y > 0f;

    }

    public static Vector3 CreateVector3(float x, float y, float z)
    {
        return new Vector3(x, y, z);
    }

    public static Vector3 GetPosition(Character owner)
    {
        return owner.transform.position;
    }

    public static bool GetAttackInput()
    {
        return PlayerUtil.GetAttackInput();
    }

    public static bool GetDashInput()
    {
        return PlayerUtil.GetDashInput();
    }

    public static Vector3 GetVelocityInput()
    {
        return PlayerUtil.GetVelocityInput();
    }

    public static void PlayAnim(Character owner, string animationName)
    {
        AnimUtil.PlayAnim(owner, animationName);
    }

    public static void PlayAnimOneSide(Character owner, string animationName)
    {
        AnimUtil.PlayAnimOneSide(owner, animationName);
    }

    public static void RotationAnim(Character owner, string animationName)
    {
        AnimUtil.RotationAnim(owner, animationName);
    }

    public static bool IsLastFrame(Character owner)
    {
        return AnimUtil.IsLastFrame(owner);
    }

    public static void ChangeAction(Character owner, string actionName)
    {
        EntityUtil.ChangeAction(owner, actionName);
    }

    public static bool StateActionMacro(Character owner)
    {
        return EntityUtil.StateActionMacro(owner);
    }

    public static bool StateActionMacroByCurrentOrder(Character owner, int order)
    {
        return EntityUtil.StateActionMacro(owner, CharacterState.CharacterStateActionOrder[order]);
    }

    public static bool StateFinishActionMacro(Character owner, int order)
    {
        return EntityUtil.StateFinishActionMacro(owner, CharacterState.CharacterStateActionOrder[order]);
    }

    public static bool IsStun(Character owner)
    {
        return EntityUtil.IsStun(owner);
    }

    public static bool IsHold(Character owner)
    {
        return EntityUtil.IsHold(owner);
    }

    public static bool IsSilence(Character owner)
    {
        return owner.Status.CurrentStates.Contains(CharacterStateType.E_Silence);
    }

    public static bool HitDeadLogicMacro(Character owner, string hitActionName, string deadActionName)
    {
        return false;
    }

    public static bool DeadLogicMacro(Character owner, string deadActionName)
    {
        return false;
    }

    public static void RotateToVelocity(Character owner, Vector3 velocity)
    {
        owner.transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
    }

    public static float GetDeltaTime()
    {
        return Time.deltaTime;
    }

    public static float FloatAdd(float a, float b)
    {
        return a + b;
    }

    public static float FloatMinus(float a, float b)
    {
        return a - b;
    }

    public static float FloatMultiple(float a, float b)
    {
        return a * b;
    }

    public static float FloatDivide(float a, float b)
    {
        return a / b;
    }

    public static Vector3 VectorAdd(Vector3 a, Vector3 b)
    {
        return a + b;
    }

    public static Vector3 VectorMinus(Vector3 a, Vector3 b)
    {
        return a - b;
    }

    public static Vector3 VectorMultiple(Vector3 a, float b)
    {
        return a * b;
    }

    public static Vector3 VectorDivide(Vector3 a, float b)
    {
        return a / b;
    }
}

