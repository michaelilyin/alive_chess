using AliveChessLibrary.Interfaces;

namespace AliveChessServer.LogicLayer.UsersManagement
{
    public class Admin : User, IAdmin
    {
        public override UserRole Role
        {
            get { return UserRole.Administrator; }
        }
    }
}
