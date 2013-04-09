using AliveChessLibrary.GameObjects.Characters;
using AliveChessLibrary.Interaction;

namespace AliveChessServer.LogicLayer.RequestExecutors.Helpers
{
    public static class Helper
    {
        public static Dialog GetSurrogate(this Dialog source)
        {
            Dialog surrogate = new Dialog();
            surrogate.Id = source.Id;
            surrogate.State = source.State;
            surrogate.Theme = source.Theme;
            surrogate.Elapsed = source.Elapsed;
            surrogate.YouStep = source.YouStep;

            //King organizator = new King(source.Organizator.ViewOnMap);
            King organizator = new King(); // with position
            organizator.Experience = source.Organizator.Experience;
            organizator.MilitaryRank = source.Organizator.MilitaryRank;

            //King respondent = new King(source.Organizator.ViewOnMap);
            King respondent = new King(); // with position
            respondent.Experience = source.Organizator.Experience;
            respondent.MilitaryRank = source.Organizator.MilitaryRank;

            surrogate.Organizator = organizator;
            surrogate.Respondent = respondent;

            return surrogate;
        }
    }
}
