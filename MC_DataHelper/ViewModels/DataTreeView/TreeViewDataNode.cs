using System.Collections.ObjectModel;
using MC_DataHelper.Models;

namespace MC_DataHelper.ViewModels;

public class TreeViewDataNode : TreeViewNode
{
    public TreeViewDataNode(IDataDefinition dataDefinition, ObservableCollection<TreeViewDataNode> children)
    {
        DataDefinition = dataDefinition;
        Children = children;
    }

    public IDataDefinition DataDefinition { get; }

    public string Label => DataDefinition.Name;

    public ObservableCollection<TreeViewDataNode> Children { get; set; }
}