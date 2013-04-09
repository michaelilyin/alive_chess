using System;
using System.IO;
using System.Reflection;
using AliveChessLibrary.Commands;
using AliveChessLibrary.Net;
using ProtoBuf;

namespace WP7_Client.NetLayer
{
    public class ProtoBufferCodec
    {
        private static readonly Assembly Assembly;
        private static readonly MethodInfo Method;
        private static readonly object kodekLock = new object();

        static ProtoBufferCodec()
        {
            Assembly = Assembly.Load("AliveChessLibrary");
            Method = typeof (ProtoBufferCodec).GetMethod("DecodeConcreteCommand",
                                                         BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static byte[] Encode<T>(T command) where T : ICommand
        {
            var s1 = new MemoryStream();
            var s2 = new MemoryStream();
            lock (kodekLock)
            {
                Serializer.Serialize(s1, command);
                s2.Write(BitConverter.GetBytes((int) command.Id), 0, 4);
                s2.Write(BitConverter.GetBytes(s1.ToArray().Length), 0, 4);
                s2.Write(s1.ToArray(), 0, s1.ToArray().Length);

                var data = s2.ToArray();
                s1.Close();
                s2.Close();
                return data;
            }
        }

        public static ICommand Decode(BytePackage package)
        {
            var stream = new MemoryStream(package.CommandBody);

            var mg = Method.MakeGenericMethod(Assembly.GetType(
                String.Concat("AliveChessLibrary.Commands.", package.CommandType,
                              ".", package.CommandName)));
            return (ICommand) mg.Invoke(null, new object[] {stream});
        }

        private static T DecodeConcreteCommand<T>(Stream stream) where T : ICommand
        {
            lock (kodekLock)
            {
                var cmd = Serializer.Deserialize<T>(stream);
                stream.Close();
                return cmd;
            }
        }
    }
}