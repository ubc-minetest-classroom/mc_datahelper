using System.Reactive;
using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public abstract class DataDefinitionFormViewModelBase<T> :  ViewModelBase where T : IDataDefinition, new()
{
    protected readonly MainWindowViewModel MainWindowViewModel;


    protected T Data = new T();

    public ReactiveCommand<Unit, Unit> SubmitFormCommand { get; }
    public ReactiveCommand<Unit, Unit> ClearFormCommand { get; }
    
    public string? Name
    {
        get => Data.Name;
        set
        {
            Data.Name = value ?? "";

            MainWindowViewModel.SelectedNode?.refreshLabel();
        }
    }


    public DataDefinitionFormViewModelBase()
    {
        MainWindowViewModel = new MainWindowViewModel();
        SubmitFormCommand = ReactiveCommand.Create(SubmitForm);
        ClearFormCommand = ReactiveCommand.Create(ClearForm);
    }

    public DataDefinitionFormViewModelBase(MainWindowViewModel mainWindowViewModel)
    {
        MainWindowViewModel = mainWindowViewModel;
        SubmitFormCommand = ReactiveCommand.Create(SubmitForm);
        ClearFormCommand = ReactiveCommand.Create(ClearForm);
    }


    protected void SubmitForm()
    {
        MainWindowViewModel.AddDataDefinition(Data);
        ClearForm();
    }

    protected void ClearForm()
    {
        MainWindowViewModel.SelectedNode = null;
        MainWindowViewModel.SelectedTreeViewItem = null;
        Data = new T();
        UpdateProperties();
    }
    
    public void UpdateDataSource(T nodeDataDefinition, TreeViewDataNode treeViewDataNode)
    {
        Data = nodeDataDefinition;
        UpdateProperties();
    }
    
    protected virtual void UpdateProperties()
    {
        this.RaisePropertyChanged(nameof(Name));
    }
}