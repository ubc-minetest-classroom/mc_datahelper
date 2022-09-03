using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace MC_DataHelper.Views.UserControls;

public partial class ModConfigUserControl : UserControl
{
    public ModConfigUserControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}