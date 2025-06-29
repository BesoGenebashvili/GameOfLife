namespace GameOfLife;

public static class Pattern
{
    public enum Name : byte
    {
        Random,
        Bunnies,
        Glider,
        Blinkers,
        Toad,
        LWSS,
        Diamond,
        BulletHeptomino
    }

    public static int[][] Resolve(
        int gridSize,
        Name pattern)
    {
        var cleanGrid = Array.Create2D(gridSize, gridSize);

        return pattern switch
        {
            Name.Random => Random(cleanGrid),
            var p => ResolveCenteredPattern(p, cleanGrid),
        };

        static int[][] Random(int[][] cleanGrid)
        {
            cleanGrid.FillWithRandomBits();
            return cleanGrid;
        }

        static int[][] ResolveCenteredPattern(Name pattern, int[][] cleanGrid)
        {
            var patternData = pattern switch
            {
                Name.Bunnies => Bunnies,
                Name.Glider => Glider,
                Name.Blinkers => Blinkers,
                Name.Toad => Toad,
                Name.LWSS => LWSS,
                Name.Diamond => Diamond,
                Name.BulletHeptomino => BulletHeptomino,
                _ => throw new NotSupportedException("Pattern not supported")
            };

            return CenterPattern(cleanGrid, patternData);
        }
    }

    static int[][] CenterPattern(int[][] cleanGrid, int[][] pattern)
    {
        int rowOffset = (cleanGrid.Length - pattern.Length) / 2;
        int colOffset = (cleanGrid.Length - pattern[0].Length) / 2;

        for (int i = 0; i < pattern.Length; i++)
        {
            var source = pattern[i];
            var destination = cleanGrid[i + rowOffset];

            for (int j = 0; j < source.Length; j++)
            {
                destination[j + colOffset] = source[j];
            }
        }

        return cleanGrid;
    }


    /// <summary>
    /// This is a parent of rabbits and was found independently by Robert Wainwright and Andrew Trevorrow.
    /// https://playgameoflife.com/lexicon/bunnies
    /// </summary>
    private static int[][] Bunnies =>
    [
        [1,0,0,0,0,0,1,0],
        [0,0,1,0,0,0,1,0],
        [0,0,1,0,0,1,0,1],
        [0,1,0,1,0,0,0,0]
    ];

    private static int[][] Glider =>
    [
        [0,1,0],
        [0,0,1],
        [1,1,1]
    ];

    private static int[][] Blinkers =>
    [
        [0,1,1,1,0,0,0,1,1,1,0],
        [0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0],
        [0,0,0,0,0,0,0,0,0,0,0],
        [0,1,1,1,0,0,0,1,1,1,0]
    ];

    private static int[][] Toad =>
    [
        [0,1,1,1],
        [1,1,1,0]
    ];

    private static int[][] LWSS =>
    [
        [0,1,0,0,1],
        [1,0,0,0,0],
        [1,0,0,0,1],
        [1,1,1,1,0]
    ];

    private static int[][] Diamond =>
    [
        [0,1,0],
        [1,1,1],
        [1,0,1],
        [0,1,0],
    ];

    private static int[][] BulletHeptomino =>
    [
        [0,1,0],
        [1,1,1],
        [1,1,1],
    ];
}
