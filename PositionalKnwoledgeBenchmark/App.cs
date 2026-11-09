namespace PositionalKnwoledgeBenchmark;

public sealed class App : Application
{
    private readonly BenchmarkPage benchmarkPage;

    public App(BenchmarkPage benchmarkPage) => this.benchmarkPage = benchmarkPage;

    protected override Window CreateWindow(IActivationState? activationState) => new(benchmarkPage);
}
