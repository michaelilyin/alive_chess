using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Assets.GameLogic.Network
{
    public static class ProtoBufferCodec
    {
        private static Assembly _assembly;
        private static MethodInfo _method;

        static ProtoBufferCodec()
        {
            _assembly = Assembly.Load("AliveChessLibrary");
            _method = typeof(ProtoBufferCodec).GetMethod("DecodeConcreteCommand", BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static byte[] Encode<T>(T command) where T : ICommand
        {
            MemoryStream s1 = new MemoryStream();
            MemoryStream s2 = new MemoryStream();
            Serializer.Serialize(s1, command);
            s2.Write(BitConverter.GetBytes((int)command.Id), 0, 4);
            s2.Write(BitConverter.GetBytes(s1.ToArray().Length), 0, 4);
            s2.Write(s1.ToArray(), 0, s1.ToArray().Length);
            byte[] data = s2.ToArray();
            s1.Close();
            s2.Close();
            return data;
        }

        public static byte[] EncodeNonSerialized<T>(T command) where T : INonSerializable
        {
            MemoryStream s2 = new MemoryStream();
            s2.Write(BitConverter.GetBytes((int)command.Id), 0, 4);
            s2.Write(BitConverter.GetBytes(command.ToBytes().Length), 0, 4);
            s2.Write(command.ToBytes(), 0, command.ToBytes().Length);

            byte[] data = s2.ToArray();
            s2.Close();
            return data;
        }

        public static ICommand Decode(BytePackage package)
        {
            MemoryStream stream = new MemoryStream(package.CommandBody);
            MethodInfo mg = _method.MakeGenericMethod(_assembly.GetType(
                String.Concat("AliveChessLibrary.Commands.", package.CommandType, ".", package.CommandName)));
            try
            {
                return (ICommand)mg.Invoke(null, new object[] { stream });
            }
            catch (ProtoException)
            {
                return null;
            }
        }

        private static T DecodeConcreteCommand<T>(MemoryStream stream) where T : ICommand
        {
            T cmd = Serializer.Deserialize<T>(stream);
            stream.Close();
            return cmd;
        }
    }
}
