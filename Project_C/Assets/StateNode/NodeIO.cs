using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace StateBehavior.Node
{
    [Serializable]
    public class NodeDataBase
    {
        public string GUID;

        public NodeDataBase()
        {
            GUID = Guid.NewGuid().ToString();
        }
    }

    [Serializable]
    public class NodeData : NodeDataBase
    {
        public Rect rect;
        public string methodName;
        public string returnType;
        public NodeType nodeType;

        public List<string> points = new List<string>();

        public NodeData(Vector2 position, string methodName, string returnType, NodeType nodeType) : base()
        {
            this.rect.position = position;
            this.methodName = methodName;
            this.returnType = returnType;
            this.nodeType = nodeType;
        }

        public virtual void CopyData(NodeData other)
        {
            GUID = other.GUID;
            methodName = other.methodName;
            returnType = other.returnType;
            nodeType = other.nodeType;
            points = other.points;
        }
    }

    [Serializable]
    public class NodeFuncData : NodeData
    {
        public string funcClassType;

        public NodeFuncData(string funcClassType, Vector2 position, string methodName, string returnType, NodeType nodeType) 
            : base(position, methodName, returnType, nodeType)
        {
            this.funcClassType = funcClassType;
            this.methodName = methodName;
            this.returnType = returnType;
            this.nodeType = nodeType;
        }

        public override void CopyData(NodeData other)
        {
            GUID = other.GUID;
            methodName = other.methodName;
            returnType = other.returnType;
            nodeType = other.nodeType;
            points = other.points;

            funcClassType = (other as NodeFuncData).funcClassType;
        }
    }

    [Serializable]
    public class NodePointData : NodeDataBase
    {
        public Rect rect = new Rect(0, 0, 15f, 15f);
        public int index;
        public string nodeGUID;
        public ConnectionPointType pointType;
        public string parameterType;
        public string parameterName;

        public string cachedValue;

        public List<string> connections = new List<string>();

        public NodePointData(int index, string nodeGUID, ConnectionPointType pointType, string parameterType, string parameterName)
            : base()
        {
            this.index = index;
            this.nodeGUID = nodeGUID;
            this.pointType = pointType;
            this.parameterType = parameterType;
            this.parameterName = parameterName;
            //unityengine.vector3 어셈블리 이름 타입에 적어줘야함
            this.cachedValue = NodeGUIUtility.GetDefaultValue(NodeGUIUtility.GetType(parameterType))?.ToString() ?? "null";
        }

        public NodePointData(int index, string nodeGUID, ConnectionPointType pointType, string parameterType, string parameterName,
            object cachedValue) 
            : base()
        {
            this.index = index;
            this.nodeGUID = nodeGUID;
            this.pointType = pointType;
            this.parameterType = parameterType;
            this.parameterName = parameterName;
            this.cachedValue = cachedValue == null ? "null" : cachedValue.ToString();
        }

        public void CopyData(NodePointData other)
        {
            GUID = other.GUID;
            index = other.index;
            nodeGUID = other.nodeGUID;
            pointType = other.pointType;
            parameterType = other.parameterType;
            parameterName = other.parameterName;
            connections = other.connections;
        }
    }

    [Serializable]
    public class NodeConnectionData : NodeDataBase
    {
        public string inGUID, outGUID;

        public NodeConnectionData(string inGUID, string outGUID)
            : base()
        {
            this.inGUID = inGUID;
            this.outGUID = outGUID;
        }

        public void CopyData(NodeConnectionData other)
        {
            GUID = other.GUID;
            inGUID = other.inGUID;
            outGUID = other.outGUID;
        }
    }
}
