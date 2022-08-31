namespace MC_DataHelper.Models.DataDefinitions;

public class UnknownDataDefinition : IDataDefinition
{
    public UnknownDataDefinition(dynamic data)
    {
        Data = data;
    }

    public dynamic Data { get; }

    public string Name
    {
        get => Data.name;
        set => Data.name = value;
    }

    public string JsonType => Data._jsonType;
}