namespace xMinesweeper.Data
{
    public class GamePiece
    {
        public PieceState State { get; set; }
        public bool IsOpen { get; set; }
        public bool IsFlag { get; set; }
        public int NearbyBombsCount { get; set; }
        public GamePiece(PieceState state)
        {
            State = state;
            IsOpen = false;
            IsFlag = false;
            NearbyBombsCount = 0;
        }
        public string GetPieceState()
        {
            if (IsFlag)
                return PieceState.Banner.ToString().ToLower();
            if (IsOpen)
            {
                switch (State)
                {
                    case PieceState.Empty:
                        return State.ToString().ToLower();
                    case PieceState.Nearby:
                        return $"{State.ToString().ToLower()}-{NearbyBombsCount}";
                    case PieceState.Bomb:
                        return State.ToString().ToLower();
                }
            }
            else
                return "closed";
            
            return "";
        }
    }
}
