using Spectre.Console;

namespace GameOfLife;

public enum MenuAction : byte
{
    StartGame,
    Settings,
    Exit
}

public enum SettingsAction : byte
{
    SetGameSpeed,
    SetGridSize
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
        GameConfiguration configuration,
        long generation)
    {
        DrawGenerationWithCanvas(pattern);
        DrawProperties(
            configuration,
            generation);
    }

    public static void DrawProperties(
        GameConfiguration configuration,
        long generation)
    {
        AnsiConsole.MarkupLine($"• Size: [green]{configuration.GridSize}x{configuration.GridSize}[/]");
        AnsiConsole.MarkupLine($"• Speed: [green]{configuration.SpeedInMs}ms[/]");
        AnsiConsole.MarkupLine($"• Pattern: [green]{configuration.PatternName}[/]");
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
                                .Title("\n    [green]MENU[/]")
                                .AddChoices(Enum.GetValues<MenuAction>()
                                                .Select(Show)
                                                .ToArray()));

        return Read(action);

        static string Show(MenuAction value) => value switch
        {
            MenuAction.StartGame => "Start Game",
            MenuAction.Settings => "Settings",
            MenuAction.Exit => "Exit",
            _ => throw NotImplemented()
        };

        static MenuAction Read(string value) => value switch
        {
            "Start Game" => MenuAction.StartGame,
            "Settings" => MenuAction.Settings,
            "Exit" => MenuAction.Exit,
            _ => throw NotImplemented()
        };

        static NotImplementedException NotImplemented() => new("Menu action is not implemented");
    }
    public static SettingsAction PromptSettingsAction()
    {
        var action = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("\n    [green]SETTINGS[/]")
                            .AddChoices(Enum.GetValues<SettingsAction>()
                                            .Select(Show)
                                            .ToArray()));

        return Read(action);

        static string Show(SettingsAction value) => value switch
        {
            SettingsAction.SetGameSpeed => "Set Game Speed",
            SettingsAction.SetGridSize => "Set Grid Size",
            _ => throw NotImplemented()
        };

        static SettingsAction Read(string value) => value switch
        {
            "Set Game Speed" => SettingsAction.SetGameSpeed,
            "Set Grid Size" => SettingsAction.SetGridSize,
            _ => throw NotImplemented()
        };

        static NotImplementedException NotImplemented() => new("Settings action is not implemented");
    }

    public static int PromptGridSize(int defaultValue)
    {
        AnsiConsole.MarkupLine($"[gray]Range: 16x16-64x64 | Default: {defaultValue}x{defaultValue}[/]");

        return AnsiConsole.Prompt(
                new TextPrompt<int>($"\nEnter grid size:")
                        .Validate((n) => n switch
                        {
                            < 16 => ValidationResult.Error("[red]Too small[/]"),
                            > 64 => ValidationResult.Error("[red]Too big[/]"),
                            _ => ValidationResult.Success(),
                        }));
    }

    public static int PromptSpeedInMs(int defaultValue)
    {
        AnsiConsole.MarkupLine($"[gray]Range: 50-1000ms | Default: {defaultValue}ms[/]");

        return AnsiConsole.Prompt(
                new TextPrompt<int>($"\nEnter speed in ms:")
                        .Validate((n) => n switch
                        {
                            < 50 => ValidationResult.Error("[red]Too fast[/]"),
                            > 1000 => ValidationResult.Error("[red]Too slow[/]"),
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
