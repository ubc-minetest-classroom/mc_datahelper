using System.Collections.ObjectModel;
using DynamicData.Binding;
using MC_DataHelper.Models;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class TreeViewDataNode : ReactiveObject, ITreeViewNode
{
    public TreeViewDataNode(ITreeViewNode parentNode, IDataDefinition dataDefinition)
    {
        ParentNode = parentNode;
        DataDefinition = dataDefinition;
        Children = new ObservableCollectionExtended<TreeViewDataNode>();
    }

    public ITreeViewNode ParentNode { get; }
    public IDataDefinition DataDefinition { get; }


    public string Label => DataDefinition.DataName;

    public ObservableCollection<TreeViewDataNode> Children { get; init; }

    public void refresh()
    {
        this.RaisePropertyChanged(nameof(Label));
        this.RaisePropertyChanged(nameof(Children));
    }

    public void refreshLabel()
    {
        this.RaisePropertyChanged(nameof(Label));
    }
}