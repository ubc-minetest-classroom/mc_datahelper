namespace MC_DataHelper.Models;

public class UnknownDataDefinition : IDataDefinition
{
    public dynamic Data { get; }

    public string DataName
    {
        get => Data.name;
        set => Data.name = value;
    }

    public string JsonType => Data._jsonType;

    public UnknownDataDefinition(dynamic data)
    {
        Data = data;
    }
}