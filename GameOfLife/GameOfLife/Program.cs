using GameOfLife;
using Spectre.Console;
using AnsiConsoleExtensions = GameOfLife.AnsiConsoleExtensions;

ConsoleInteropService.Configure(ConsoleInteropServiceConfiguration.Default);

var defaultConfiguration = GameConfiguration.Default;

await Run(defaultConfiguration);

static async Task Run(GameConfiguration gameConfiguration)
{
    AnsiConsoleExtensions.DrawMenu();

    var menuAction = AnsiConsoleExtensions.PromptMenuAction();

    switch (menuAction)
    {
        case MenuAction.StartGame:
            await StartGame(gameConfiguration);
            break;

        case MenuAction.Settings:
            await Settings(gameConfiguration);
            break;

        case MenuAction.Exit:
            return;

        default:
            throw new NotImplementedException(nameof(menuAction));
    }

    static async Task StartGame(GameConfiguration gameConfiguration)
    {
        AnsiConsole.Clear();

        var patternName = AnsiConsoleExtensions.PromptPattern();

        // Move to settings?
        gameConfiguration = gameConfiguration with { PatternName = patternName };

        await Game.Play(gameConfiguration);
    }

    static async Task Settings(GameConfiguration gameConfiguration)
    {
        var settingsAction = AnsiConsoleExtensions.PromptSettingsAction();

        Console.WriteLine();

        switch (settingsAction)
        {
            case SettingsAction.SetGameSpeed:

                var speedInMs = AnsiConsoleExtensions.PromptSpeedInMs(gameConfiguration.SpeedInMs);

                AnsiConsole.MarkupLine($"[green]Game speed set to {speedInMs}ms[/]");
                AnsiConsole.MarkupLine("[gray]Press any key to return to the menu...[/]");

                Console.ReadKey(true);
                AnsiConsole.Clear();
                await Run(gameConfiguration with { SpeedInMs = speedInMs });

                break;

            case SettingsAction.SetGridSize:

                var gridSize = AnsiConsoleExtensions.PromptGridSize(gameConfiguration.GridSize);
                AnsiConsole.MarkupLine($"[green]Game grid size set to {gridSize}X{gridSize}[/]");
                AnsiConsole.MarkupLine("[gray]Press any key to return to the menu...[/]");
                Console.ReadKey(true);
                AnsiConsole.Clear();
                await Run(gameConfiguration with { GridSize = gridSize });
                break;

            default:
                throw new NotImplementedException(nameof(settingsAction));
        }
    }
}