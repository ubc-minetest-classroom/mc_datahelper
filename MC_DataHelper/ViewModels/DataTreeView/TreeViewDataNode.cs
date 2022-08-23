using System.Collections.ObjectModel;
using MC_DataHelper.Models;

namespace MC_DataHelper.ViewModels;

public class TreeViewDataNode : ITreeViewNode
{
    public TreeViewDataNode(ITreeViewNode parentNode, IDataDefinition dataDefinition)
    {
        this.ParentNode = parentNode;
        DataDefinition = dataDefinition;
        Children = null;
    }

    public IDataDefinition DataDefinition { get; }

    public string Label => DataDefinition.DataName;

    public ObservableCollection<TreeViewDataNode> Children { get; set; }
    public ITreeViewNode ParentNode { get; }
}