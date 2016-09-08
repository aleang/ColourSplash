using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ColourSplash;
using ColourSplash.Database;
using Java.Lang;

namespace ColourSplash.Fragments
{
    public class SaveNameDialogFragment : DialogFragment
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            // Use the Builder class for convenient dialog construction
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);

            LayoutInflater inflater = Activity.LayoutInflater;
            //builder.SetMessage("123")

            var DialogAsView = inflater.Inflate(Resource.Layout.SaveHighScoreDialog, null);
            builder.SetView(DialogAsView)
                .SetTitle("Please enter your name...")
                .SetMessage("Please enter your initials (3 letters max).")
                .SetPositiveButton(
                    "Save",
                    delegate
                    {
                        var textField = DialogAsView.FindViewById<TextView>(Resource.Id.playerNameTextField);
                        HighScoreDatabase.OpenDatabase();
                        HighScoreDatabase.InsertHighScore(textField.Text, GameFragment.FinalScore);
                        HighScoreDatabase.CloseConnection();
                    });
            
            return builder.Create();
        }
        
    }
}