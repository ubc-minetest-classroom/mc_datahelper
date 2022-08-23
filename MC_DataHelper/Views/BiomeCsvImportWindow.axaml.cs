using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MC_DataHelper.Views;

public partial class BiomeCsvImportWindow : Window
{
    public BiomeCsvImportWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}