using ConsoleTetris.Events;
using ConsoleTetris.GameObjects.Pieces;
using ConsoleTetris.Helpers;
using ConsoleTetris.State;

namespace ConsoleTetris.GameObjects.Board;

public class GameBoard {
    public static List<List<BoardBox>> Board = new List<List<BoardBox>>();
    private Keyboard _keyboard = Keyboard.GetInstance();
    private GameState _state = GameState.GetInstance();
    private List<ConsoleKey> _pressedKeys = new List<ConsoleKey>();
    private ConsoleKey? _pressedKey = ConsoleKey.Enter;
    private Piece _currentPiece;
    private Piece _nextPiece;
    private bool _createNewPiece = false;
    private double _time = 0;
    private List<List<int>> _rowsToKep = new List<List<int>>();
    private bool _existCompletedRows = false;
    private int _dropTime = 800;

    public GameBoard() {
        _keyboard.KeyPressEvent += KeyPressHandler;
        _nextPiece = GetRandomPiece();
        _currentPiece = GetRandomPiece();

        for (int row = 0; row < 22; row++) {
            Board.Add(new List<BoardBox>());
            for (int col = 0; col < 10; col++) {
                Board[row].Add(new BoardBox(row, col, 0));
            }
        }
    }

    public void Init() {
        _currentPiece = GetRandomPiece();
        _nextPiece = GetRandomPiece();
        _createNewPiece = false;
        _pressedKey = null;
        _time = 0;
        _rowsToKep.Clear();
        _existCompletedRows = false;
        _dropTime = 800;
        ClearBoard();
    }

    public void Update(double delta) {
        _time += delta;

        if (_existCompletedRows) {
            DeleteCompletedRows();    
            SetDropTimeByLevel();      
        }

        if (_createNewPiece) {
            _currentPiece = _nextPiece;
            _nextPiece = GetRandomPiece();
            _createNewPiece = false;
        } 

        CheckPressedKeys();

        if (_time >= _dropTime) {
            _currentPiece.Drop();
            _time = 0;
        }

        if (_currentPiece.IsPlaced) {
            if (_currentPiece.Shape.Any(box => box.Row < 2)) {
                _state.EndGame();
            }

            foreach (var box in _currentPiece.Shape) {
                Board[box.Row][box.Col].Value = box.Value;
            }
            _createNewPiece = true;

            SetRowsToKep();
        }        
    }

    public void Render() {
        PrintNextPiece();
        PrintBoard();
    }

    private void CheckPressedKeys() {
        if (_pressedKey == ConsoleKey.X || _pressedKey == ConsoleKey.I) {
            _currentPiece.Rotate(RotateDirection.TO_RIGTH);
        } else if (_pressedKey == ConsoleKey.Z) {
            _currentPiece.Rotate(RotateDirection.TO_LEFT);
        }

        if (_pressedKey == ConsoleKey.L || _pressedKey == ConsoleKey.RightArrow) {
            _currentPiece.MoveToRight();
        } else if (_pressedKey == ConsoleKey.J || _pressedKey == ConsoleKey.LeftArrow) {
            _currentPiece.MoveToLeft();
        } else if (_pressedKey == ConsoleKey.K || _pressedKey == ConsoleKey.DownArrow) {
            _currentPiece.Drop();
        }

        if (_pressedKey == ConsoleKey.Spacebar) {
            _currentPiece.PlaceToBottom();
        }

        _pressedKey = null;
    }

    private void PrintBoard() {
        for (int i = 0; i < Board.Count; i++) {
            var row = Board[i];
            if (i < 1) continue;

            foreach (var box in row)
            {
                Console.SetCursorPosition(Constants.BOARD_X + (box.Col * 2), Constants.BOARD_Y + box.Row );

                if(_currentPiece.Shape.Any(pieceBox => pieceBox.Col == box.Col && pieceBox.Row == box.Row)) {
                    Console.ForegroundColor = i < 2 ? ConsoleColor.White : GetColorByValue(_currentPiece.Shape[0].Value);
                    Console.Write("[]");
                    continue;
                } 

                if (box.IsEmpty) {
                    Console.ForegroundColor = GetColorByValue(0);
                    Console.Write(i > 1 ? " ." : "  ");
                } else {
                    Console.ForegroundColor = GetColorByValue(box.Value);
                    Console.Write("[]");
                    Console.ForegroundColor = GetColorByValue(0);
                }
            }
        }
    }

    private void PrintNextPiece() {
        var pieceValue = _nextPiece.Shape[0].Value; 
        var fixOffset = pieceValue switch {
            4 => 1,
            1 => -1,
            _ => 0
        };

        Console.SetCursorPosition(Constants.NEXT_PIECE_X - 1, Constants.NEXT_PIECE_Y);
        Console.Write("          ");
        Console.SetCursorPosition(Constants.NEXT_PIECE_X - 1, Constants.NEXT_PIECE_Y + 1);
        Console.Write("          ");

        foreach (var box in _nextPiece.Shape) {
            
            Console.SetCursorPosition(
                Constants.NEXT_PIECE_X + ((box.Col - _nextPiece.Col) * 2) + fixOffset, 
                Constants.NEXT_PIECE_Y + (box.Row - _nextPiece.Row)
            );
            Console.ForegroundColor = GetColorByValue(_nextPiece.Shape[0].Value);
            Console.Write("[]");
        }
    }

    private void ClearBoard() {
        foreach (var row in Board) {
            foreach (var box in row) {
                box.Value = 0;
            }
        }
    }

    private void SetRowsToKep() {
        _existCompletedRows = Board.Any(row => row.All(box => !box.IsEmpty));
        if (!_existCompletedRows) return;

        for (int i = 0; i < Board.Count; i++) {
            if (Board[i].Any(box => box.IsEmpty)) {
                _rowsToKep.Add(
                    Board[i].Select(box => box.Value).ToList()
                );
            }
        }
    }

    private void DeleteCompletedRows() {
        ClearBoard();
        int rowIdxRef = Board.Count - _rowsToKep.Count;
        foreach (var rowToKep in _rowsToKep) {
            for (int i = 0; i < rowToKep.Count; i++) {
                Board[rowIdxRef][i].Value = rowToKep[i];
            }
            rowIdxRef++;
        }

        var deletedRows = Board.Count - _rowsToKep.Count;
        for (int i = 0; i < deletedRows; i++) {
            _state.IncreaseScore();
        }

        _existCompletedRows = false;
        _rowsToKep.Clear();
    }

    private void KeyPressHandler(object sender, KeyPressEventArgs e) {
        if (_state.GameOver || _state.GamePaused) return;
        _pressedKey = e.KeyInfo.Key;
    }

    private ConsoleColor GetColorByValue(int value) {
        return value switch {
            1 => ConsoleColor.Cyan, // Pieza I
            2 => ConsoleColor.Blue, // Pieza J
            3 => ConsoleColor.Gray, // Pieza L
            4 => ConsoleColor.Yellow, // Peza O
            5 => ConsoleColor.Green, // Pieza S
            6 => ConsoleColor.Magenta, // Pieza T
            7 => ConsoleColor.Red, // Pieza Z
            _ => ConsoleColor.White
        };
    }

    private Piece GetRandomPiece() {
        var rnd = new Random();
        int pieceNumber = rnd.Next(minValue: 1, maxValue: 8);

        Piece newPiece = pieceNumber switch {
            1 => new IPiece(1, 3),
            2 => new JPiece(0, 4),
            3 => new LPiece(0, 4),
            4 => new OPiece(0, 4),
            5 => new SPiece(0, 4),
            6 => new TPiece(0, 4),
            7 => new ZPiece(0, 4),
            _ => throw new Exception("Invalid piece number")
        };

        return newPiece;
    }

    private void SetDropTimeByLevel() {
        var newDropTime = _state.Level switch {
            1 => 800,
            2 => 740,
            3 => 680,
            4 => 620,
            5 => 560,
            6 => 500,
            7 => 440,
            8 => 380,
            9 => 320,
            10 => 260,
            _ => throw new Exception("Nivel invalido")
        };

        _dropTime = newDropTime;
    }
}
