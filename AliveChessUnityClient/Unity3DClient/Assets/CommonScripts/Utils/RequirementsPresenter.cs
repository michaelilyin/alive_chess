using AliveChessLibrary.GameObjects.Buildings;
using GameModel.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.CommonScripts.Utils
{
    public static class RequirementsPresenter
    {
        private static StringBuilder builder;

        static RequirementsPresenter()
        {
            builder = new StringBuilder();
        }

        public static string TextView(this CreationRequirements requirements)
        {
            builder.Remove(0, builder.Length);
            builder.Append("Required buildings:\n");
            foreach (var building in requirements.RequiredBuildings)
                builder.AppendFormat("{0}, ", NamesConverter.GetNameByType(building));
            builder.Append("\nRequired resources: ");
            foreach (var res in requirements.Resources)
                builder.AppendFormat("{0}:{1}, ", NamesConverter.GetNameByType(res.Key), res.Value);
            builder.AppendFormat("\nBuild time: {0}", requirements.CreationTime);
            return builder.ToString();
        }
    }
}
