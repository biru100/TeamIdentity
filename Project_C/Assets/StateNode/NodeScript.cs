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
