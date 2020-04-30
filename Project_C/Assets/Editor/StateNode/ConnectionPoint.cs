using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace StateBehavior.Node
{
    [Serializable]
    public enum ConnectionPointType { In, Parameter, Out, Return }

    [Serializable]
    public class ConnectionPoint
    {
        public NodePointData data;

        public ConnectionPoint(NodePointData data)
        {
            NodeGUIUtility.AddInstance(data);
            this.data = data;
        }

        public void Draw()
        {
            NodeData node = NodeGUIUtility.GetInstance<NodeData>(data.nodeGUID);

            data.rect.y = node.rect.y + 15f - data.rect.height * 0.5f + 40f * data.index;

            switch (data.pointType)
            {
                case ConnectionPointType.In:
                    data.rect.x = node.rect.x - data.rect.width + 8f;
                    if (GUI.Button(data.rect, "",NodeGUIResources.styles.flowInPort))
                    {
                        NodeBaseEditor.Current.OnClickPoint(this);
                    }
                    break;
                case ConnectionPointType.Parameter:
                    data.rect.x = node.rect.x - data.rect.width + 8f;
                    if (GUI.Button(data.rect, "", NodeGUIResources.styles.parameterPort))
                    {
                        NodeBaseEditor.Current.OnClickPoint(this);
                    }
                    break;

                case ConnectionPointType.Out:
                    data.rect.x = node.rect.x + node.rect.width - 8f;
                    if (GUI.Button(data.rect, "", NodeGUIResources.styles.flowOutPort))
                    {
                        NodeBaseEditor.Current.OnClickPoint(this);
                    }
                    break;
                case ConnectionPointType.Return:
                    data.rect.x = node.rect.x + node.rect.width - 8f;
                    if (GUI.Button(data.rect, "", NodeGUIResources.styles.returnPort))
                    {
                        NodeBaseEditor.Current.OnClickPoint(this);
                    }
                    break;
            }
        }
    }
}
