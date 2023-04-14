using ConsoleTetris.Events;
using ConsoleTetris.Helpers;
using ConsoleTetris.State;

namespace ConsoleTetris.GameObjects.Menus;

public class GamePausedMenu {

    private GameState State;
    private Keyboard Keyboard;
    private ConsoleKey? PressedKey;
    private string[] MenuLayout;
    private bool Rendered = false;
    
    public GamePausedMenu() {
        State = GameState.GetInstance();
        Keyboard = Keyboard.GetInstance();
        Keyboard.KeyPressEvent += KeyPressHandler;
        MenuLayout = new string[]{
            "┌────────────────────────┐",
            "│                        │",
            "│         PAUSE          │",
            "│                        │",
            "│   ENTER to continue    │",
            "│                        │",
            "└────────────────────────┘"
        };
    }

    public void Update() {
        if (State.GamePaused && PressedKey == ConsoleKey.Enter) {
            ConfirmOption();
        }
    }

    public void Render() {
        if (Rendered) return;

        Console.BackgroundColor = ConsoleColor.Black;
        for (int i = 0; i < MenuLayout.Length; i++) {
            Console.SetCursorPosition(Constants.GAMEPAUSED_MENU_X, Constants.GAMEPAUSED_MENU_Y + i);
            Console.Write(MenuLayout[i]);
        }

        Rendered = true;
    }

    private void ConfirmOption() {
        State.ResumeGame();
        for (int i = 0; i < MenuLayout.Length; i++) {
            Console.SetCursorPosition(0, Constants.GAMEPAUSED_MENU_Y + i);
            var line = Constants.MAIN_LAYOUT[Constants.GAMEPAUSED_MENU_Y + i];
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(line);
        }
        Rendered = false;
    }

    private void KeyPressHandler(object sender, KeyPressEventArgs e) {
        PressedKey = e.KeyInfo.Key;
    }
}
