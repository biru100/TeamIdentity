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

    public static void StopMovement(Character owner)
    {
        owner.NavAgent.isStopped = true;
        owner.NavAgent.ResetPath();
    }

    public static void DestroyEntity(Character target)
    {
        UnityEngine.Object.Destroy(target.gameObject);
    }

    public static void AvoidFormPlayer(Character owner)
    {
        //Quaternion avoidDir = Quaternion.LookRotation(Player.CurrentPlayer.transform.position - owner.transform.position);
        //NavMeshHit hit;
        //if(NavMesh.Raycast(owner.transform.position, 
        //    owner.transform.position + avoidDir * -Vector3.forward * 3f * Isometric.IsometricTileSize.x, 
        //    out hit, 0))
        //{
        //    if(NavMesh.Raycast(owner.transform.position,
        //    owner.transform.position + avoidDir * Quaternion.Euler(0f, 60f, 0f) * -Vector3.forward * 3f * Isometric.IsometricTileSize.x,
        //    out hit, 0))
        //    {
        //        if (NavMesh.Raycast(owner.transform.position,
        //            owner.transform.position + avoidDir * Quaternion.Euler(0f, 60f, 0f) * -Vector3.forward * 3f * Isometric.IsometricTileSize.x,
        //            out hit, 0))
        //        {

        //        }
        //    }
        //}
        //else
        //{
        //    NavMesh.SamplePosition(
        //}    
    }

    public static void LookPlayer(Character owner)
    {
        owner.transform.rotation = Quaternion.LookRotation((Player.CurrentPlayer.transform.position - owner.transform.position).normalized
            , Vector3.up);
    }

    public static Character CreateEntity(string name, Vector3 position)
    {
        return UnityEngine.Object.Instantiate(ResourceManager.GetResource<GameObject>("Tiles/" + name), position,
            Quaternion.Euler(0f, 135f, 0f)).GetComponent<Character>();
    }

    public static void TakeDamageToPlayer(float damage)
    {
        Player.CurrentPlayer.AddState(new CharacterHitState(Player.CurrentPlayer, damage, 0.1f).Init());
    }

    public static void TakeDamage(Character target, float damage)
    {
        target.AddState(new CharacterHitState(target, damage, 0.1f).Init());
    }

    public static float GetDamage(Character owner)
    {
        return owner.Status.CurrentDamage;
    }

    public static void DrawCard()
    {
        InGameInterface.Instance.DrawCard();
    }

    public static void BurnCard()
    {
        InGameInterface.Instance.DestroyCard();
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

