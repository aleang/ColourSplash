using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Android.Animation;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.Animations;
using ColourSplash.Fragments;
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
		private Stopwatch sw;
		private int _mistakes;
		private int _progressBarMax;

		public override void OnActivityCreated(Bundle bundle)
		{
			base.OnActivityCreated(bundle);
			FindViews();
			HandleEvents();

			_progressBarMax = int.Parse(Resources.GetString(Resource.String.ProgressBarMax));
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
			if (progressBar.Progress >= _progressBarMax-100)
			{
				int finalScore;
				var gameResult = GetGameResult(out finalScore);
				AnimateTheProgressBar(_progressBarMax-100);

                DisplayScoreAtEndOfGame(finalScore, gameResult);
			}
			else
			{
				AnimateTheProgressBar(progressBar.Progress);
			}
		}

	    private void DisplayScoreAtEndOfGame(int finalScore, string gameResult)
	    {
	        var saveDialog = new SaveNameDialogFragment(gameResult, finalScore);
	        saveDialog.SetTargetFragment(this, 0);
	        saveDialog.Show(FragmentManager, "SaveNameDialogFragment");
	        ResetViewsAfterGame();
	    }

	    private void ResetViewsAfterGame()
		{
			progressBar.Visibility = ViewStates.Gone;
			gameButtons.ForEach(b => b.Visibility = ViewStates.Gone);
			playButton.Visibility = ViewStates.Visible;
			_mistakes = 0;
		}

		private void AnimateTheProgressBar(int progress)
		{
			ObjectAnimator animation = ObjectAnimator.OfInt(progressBar, "Progress", progress, (progress / 100 + 1) * 100);
			animation.SetDuration(500);
			animation.SetInterpolator(new DecelerateInterpolator());
			animation.Start();
		}

		private string GetGameResult(out int finalScore)
		{
			sw.Stop();
			double elapsedTime = sw.Elapsed.TotalSeconds;
		    double mistakePenalty = _mistakes / 3.0;

			finalScore = (int) Math.Ceiling(elapsedTime + mistakePenalty);

		    mistakePenalty = Math.Truncate(mistakePenalty * 100) / 100;

            return 
                $"You took {sw.Elapsed.ToString("ss\\.ff")} s.\n" +
                   (_mistakes == 0 ? 
                        "You made no mistake!" :
                        $"You made {_mistakes} mistake{(_mistakes == 1 ? "" : "s")} (+{mistakePenalty} s).") +
				$"\nYour final time is {finalScore} seconds.";
		}

		private void PlayButton_Click(object sender, EventArgs e)
		{
			var dialog = new AlertDialog.Builder(View.Context);
			dialog.SetTitle("How to Play");
			dialog.SetCancelable(true);
			dialog.SetPositiveButton("Play",
									 delegate
									 {
										 sw = Stopwatch.StartNew();
										 playButton.Visibility = ViewStates.Gone;
										 progressBar.Visibility = ViewStates.Visible;
										 progressBar.Progress = -1;
										 gameButtons.ForEach(b => b.Visibility = ViewStates.Visible);
									 });
			dialog.SetMessage("Four buttons will display with a colour word. Pick the one which is coloured differently to the word.\n\nThe timer starts when you press \"Play\"");
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
					gameButtons[i].Background = Resources.
						GetDrawable(
							db.GetTheWrongColour(
								tiles[i].Name, 
								tiles.Where(t => !t.IsAnswer).Select(t => t.Name).ToList()
							));
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
			button1 = View.FindViewById<Button>(Resource.Id.button1);
			button2 = View.FindViewById<Button>(Resource.Id.button2);
			button3 = View.FindViewById<Button>(Resource.Id.button3);
			button4 = View.FindViewById<Button>(Resource.Id.button4);
			progressBar = View.FindViewById<ProgressBar>(Resource.Id.progressBar);
			gameButtons = new List<Button>
						  {
							  button1, button2, button3, button4
						  };
			playButton = View.FindViewById<Button>(Resource.Id.playButton);
			
		}

		private void HandleEvents()
		{
			playButton.Click += PlayButton_Click;
			gameButtons.ForEach(button => button.Click += GameButton_Click);

		}
	}
}

