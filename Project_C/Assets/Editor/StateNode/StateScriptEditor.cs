using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

using System;
using System.Reflection;
using System.Linq;
using System.Text;
using System.IO;
using StateBehavior.Node;

public class StateScriptEditor : EditorWindow
{
    // Start is called before the first frame update
    [MenuItem("StateScript/Build")]
    static void Build()
    {
        
    }
}

public class StateCompiler
{
    public NodeSerializableData data;
    public StateClassCode classCode;
    public NodeGUIDMap guidMap;

    public static void CompileNodeScript(NodeScript script)
    {
        StateCompiler compiler = new StateCompiler(script);
        compiler.CompileCode();
        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/Script/Action/" + script.userName);
        if (!di.Exists)
            di.Create();

        File.WriteAllText(Application.dataPath + "/Script/Action/" + script.userName + "/" + script.userName + script.stateName + "Action.cs",
            compiler.classCode.ToCodeText());
    }


    public StateCompiler(NodeScript script)
    {
        data = script.serializableNodeData;
        classCode = new StateClassCode(script.userName + script.stateName + "Action");
        guidMap = new NodeGUIDMap(data);
    }

    public void CompileCode()
    {
        List<NodeCode> nodes = new List<NodeCode>();

        foreach(var node in data.nodeDatas)
        {
            nodes.Add(new NodeCode(guidMap, node));
        }

        foreach(var node in data.nodeFuncDatas)
        {
            nodes.Add(new NodeCode(guidMap, node));
        }

        foreach(var node in nodes)
        {
            node.AttachParameter();
        }

        List<NodeCode> parents = nodes.FindAll((node) => node.funcNodeData.methodName == "TimeLine");
        foreach (var node in parents)
        {
            CompileTimeLine(node, data.nodeDatas.FindIndex((n)=>n == node.funcNodeData), classCode.ClassBlock);
        }

        NodeCode parent = nodes.Find((node) => node.funcNodeData.methodName == "Start");
        if (parent != null)
        {
            CompileFunction(parent, (CodeBlock)classCode.ClassBlock.CommandLines[1]);
        }

        parent = nodes.Find((node) => node.funcNodeData.methodName == "Update");
        if (parent != null)
        {
            CompileFunction(parent, (CodeBlock)classCode.ClassBlock.CommandLines[2]);
        }


        parent = nodes.Find((node) => node.funcNodeData.methodName == "Finish");
        if (parent != null)
        {
            CompileFunction(parent, (CodeBlock)classCode.ClassBlock.CommandLines[3]);
        }
    }

    void CompileTimeLine(NodeCode parent, int index, CodeBlock targetFunctionBlock)
    {
        CodeBlock TimeLineCodeBlock = new CodeBlock(new Variable() { Var = "void TimeLine_" + index + "()" }, new List<ICodeBase>());
        targetFunctionBlock.CommandLines.Add(TimeLineCodeBlock);

        ((CodeBlock)(targetFunctionBlock.CommandLines[1])).CommandLines.Add(new Variable(
            "TimelineEvents.Add(new TimeLineEvent(" + parent.logicParamCode.ToCodeText() + ", TimeLine_" + index + "));\n"
            ));

        List<NodePointData> flowPoint = parent.GetPointData(ConnectionPointType.Out);
        NodePointData currentFlow = flowPoint[0];

        if (currentFlow.connections.Count == 0)
            return;

        NodeCode nextCode = guidMap.GetCode(guidMap.GetData<NodePointData>(
            guidMap.GetData<NodeConnectionData>(currentFlow.connections[0]).inGUID).nodeGUID);
        CompileFunction(nextCode, TimeLineCodeBlock);
    }

    void CompileFunction(NodeCode parent, CodeBlock targetFunctionBlock)
    {
        List<NodePointData> flowPoint = parent.GetPointData(ConnectionPointType.Out);

        NodePointData currentFlow = flowPoint[0];

        if (parent.funcNodeData.nodeType == NodeType.Event)
        {
            if (currentFlow.connections.Count == 0)
                return;

            NodeCode nextCode = guidMap.GetCode(guidMap.GetData<NodePointData>(
                guidMap.GetData<NodeConnectionData>(currentFlow.connections[0]).inGUID).nodeGUID);
            CompileFunction(nextCode, targetFunctionBlock);
        }
        else if (parent.funcNodeData.nodeType == NodeType.Condition)
        {
            if (currentFlow.connections.Count != 0)
            {
                NodeCode nextCode = guidMap.GetCode(guidMap.GetData<NodePointData>(
                    guidMap.GetData<NodeConnectionData>(currentFlow.connections[0]).inGUID).nodeGUID);
                CompileFunction(nextCode, ((ConditionCode)parent.code).isTrue);
            }

            currentFlow = flowPoint[1];

            if (currentFlow.connections.Count != 0)
            {
                NodeCode nextCode = guidMap.GetCode(guidMap.GetData<NodePointData>(
                    guidMap.GetData<NodeConnectionData>(currentFlow.connections[0]).inGUID).nodeGUID);
                CompileFunction(nextCode, ((ConditionCode)parent.code).isFalse);
            }

            targetFunctionBlock.CommandLines.Add(parent.code);
        }
        else if(parent.funcNodeData.nodeType == NodeType.Constructor)
        {
            targetFunctionBlock.CommandLines.Add(new KeywordCode() { keyword = new Variable() { Var = "new"}, code = parent.code });

            if (currentFlow.connections.Count == 0)
                return;

            NodeCode nextCode = guidMap.GetCode(guidMap.GetData<NodePointData>(
                guidMap.GetData<NodeConnectionData>(currentFlow.connections[0]).inGUID).nodeGUID);
            CompileFunction(nextCode, targetFunctionBlock);
        }
        else
        {
            targetFunctionBlock.CommandLines.Add(new CodeLine() { code = parent.code });

            if (currentFlow.connections.Count == 0)
                return;

            NodeCode nextCode = guidMap.GetCode(guidMap.GetData<NodePointData>(
                guidMap.GetData<NodeConnectionData>(currentFlow.connections[0]).inGUID).nodeGUID);
            CompileFunction(nextCode, targetFunctionBlock);
        }


    }
}

public class NodeGUIDMap
{
    public Dictionary<string, NodeDataBase> guidMap;
    public Dictionary<string, NodeCode> guidCodeMap;

    public NodeGUIDMap(NodeSerializableData data)
    {
        guidMap = new Dictionary<string, NodeDataBase>();
        guidCodeMap = new Dictionary<string, NodeCode>();

        foreach (var node in data.nodeFuncDatas)
        {
            guidMap.Add(node.GUID, node);
        }

        foreach (var node in data.nodeDatas)
        {
            guidMap.Add(node.GUID, node);
        }

        foreach (var point in data.nodePointDatas)
        {
            guidMap.Add(point.GUID, point);
        }

        foreach (var connect in data.nodeConnectionDatas)
        {
            guidMap.Add(connect.GUID, connect);
        }
    }

    public void RegisterCode(NodeCode code)
    {
        guidCodeMap.Add(code.funcNodeData.GUID, code);
    }

    public T GetData<T>(string guid) where T : NodeDataBase
    {
        return guidMap[guid] as T;
    }

    public NodeCode GetCode(string guid)
    {
        return guidCodeMap[guid];
    }
}

public class ParmeterConnectingAction
{
    public string pointGUID;
    public NodeCode owner;
    public Action<ICodeBase> connectiongAction;
}

public class NodeCode
{
    public NodeGUIDMap guidMap;
    public ICodeBase code;
    public NodeData funcNodeData;

    public ICodeBase logicParamCode;

    public Dictionary<string, ParmeterConnectingAction> connectMapping;


    public NodeCode(NodeGUIDMap guidMap, NodeData funcNodeData)
    {
        this.guidMap = guidMap;
        this.funcNodeData = funcNodeData;
        connectMapping = new Dictionary<string, ParmeterConnectingAction>();

        if (funcNodeData is NodeFuncData)
            InitFunc();
        else
            InitLogic();

        guidMap.RegisterCode(this);
    }

    public void InitLogic()
    {
        if (funcNodeData.nodeType == NodeType.Event)
        {
            code = new Variable() { Var = "Owner" };
        }
        else if(funcNodeData.nodeType == NodeType.TimeLine)
        {
            code = new Variable() { Var = "Owner" };
            List<NodePointData> parameterPoint = GetPointData(ConnectionPointType.Parameter);

            AddMapping(parameterPoint[0].GUID, (c) =>
            {
                logicParamCode = c;
            });
        }
        else if(funcNodeData.nodeType == NodeType.Condition)
        {
            ConditionCode condition = new ConditionCode();
            CodeBlock trueBlock = new CodeBlock(), 
                falseBlock = new CodeBlock();
            FunctionCall trueBlockCondition = new FunctionCall();

            trueBlockCondition.Access = new Variable() { Var = "if" };
            trueBlockCondition.Parameter = new ParameterBlock();
            falseBlock.Condition = new Variable() { Var = "else" };

            trueBlock.Condition = trueBlockCondition;

            condition.isTrue = trueBlock;
            condition.isFalse = falseBlock;

            code = condition;

            List<NodePointData> parameterPoint = GetPointData(ConnectionPointType.Parameter);

            AddMapping(parameterPoint[0].GUID, (c)=>
            {
                ((ParameterBlock)((FunctionCall)((ConditionCode)code).isTrue.Condition).Parameter).Parameters.Add(c);
            }
            );
        }
    }

    public void InitFunc()
    {
        FunctionCall func = new FunctionCall();

        NodeFuncData funcNodeData = this.funcNodeData as NodeFuncData;
        Type methodClassType = NodeGUIUtility.GetType(funcNodeData.funcClassType);
        List<NodePointData> parameterPoint = GetPointData(ConnectionPointType.Parameter);

        List<Type> methodParameters = new List<Type>();
        foreach (var point in funcNodeData.parameters)
        {
            methodParameters.Add(NodeGUIUtility.GetType(point));
        }

        MethodBase method;
        if (funcNodeData.nodeType == NodeType.Constructor)
        {
            method = methodClassType.GetConstructor(methodParameters.ToArray());

            KeywordCode keyCode = new KeywordCode() { keyword = new Variable() { Var = "new"},
                code = new Variable() { Var = funcNodeData.funcClassType} };

            foreach (var param in parameterPoint)
            {
                AddMapping(param.GUID, (c) => ((ParameterBlock)((FunctionCall)code).Parameter).Parameters.Add(c));
            }

            func.Access = keyCode;
            func.Parameter = new ParameterBlock();

            code = func;
        }
        else if(funcNodeData.nodeType == NodeType.Property)
        {
            method = methodParameters.Count != 0 ? 
                methodClassType.GetMethod(funcNodeData.methodName, methodParameters.ToArray()) :
                methodClassType.GetMethod(funcNodeData.methodName);

            AccessCode access = new AccessCode();

            if (method.IsStatic)
            {
                access.ownerObject = new Variable() { Var = methodClassType.Name };
                func.Access = access;
            }
            else
            {
                NodePointData ownerPoint = parameterPoint.Find((p) => p.parameterType == funcNodeData.funcClassType);
                AddMapping(ownerPoint.GUID, (c) =>
                {
                    if(code is OperatorBlock)
                    {
                        ((AccessCode)((OperatorBlock)code).A).ownerObject = c;
                    }
                    else
                    {
                        ((AccessCode)code).ownerObject = c;
                    }
                });
                parameterPoint.Remove(ownerPoint);
            }

            foreach (var param in parameterPoint)
            {
                AddMapping(param.GUID, (c) =>
                {
                    OperatorBlock oper = new OperatorBlock();
                    oper.Operator = "=";
                    oper.A = code;
                    oper.B = c;
                    code = oper;
                });
            }

            access.member = new Variable() { Var = method.Name.Split('_')[1] };
            code = access;
        }
        else
        {
            method = methodParameters.Count != 0 ?
                methodClassType.GetMethod(funcNodeData.methodName, methodParameters.ToArray()) :
                methodClassType.GetMethod(funcNodeData.methodName);

            AccessCode access = new AccessCode();

            if (method.IsStatic)
            {
                access.ownerObject = new Variable() { Var = methodClassType.Name };
                func.Access = access;
            }
            else
            {
                NodePointData ownerPoint = parameterPoint.Find((p) => p.parameterType == funcNodeData.funcClassType);
                AddMapping(ownerPoint.GUID, (c) => ((AccessCode)((FunctionCall)code).Access).ownerObject = c);
                parameterPoint.Remove(ownerPoint);
            }

            foreach (var param in parameterPoint)
            {
                AddMapping(param.GUID, (c) => ((ParameterBlock)((FunctionCall)code).Parameter).Parameters.Add(c));
            }

            access.member = new Variable() { Var = method.Name };
            func.Access = access;
            func.Parameter = new ParameterBlock();
        
            code = func;
        }
        
    }

    public void AttachParameter()
    {
        List<NodePointData> parameterPoint = GetPointData(ConnectionPointType.Parameter);

        foreach (var param in parameterPoint)
        {
            if(param.connections.Count == 0)
            {
                Variable var = new Variable();
                if(param.parameterType == typeof(string).FullName)
                {
                    var.Var = "\"" + param.cachedValue + "\"";
                }
                else if(param.parameterType == typeof(float).FullName)
                {
                    var.Var = param.cachedValue + "f";
                }
                else if(param.parameterType == typeof(bool).FullName)
                {
                    var.Var = param.cachedValue.ToLower();
                }
                else
                {
                    var.Var = param.cachedValue;
                }

                if(connectMapping.ContainsKey(param.GUID))
                    connectMapping[param.GUID].connectiongAction.Invoke(var);
            }
            else
            {
                NodeCode paramCode = guidMap.GetCode(guidMap.GetData<NodePointData>(guidMap.GetData<NodeConnectionData>(param.connections[0]).outGUID).nodeGUID);
                connectMapping[param.GUID].connectiongAction.Invoke(paramCode.code);
            }
        }
    }

    public void AddMapping(string pointGUID, Action<ICodeBase> connectiongAction)
    {
        connectMapping.Add(pointGUID, new ParmeterConnectingAction()
        {
            pointGUID = pointGUID,
            owner = this,
            connectiongAction = connectiongAction
        });
    }

    public List<NodePointData> GetPointData(ConnectionPointType type)
    {
        List<NodePointData> retVal = new List<NodePointData>();
        foreach (var guid in funcNodeData.points)
        {
            NodePointData point = guidMap.GetData<NodePointData>(guid);
            if (point.pointType == type)
                retVal.Add(point);
        }

        return retVal;
    }
}



public interface ICodeBase
{
    string ToCodeText();
}

public class StateClassCode : ICodeBase
{
    public string ClassName { get; set; }
    public CodeBlock ClassBlock { get; set; }

    public StateClassCode(string className)
    {
        ClassName = className;

        ClassBlock = new CodeBlock();

        CodeBlock startAction = new CodeBlock();
        CodeBlock updateAction = new CodeBlock();
        CodeBlock finishAction = new CodeBlock();

        Variable defineInstance = new Variable() { Var = "\npublic static " + ClassName + " GetInstance() { return ObjectPooling.PopObject<" + ClassName + ">(); }\n" };

        startAction.Condition = new Variable() { Var = "public override void StartAction(Character owner)" };
        updateAction.Condition = new Variable() { Var = "public override void UpdateAction()" };
        finishAction.Condition = new Variable() { Var = "public override void FinishAction()" };

        startAction.CommandLines.Add(new Variable() { Var = "base.StartAction(owner);\n" });
        updateAction.CommandLines.Add(new Variable() { Var = "base.UpdateAction();\n" });
        finishAction.CommandLines.Add(new Variable() { Var = "base.FinishAction();\n" });

        ClassBlock.CommandLines.Add(defineInstance);
        ClassBlock.CommandLines.Add(startAction);
        ClassBlock.CommandLines.Add(updateAction);
        ClassBlock.CommandLines.Add(finishAction);
    }

    public string ToCodeText()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("using System;\n");
        sb.Append("using System.Collections;\n");
        sb.Append("using System.Collections.Generic;\n");
        sb.Append("using UnityEngine;\n\n\n\n");
        sb.Append("public class " + ClassName + " : CharacterAction");
        sb.Append(ClassBlock.ToCodeText());
        return sb.ToString();
    }
}

public class CodeLine : ICodeBase
{
    public ICodeBase code;

    public string ToCodeText()
    {
        return code?.ToCodeText() + ";\n";
    }
}

public class ConditionCode : ICodeBase
{
    public CodeBlock isTrue, isFalse;

    public string ToCodeText()
    {
        return isTrue.ToCodeText() + isFalse.ToCodeText();
    }
}

public class CodeBlock : ICodeBase
{
    public ICodeBase Condition;
    public List<ICodeBase> CommandLines;

    public CodeBlock()
    {
        CommandLines = new List<ICodeBase>();
    }

    public CodeBlock(ICodeBase condition, List<ICodeBase> commandLines)
    {
        Condition = condition;
        CommandLines = commandLines;
    }

    public string ToCodeText()
    {
        StringBuilder text = new StringBuilder("\n");
        if (Condition != null)
            text.Append(Condition.ToCodeText()).Append("\n");
        text.Append("{\n");
        foreach (var e in CommandLines)
        {
            text.Append(e.ToCodeText());
        }
        text.Append("}\n");
        return text.ToString();
    }
}

public class OperatorBlock : ICodeBase
{
    public ICodeBase A;
    public ICodeBase B;
    public string Operator;

    public string ToCodeText()
    {
        return A.ToCodeText() + " " + Operator + " " + B.ToCodeText();
    }
}

public class Variable : ICodeBase
{
    public string Var;

    public Variable()
    {

    }

    public Variable(string var)
    {
        Var = var;
    }

    public string ToCodeText()
    {
        return Var;
    }
}

public class ParameterBlock : ICodeBase
{
    public List<ICodeBase> Parameters;
    public ParameterBlock()
    {
        Parameters = new List<ICodeBase>();
    }

    public string ToCodeText()
    {
        StringBuilder text = new StringBuilder("(");
        foreach(var e in Parameters)
        {
            text.Append(e.ToCodeText() + " ,");
        }

        if(Parameters.Count != 0)
        {
            text.Remove(text.Length - 2, 2);
        }

        text.Append(")");

        return text.ToString();
    }
}

public class AccessCode : ICodeBase
{
    public ICodeBase ownerObject;
    public ICodeBase member;

    public string ToCodeText()
    {
        return ownerObject?.ToCodeText() + "." + member?.ToCodeText();
    }
}

public class KeywordCode : ICodeBase
{
    public ICodeBase keyword;
    public ICodeBase code;

    public string ToCodeText()
    {
        return keyword?.ToCodeText() + " " + code?.ToCodeText();
    }
}

public class FunctionCall : ICodeBase
{
    public ICodeBase Access;
    public ICodeBase Parameter;

    public string ToCodeText()
    {
        return Access?.ToCodeText() + Parameter?.ToCodeText();
    }
}
