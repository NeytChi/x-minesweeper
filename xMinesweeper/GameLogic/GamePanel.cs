using System.Timers;
using xMinesweeper.Data;

namespace xMinesweeper.GameLogic
{
    public class GamePanel
    {
        public readonly Timer Timer = new Timer();
        public PanelState State;
        public GamePiece[,] Pieces;
        public int FlagCountEnable = 0;
        public int CheckedPieces = 0;
        public GamePanel(GamePiece[,] pieces)
        {
            State = PanelState.Gaming;
            Pieces = pieces;
            FlagCountEnable = PanelSettings.BombsCount;
            CheckedPieces = PanelSettings.Height * PanelSettings.Width;
            Timer.Start();
        }
    }
}