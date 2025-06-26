using GameOfLife;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using AnsiConsoleExtensions = GameOfLife.AnsiConsoleExtensions;
// Centered text and game?

var configurationRoot = GetConfigurationRoot();
var consoleConfiguration = GetConsoleConfiguration(configurationRoot);
ConsoleInteropService.Configure(consoleConfiguration);

await Run(GameConfiguration.Default);

static async Task Run(GameConfiguration configuration)
{
    AnsiConsoleExtensions.DrawMenu();

    var menuAction = AnsiConsoleExtensions.PromptMenuAction();

    switch (menuAction)
    {
        case MenuAction.StartGame:
            await StartGame(configuration);
            break;

        case MenuAction.Settings:
            await Settings(configuration);
            break;

        case MenuAction.Exit:
            return;

        default:
            throw new NotImplementedException(nameof(menuAction));
    }

    static async Task StartGame(GameConfiguration configuration)
    {
        AnsiConsole.Clear();

        var patternName = AnsiConsoleExtensions.PromptPattern();

        // Move to settings?
        configuration = configuration with { PatternName = patternName };

        await Game.Play(configuration);
    }

    static async Task Settings(GameConfiguration configuration)
    {
        var settingsAction = AnsiConsoleExtensions.PromptSettingsAction();

        Console.WriteLine();

        switch (settingsAction)
        {
            case SettingsAction.SetGameSpeed:

                var newSpeedInMs = AnsiConsoleExtensions.PromptSpeedInMs(
                    configuration.SpeedInMs, 
                    GameConfiguration.Default.SpeedInMs);

                AnsiConsole.MarkupLine($"[green]Game speed set to {newSpeedInMs}ms[/]");
                AnsiConsole.MarkupLine("[gray]Press any key to return to the menu...[/]");

                Console.ReadKey(true);
                AnsiConsole.Clear();
                await Run(configuration with { SpeedInMs = newSpeedInMs });

                break;

            case SettingsAction.SetGridSize:

                var newGridSize = AnsiConsoleExtensions.PromptGridSize(
                    configuration.GridSize,
                    GameConfiguration.Default.GridSize);

                AnsiConsole.MarkupLine($"[green]Game grid size set to {newGridSize}X{newGridSize}[/]");
                AnsiConsole.MarkupLine("[gray]Press any key to return to the menu...[/]");

                Console.ReadKey(true);
                AnsiConsole.Clear();
                await Run(configuration with { GridSize = newGridSize });
                break;

            default:
                throw new NotImplementedException(nameof(settingsAction));
        }
    }
}

static IConfigurationRoot GetConfigurationRoot() =>
    new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

static ConsoleConfiguration GetConsoleConfiguration(IConfigurationRoot configurationRoot) =>
    configurationRoot.GetSection(ConsoleConfiguration.SectionName)
                     .Get<ConsoleConfiguration>()
                     ?? ConsoleConfiguration.Default;
