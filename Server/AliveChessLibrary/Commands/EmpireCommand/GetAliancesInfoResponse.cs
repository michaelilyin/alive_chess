using System.Collections.Generic;
using ProtoBuf;

namespace AliveChessLibrary.Commands.EmpireCommand
{
    /// <summary>
    /// получение краткой информации о всех союзах
    /// </summary>
    [ProtoContract]
    public class GetAliancesInfoResponse : ICommand
    {
        [ProtoMember(1)]
        private List<AlianceInfo> _aliances;

        public GetAliancesInfoResponse()
        {
        }

        public GetAliancesInfoResponse(List<AlianceInfo> aliances)
        {
            this._aliances = aliances;
        }

        public Command Id
        {
            get { return Command.GetAliancesInfoResponse; }
        }

        public List<AlianceInfo> Aliances
        {
            get { return _aliances; }
            set { _aliances = value; }
        }

        [ProtoContract]
        public class AlianceInfo
        {
            [ProtoMember(2)]
            private int _id;
            [ProtoMember(3)]
            private string _name;
            [ProtoMember(4)]
            private GetAlianceInfoResponse.MemberInfo _leader;

            public int Id
            {
                get { return _id; }
                set { _id = value; }
            }

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            public GetAlianceInfoResponse.MemberInfo Leader
            {
                get { return _leader; }
                set { _leader = value; }
            }
        }
    }
}
