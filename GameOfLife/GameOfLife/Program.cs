using GameOfLife;
using Spectre.Console;

ConsoleInteropService.Configure(ConsoleInteropServiceConfiguration.Default);

var xs = GetBunnies();

DrawWithCanvas(xs);

await Task.Delay(100);

for (int i = 0; i < 100; i++)
{
    Console.Clear();

    var nextGrid = ResolveNextGrid(xs);

    DrawWithCanvas(nextGrid);
    xs = nextGrid;

    await Task.Delay(100);
}

Console.ReadLine();

static int[][] ResolveNextGrid(int[][] xs)
{
    const int DEAD = 0;
    const int ALIVE = 1;

    var ys = Create2DArray(xs.Length, xs[0].Length);

    for (int i = 0; i < xs.Length; i++)
    {
        for (int j = 0; j < xs[i].Length; j++)
        {
            var state = xs[i][j];

            var neighbors = CountNeighbors(xs, i, j);

            if (state is DEAD && neighbors is 0)
            {
                ys[i][j] = DEAD; // Stay dead
            }
            else if (state is ALIVE && neighbors <= 1)
            {
                ys[i][j] = DEAD; // Solitude
            }
            else if (state is ALIVE && neighbors >= 4)
            {
                ys[i][j] = DEAD; // Overpopulation
            }
            else if (state is DEAD && neighbors is 3)
            {
                ys[i][j] = ALIVE; // Reproduction
            }
            else
            {
                ys[i][j] = state; // Stay alive
            }
        }
    }

    return ys;
}

static int CountNeighbors(int[][] xs, int row, int col)
{
    int sum = 0;

    for (int i = row - 1; i < row + 2; i++)
    {
        for (int j = col - 1; j < col + 2; j++)
        {
            // Skip the center cell
            if (i == row && j == col)
                continue;

            var r = (i + xs.Length) % xs.Length; // Wrap around rows
            var c = (j + xs[0].Length) % xs[0].Length; // Wrap around columns

            sum += xs[r][c];
        }
    }

    return sum;

    /*
    sum += xs[i - 1][j - 1];
    sum += xs[i - 1][j];
    sum += xs[i - 1][j + 1];

    sum += xs[i][j - 1];
    sum += xs[i][j + 1];

    sum += xs[i + 1][j - 1];
    sum += xs[i + 1][j];
    sum += xs[i + 1][j + 1];
    */

    /*
    [-1][-1], [-1][0], [-1][1],
    [0][-1], [0][0], [0][1],
    [1][-1], [1][0], [1][1],
     */
}

static int[][] Create2DArray(int rows, int cols)
{
    int[][] xs = new int[rows][];

    for (int i = 0; i < rows; i++)
    {
        xs[i] = new int[cols];
    }

    return xs;
}

static void FillArrayRandomly(int[][] xs)
{
    var random = new Random();

    for (int i = 0; i < xs.Length; i++)
    {
        for (int j = 0; j < xs[i].Length; j++)
        {
            xs[i][j] = random.Next(0, 2);
        }
    }
}

static void Draw(int[][] xs)
{
    for (int i = 0; i < xs.Length; i++)
    {
        for (int j = 0; j < xs[i].Length; j++)
        {
            Console.Write(xs[i][j] + " ");
        }

        Console.WriteLine();
    }
}

static void DrawWithCanvas(int[][] xs)
{
    var canvas = new Canvas(xs.Length, xs[0].Length);

    for (int i = 0; i < xs.Length; i++)
    {
        for (int j = 0; j < xs[i].Length; j++)
        {
            var color = ResolveColor(xs[i][j]);

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

// https://playgameoflife.com/lexicon/bunnies
static int[][] GetBunnies() =>
    [
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,1,0,0,0,0,0,1,0,0,0,0,0],
        [0,0,0,0,0,0,1,0,0,0,1,0,0,0,0,0],
        [0,0,0,0,0,0,1,0,0,1,0,1,0,0,0,0],
        [0,0,0,0,0,1,0,1,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]
    ];