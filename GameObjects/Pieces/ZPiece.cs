using ConsoleTetris.GameObjects.Board;

namespace ConsoleTetris.GameObjects.Pieces;

public class ZPiece : Piece {
    
    public ZPiece(int row, int col) : base(row, col) {
        Shape = GetShape(0);
    }

    /*
        X: Casilla de referencia (row y col)
        @: Casilla de pieza (puede superponerse con X)

        X # #      X @ #   X @ #   X # #   X # @
        # # #  =>  # @ @   @ @ #   @ @ #   # @ @
        # # #      # # #   @ # #   # @ @   # @ #
        
                     0°     90°     180°    270°
    */

    protected override List<BoardBox> GetShape(int angle) {
        return angle switch {
            0 => new List<BoardBox> {
                new BoardBox(Row    , Col    , 7),
                new BoardBox(Row    , Col + 1, 7),
                new BoardBox(Row + 1, Col + 1, 7),
                new BoardBox(Row + 1, Col + 2, 7)
            },
            90 => new List<BoardBox> {
                new BoardBox(Row    , Col + 1, 7),
                new BoardBox(Row + 1, Col    , 7),
                new BoardBox(Row + 1, Col + 1, 7),
                new BoardBox(Row + 2, Col    , 7)
            },
            180 => new List<BoardBox> {
                new BoardBox(Row + 1, Col    , 7),
                new BoardBox(Row + 1, Col + 1, 7),
                new BoardBox(Row + 2, Col + 1, 7),
                new BoardBox(Row + 2, Col + 2, 7)
            },
            270 => new List<BoardBox> {
                new BoardBox(Row    , Col + 2, 7),
                new BoardBox(Row + 1, Col + 1, 7),
                new BoardBox(Row + 1, Col + 2, 7),
                new BoardBox(Row + 2, Col + 1, 7)
            },
            _ => throw new Exception("Invalid angle")
        };
    }
}
