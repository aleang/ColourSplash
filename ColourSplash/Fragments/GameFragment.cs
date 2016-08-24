using System;
using System.Collections.Generic;
using System.Linq;
using Android.Animation;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using ColourSplash.Model;

namespace ColourSplash
{

    public class GameFragment : Fragment
    {
        private Button button1, button2, button3, button4;
        private ProgressBar progressBar;
        private List<Button> gameButtons;
        private Button playButton;
        private List<ColourTile> tiles;
        private DateTime lastClicked;
        private DateTime startTime;
        private int _mistakes;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override void OnActivityCreated(Bundle bundle)
        {
            base.OnActivityCreated(bundle);
            
            FindViews();
            HandleEvents();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.GameFragment, container, false);
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
                clicked.Background = Resources.GetDrawable(Android.Resource.Color.Black);
               // clicked..setPaintFlags(t.getPaintFlags() | Paint.STRIKE_THRU_TEXT_FLAG);
                _mistakes++;
            }
            lastClicked = DateTime.Now;
        }

        private void CheckIfGameIsFinished()
        {
            if (progressBar.Progress >= progressBar.Max-100)
            {
                var time = GetGameResult();
                AnimateTheProgressBar(progressBar.Max-100);
                var dialog = new AlertDialog.Builder(View.Context);
                dialog.SetTitle("You've made it!");
                dialog.SetCancelable(false); ;
                dialog.SetPositiveButton("Play",
                                         handler: delegate
                                         {
                                             progressBar.Visibility = ViewStates.Invisible;
                                             gameButtons.ForEach(b => b.Visibility = ViewStates.Invisible);
                                             playButton.Visibility = ViewStates.Visible;
                                         });
                dialog.SetMessage(time);
                dialog.Show();
            }
            else
            {
                AnimateTheProgressBar(progressBar.Progress);
            }
        }

        private void AnimateTheProgressBar(int progress)
        {

            ObjectAnimator animation = ObjectAnimator.OfInt(progressBar, "Progress", progress, (progress / 100 + 1) * 100);
            animation.SetDuration(500);
            animation.SetInterpolator(new DecelerateInterpolator());
            animation.Start();
        }

        private TimeSpan GetElapsedTime()
        {
            return startTime == default(DateTime) ?
                TimeSpan.MinValue :
                DateTime.Now - startTime;
        }

        private string GetGameResult()
        {
            var elapsedTime = GetElapsedTime();
            var displayString = "You took ";
            displayString += elapsedTime.ToString("ss\\.ff");
            displayString += " seconds to complete 10 rounds.\n\n" +
                             $"You also made {_mistakes} mistake{(_mistakes == 1 ? "" : "s")}.\n\n" +
                             $"Your score is {50 - (_mistakes*2 + (int)elapsedTime.TotalSeconds)}";
            return displayString;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            var dialog = new AlertDialog.Builder(View.Context);
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
            dialog.SetMessage("Four buttons will display with a colour word. Pick the one which is coloured differently to the word. Try to be the fastest to complete 10 rounds.\n\nThe timer starts when you press \"Play\"");
            dialog.Show();

            LoadSetOfColours();
        }

        private void LoadSetOfColours()
        {
            var db = new ColourTileRepository();
            tiles = db.GetNewSetOfTiles();
            for (int i = 0; i < 4; i++)
            {
                gameButtons[i].Enabled = true;

                gameButtons[i].Text = tiles[i].Name;
                if (tiles[i].IsAnswer)
                {
                    gameButtons[i].Background =
                        Resources.GetDrawable(db.GetTheWrongColour(tiles[i].Name));
                    //Resource.Color.brown;
                }
                else
                {
                    gameButtons[i].Background =
                        //Resources.GetDrawable(tiles[i].GetRandomColour());
                        Resources.GetDrawable(tiles[i].GetRandomColour());
                }
            }
        }

        private void FindViews()
        {
            button1 = this.View.FindViewById<Button>(Resource.Id.button1);
            button2 = this.View.FindViewById<Button>(Resource.Id.button2);
            button3 = this.View.FindViewById<Button>(Resource.Id.button3);
            button4 = this.View.FindViewById<Button>(Resource.Id.button4);
            progressBar = this.View.FindViewById<ProgressBar>(Resource.Id.progressBar);
            gameButtons = new List<Button>
                          {
                              button1, button2, button3, button4
                          };
            playButton = this.View.FindViewById<Button>(Resource.Id.playButton);

        }

        private void HandleEvents()
        {
            playButton.Click += PlayButton_Click;
            gameButtons.ForEach(button => button.Click += GameButton_Click);
        }
    }
}

