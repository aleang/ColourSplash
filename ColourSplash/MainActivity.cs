using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ColourSplash.Fragments;

namespace ColourSplash
{
    [Activity(
        Label = "Colour Splash",
        MainLauncher = true,
        Icon = "@drawable/icon",
        Theme = "@android:style/Theme.Material.Light")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainView);

            AddTab("Game", new GameFragment());
            AddTab("Highscore", new HighscoreFragment());

            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
        }

        private void AddTab(string tabText, Fragment view)
        {
            var tab = ActionBar.NewTab();
            tab.SetText(tabText);
        //    tab.SetIcon(iconResourceId);

            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                {
                    e.FragmentTransaction.Remove(fragment);
                }
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };

            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                e.FragmentTransaction.Remove(view);
            };

            this.ActionBar.AddTab(tab);

        }
    }
}