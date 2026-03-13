using CommunityToolkit.Mvvm.ComponentModel;

namespace Sufni.Bridge.ViewModels.SessionPages;

public partial class SpringPageViewModel() : PageViewModelBase("Spring")
{
    [ObservableProperty] private string? travelComparisonHistogram;
    [ObservableProperty] private string? frontTravelHistogram;
    [ObservableProperty] private string? rearTravelHistogram;
}
