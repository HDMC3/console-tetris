using ConsoleTetris.Events;
using ConsoleTetris.GameObjects.Board;
using ConsoleTetris.GameObjects.Menus;
using ConsoleTetris.Helpers;
using ConsoleTetris.State;
using System.Diagnostics;

namespace ConsoleTetris;

public class Game {

    private Keyboard _keyboard = Keyboard.GetInstance();
    private Thread _keyboardThread = new Thread(new ThreadStart(KeyboardThreadHandler));
    private ConsoleKey? _pressedKey;
    private GameState _state = GameState.GetInstance();
    private GameBoard _gameBoard;
    private GameOverMenu _gameOverMenu;
    private GamePausedMenu _gamePausedMenu;

    public Game() {
        Console.SetWindowSize(1, 1);
        Console.SetBufferSize(60, 26);
        Console.SetWindowSize(61, 26);
        _gameBoard = new GameBoard();
        _gameOverMenu = new GameOverMenu();
        _gamePausedMenu = new GamePausedMenu();
        _keyboard.KeyPressEvent += KeyboardHandler;
        Console.CancelKeyPress += new ConsoleCancelEventHandler(ConsoleCancelHandler);
    }

    public void Start() {
        _keyboardThread.IsBackground = true;
        _keyboardThread.Start();

        Init();
        
        double lastTime = Stopwatch.GetTimestamp();
        double delta = 0;
        double timeToRender = 0;

        while(!_state.Exit) {
            timeToRender += delta;
            Update(delta);
            
            if (timeToRender >= 33.33) {
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;
                timeToRender = 0;
                Render();
            }

            double currentTime = Stopwatch.GetTimestamp();
            delta = (currentTime - lastTime) / TimeSpan.TicksPerMillisecond;
            lastTime = currentTime;
        }
    }

    private void Init() {
        Console.SetCursorPosition(0, 0);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Write(string.Join('\n', Constants.MAIN_LAYOUT));
    }

    public void Update(double delta) {
        CheckPressedKey();

        if (_state.GameOver) return;

        if (_state.GamePaused) {
            _gamePausedMenu.Update();
            return;
        }

        if (_state.GameRestarted) {
            _state.Init();
            Init();
            _gameBoard.Init();
            return;
        }
        
        _state.Time += delta;
        _state.UpdateLevel();
        _gameBoard.Update(delta);
    }

    public void Render() {
        if (_state.GameOver) {
            _gameOverMenu.Render();
            return;
        }

        if (_state.GamePaused) {
            _gamePausedMenu.Render();
            return;
        };

        PrintTime();
        PrintLevel();
        PrintScore();

        _gameBoard.Render();
    }

    private void PrintTime() {
        Console.SetCursorPosition(6, 2);
        var minutes = (int)Math.Floor(_state.Time / 60000);
        var seconds = (int)Math.Floor((_state.Time % 60000) / 1000);
        var minutesStr = minutes < 10 ? $"0{minutes}" : minutes.ToString();
        var secondsStr = seconds < 10 ? $"0{seconds}" : seconds.ToString();
        Console.Write($"{minutesStr}:{secondsStr}");
    }

    private void PrintLevel() {
        Console.SetCursorPosition(7, 6);
        Console.Write(_state.Level < 10 ? "0" + _state.Level : _state.Level);
    }

    private void PrintScore() {
        Console.SetCursorPosition(48, 2);
        var scoreLength = _state.Score.ToString().Length;
        var scoreStr = "";
        for (int i = 0; i < 7 - scoreLength; i++) {
            scoreStr += "0";
        }
        Console.Write(scoreStr + _state.Score);
    }

    private void CheckPressedKey() {
        if (_pressedKey == ConsoleKey.P) {
            _state.PauseGame();
        }        

        if (!_state.GamePaused && !_state.GameOver && _pressedKey == ConsoleKey.R) {
            _state.RestartGame();
        }

        if (!_state.GamePaused && !_state.GameOver && _pressedKey == ConsoleKey.E) {
            _state.ExitGame();
        }

        _pressedKey = null;
    }

    private void KeyboardHandler(object sender, KeyPressEventArgs e) {
        _pressedKey = e.KeyInfo.Key;
    }

    private static void ConsoleCancelHandler(object sender, ConsoleCancelEventArgs args) {
        var state = GameState.GetInstance();
        args.Cancel = true;
        state.ExitGame();
    }

    private static void KeyboardThreadHandler() {
        var state = GameState.GetInstance();
        var keyboard = Keyboard.GetInstance();
        while(!state.Exit) {
            var k = Console.ReadKey(true);
            keyboard.KeyPress(k);
        }
    }
}
