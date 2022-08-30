using ReactiveUI;

namespace MC_DataHelper.ViewModels;

public class FieldHeaderPair : ReactiveObject
{
    private string _field;
    private string _header;

    public string Field
    {
        get => _field;
        set => this.RaiseAndSetIfChanged(ref _field, value);
    }

    public string Header
    {
        get => _header;
        set => this.RaiseAndSetIfChanged(ref _header, value);
    }

    public FieldHeaderPair(string field, string header)
    {
        _field = field;
        _header = header;
    }
}