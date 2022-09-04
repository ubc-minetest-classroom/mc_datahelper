using System.Collections.ObjectModel;

namespace MC_DataHelper.ViewModels.DataTreeView;

public interface ITreeViewNode
{
    public string Label { get; }
    public ObservableCollection<TreeViewDataNode> Children { get; }

    public void NotifyUpdate();
}