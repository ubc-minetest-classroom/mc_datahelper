using System.Collections.ObjectModel;

namespace MC_DataHelper.ViewModels;

public interface TreeViewNode
{
    public string Label { get; }
    public ObservableCollection<TreeViewDataNode> Children { get; }
}