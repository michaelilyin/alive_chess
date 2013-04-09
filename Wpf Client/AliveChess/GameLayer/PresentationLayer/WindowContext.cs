using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AliveChess.GameLayer.PresentationLayer
{
    public class WindowContext
    {
        private readonly List<GameScene> _pages = new List<GameScene>();

        public void AttachPage(GameScene page)
        {
            if (!_pages.Contains(page))
                _pages.Add(page);
        }

        public void DetachPage(GameScene page)
        {
            if (_pages.Contains(page))
                _pages.Remove(page);
        }

        public Page Find(string name, bool withOwnerCredentials)
        {
            foreach (GameScene page in _pages)
            {
                if (!withOwnerCredentials)
                {
                    if ((bool) page.Dispatcher.Invoke(
                        DispatcherPriority.Normal,
                        new Func<ComparableImpl, bool>(Comparator),
                        new ComparableImpl(page, name)))
                        return page;
                }
                else // Search from UI thread
                {
                    if (page.Name == name)
                        return page;
                }
            }
            return null;
        }

        private bool Comparator(ComparableImpl comparable)
        {
            return comparable.Match();
        }

        private struct ComparableImpl
        {
            private readonly GameScene _page;
            private readonly string _name;

            public ComparableImpl(GameScene page, string name)
            {
                _page = page;
                _name = name;
            }

            public bool Match()
            {
                return _page.Name.Equals(_name);
            }
        }
    }
}
