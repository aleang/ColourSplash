using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace ColourSplash.Fragments
{
    public class HighscoreFragment : Fragment
    {
        private ListView highscoreListView;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override void OnActivityCreated(Bundle bundle)
        {
            base.OnActivityCreated(bundle);

            FindViews();
            FindHighScores();
        }

        private void FindHighScores()
        {
            var items = new string[] { "Vegetables", "Fruits", "Flower Buds", "Legumes", "Bulbs", "Tubers" };
            highscoreListView.Adapter = new ArrayAdapter<String>(
                View.Context, 
                Android.Resource.Layout.SimpleListItem1, 
                items);
        }

        private void FindViews()
        {
            highscoreListView = this.View.FindViewById<ListView>(Resource.Id.highscoreListView);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.HighScoreFragment, container, false);
        }
    }
}