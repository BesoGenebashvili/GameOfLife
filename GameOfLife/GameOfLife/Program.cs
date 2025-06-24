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

    if (menuAction is MenuAction.Exit)
    {
        return;
    }

    if (menuAction is MenuAction.Settings)
    {
        var settingsAction = AnsiConsoleExtensions.PromptSettingsAction();

        // TODO: Implement


        Console.WriteLine();
        var speedInMs = AnsiConsoleExtensions.PromptSpeedInMs(gameConfiguration.SpeedInMs);
        AnsiConsole.MarkupLine($"[green]Speed set speed to {speedInMs}ms[/]");
        AnsiConsole.MarkupLine("[gray]Press any key to return to the menu[/]");

        Console.ReadKey(true);
        AnsiConsole.Clear();
        await Run(gameConfiguration with { SpeedInMs = speedInMs });
    }

    if (menuAction is MenuAction.StartGame)
    {
        AnsiConsole.Clear();

        var patternName = AnsiConsoleExtensions.PromptPattern();

        gameConfiguration = gameConfiguration with { PatternName = patternName };

        await Game.Play(gameConfiguration);
    }
}