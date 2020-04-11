using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

namespace StateBehavior.Node
{

    public static class NodeGUIUtility
    {
        public static Type GetType(string typeName)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().First((a) => a.GetType(typeName) != null);
            return assembly.GetType(typeName);
        }

        public static T GetInstance<T>(string guid) where T : NodeDataBase
        {
            return NodeBaseEditor.Current.elements.GetInstanceByGUID<T>(guid);
        }

        public static void AddInstance(NodeDataBase data)
        {
            if(!NodeBaseEditor.Current.elements.NodeElementsData.ContainsKey(data.GUID))
                NodeBaseEditor.Current.elements.NodeElementsData.Add(data.GUID, data);
        }

        public static void RemoveInstance(NodeDataBase data)
        {
            NodeBaseEditor.Current.elements.NodeElementsData.Remove(data.GUID);
        }


        public static object GetDefaultValue(Type type)
        {
            //if(type == null)
            //{
            //    int a = 0;
            //    return null;
            //}

            if (type == typeof(string))
                return "";
            else if (type.IsClass)
                return null;
            else if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        public static void IfFunc(bool isTrue)
        {

        }

        public static string Draw(NodePointData data)
        {
            if (typeof(void).FullName == data.parameterType)
            {
                return "";
            }

            if (data.connections.Count != 0)
            {
                DrawParameter(data);
                return null;
            }

            if (typeof(bool).FullName == data.parameterType)
            {
                return DrawBool(data);
            }
            else if (typeof(string).FullName == data.parameterType)
            {
                return DrawString(data);
            }
            else if (typeof(float).FullName == data.parameterType)
            {
                return DrawFloat(data);
            }
            else if (typeof(int).FullName == data.parameterType)
            {
                return DrawInt(data);
            }
            else
            {
                DrawParameter(data);
                return null;
            }
        }

        public static void DrawParameter(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.inputPort, GUILayout.Height(20));
            GUILayout.Label("", NodeGUIResources.styles.inputPort, GUILayout.Height(20));
        }

        public static string DrawString(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.inputPort, GUILayout.Height(20));
            return EditorGUILayout.TextField(data.cachedValue, GUILayout.Width(100), GUILayout.Height(17));
        }

        public static string DrawInt(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.inputPort, GUILayout.Height(20));
            return EditorGUILayout.IntField(int.Parse(data.cachedValue), GUILayout.Width(100), GUILayout.Height(17)).ToString();
        }

        public static string DrawFloat(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.inputPort, GUILayout.Height(20));
            return EditorGUILayout.FloatField(float.Parse(data.cachedValue), GUILayout.Width(100), GUILayout.Height(17)).ToString();
        }


        public static string DrawBool(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.inputPort, GUILayout.Height(20));
            return EditorGUILayout.Toggle(bool.Parse(data.cachedValue), GUILayout.Width(20), GUILayout.Height(20)).ToString();
        }
    }
}

public static class NodeUtil
{

    public static void Move(Character owner, Vector3 targetPosition)
    {
        owner.NavAgent.Move(targetPosition);
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

    public static bool HitDeadLogicMacro(Character owner, string hitActionName, string deadActionName)
    {
        return EntityUtil.HitDeadLogicMacro(owner, hitActionName, deadActionName);
    }

    public static bool DeadLogicMacro(Character owner, string deadActionName)
    {
        return EntityUtil.DeadLogicMacro(owner, deadActionName);
    }

    public static bool GetDamageNotify(Character owner)
    {
        return EntityUtil.GetDamageNotify(owner);
    }

    public static bool GetDeadNotify(Character owner)
    {
        return EntityUtil.GetDeadNotify(owner);
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
