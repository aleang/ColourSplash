using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using ColourSplash.Model;

namespace ColourSplash
{
    [Activity(
        Label = "Colour Splash", 
        MainLauncher = true, 
        Icon = "@drawable/icon", 
        Theme = "@android:style/Theme.Material.Light")]
    public class MainActivity : Activity
    {
        private Button button1, button2, button3, button4;
        private ProgressBar progressBar;
        private List<Button> gameButtons;
        private Button playButton;
        private List<ColourTile> tiles;
        private DateTime lastClicked;
        private DateTime startTime;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            FindViews();
            HandleEvents();
        }

        private void FindViews()
        {
            button1 = FindViewById<Button>(Resource.Id.button1);
            button2 = FindViewById<Button>(Resource.Id.button2);
            button3 = FindViewById<Button>(Resource.Id.button3);
            button4 = FindViewById<Button>(Resource.Id.button4);
            progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            gameButtons = new List<Button>
                          {
                              button1, button2, button3, button4
                          };
            playButton = FindViewById<Button>(Resource.Id.playButton);

        }

        private void HandleEvents()
        {
            playButton.Click += PlayButton_Click;
            gameButtons.ForEach(button => button.Click += GameButton_Click);
        }

        private void GameButton_Click(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            if ((DateTime.Now - lastClicked).TotalMilliseconds < 100)
            {
                return;
            }
            if (tiles.Where(t => t.Name == clicked.Text).First().IsAnswer)
            {
                CheckIfGameIsFinished();
                LoadSetOfColours();
            }
            else
            {
                clicked.Enabled = false;
                clicked.Background = GetDrawable(Android.Resource.Color.Black);
            }
            lastClicked = DateTime.Now;
        }

        private void CheckIfGameIsFinished()
        {
            if (progressBar.Progress == progressBar.Max)
            {
                var time = GetDisplayTime();

                progressBar.Visibility = ViewStates.Invisible;
                gameButtons.ForEach(b => b.Visibility = ViewStates.Invisible);
                playButton.Visibility = ViewStates.Visible;

                var dialog = new AlertDialog.Builder(this);
                dialog.SetTitle("You've made it!");
                dialog.SetMessage(GetDisplayTime());
                dialog.Show();

            }
        }

        private TimeSpan GetElapsedTime()
        {
            return startTime == default(DateTime) ?
                TimeSpan.MinValue :
                DateTime.Now - startTime;
        }

        private string GetDisplayTime()
        {
            var displayString = "You took ";
            displayString += GetElapsedTime().ToString("ss\\.ff");
            displayString += " seconds to complete 10 rounds. Press Play to try again.";
            return displayString;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            var dialog = new AlertDialog.Builder(this);
            dialog.SetTitle("How to Play");
            dialog.SetCancelable(false); ;
            dialog.SetPositiveButton("Play",
                                     handler : delegate
                                               {
                                                   startTime = DateTime.Now;
                                                   playButton.Visibility = ViewStates.Invisible;
                                                   progressBar.Visibility = ViewStates.Visible;
                                                   progressBar.Progress = -1;
                                                   gameButtons.ForEach(b => b.Visibility = ViewStates.Visible);
                                               });
            dialog.SetMessage("Four buttons will display with a colour word. Pick the one which is coloured differently to the word. There will be 10 rounds before your time is displayed. Try to be the fastest.\nTimer starts when you press \"Play\"");
            dialog.Show();

            LoadSetOfColours();
        }

        private void LoadSetOfColours()
        {
            //progressBar.IncrementProgressBy(1);
            var progress = progressBar.Progress;
            ObjectAnimator animation = ObjectAnimator.OfInt(progressBar, "Progress", 3, 7);
            animation.SetDuration(500); // 0.5 second
            animation.SetInterpolator(new LinearInterpolator());
            animation.Start();

            var db = new ColourTileRepository();
            tiles = db.GetNewSetOfTiles();
            for (int i = 0; i < 4; i++)
            {
                gameButtons[i].Enabled = true;

                gameButtons[i].Text = tiles[i].Name;
                if (tiles[i].IsAnswer)
                {
                    gameButtons[i].Background =
                        GetDrawable(db.GetTheWrongColour(tiles[i].Name));
                    //Resource.Color.brown;
                }
                else
                {
                    gameButtons[i].Background =
                        //Resources.GetDrawable(tiles[i].GetRandomColour());
                        GetDrawable(tiles[i].GetRandomColour());
                }
            }
        }
    }
}

