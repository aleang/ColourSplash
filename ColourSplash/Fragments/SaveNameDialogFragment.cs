using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ColourSplase;

namespace ColourSplash.Fragments
{
    public class SaveNameDialogFragment : DialogFragment
    {
        private int finalScore;

        public SaveNameDialogFragment(int finalScore)
        {
            this.finalScore = finalScore;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the Builder class for convenient dialog construction
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);

            LayoutInflater inflater = Activity.LayoutInflater;
            //builder.SetMessage("123")

            var dialogAsView = inflater.Inflate(Resource.Layout.SaveHighScoreDialog, null);
            builder.SetView(dialogAsView)
                .SetTitle("Please enter your name...")
                .SetMessage("Please enter your initials (3 letters max).")
                .SetPositiveButton(
                    "Save",
                    delegate
                    {
                        var textField = dialogAsView.FindViewById<TextView>(Resource.Id.playerNameTextField);
                        HighScoreDatabase.OpenDatabase();
                        HighScoreDatabase.InsertHighScore(textField.Text, finalScore);
                        HighScoreDatabase.CloseConnection();
                    });
            
            return builder.Create();
        }
    }
}