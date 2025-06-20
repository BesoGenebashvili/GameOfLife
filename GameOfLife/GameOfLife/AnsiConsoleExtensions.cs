using Spectre.Console;

namespace GameOfLife;

public sealed class AnsiConsoleExtensions
{
    public static Pattern.Name PromptPattern() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<Pattern.Name>()
                .Title("Select [green]Pattern[/]:")
                .AddChoices(Enum.GetValues<Pattern.Name>()));

    public static void MarkupLineProperties(
        int speed,
        Pattern.Name patternName,
        long generation)
    {
        AnsiConsole.MarkupLine($"• Speed: [green]{speed}ms[/]");
        AnsiConsole.MarkupLine($"• Pattern: [green]{patternName}[/]");
        AnsiConsole.MarkupLine($"• Generation: [green]{generation}[/]");
    }

    public static void DrawGenerationWithCanvas(int[][] xs)
    {
        var canvas = new Canvas(xs.Length, xs[0].Length);

        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                var color = ResolveColor(xs[i][j]);

                // j is the X coordinate (column)
                // i is the Y coordinate (row)
                canvas.SetPixel(j, i, color);
            }
        }

        AnsiConsole.Write(canvas);

        static Color ResolveColor(int value) => value switch
        {
            0 => Color.White,
            1 => Color.Black,
            _ => throw new InvalidOperationException("Unexpected cell value")
        };
    }
}
