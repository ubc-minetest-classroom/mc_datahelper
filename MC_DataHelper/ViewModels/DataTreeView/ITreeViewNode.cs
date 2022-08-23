using System.Collections.ObjectModel;
using System.ComponentModel;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public interface ITreeViewNode
{
    public string Label { get; }
    public ObservableCollection<TreeViewDataNode> Children { get; }
}