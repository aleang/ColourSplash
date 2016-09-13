using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using ColourSplase;

namespace ColourSplash.Fragments
{
    public class SaveNameDialogFragment : DialogFragment
    {
        private int finalScore;
        private string gameResult;
        private int[] _spinnerIds;

        public SaveNameDialogFragment(string gameResult, int finalScore)
        {
            this.gameResult = gameResult;
            this.finalScore = finalScore;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the Builder class for convenient dialog construction
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);

            LayoutInflater inflater = Activity.LayoutInflater;

            var dialogAsView = inflater.Inflate(Resource.Layout.SaveHighScoreDialog, null);

            SetAlphabetToSpinner(dialogAsView);

            builder.SetView(dialogAsView)
                .SetTitle("Save Highscore")
                .SetMessage(gameResult + "\n\nSelect your initials")
                .SetCancelable(false)
                .SetPositiveButton(
                    "Save",
                    delegate
                    {
                        var playersInitial = GetInitialValues(dialogAsView);
                        HighScoreDatabase.OpenDatabase();
                        HighScoreDatabase.InsertHighScore(playersInitial, finalScore);
                        HighScoreDatabase.CloseConnection();
                    })
                .SetNegativeButton("Skip", delegate {});
            return builder.Create();
        }

        private string GetInitialValues(View dialogAsView)
        {
            string value = "";
            foreach (var i in _spinnerIds)
            {
                var numPicker = dialogAsView.FindViewById<NumberPicker>(i);
                value += (char)(numPicker.Value + 'A');
            }

            return value;
        }

        private void SetAlphabetToSpinner(View dialogAsView)
        {
            _spinnerIds = new [] {
                                      Resource.Id.numberPicker1,
                                      Resource.Id.numberPicker2,
                                      Resource.Id.numberPicker3
                                  };

            var alphabet = Enumerable
                .Range('A', 26)
                .Select(c => ((char)c).ToString())
                .ToArray();

            foreach (var i in _spinnerIds)
            {
                var numPicker = dialogAsView.FindViewById<NumberPicker>(i);
                var array = alphabet;
                numPicker.SetDisplayedValues(array);
                numPicker.MinValue = 0;
                numPicker.MaxValue = 25;
                numPicker.WrapSelectorWheel = true;
            }
        }
    }
}