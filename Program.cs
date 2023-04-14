namespace ConsoleTetris;
public class Program
{
    static void Main(string[] args)
    {
        Console.Title = "C#nsole TETRIS";
        Console.Clear();
        var game = new Game();
        game.Start();
        Console.ResetColor();
        Console.CursorVisible = true;
        Console.Clear();   
    }  
}
