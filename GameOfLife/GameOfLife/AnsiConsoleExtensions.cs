using Spectre.Console;

namespace GameOfLife;

public enum MenuAction : byte
{
    StartGame,
    Settings,
    Exit
}

public sealed class AnsiConsoleExtensions
{
    public static void DrawMenu()
    {
        AnsiConsole.Write(
            new FigletText("Game of Life")
                .Centered()
                .Color(Color.Green));

        AnsiConsole.MarkupLine(
            "The Game of Life is not your typical computer game.\n" +
            "It is a [yellow]cellular automaton[/], invented by mathematician [blue]John Conway[/].\n" +
            "The game consists of a grid of cells that can be either [bold]alive (1)[/] or [bold]dead (0)[/].\n\n" +

            "[underline]Rules:[/]\n\n" +

            "[bold]For a live (populated) cell:[/]\n" +
            "  - Fewer than 2 neighbors -> [red]dies[/] (as if by solitude)\n" +
            "  - More than 3 neighbors -> [red]dies[/] (as if by overpopulation)\n" +
            "  - 2 or 3 neighbors -> [green]survives[/]\n\n" +

            "[bold]For a dead (empty) cell:[/]\n" +
            "  - Exactly 3 neighbors -> [green]becomes alive[/] (reproduction)"
        );
    }

    public static void DrawGame(
        int[][] pattern,
        int speedInMs,
        Pattern.Name patternName,
        long generation)
    {
        DrawGenerationWithCanvas(pattern);
        DrawProperties(
            speedInMs,
            patternName,
            generation);
    }

    public static void DrawProperties(
        int speed,
        Pattern.Name patternName,
        long generation)
    {
        AnsiConsole.MarkupLine($"• Speed: [green]{speed}ms[/]");
        AnsiConsole.MarkupLine($"• Pattern: [green]{patternName}[/]");
        AnsiConsole.MarkupLine($"• Generation: [green]{generation}[/]");
        AnsiConsole.MarkupLine($"[gray]• Press any key to exit[/]");
    }

    public static Pattern.Name PromptPattern() =>
        AnsiConsole.Prompt(
            new SelectionPrompt<Pattern.Name>()
                .Title("Select [green]Pattern[/]:")
                .AddChoices(Enum.GetValues<Pattern.Name>()));

    public static MenuAction PromptMenuAction()
    {
        var action = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                                .Title(string.Empty)
                                .AddChoices(Enum.GetValues<MenuAction>()
                                                .Select(Show)
                                                .ToArray()));

        return action switch
        {
            "Start Game" => MenuAction.StartGame,
            "Settings" => MenuAction.Settings,
            "Exit" => MenuAction.Exit,
            _ => throw NotSupported()
        };

        string Show(MenuAction action) => action switch
        {
            MenuAction.StartGame => "Start Game",
            MenuAction.Settings => "Settings",
            MenuAction.Exit => "Exit",
            _ => throw NotSupported()
        };

        NotSupportedException NotSupported() => new("Menu action not supported");
    }

    public static int PromptSpeedInMs(int defaultValue)
    {
        AnsiConsole.MarkupLine($"[gray]Minimum value is 50ms[/]");
        AnsiConsole.MarkupLine($"[gray]Maximum value is 400ms[/]");
        AnsiConsole.MarkupLine($"[gray]Default value is {defaultValue}ms[/]");

        return AnsiConsole.Prompt(
                new TextPrompt<int>($"\nEnter speed in ms:")
                        .Validate((n) => n switch
                        {
                            < 50 => ValidationResult.Error("[red]Too slow[/]"),
                            > 400 => ValidationResult.Error("[red]Too fast[/]"),
                            _ => ValidationResult.Success(),
                        }));
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

    /// <summary>
    /// Just for testing purposes
    /// </summary>
    public static void DrawGenerationWithoutCanvas(int[][] xs)
    {
        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < xs[i].Length; j++)
            {
                Console.Write(xs[i][j] is 1 ? "0" : " ");
            }
            Console.WriteLine();
        }
    }
}
