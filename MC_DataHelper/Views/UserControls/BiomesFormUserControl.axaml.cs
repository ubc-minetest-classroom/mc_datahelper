using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MC_DataHelper.Views.UserControls;

public partial class BiomesFormUserControl : UserControl
{
    public BiomesFormUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}