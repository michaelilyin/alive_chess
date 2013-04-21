using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using ProtoBuf;

namespace AliveChessClient.NetLayer.Transport
{
    public class ProtoBufferCodec
    {
        public static byte[] Encode<T>(T command) where T : ICommand
        {
            MemoryStream s1 = new MemoryStream();
            MemoryStream s2 = new MemoryStream();
            Serializer.Serialize<T>(s1, command);
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

            Assembly assembly = Assembly.Load("AliveChessLibrary");

            MethodInfo m = typeof(ProtoBufferCodec).GetMethod("DecodeConcreteCommand", BindingFlags.NonPublic |
                BindingFlags.Static);


            MethodInfo mg = m.MakeGenericMethod(assembly.GetType(
                String.Concat("AliveChessLibrary.Commands.", package.CommandType,
                ".", package.CommandName)));

            try
            {
                return (ICommand)mg.Invoke(null, new object[] { stream });
            }
            catch (Exception ex)
            {
                Debug.Fail(ex.Message);
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
