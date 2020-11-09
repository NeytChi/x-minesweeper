using xMinesweeper.Data;

namespace xMinesweeper.GameLogic
{
    public class GameControl
    {
        public GamePanelFlow PanelFlow { get; set; }
        public GamePanel Panel { get; set; }
        
        public GameControl()
        {
            PanelFlow = new GamePanelFlow();
            Panel = PanelFlow.Create();
        }
        public void Reset()
        {
            Panel = PanelFlow.Create();
        }
        public void SetFlag(int x, int y)
        {
            var piece = Panel.Pieces[x, y];

            if (!piece.IsOpen && !piece.IsFlag)
            {
                --Panel.FlagCountEnable;
                --Panel.CheckedPieces;
                piece.IsFlag = true;
            }
        }
        public bool TryOpenPiece(int x, int y)
        {
            var piece = Panel.Pieces[x, y];

            if (PanelFlow.IsPieceBomb(piece) || PanelFlow.IsPieceFlag(piece))
            {
                HandleMissedPiece(x, y);
                return false;
            }

            PanelFlow.OpenAvailablePieces(ref Panel.Pieces, x, y);
            Panel.CheckedPieces = PanelFlow.CountCheckedPieces(Panel.Pieces);
            return true;
        }
        public bool HandleMissedPiece(int x, int y)
        {
            var piece = Panel.Pieces[x, y];
            
            if (PanelFlow.IsPieceFlag(piece))
                return false;

            if (PanelFlow.IsPieceBomb(piece))
            {
                Panel.State = PanelState.Lose;
                PanelFlow.OpenBombs(ref Panel.Pieces);
                return true;
            }

            return false;
        }
        public bool IsWinGame()
        {
            if (Panel.CheckedPieces == 0)
            {
                Panel.State = PanelState.Win;
                return true;                
            }

            return false;
        }
        public bool IsLoseGame()
        {
            return Panel.State == PanelState.Lose;
        }
    }
}