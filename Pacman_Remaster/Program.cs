using Pacman_Remaster;

internal class Program
{
    private static void Main(string[] args)
    {
        using var game = new Game();
        game.Run();
    }
}