namespace ConsoleTetris.GameObjects.Board;

public class BoardBox {
    public int Col;
    public int Row;
    public int Value { get; set; }
    public bool IsEmpty {
        get {
           return Value == 0; 
        }
    }
    public BoardBox(int row, int col, int value = 0) {
        this.Col = col;
        this.Row = row;
        this.Value = value;
    }
}
