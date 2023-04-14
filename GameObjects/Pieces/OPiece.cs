using ConsoleTetris.GameObjects.Board;

namespace ConsoleTetris.GameObjects.Pieces;

public class OPiece : Piece {
    public OPiece(int row, int col) : base(row, col) {
        Shape = GetShape(0);
    }

    protected override List<BoardBox> GetShape(int angle) {
        return new List<BoardBox>() {
            new BoardBox(Row    , Col    , 4),
            new BoardBox(Row    , Col + 1, 4),
            new BoardBox(Row + 1, Col    , 4),
            new BoardBox(Row + 1, Col + 1, 4)
        };
    }
}
