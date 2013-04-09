using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using AliveChessServer.LogicLayer;
using ProtoBuf;

namespace AliveChessServer.NetLayer
{
    public static class ProtoBufferCodec
    {
        private static Assembly assembly;
        private static MethodInfo method;

        private static Queue<ProtoException> _log = new Queue<ProtoException>();

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

            MethodInfo mg = method.MakeGenericMethod(assembly.GetType(
                String.Concat("AliveChessLibrary.Commands.", package.CommandType,
                              ".", package.CommandName)));
            try
            {
                ICommand command = (ICommand) mg.Invoke(null, new object[] {stream});
                return command;
            }
            catch (ProtoException ex)
            {
                AliveChessLogger.LogError(ex);
                return null;
            }
        }

        private static T DecodeConcreteCommand<T>(MemoryStream stream) where T : ICommand
        {
            T cmd = Serializer.Deserialize<T>(stream);
            stream.Close();
            return cmd;
        }

        public static Queue<ProtoException> ErrorLog { get { return _log; } }
    }
}
