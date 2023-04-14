namespace ConsoleTetris.State;

public class GameState {
    private readonly static GameState _instance = new GameState();
    public bool Exit { get; private set; }
    public bool GameOver { get; private set; }
    public bool GamePaused { get; private set; }
    public bool GameRestarted { get; private set; }
    public int Level { get; private set; }
    public int Score { get; private set; }
    public double Time { get; set; }

    private GameState() {
        Init();
    }

    public static GameState GetInstance() => _instance;

    public void Init() {
        Level = 1;
        Score = 0;
        Time = 0;
        GameOver = false;
        GamePaused = false;
        GameRestarted = false;
        Exit = false;
    }

    public void EndGame() => GameOver = true;

    public void PauseGame() => GamePaused = true;

    public void ResumeGame() => GamePaused = false;
    
    public void ExitGame () => Exit = true;

    public void RestartGame() {
        GameRestarted = true;
        GameOver = false;
    }

    public void UpdateLevel() {
        var newLevel = Score switch {
            >= 900_000 => 10,
            >= 720_000 => 9,
            >= 560_000 => 8,
            >= 420_000 => 7,
            >= 300_000 => 6,
            >= 200_000 => 5,
            >= 120_000 => 4,
            >= 60_000 => 3,
            >= 20_000 => 2,
            _ => 1
        };

        Level = newLevel;
    }

    public void IncreaseScore() {
        var increment = Level switch {
            1 => 1000,
            2 => 2000,
            3 => 3000,
            4 => 4000,
            5 => 5000,
            6 => 6000,
            7 => 7000,
            8 => 8000,
            9 => 9000,
            10 => 10000,
            _ => 0
        };

        Score += increment;
    }
}
