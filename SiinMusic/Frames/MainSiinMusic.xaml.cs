using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace SiinMusic.Frames
{
    public sealed partial class MainSiinMusic : Page
    {
        public MainSiinMusic()
        {
            this.InitializeComponent();
        }

        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page 
        private readonly IList<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
        {
            ("myhome", typeof(HomeSiinMusic)),
            ("recentplay", typeof(RecentPlay)),
            ("upload", typeof(Share_Song)),
        };

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // You can also add items in code behind
            NavView.MenuItems.Add(new NavigationViewItemSeparator());

            ContentFrame.Navigated += On_Navigated;

            // NavView doesn't load any page by default: you need to specify it
            NavView_Navigate("myhome");

            // Add keyboard accelerators for backwards navigation
            var goBack = new KeyboardAccelerator { Key = VirtualKey.GoBack };
            goBack.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(goBack);

            // ALT routes here
            var altLeft = new KeyboardAccelerator
            {
                Key = VirtualKey.Left,
                Modifiers = VirtualKeyModifiers.Menu
            };
            altLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(altLeft);
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            if (args.IsSettingsInvoked)
                ContentFrame.Navigate(typeof(Setting));
            else
            {
                // Getting the Tag from Content (args.InvokedItem is the content of NavigationViewItem)
                var navItemTag = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(i => args.InvokedItem.Equals(i.Content))
                    .Tag.ToString();

                NavView_Navigate(navItemTag);
            }
        }

        private void NavView_Navigate(string navItemTag)
        {
            var item = _pages.First(p => p.Tag.Equals(navItemTag));
            ContentFrame.Navigate(item.Page);
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            On_BackRequested();
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private bool On_BackRequested()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed
            if (NavView.IsPaneOpen &&
                (NavView.DisplayMode == NavigationViewDisplayMode.Compact ||
                NavView.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            NavView.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(Setting))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
            }
            else
            {
                var item = _pages.First(p => p.Page == e.SourcePageType);

                NavView.SelectedItem = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(n => n.Tag.Equals(item.Tag));
            }
        }

        public static void Logout() => Frame.Navigate(typeof(MainPage));
    }
}
