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
using Android.Views.InputMethods;
using static Android.Views.View;
/// <summary>
/// Creator: Ryan Cunneen
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
/// Date created: 09-May-2017
/// Date modified: 09-May-2017
namespace SCaR_Arcade
{
    [Activity(
        Label = "",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
    public class UserInputActivity : Activity
    {
        private Button saveBtn;
        private Button menuBtn;
        private EditText enterNameTxt;
        private TextView congratTxtView;
        private TextView scoreTxtView;
        private TextView timeTxtView;
        private CheckBox chkBoxName;
        private const string DEFAULTNAME = "Unknown";
        private const string DEFAULTENTERNAMEHERE = "Enter name here.";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.UserInput);

            saveBtn = FindViewById<Button>(Resource.Id.saveBtn);
            menuBtn = FindViewById<Button>(Resource.Id.menuBtn);
            enterNameTxt = FindViewById<EditText>(Resource.Id.enterNameETxt);
            scoreTxtView = FindViewById<TextView>(Resource.Id.scoreTxtView);
            timeTxtView = FindViewById<TextView>(Resource.Id.timeTxtView);
            congratTxtView = FindViewById<TextView>(Resource.Id.congratulationsTxtView);
            chkBoxName = FindViewById<CheckBox>(Resource.Id.chkBoxPreviousName);
            // Event handlers:
            enterNameTxt.Click += EditTextClick;
            menuBtn.Click += MenuButtonClick;
            chkBoxName.Click += CheckBoxClick;
            saveBtn.Click += SaveButtonClick;

            // Initializing data for the User input.
            enterNameTxt.Text = DEFAULTENTERNAMEHERE;
            
            string content = Intent.GetStringExtra(GlobalApp.getPlayersScoreVariable());

            Char delimiter = '-';
            String[] subStrings = content.Split(delimiter);

            string score = subStrings[1];
            string dif = subStrings[2];
            string time = subStrings[3];
            scoreTxtView.Text += " "+score;
            timeTxtView.Text += " "+time;
            
            chkBoxName.Enabled = !GlobalApp.isNewPlayer();

            // We don't want the checkbox to be auto checked. 
            if (chkBoxName.Enabled)
            {
                chkBoxName.Checked = false;
            }

            checkForNewPositionToLocalAndOnline(score, time, dif);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Overwritten method to close the soft keyboard on EditText (enterNameTxt), when the user has clicked outside of the EditText view.
        // Resource: http://stackoverflow.com/questions/39636698/how-to-hide-keyboard-in-xamarin-android-after-clicking-outside-edittext
        public override bool OnTouchEvent(MotionEvent e)
        {
            InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.HideSoftInputFromWindow(enterNameTxt.WindowToken, HideSoftInputFlags.None);
            if (String.Compare(enterNameTxt.Text, "") == 0 || String.Compare(enterNameTxt.Text, DEFAULTENTERNAMEHERE) == 0)
            {
                enterNameTxt.Text = DEFAULTENTERNAMEHERE;
            }
            return base.OnTouchEvent(e);
        }

        // ----------------------------------------------------------------------------------------------------------------
        protected void EditTextClick(Object sender, EventArgs args)
        {
            enterNameTxt.Text = "";
        }
        // ----------------------------------------------------------------------------------------------------------------
        protected void CheckBoxClick(Object sender, EventArgs args)
        {
            // Get the current name of the player. 
            enterNameTxt.Text = GlobalApp.getName();
        }
        // ----------------------------------------------------------------------------------------------------------------
        protected void SaveButtonClick(Object sender, EventArgs args)
        {

            string content = Intent.GetStringExtra(GlobalApp.getPlayersScoreVariable());

            if (GlobalApp.isNewPlayer())
            {
                if (String.Compare(enterNameTxt.Text, DEFAULTENTERNAMEHERE) == 0)
                {
                    GlobalApp.setName(DEFAULTNAME);
                    content = DEFAULTNAME + content;
                }
                else
                {
                    GlobalApp.setName(enterNameTxt.Text);
                    content = enterNameTxt.Text + content;
                }
            }
            else
            {
                if (String.Compare(enterNameTxt.Text, DEFAULTENTERNAMEHERE) == 0)
                {
                    GlobalApp.setName(DEFAULTNAME);
                    content = DEFAULTNAME + content;
                }
                else
                {
                    GlobalApp.setName(enterNameTxt.Text);
                    content = enterNameTxt.Text + content;
                }
            }

            // Now we can add the new score into the local leaderboard. 
            // Method: addNewScore will also determine if the score can be added into the Online leaderboard.
            LeaderBoardInterface.addNewScore(content);

            // Return back to the Game menu. 
            GlobalApp.BeginActivity(this, typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0));
        }
        // ----------------------------------------------------------------------------------------------------------------
        protected void MenuButtonClick(Object sender, EventArgs args)
        {
            GlobalApp.BeginActivity(this, typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0));
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will determine if the players score, and time can be added to either local, or online. 
        private void checkForNewPositionToLocalAndOnline(string scoreStr, string timeStr,string difStr)
        {
            bool ifNewHighScore = LeaderBoardInterface.newHighTimeScore(scoreStr, timeStr, difStr);
            saveBtn.Enabled = ifNewHighScore;
            enterNameTxt.Enabled = ifNewHighScore;
        }        
    }
}