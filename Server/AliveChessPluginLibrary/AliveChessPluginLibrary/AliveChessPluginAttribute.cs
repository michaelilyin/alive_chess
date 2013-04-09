using System;

namespace AliveChessPluginLibrary
{
    /// <summary>
    /// атрибут для загрузки плагина
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class AliveChessPluginAttribute : Attribute
    {
        private bool forLoad = false;

        public AliveChessPluginAttribute(bool forLoad)
        {
            this.forLoad = forLoad;
        }

        public bool ForLoad
        {
            get { return forLoad; }
        }
    }
}