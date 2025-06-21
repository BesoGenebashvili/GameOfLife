using GameOfLife;

ConsoleInteropService.Configure(ConsoleInteropServiceConfiguration.Default);

var patternName = AnsiConsoleExtensions.PromptPattern();
var speedInMs = 200;

await Game.Play(speedInMs, patternName);
