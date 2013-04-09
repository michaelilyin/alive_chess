using System.Collections.Generic;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// получение информации о союзе
    /// </summary>
    [ProtoContract]
    public class GetAlianceInfoResponse : ICommand
    {
        [ProtoMember(1)]
        private int _unionId;
        [ProtoMember(2)]
        private List<MemberInfo> _members;

        public GetAlianceInfoResponse()
        {
        }

        public GetAlianceInfoResponse(int unionId, List<MemberInfo> members)
        {
            this._unionId = unionId;
            this._members = members;
        }

        public Command Id
        {
            get { return Command.GetAlianceInfoResponse; }
        }

        public int UnionId
        {
            get { return _unionId; }
            set { _unionId = value; }
        }

        public List<MemberInfo> Members
        {
            get { return _members; }
            set { _members = value; }
        }

        [ProtoContract]
        public class MemberInfo
        {
            [ProtoMember(3)]
            private int _memberId;
            [ProtoMember(4)]
            private string _memberName;

            public int MemberId
            {
                get { return _memberId; }
                set { _memberId = value; }
            }

            public string MemberName
            {
                get { return _memberName; }
                set { _memberName = value; }
            }
        }
    }
}
