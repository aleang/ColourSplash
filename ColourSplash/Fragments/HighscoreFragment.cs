using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ColourSplash.Adapter;
using ColourSplash.Database;
using ColourSplash.Models;

namespace ColourSplash.Fragments
{
    public class HighscoreFragment : Fragment
    {
        private ListView highscoreListView;
        private string _path;
        private Button highScoreButton;
        private Button clearHighScoreButton;
        private List<HighScore> _highScoreItems;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }
        public override void OnActivityCreated(Bundle bundle)
        {
            base.OnActivityCreated(bundle);

            FindViews();
            OpenDatabaseConnection();
            LoadHighScores();
           
        }

        public override void OnDestroy()
        {
            HighScoreDatabase.CloseConnection();
            base.OnDestroy();
        }
        private void LoadHighScores()
        {
            _highScoreItems = HighScoreDatabase.ReadDatabase();

            highscoreListView.Adapter = new HighScoreAdapter(Activity, _highScoreItems);
        }

        private void OpenDatabaseConnection()
        {
            HighScoreDatabase.OpenDatabase();
        }

        private void FindViews()
        {
            highscoreListView = View.FindViewById<ListView>(Resource.Id.highscoreListView);

            highScoreButton = View.FindViewById<Button>(Resource.Id.addFakeHighScore);
            highScoreButton.Click += delegate {
                HighScoreDatabase.InsertHighScore("PHE", 12);
                LoadHighScores();
            };

            clearHighScoreButton = View.FindViewById<Button>(Resource.Id.clearHighScore);
            clearHighScoreButton.Click += delegate {
                HighScoreDatabase.ClearDatabase();
                LoadHighScores();
            };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.HighScoreFragment, container, false);
        }
    }
}