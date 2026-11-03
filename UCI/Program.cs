namespace SharpShooter.UCI;

internal static class Program
{
  private static void Main(string[] args)
  {
    using StreamReader reader = new StreamReader(Console.OpenStandardInput());
    // :TODO: integration tests should try and use this.
    string? line;
    while ((line = reader.ReadLine()) != null)
    {
      line = line.Trim();
      if (string.IsNullOrEmpty(line))
      {
        continue;
      }

      UciCommandParser.ParseUciCommand(line);
    }
  }

}
