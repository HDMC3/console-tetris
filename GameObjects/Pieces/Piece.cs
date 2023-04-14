using ConsoleTetris.GameObjects.Board;
using ConsoleTetris.Helpers;

public class Piece {
    public List<BoardBox> Shape;
    public int Col { get; set; }
    public int Row { get; set; }
    public bool IsPlaced { get; set; }
    protected int Angle { get; set; }

    public Piece(int row, int col) {
        this.Col = col;
        this.Row = row;
        IsPlaced = false;
        Angle = 0;
        Shape = new List<BoardBox>();
    }
    public virtual void Update() { }
    public virtual void Render() { }
    public virtual void Drop() {
        if (IsPlaced) return;

        if (Shape.Any(box => box.Row == GameBoard.Board.Count - 1)) {
            IsPlaced = true;
            return;
        }

        if (Shape.Any(box => !GameBoard.Board[box.Row + 1][box.Col].IsEmpty)) {
            IsPlaced = true;
            return;
        }

        UpdatePosition(rowIncrement: 1);
    }
    public virtual void Rotate(RotateDirection direction) {
        var newAngle = Angle;
        if (direction == RotateDirection.TO_LEFT) {
            newAngle = (Angle + 90) % 360;
        } 

        if (direction == RotateDirection.TO_RIGTH) {
            newAngle = (360 + Angle - 90) % 360;
        }

        var newShape = GetShape(newAngle);

        if (newShape.Any(box => box.Col < 0 || box.Col > 9 || box.Row > 21 || box.Row < 0)) return;
        if (newShape.Any(box => !GameBoard.Board[box.Row][box.Col].IsEmpty)) return;

        Angle = newAngle;
        Shape = newShape;
    }
    public virtual void MoveToLeft() {
        if (IsPlaced) return;
        if (Shape.Any(box => box.Col == 0)) return;
        if (Shape.Any(box => !GameBoard.Board[box.Row][box.Col - 1].IsEmpty)) return;

        UpdatePosition(colIncrement: -1);
    }
    public virtual void MoveToRight() {
        if (IsPlaced) return;
        if (Shape.Any(box => box.Col == GameBoard.Board[0].Count - 1)) return;
        if (Shape.Any(box => !GameBoard.Board[box.Row][box.Col + 1].IsEmpty)) return;
        
        UpdatePosition(colIncrement: 1);
    }
    public virtual void MoveToDown() { }

    public virtual void PlaceToBottom() {
        if (IsPlaced) return;

        int rowIncrement= 0;
        var locationFound = false;
        
        while(!locationFound) {
            rowIncrement++;
            locationFound = Shape.Any(box => {
                return box.Row + rowIncrement == 22 || !GameBoard.Board[box.Row + rowIncrement][box.Col].IsEmpty;
            });
        }

        UpdatePosition(rowIncrement - 1);
    }

    protected virtual void UpdatePosition(int rowIncrement = 0, int colIncrement = 0) {
        Row += rowIncrement;
        Col += colIncrement;

        foreach (var box in Shape) {
            box.Row += rowIncrement;
            box.Col += colIncrement;
        }
    }

    protected virtual List<BoardBox> GetShape(int angle) => new List<BoardBox>();
}
