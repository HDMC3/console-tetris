using ConsoleTetris.GameObjects.Board;

namespace ConsoleTetris.GameObjects.Pieces;

public class IPiece : Piece {
    
    public IPiece(int row, int col) : base(row, col) {
        Shape = GetShape(0);
    }

    /*
        X: Casilla de referencia (row y col)
        @: Casilla de pieza (puede superponerse con X)

        # # # #      # # # #   # @ # #   # # # #   # # @ #
        X # # #  =>  X @ @ @   X @ # #   X # # #   X # @ #
        # # # #      # # # #   # @ # #   @ @ @ @   # # @ #
        # # # #      # # # #   # @ # #   # # # #   # # @ #
        
                        0°       90°       180°      270°
    */

    protected override List<BoardBox> GetShape(int angle) {
        return angle switch 
        {
            0 => new List<BoardBox> {
                new BoardBox(Row, Col, 1),
                new BoardBox(Row, Col + 1, 1),
                new BoardBox(Row, Col + 2, 1),
                new BoardBox(Row, Col + 3, 1)
            },
            90 => new List<BoardBox> {
                new BoardBox(Row - 1, Col + 1, 1),
                new BoardBox(Row    , Col + 1, 1),
                new BoardBox(Row + 1, Col + 1, 1),
                new BoardBox(Row + 2, Col + 1, 1)
            },
            180 => new List<BoardBox> {
                new BoardBox(Row + 1, Col    , 1),
                new BoardBox(Row + 1, Col + 1, 1),
                new BoardBox(Row + 1, Col + 2, 1),
                new BoardBox(Row + 1, Col + 3, 1)
            },
            270 => new List<BoardBox> {
                new BoardBox(Row - 1, Col + 2, 1),
                new BoardBox(Row    , Col + 2, 1),
                new BoardBox(Row + 1, Col + 2, 1),
                new BoardBox(Row + 2, Col + 2, 1)
            },
            _ => throw new Exception("Invalid angle")
        };
    }
}
