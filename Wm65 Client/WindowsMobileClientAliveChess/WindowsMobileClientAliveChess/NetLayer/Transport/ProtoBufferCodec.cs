using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using ProtoBuf;

namespace WindowsMobileClientAliveChess.NetLayer.Transport
{
    public class ProtoBufferCodec
    {
        private static Assembly assembly;
        private static MethodInfo method;

        static ProtoBufferCodec()
        {
            assembly = Assembly.Load("AliveChessLibrary");
            method = typeof(ProtoBufferCodec).GetMethod("DecodeConcreteCommand",
                BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static byte[] Encode<T>(T command) where T : ICommand
        {
            MemoryStream s1 = new MemoryStream();
            MemoryStream s2 = new MemoryStream();
            ProtoBuf.Serializer.Serialize<T>(s1, command);
            s2.Write(BitConverter.GetBytes((int)command.Id), 0, 4);
            s2.Write(BitConverter.GetBytes(s1.ToArray().Length), 0, 4);
            s2.Write(s1.ToArray(), 0, s1.ToArray().Length);

            byte[] data = s2.ToArray();
            s1.Close();
            s2.Close();
            return data;
        }

        public static ICommand Decode(BytePackage package)
        {
            MemoryStream stream = new MemoryStream(package.CommandBody);

            MethodInfo mg = method.MakeGenericMethod(assembly.GetType(
                String.Concat("AliveChessLibrary.Commands.", package.CommandType,
                              ".", package.CommandName)));
            try
            {
                return (ICommand)mg.Invoke(null, new object[] { stream });
            }
            catch (ProtoException ex)
            {
                Debug.Fail(ex.Message+ex.StackTrace);
                return null;
            }
        }

        private static T DecodeConcreteCommand<T>(MemoryStream stream) where T : ICommand
        {
            T cmd = ProtoBuf.Serializer.Deserialize<T>(stream);
            stream.Close();
            return cmd;
        }
    }
}
