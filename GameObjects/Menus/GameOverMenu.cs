using ConsoleTetris.Events;
using ConsoleTetris.Helpers;
using ConsoleTetris.State;

namespace ConsoleTetris.GameObjects.Menus;

public class GameOverMenu { 
    private GameState State = GameState.GetInstance();
    private Keyboard keyboard = Keyboard.GetInstance();
    private int OptionPointer = 0;
    private bool Rendered = false;

    public GameOverMenu() {
        keyboard.KeyPressEvent += KeyPressHandler;
    }

    public void Render() {
        if (Rendered) return;
        
        Console.BackgroundColor = ConsoleColor.Black;
        var menu = new string[]{
            "┌────────────────────────┐",
            "│                        │",
            "│       GAME OVER!       │",
            "│                        │",
            "│        New Game        │",
            "│          Exit          │",
            "│                        │",
            "└────────────────────────┘"
        };
        for (int i = 0; i < menu.Length; i++) {
            Console.SetCursorPosition(Constants.GAMEOVER_MENU_X, Constants.GAMEOVER_MENU_Y + i);
            Console.Write(menu[i]);
        }

        SelectMenu();

        Rendered = true;
    }

    private void MoveOptionUp() {
        OptionPointer = (2 + OptionPointer - 1) % 2;
        SelectMenu();
    }

    private void MoveOptionDown() {
        OptionPointer = (OptionPointer + 1) % 2;
        SelectMenu();
    }

    private void ConfirmOption() {
        if (OptionPointer == 0) {
            State.RestartGame();
        }

        if (OptionPointer == 1) {
            State.ExitGame();
        }
        Rendered = false;
    }

    private void SelectMenu() {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.SetCursorPosition(Constants.GAMEOVER_MENU_X + 8, Constants.GAMEOVER_MENU_Y + 4);
        Console.Write(" New Game ");
        Console.SetCursorPosition(Constants.GAMEOVER_MENU_X + 8, Constants.GAMEOVER_MENU_Y + 5);
        Console.Write("   Exit   ");

        if (OptionPointer == 0) {
            Console.SetCursorPosition(Constants.GAMEOVER_MENU_X + 8, Constants.GAMEOVER_MENU_Y + 4);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(" New Game ");
        }

        if (OptionPointer == 1) {
            Console.SetCursorPosition(Constants.GAMEOVER_MENU_X + 8, Constants.GAMEOVER_MENU_Y + 5);
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("   Exit   ");
        }
    }

    private void KeyPressHandler(object sender, KeyPressEventArgs e) {
        if (State.GameOver && e.KeyInfo.Key == ConsoleKey.UpArrow) {
            MoveOptionUp();
        } else if(State.GameOver && e.KeyInfo.Key == ConsoleKey.DownArrow) {
            MoveOptionDown();
        }

        if (State.GameOver && e.KeyInfo.Key == ConsoleKey.Enter) {
            ConfirmOption();
        }
    }
}
