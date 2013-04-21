using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AliveChess.GameLayer.LogicLayer
{
    public enum ExecutorType
    {
        AuthorizeResponse    = 1,
        BigMapResponse       = 7,
        CaptureMineResponce = 11,
        GetMapResponse       = 22,
        GetGameStateResponse = 32,
        GetKingResponse      = 38,
        MoveKingResponse     = 18,
        GetObjectsResponse   = 24,
        GetResourceMessage   = 28,
        ComeInCastleResponse = 13,
        GetListBuildingsInCastleResponse = 81
    }
}
