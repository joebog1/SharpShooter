namespace PositionalKnwoledgeBenchmark;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();
        builder.Services.AddSingleton<BenchmarkViewModel>();
        builder.Services.AddSingleton<BenchmarkPage>();
        return builder.Build();
    }
}
