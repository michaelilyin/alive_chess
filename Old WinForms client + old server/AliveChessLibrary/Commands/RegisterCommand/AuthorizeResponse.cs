using System;
using System.Collections.Generic;
using AliveChessLibrary.GameObjects.Buildings;
using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.GameObjects.Resources;
using ProtoBuf;

namespace AliveChessLibrary.Commands.RegisterCommand
{
    [Serializable, ProtoContract]
    public class AuthorizeResponse : ICommand
    {
        [ProtoMember(1)]
        private King king;
        [ProtoMember(2)]
        private Castle castle;
        [ProtoMember(3)]
        private bool isSuperUser;
        [ProtoMember(4)]
        private List<Resource> startResources;

        public AuthorizeResponse()
        {
        }

        public AuthorizeResponse(King king, Castle castle, bool isSuperUser,
            List<Resource> resources)
        {
            this.king = king;
            this.castle = castle;
            this.isSuperUser = isSuperUser;
            this.startResources = resources;
        }

        public Command Id
        {
            get { return Command.AuthorizeResponse; }
        }

        public King King
        {
            get { return king; }
            set { king = value; }
        }

        public Castle Castle
        {
            get { return castle; }
            set { castle = value; }
        }

        public bool IsSuperUser
        {
            get { return isSuperUser; }
            set { isSuperUser = value; }
        }

        public List<Resource> StartResources
        {
            get { return startResources; }
            set { startResources = value; }
        }
    }
}
