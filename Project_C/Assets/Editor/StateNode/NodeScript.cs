using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.IO;
using System.Reflection;


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
            if (!NodeBaseEditor.Current.elements.NodeElementsData.ContainsKey(data.GUID))
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
                return data.cachedValue;
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
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.nodeElement, GUILayout.Height(20));
            GUILayout.Label("", GUILayout.Height(20));
        }

        public static string DrawString(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.nodeElement, GUILayout.Height(20));
            return EditorGUILayout.TextField(data.cachedValue, NodeGUIResources.styles.inputField, GUILayout.Width(70), GUILayout.Height(17));
        }

        public static string DrawInt(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.nodeElement, GUILayout.Height(20));
            return EditorGUILayout.IntField(int.Parse(data.cachedValue), NodeGUIResources.styles.inputField, GUILayout.Width(70), GUILayout.Height(17)).ToString();
        }

        public static string DrawFloat(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.nodeElement, GUILayout.Height(20));
            return EditorGUILayout.FloatField(float.Parse(data.cachedValue), NodeGUIResources.styles.inputField, GUILayout.Width(70), GUILayout.Height(17)).ToString();
        }


        public static string DrawBool(NodePointData data)
        {
            GUILayout.Label(data.parameterName, NodeGUIResources.styles.nodeElement, GUILayout.Height(20));
            if (data.cachedValue == null)
                data.cachedValue = "False";
            return EditorGUILayout.Toggle(bool.Parse(data.cachedValue), GUILayout.Width(20), GUILayout.Height(20)).ToString();
        }
    }
[Serializable]
    public class NodeSerializableData
    {
        public List<NodeData> nodeDatas;
        public List<NodeFuncData> nodeFuncDatas;
        public List<NodePointData> nodePointDatas;
        public List<NodeConnectionData> nodeConnectionDatas;

        public NodeSerializableData()
        {
            nodeDatas = new List<NodeData>();
            nodeFuncDatas = new List<NodeFuncData>();
            nodePointDatas = new List<NodePointData>();
            nodeConnectionDatas = new List<NodeConnectionData>();
        }
    }

    [Serializable, CreateAssetMenu(fileName = "New Node Script", menuName = "Node/Node Script")]
    public class NodeScript : ScriptableObject
    {
        public string userName;
        public string stateName;

        public string dataJson;

        public NodeSerializableData serializableNodeData;

        public void Save(List<NodeDataBase> datas)
        {
            serializableNodeData = new NodeSerializableData();

            foreach(var e in datas)
            {
                if (e is NodeFuncData)
                    serializableNodeData.nodeFuncDatas.Add(e as NodeFuncData);
                else if(e is NodeData)
                    serializableNodeData.nodeDatas.Add(e as NodeData);
                else if (e is NodePointData)
                    serializableNodeData.nodePointDatas.Add(e as NodePointData);
                else if (e is NodeConnectionData)
                    serializableNodeData.nodeConnectionDatas.Add(e as NodeConnectionData);
            }

            dataJson = JsonUtility.ToJson(serializableNodeData);
        }

        public NodeSerializableData Load()
        {
            //if (dataJson.Count == 0)
            //    return new NodeSerializableData();

            serializableNodeData = JsonUtility.FromJson<NodeSerializableData>(dataJson);
            return serializableNodeData;
        }

        public void OnDestroy()
        {
            EditorUtility.SetDirty(this);
        }

        void OnInspectorDraw()
        {

        }
    }
}
