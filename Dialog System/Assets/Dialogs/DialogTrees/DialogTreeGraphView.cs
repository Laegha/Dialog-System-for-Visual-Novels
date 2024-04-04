using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class DialogTreeGraphView : GraphView
{
    public new class UxmlFactory : UxmlFactory<DialogTreeGraphView, GraphView.UxmlTraits> { }

    public Action<DialogNodeView> OnNodeSelected;

    public DialogTreeEditor editor;

    public DialogTreeGraphView() 
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Dialogs/DialogTrees/DialogTreeEditor.uss");
        styleSheets.Add(styleSheet);

    }
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction($"[{System.Type.GetType("DialogNode")}]", (a) => CreateNode(null));
    }

    public void PopulateView(DialogTree dialogTree)
    {
        Dialog[] currTreeDialogs = dialogTree.Dialogs;
        foreach (Dialog dialog in currTreeDialogs)
            CreateNode(dialog);
    }

    void CreateNode(Dialog nodeDialog)
    {
        if (!editor.IsTreeRefreshed())
            return;
        //DialogNode node = new DialogNode(nodeDialog);
        DialogNode node = ScriptableObject.CreateInstance("DialogNode") as DialogNode;
        node.Dialog = nodeDialog;
        CreateNodeView(node);
    }

    void CreateNodeView(DialogNode node)
    {
        DialogNodeView newNode = new DialogNodeView();
        newNode.node = node;
        newNode.OnNodeSelected = OnNodeSelected;
        newNode.title = "New DialogNode";
        AddElement(newNode);
    }
}