using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using System.Runtime;

namespace StateBehavior.Node
{
    public class NodeListGUI
    {
        public string SubMenuType;
        public Type CurrentType;

        public string text;
        public Vector2 mousePosition;

        public List<Type> allTypes;

        public Rect rect;

        public Vector2 scrollPos;

        protected string lastText;
        protected List<Type> cachedList;

        public NodeListGUI()
        {
            rect = new Rect(Vector2.zero, new Vector2(350f, 4000f));
            SubMenuType = "";
            CurrentType = null;
            text = "";
            allTypes = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assem in assemblies)
            {
                allTypes.AddRange(assem.GetTypes().Where((t) => t.IsPublic));
            }
            lastText = "";
        }

        public void Draw()
        {
            rect.position = mousePosition;
            GUILayout.BeginArea(rect);
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(400));

            if (SubMenuType.Length == 0 && CurrentType == null && text.Length == 0)
            {
                text = GUILayout.TextField("", GUILayout.Height(20));
                if(GUILayout.Button("FSM Base Function", GUILayout.Height(20)))
                {
                    SubMenuType = "FSM Base Function";
                }
                if (GUILayout.Button("Logic Node", GUILayout.Height(20)))
                {
                    SubMenuType = "Logic Node";
                }
                if (GUILayout.Button("NodeUtil", GUILayout.Height(20)))
                {
                    CurrentType = typeof(NodeUtil);
                }
            }
            else if (CurrentType == null && text.Length == 0)
            {
                if (SubMenuType == "FSM Base Function")
                {
                    if (GUILayout.Button("<(Back)", GUILayout.Height(20)))
                    {
                        SubMenuType = "";
                    }
                    if (GUILayout.Button("Start", GUILayout.Height(20)))
                    {
                        NodeBaseEditor.Current.OnClickAddNode(mousePosition, NodeType.Event, "Start");
                        NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                    }
                    if (GUILayout.Button("Update", GUILayout.Height(20)))
                    {
                        NodeBaseEditor.Current.OnClickAddNode(mousePosition, NodeType.Event, "Update");
                        NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                    }
                    if (GUILayout.Button("Finish", GUILayout.Height(20)))
                    {
                        NodeBaseEditor.Current.OnClickAddNode(mousePosition, NodeType.Event, "Finish");
                        NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                    }
                    if (GUILayout.Button("TimeLine", GUILayout.Height(20)))
                    {
                        NodeBaseEditor.Current.OnClickAddNode(mousePosition, NodeType.TimeLine, "TimeLine");
                        NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                    }
                }
                else if (SubMenuType == "Logic Node")
                {
                    if(GUILayout.Button("<(Back)", GUILayout.Height(20)))
                    {
                        SubMenuType = "";
                    }
                    if (GUILayout.Button("IF", GUILayout.Height(20)))
                    {
                        NodeBaseEditor.Current.OnClickAddNode(mousePosition, NodeType.Condition, "IF");
                        NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                    }
                    if (GUILayout.Button("For", GUILayout.Height(20)))
                    {
                        NodeBaseEditor.Current.OnClickAddNode(mousePosition, NodeType.For, "For");
                        NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                    }
                }
            }
            else if (CurrentType == null)
            {
                text = GUILayout.TextField(text, GUILayout.Height(20));
                MakeSearchCache();

                if (GUILayout.Button("<(Back)", GUILayout.Height(20)))
                {
                    SubMenuType = "";
                    ClearSearchCache();
                }

                if (cachedList != null)
                {
                    foreach (var type in cachedList)
                    {
                        if (GUILayout.Button(type.Name, GUILayout.Height(20)))
                        {
                            CurrentType = type;
                            text = "";
                        }
                    }
                }
            }
            else if(CurrentType != null)
            {
                if (GUILayout.Button("<(Back)", GUILayout.Height(20)))
                {
                    SubMenuType = "";
                    ClearSearchCache();
                    CurrentType = null;
                }

                if (CurrentType != null)
                {
                    ConstructorInfo[] constructors = CurrentType.GetConstructors().Where((m) => m.IsPublic).ToArray();
                    MethodInfo[] methods = CurrentType.GetMethods().Where((m) => m.IsPublic).ToArray();

                    foreach (var constructor in constructors)
                    {
                        if (GUILayout.Button(CurrentType.Name + "/" + constructor.Name, GUILayout.Height(20)))
                        {
                            NodeBaseEditor.Current.OnClickAddNode(mousePosition, CurrentType, constructor);
                            NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                        }
                    }

                    foreach (var method in methods)
                    {
                        if (GUILayout.Button(CurrentType.Name + "/" + method.Name, GUILayout.Height(20)))
                        {
                            NodeBaseEditor.Current.OnClickAddNode(mousePosition, CurrentType, method);
                            NodeBaseEditor.Current.IsVisibleNodeMenu = false;
                        }
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        protected void ClearSearchCache()
        {
            text = "";
            lastText = "";
            cachedList = null;
        }

        protected void MakeSearchCache()
        {
            if (!text.Equals(lastText))
            {
                if (text.Length < 3)
                {
                    cachedList = null;
                    lastText = text;
                }
                else
                {
                    cachedList = allTypes.FindAll((t) => t.Name.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) >= 0);
                    lastText = text;
                }
            }
        }
    }

}
