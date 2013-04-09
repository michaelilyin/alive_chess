using System;
using System.Net;
using System.Net.Sockets;

namespace AliveChessLibrary.Interfaces
{
    /// <summary>
    /// сетевое соединение
    /// </summary>
    public interface IConnectionInfo : IEquatable<IPEndPoint>
    {
        Socket Socket { get; set; }
    }
}
