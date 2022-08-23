using System.Collections.ObjectModel;

namespace MC_DataHelper.ViewModels;

public interface ITreeViewNode
{
    public string Label { get; }
    public ObservableCollection<TreeViewDataNode> Children { get; }
}