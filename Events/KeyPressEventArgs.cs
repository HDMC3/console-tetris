namespace ConsoleTetris.Events;
public class KeyPressEventArgs : EventArgs {
    public ConsoleKeyInfo KeyInfo { get; set; }
}
