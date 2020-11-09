using System;
using System.Collections.Generic;
using System.IO;
using xMinesweeper.Data;

namespace xMinesweeper.GameLogic
{
    public class GamePanelFlow
    {
        public GamePanel Create()
        {
            var pieces = new GamePiece[PanelSettings.Width, PanelSettings.Height];
            for (int i = 0; i < PanelSettings.Width; i++)
            {
                for (int j = 0; j < PanelSettings.Height; j++)
                {
                    pieces[i, j] = new GamePiece(PieceState.Empty);
                }    
            }
            CreateBombs(ref pieces);
            SetupNearbyPieces(ref pieces);
            return new GamePanel(pieces);
        }
        private void CreateBombs(ref GamePiece[,] pieces)
        {
            var rand = new Random();

            for (int i = 0; i < PanelSettings.BombsCount; i++)
            {
                int indexX = rand.Next(0, PanelSettings.Width - 1);
                int indexY = rand.Next(0, PanelSettings.Height - 1);
                pieces[indexX, indexY].State = PieceState.Bomb;
            }
        }
        private void SetupNearbyPieces(ref GamePiece[,] pieces)
        {
            for (int i = 0; i < PanelSettings.Width; i++)
            {
                for (int j = 0; j < PanelSettings.Height; j++)
                {
                    if (pieces[i, j].State != PieceState.Bomb)
                    {
                        pieces[i, j].NearbyBombsCount = NearbyBombsCount(pieces, i, j);
                        if (pieces[i, j].NearbyBombsCount > 0)
                            pieces[i, j].State = PieceState.Nearby;
                    }
                }    
            }
        }

        private int NearbyBombsCount(GamePiece[,] pieces, int x, int y)
        {
            int bombsCount = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (IsNextStepExist(x + i, y + j))
                    {
                        if (pieces[x + i, y + j].State == PieceState.Bomb)
                            ++bombsCount;   
                    }                            
                }    
            }

            return bombsCount;
        }
        public void OpenAvailablePieces(ref GamePiece[,] pieces, int x, int y)
        {
            var piece = pieces[x, y];
            
            if (IsPieceFlag(piece) || IsPieceBomb(piece))
                return;
            
            pieces[x, y].IsOpen = true;
            if (IsPieceNearby(piece))
                return;
            
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (IsNextStepExist(x + i, y + j) && !pieces[x + i, y + j].IsOpen)
                        OpenAvailablePieces(ref pieces, x + i, y + j);
                }
            }
        }
        private bool IsNextStepExist(int x, int y)
        {
            if (!(x >= 0 && x < PanelSettings.Width))
                return false; 
            
            return y >= 0 && y < PanelSettings.Height;
        }
        public bool IsPieceBomb(GamePiece piece)
        {
            return piece.State == PieceState.Bomb;
        }
        public bool IsPieceFlag(GamePiece piece)
        {
            return piece.IsFlag;
        }
        private bool IsPieceNearby(GamePiece piece)
        {
            return piece.State == PieceState.Nearby;
        }

        public void OpenBombs(ref GamePiece[,] pieces)
        {
            for (int i = 0; i < PanelSettings.Width; i++)
            {
                for (int j = 0; j < PanelSettings.Height; j++)
                {
                    if (pieces[i, j].State == PieceState.Bomb)
                    {
                        pieces[i, j].IsOpen = true;
                    }
                }    
            }
        }
        public int CountCheckedPieces(GamePiece[,] pieces)
        {
            var checkedCount = PanelSettings.Width * PanelSettings.Height;
            for (int i = 0; i < PanelSettings.Width; i++)
            {
                for (int j = 0; j < PanelSettings.Height; j++)
                {
                    if (pieces[i, j].IsOpen || pieces[i, j].IsFlag)
                    {
                        checkedCount--;
                    }
                }
            }

            return checkedCount;

        }
    }
}