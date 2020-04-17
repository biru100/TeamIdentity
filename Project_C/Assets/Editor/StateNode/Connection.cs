using System;
using UnityEditor;
using UnityEngine;

namespace StateBehavior.Node
{
    [Serializable]
    public class Connection
    {
        public NodeConnectionData data;

        public static Connection CreateConnectionInRuntime(NodeConnectionData data)
        {
            Connection c = new Connection(data);
            NodePointData inP = NodeGUIUtility.GetInstance<NodePointData>(data.inGUID)
                , outP = NodeGUIUtility.GetInstance<NodePointData>(data.outGUID);
            inP.connections.Add(data.GUID);
            outP.connections.Add(data.GUID);
            return c;
        }

        public static Connection CreateConnectionInLoad(NodeConnectionData data)
        {
            return new Connection(data);
        }

        public Connection(NodeConnectionData data)
        {
            NodeGUIUtility.AddInstance(data);
            this.data = data;
        }

        public void Draw()
        {
            NodePointData inP = NodeGUIUtility.GetInstance<NodePointData>(data.inGUID), outP = NodeGUIUtility.GetInstance<NodePointData>(data.outGUID);

            Handles.DrawBezier(
                inP.rect.center,
                outP.rect.center,
                inP.rect.center + Vector2.left * 50f,
                outP.rect.center - Vector2.left * 50f,
                Color.white,
                null,
                4f
            );

            if (Handles.Button((inP.rect.center + outP.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
            {
                NodeBaseEditor.Current.OnClickRemoveConnection(this);
            }
        }
    }
}
