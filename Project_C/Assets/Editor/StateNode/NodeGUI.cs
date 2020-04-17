using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace StateBehavior.Node
{
    [Serializable]
    public enum NodeType
    {
        Logic, // 로직 커넥션이 있어야함
        Func, // 리턴함수,
        Variable,
        Condition,
        For,
        Event,
        TimeLine
    }

    [Serializable]
    public class NodeGUI
    {
        public NodeData data;

        [NonSerialized] public bool isDragged;
        [NonSerialized] public bool isSelected;
        [NonSerialized] public List<ConnectionPoint> points = new List<ConnectionPoint>();

        public static NodeGUI CreateNodeInRuntime(NodeData data)
        {
            NodeGUI node = new NodeGUI(data);
            node.MakePoints();
            node.UpdateResize();
            return node;
        }

        public static NodeGUI CreateNodeInLoad(NodeData data)
        {
            NodeGUI node = new NodeGUI(data);
            return node;
        }

        public void AttachPoint(NodePointData data)
        {
            points.Add(new ConnectionPoint(data));
            UpdateResize();
        }

        public void AddPoint(NodePointData point)
        {
            points.Add(new ConnectionPoint(point));
            data.points.Add(point.GUID);
        }

        public void RemovePoints()
        {
            foreach(var p in points)
            {
                NodeGUIUtility.RemoveInstance(p.data);
            }
            points.Clear();
        }

        public NodeGUI(NodeData data)
        {
            NodeGUIUtility.AddInstance(data);
            this.data = data;
            data.rect.size = new Vector2(180f, 4000f);
        }

        public void Drag(Vector2 delta)
        {
            data.rect.position += delta;
        }

        public void Draw()
        {
            GUILayout.BeginArea(data.rect);
            if (isSelected)
            {
                GUILayout.BeginVertical(NodeGUIResources.styles.selectedNodeStyle);
            }
            else
            {
                GUILayout.BeginVertical(NodeGUIResources.styles.nodeStyle);
            }

            GUILayout.Label(data.methodName, NodeGUIResources.styles.nodeHeader, GUILayout.Height(20));
            GUILayout.Label(data.returnType, NodeGUIResources.styles.nodeHeader, GUILayout.Height(20));

            List<ConnectionPoint> inPoints = points.FindAll((p) => (int)p.data.pointType < 2);

            foreach (var point in inPoints)
            {
                point.data.cachedValue = NodeGUIUtility.Draw(point.data);
            }

            GUILayout.Label("", NodeGUIResources.styles.nodeHeader, GUILayout.Height(20));

            GUILayout.EndVertical();
            GUILayout.EndArea();

            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i].Draw();
                }
            }
        }

        private void UpdateResize()
        {
            int inCount = points.FindAll((p) => (int)p.data.pointType < 2).Count;
            int maxIndex = 0;
            for(int i  = 0; i < points.Count; ++i)
            {
                maxIndex = points[i].data.pointType == ConnectionPointType.Parameter && maxIndex < points[i].data.index ? points[i].data.index : maxIndex;
            }
            data.rect.height = Mathf.Max(60f, maxIndex * 40f + 60f);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (data.rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                        }
                    }

                    if (e.button == 1 && isSelected && data.rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            NodeBaseEditor.Current.OnClickRemoveNode(this);
        }

        public void MakePoints()
        {
            if (this.data is NodeFuncData)
            {
                NodeFuncData data = this.data as NodeFuncData;

                MethodInfo method = Type.GetType(data.funcClassType).GetMethod(data.methodName);
                ParameterInfo[] parameters = method.GetParameters();

                int index = 1;

                if (method.ReturnParameter.ParameterType == typeof(void))
                {
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.In, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                    data.nodeType = NodeType.Logic;
                }
                else
                {
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Return, method.ReturnParameter.ParameterType.FullName, method.ReturnParameter.Name));
                    data.nodeType = NodeType.Func;
                }

                if(!method.IsStatic)
                {
                    AddPoint(new NodePointData(index++, data.GUID, ConnectionPointType.Parameter, data.funcClassType, "Owner"));
                }

                foreach (var parameter in parameters)
                {
                    AddPoint(new NodePointData(index++, data.GUID, ConnectionPointType.Parameter, parameter.ParameterType.FullName, parameter.Name));
                }
            }
            else
            {
                if (data.nodeType == NodeType.Condition)
                {
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.In, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Parameter, typeof(bool).FullName, "Condition"));
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                }
                else if (data.nodeType == NodeType.For)
                {
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.In, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Parameter, typeof(int).FullName, "LoofCount"));
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                }
                else if (data.nodeType == NodeType.Event)
                {
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Return, typeof(Character).FullName, "Owner", null));
                }
                else if (data.nodeType == NodeType.TimeLine)
                {
                    AddPoint(new NodePointData(0, data.GUID, ConnectionPointType.Out, typeof(void).FullName, "Logic", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Return, typeof(Character).FullName, "Owner", null));
                    AddPoint(new NodePointData(1, data.GUID, ConnectionPointType.Parameter, typeof(float).FullName, "Time"));
                }
            }
        }
    }



}
