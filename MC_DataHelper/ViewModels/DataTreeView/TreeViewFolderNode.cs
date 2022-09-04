using System.Collections.ObjectModel;
using ReactiveUI;

namespace MC_DataHelper.ViewModels.DataTreeView;

public class TreeViewFolderNode : ReactiveObject, ITreeViewNode
{
    public TreeViewFolderNode(string label)
    {
        Label = label;
        Children = new ObservableCollection<TreeViewDataNode>();
    }

    public TreeViewFolderNode(string label, ObservableCollection<TreeViewDataNode> children)
    {
        Label = label;
        Children = children;
    }

    public string Label { get; }
    public ObservableCollection<TreeViewDataNode> Children { get; }
    public void NotifyUpdate()
    {
        this.RaisePropertyChanged(nameof(Label));
    }
}