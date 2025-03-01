using CommunityToolkit.Mvvm.ComponentModel;

namespace Emerald.Models;

public partial class NavViewHeader : Model
{
    [ObservableProperty]
    private string _HeaderText;

    [ObservableProperty]
    private string _CustomButtonText;

    public Visibility CustomButtonVisibility { get => CustomButtonText == null ? Visibility.Collapsed : Visibility.Visible; }


    [ObservableProperty]
    private string _CustomContent;

    public Visibility CustomContentVisibility { get => CustomContent == null ? Visibility.Collapsed : Visibility.Visible; }

    [ObservableProperty]
    private Thickness _HeaderMargin;
}
