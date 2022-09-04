using System.Collections.ObjectModel;
using DynamicData.Binding;
using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;

namespace MC_DataHelper.ViewModels.DataTreeView;

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


    public string Label => DataDefinition.Name;

    public ObservableCollection<TreeViewDataNode> Children { get; init; }
    public void NotifyUpdate()
    {
        this.RaisePropertyChanged(nameof(Label));
    }
    
}