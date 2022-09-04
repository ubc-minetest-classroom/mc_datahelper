using System.Reactive;
using MC_DataHelper.Models.DataDefinitions;
using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public abstract class DataDefinitionFormViewModelBase<T> : ViewModelBase where T : IDataDefinition, new()
{
    private readonly MainWindowViewModel _mainWindowViewModel;


    protected T Data = new T();

    private ReactiveCommand<Unit, Unit> AddItemCommand { get; }

    public string? Name
    {
        get => Data.Name;
        set
        {
            Data.Name = value ?? "";
            _mainWindowViewModel.SelectedTreeViewItem?.NotifyUpdate();
        }
    }


    protected DataDefinitionFormViewModelBase()
    {
        _mainWindowViewModel = new MainWindowViewModel();
        AddItemCommand = ReactiveCommand.Create(AddItem);
    }

    protected DataDefinitionFormViewModelBase(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        AddItemCommand = ReactiveCommand.Create(AddItem);
    }


    protected void AddItem()
    {
        ClearForm();
        _mainWindowViewModel.AddDataDefinition(Data);
        UpdateProperties();
    }

    public void ClearForm()
    {
        _mainWindowViewModel.SelectedTreeViewItem = null;
        Data = new T();
    }

    public void UpdateDataSource(T nodeDataDefinition)
    {
        Data = nodeDataDefinition;
        UpdateProperties();
    }

    protected virtual void UpdateProperties()
    {
        this.RaisePropertyChanged(nameof(Name));
    }
}