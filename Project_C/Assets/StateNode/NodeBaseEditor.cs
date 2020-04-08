using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Reflection;
using System;
using System.Linq;

namespace StateBehavior.Node
{
    public class NodeBaseEditor : EditorWindow
    {
        public class NodeEditorData
        {
            public Dictionary<string, NodeDataBase> NodeElementsData { get; set; }

            public NodeEditorData()
            {
                NodeElementsData = new Dictionary<string, NodeDataBase>();
            }

            public T GetInstanceByGUID<T>(string guid) where T : NodeDataBase
            {
                return NodeElementsData[guid] as T;
            }
        }

        public static NodeBaseEditor Current;

        public NodeEditorData elements = new NodeEditorData();

        private List<NodeGUI> nodes = new List<NodeGUI>();
        private List<Connection> connections = new List<Connection>();

        private NodeScript owner;

        private List<ConnectionPoint> selectedPoint = new List<ConnectionPoint>();

        private Vector2 offset;
        private Vector2 drag;

        [MenuItem("Window/Node Base Editor")]
        private static void OpenWindow()
        {
            NodeBaseEditor window = GetWindow<NodeBaseEditor>();
            window.titleContent = new GUIContent("Node Base Editor");
            Current = window;
        }

        private void OnInspectorGUI()
        {
            
        }

        private void OnGUI()
        {
            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);
            DrawSaveButton();

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawSaveButton()
        {
            if(GUI.Button(new Rect(10, 10, 80, 30), "Save"))
            {
                if(owner != null)
                {
                    owner.Save(elements.NodeElementsData.Values.ToList());
                }
            }
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedPoint.Count == 1 && (int)selectedPoint[0].data.pointType < 2)
            {
                Handles.DrawBezier(
                    selectedPoint[0].data.rect.center,
                    e.mousePosition,
                    selectedPoint[0].data.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedPoint.Count == 1 && (int)selectedPoint[0].data.pointType > 1)
            {
                Handles.DrawBezier(
                    selectedPoint[0].data.rect.center,
                    e.mousePosition,
                    selectedPoint[0].data.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        private void DrawNodes()
        {
            if(nodes != null)
            {
                foreach(var node in nodes)
                {
                    node.Draw();
                }
            }
        }

        private void ProcessNodeEvents(Event e)
        {

            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }
        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();

            genericMenu.AddItem(new GUIContent("Start"), false, () => OnClickAddNode(mousePosition, NodeType.Event, "Start"));
            genericMenu.AddItem(new GUIContent("Update"), false, () => OnClickAddNode(mousePosition, NodeType.Event, "Update"));
            genericMenu.AddItem(new GUIContent("IF"), false, () => OnClickAddNode(mousePosition, NodeType.Condition, "IF"));
            genericMenu.AddItem(new GUIContent("For"), false, () => OnClickAddNode(mousePosition, NodeType.For, "For"));

            MethodInfo[] methods = typeof(NodeUtil).GetMethods();
            foreach(var method in methods)
            {
                genericMenu.AddItem(new GUIContent(method.Name), false, () => OnClickAddNode(mousePosition, typeof(NodeUtil), method));
            }
            genericMenu.ShowAsContext();
        }

        private void OnClickAddNode(Vector2 mousePosition, Type cls, MethodInfo method)
        {
            if (nodes == null)
            {
                nodes = new List<NodeGUI>();
            }

            CreateNodeInRuntime(new NodeFuncData(cls.FullName, mousePosition, method.Name, method.ReturnType.Name, NodeType.Func));
        }

        private void OnClickAddNode(Vector2 mousePosition, NodeType nodeType, string nodeName)
        {
            if (nodes == null)
            {
                nodes = new List<NodeGUI>();
            }

            if (nodeType == NodeType.Event)
            {
                CreateNodeInRuntime(new NodeData(mousePosition, nodeName, "Character", nodeType));
            }
            else if(nodeType == NodeType.Condition)
            {
                CreateNodeInRuntime(new NodeData(mousePosition, nodeName, "True Logic, False Logic", nodeType));
            }
            else if(nodeType == NodeType.For)
            {
                CreateNodeInRuntime(new NodeData(mousePosition, nodeName, "Finish Logic, Loop Logic", nodeType));
            }
        }

        public void OnClickRemoveNode(NodeGUI node)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                {
                    if (node.data.points.Contains(connections[i].data.inGUID) 
                        || node.data.points.Contains(connections[i].data.outGUID))
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    OnClickRemoveConnection(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }
            nodes.Remove(node);
            node.RemovePoints();
            NodeGUIUtility.RemoveInstance(node.data);
        }

        public void OnClickPoint(ConnectionPoint inPoint)
        {
            selectedPoint.Add(inPoint);

            if (selectedPoint.Count == 2)
            {

                if ((selectedPoint[0].data.nodeGUID != selectedPoint[1].data.nodeGUID) &&
                    Mathf.Abs((int)selectedPoint[0].data.pointType - (int)selectedPoint[1].data.pointType) == 2 &&
                    selectedPoint[0].data.parameterType == selectedPoint[1].data.parameterType)
                {
                    CreateConnectionInRuntime();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void GetInOutPort(out ConnectionPoint inP, out ConnectionPoint outP)
        {
            int index = selectedPoint.FindIndex((i) => (int)i.data.pointType < 2);
            inP = selectedPoint[index];
            outP = selectedPoint[(index + 1) % selectedPoint.Count];
        }

        public void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(connection);
            NodePointData inP = NodeGUIUtility.GetInstance<NodePointData>(connection.data.inGUID),
                outP = NodeGUIUtility.GetInstance<NodePointData>(connection.data.outGUID);
            inP.connections.Remove(connection.data.GUID);
            outP.connections.Remove(connection.data.GUID);
            NodeGUIUtility.RemoveInstance(connection.data);
        }


        private void CreateNodePointInLoad(NodePointData data)
        {
            nodes.Find((n) => n.data.GUID == data.nodeGUID).AttachPoint(data);
        }

        private void CreateNodeInRuntime(NodeData data)
        {
            NodeGUI node = NodeGUI.CreateNodeInRuntime(data);
            nodes.Add(node);
        }

        private void CreateNodeInLoad(NodeData data)
        {
            NodeGUI node = NodeGUI.CreateNodeInLoad(data);
            nodes.Add(node);
        }

        private void CreateConnectionInRuntime()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }
            ConnectionPoint inP, outP;
            GetInOutPort(out inP, out outP);

            connections.Add(Connection.CreateConnectionInRuntime(new NodeConnectionData(inP.data.GUID, outP.data.GUID)));
        }

        private void CreateConnectionInLoad(NodeConnectionData data)
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }
            connections.Add(Connection.CreateConnectionInLoad(data));
        }

        private void AllClear()
        {
            if (nodes == null)
            {
                elements.NodeElementsData.Clear();
                return;
            }

            foreach(var node in nodes)
            {
                OnClickRemoveNode(node);
            }
            elements.NodeElementsData.Clear();
        }

        private void LoadData()
        {
            AllClear();
            NodeSerializableData datas = owner.Load();

            if (datas == null || datas.nodeDatas == null)
                return;


            foreach (var node in datas.nodeDatas)
            {
                NodeGUIUtility.AddInstance(node);
                CreateNodeInLoad(node);
            }

            foreach (var node in datas.nodeFuncDatas)
            {
                NodeGUIUtility.AddInstance(node);
                CreateNodeInLoad(node);
            }

            foreach (var point in datas.nodePointDatas)
            {
                NodeGUIUtility.AddInstance(point);
                CreateNodePointInLoad(point);
            }

            foreach (var connect in datas.nodeConnectionDatas)
            {
                NodeGUIUtility.AddInstance(connect);
                CreateConnectionInLoad(connect);
            }
        }

        private void ClearConnectionSelection()
        {
            selectedPoint.Clear();
        }

        [InitializeOnLoadMethod]
        private static void OnLoad()
        {
            Selection.selectionChanged -= OnSelectionChanged;
            Selection.selectionChanged += OnSelectionChanged;
        }

        private static void OnSelectionChanged()
        {
            NodeScript script = Selection.activeObject as NodeScript;
            if (script && !AssetDatabase.Contains(script))
            {
                Open(script);
            }
        }

        [OnOpenAsset(0)]
        public static bool OnOpen(int instanceID, int line)
        {
            NodeScript nodeGraph = EditorUtility.InstanceIDToObject(instanceID) as NodeScript;
            if (nodeGraph != null)
            {
                Open(nodeGraph);
                return true;
            }
            return false;
        }

        public static void Open(NodeScript graph)
        {
            if (!graph) return;

            NodeBaseEditor w = GetWindow(typeof(NodeBaseEditor), false, "Node Base Editor", true) as NodeBaseEditor;
            Current = w;
            w.owner = graph;
            w.wantsMouseMove = true;
            w.LoadData();
        }
    }

    public static class NodeGUIResources
    {
        public static Texture2D dot { get { return _dot != null ? _dot : _dot = Resources.Load<Texture2D>("xnode_dot"); } }
        private static Texture2D _dot;
        public static Texture2D dotOuter { get { return _dotOuter != null ? _dotOuter : _dotOuter = Resources.Load<Texture2D>("xnode_dot_outer"); } }
        private static Texture2D _dotOuter;
        public static Texture2D nodeBody { get { return _nodeBody != null ? _nodeBody : _nodeBody = Resources.Load<Texture2D>("xnode_node"); } }
        private static Texture2D _nodeBody;
        public static Texture2D nodeHighlight { get { return _nodeHighlight != null ? _nodeHighlight : _nodeHighlight = Resources.Load<Texture2D>("xnode_node_highlight"); } }
        private static Texture2D _nodeHighlight;

        // Styles
        public static Styles styles { get { return _styles != null ? _styles : _styles = new Styles(); } }
        public static Styles _styles = null;
        public static GUIStyle OutputPort { get { return new GUIStyle(EditorStyles.label) { alignment = TextAnchor.UpperRight }; } }
        public class Styles
        {
            public GUIStyle inputPort, nodeHeader, nodeStyle, selectedNodeStyle;

            public Styles()
            {

                nodeStyle = new GUIStyle();
                nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
                nodeStyle.border = new RectOffset(12, 12, 12, 12);

                selectedNodeStyle = new GUIStyle();
                selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
                selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

                GUIStyle baseStyle = new GUIStyle("Label");
                baseStyle.fixedHeight = 20f;

                inputPort = new GUIStyle();
                inputPort.alignment = TextAnchor.MiddleLeft;
                inputPort.padding.left = 10;
                inputPort.fontSize = 15;

                nodeHeader = new GUIStyle();
                nodeHeader.alignment = TextAnchor.MiddleCenter;
                nodeHeader.fontStyle = FontStyle.Bold;
                nodeHeader.normal.textColor = Color.white;
                nodeHeader.fontSize = 15;

            }
        }
    }

}
