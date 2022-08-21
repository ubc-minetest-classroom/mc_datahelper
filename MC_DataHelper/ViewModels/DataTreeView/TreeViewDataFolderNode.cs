using System.Collections.ObjectModel;

namespace MC_DataHelper.ViewModels;

public class TreeViewDataFolderNode : TreeViewNode
{
    public TreeViewDataFolderNode(string label, ObservableCollection<TreeViewDataNode> children)
    {
        Label = label;
        Children = children;
    }

    public string Label { get; }
    public ObservableCollection<TreeViewDataNode> Children { get; }
}