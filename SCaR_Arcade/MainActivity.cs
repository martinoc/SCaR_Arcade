﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Creator: Martin O'Connor
/// Student number: 3279660
/// Date created: 18-Mar-17
/// Date modified: 20-Apr-17
/// </summary>
namespace SCaR_Arcade
{
    [Activity(
        Label = "SCaR_Arcade",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait,
        Theme = "@android:style/Theme.NoTitleBar")]
    public class MainActivity : Activity
    {
        /*
         * Sources:
         * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
         * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
         */
        private ListView lvGameList;
        // ----------------------------------------------------------------------------------------------------------------
        // Predefined method to the create to build the Activity Main.axml executes. 
        protected override void OnCreate(Bundle bundle)
        {
           try
            {
                // Bind, and create all Views, and data for the Main.axml
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Main);
                lvGameList = FindViewById<ListView>(Resource.Id.lvGameList);
                lvGameList.Adapter = new MainRowAdapter(this);
                lvGameList.ItemClick += listViewItemClick;
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: Determines which item was selected in the list of games
        // and moves to GameMenuActivity.
        private void listViewItemClick(Object sender, AdapterView.ItemClickEventArgs args)
        {
            GlobalApp.BeginActivity(this, typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), args.Position);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will disable exit
        public override void OnBackPressed()
        {
        }
    }
}

