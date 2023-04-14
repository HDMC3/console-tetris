namespace ConsoleTetris.Events;

public class Keyboard {

    private readonly static Keyboard _instance = new Keyboard();
    public event EventHandler<KeyPressEventArgs> KeyPressEvent;
    private Keyboard() { }

    public static Keyboard GetInstance() => _instance;

    public void KeyPress(ConsoleKeyInfo key) {
        var args = new KeyPressEventArgs();
        args.KeyInfo = key;
        OnKeyPress(args);
    }
    protected virtual void OnKeyPress(KeyPressEventArgs e) {
        EventHandler<KeyPressEventArgs> handler = KeyPressEvent;
        if (handler != null) {
            handler(this, e);
        }
    }
}
