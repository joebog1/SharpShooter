namespace PositionalKnwoledgeBenchmark;

public partial class BenchmarkPage : ContentPage
{
    public BenchmarkPage(BenchmarkViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
