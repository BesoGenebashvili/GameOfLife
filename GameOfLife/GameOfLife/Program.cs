using GameOfLife;
using Spectre.Console;
using AnsiConsoleExtensions = GameOfLife.AnsiConsoleExtensions;

ConsoleInteropService.Configure(ConsoleInteropServiceConfiguration.Default);

AnsiConsoleExtensions.DrawMenu();

var menuAction = AnsiConsoleExtensions.PromptMenuAction();

if (menuAction is MenuAction.Exit)
{
    return;
}

if (menuAction is MenuAction.StartGame)
{
    AnsiConsole.Clear();

    var patternName = AnsiConsoleExtensions.PromptPattern();

    var speedInMs = 200;

    await Game.Play(speedInMs, patternName);
}