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

/// <summary>
/// Created by: Martin O'Connor
/// Student number: 3279660
/// Date modified: 21-Mar-2017
/// /// Date created: 21-Mar-2017
/// </summary>

namespace SCaR_Arcade
{
    class LeaderBoardRowAdapter:BaseAdapter<LeaderBoard>
    {
        // ----------------------------------------------------------------------------------------------------------------
        /*
         * sources
         * http://blog.atavisticsoftware.com/2014/02/listview-using-activitylistitem-style.html
         * http://blog.atavisticsoftware.com/2014/01/listview-basics-for-xamarain-android.html
        */
        private List<LeaderBoard> data;
        private Activity context;

        // ----------------------------------------------------------------------------------------------------------------
        // Constructor:
        public LeaderBoardRowAdapter (Activity activity)
        {
            context = activity;
            PopulateLeaderBoardData();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override LeaderBoard this[int position]
        {
            get { return data[position]; } 
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override long GetItemId(int position)
        {
            return position; 
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Defined method signature by BaseAdapter interface.
        public override int Count
        {
            get { return data.Count; } 
        }
        // ----------------------------------------------------------------------------------------------------------------
        public override View GetView(int position, View rowView, ViewGroup parent)
        {
            var view = rowView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(SCaR_Arcade.Resource.Layout.LeaderBoardRow, null);
            }

            var row = data[position];

            
            //add text colom details down list
            TextView leaderboardPosition = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.positiontxt);
            leaderboardPosition.Text = "P#"+ row.lbPosition; ;

            TextView name = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.nametxt);
            name.Text = "N#"+row.lbName;

            TextView time = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.timetxt);
            time.Text = "T#" + row.lbTime;

            TextView score = view.FindViewById<TextView>(SCaR_Arcade.Resource.Id.scoretxt);
            score.Text = "S#"+row.lbScore;


            return view;
        }
        // ----------------------------------------------------------------------------------------------------------------
        private void PopulateLeaderBoardData()
        {
            if (data == null)
            {
                bool testing = false;
                List<LeaderBoard> lbList = new List<LeaderBoard>();
                // Local file is determine by the boolean parameter.
                FileInterface.readFromFile(false);
                if (!testing)
                {
                    if (lbList != null)
                    {
                        lbList.Add(new LeaderBoard
                        {
                            lbPosition = "1",
                            lbName = "AAA",
                            lbTime = "07.10",
                            lbScore = "13"
                        }
                        );
                        lbList.Add(new LeaderBoard
                        {
                            lbPosition = "2",
                            lbName = "DAD",
                            lbTime = "08.10",
                            lbScore = "12"
                        }
                        );
                        lbList.Add(new LeaderBoard
                        {
                            lbPosition = "3",
                            lbName = "LOL",
                            lbTime = "09.10",
                            lbScore = "11"
                        }
                        );
                    }
                }
                else
                {
                    for (int i = 0; i < 19; i++)
                    {
                        string line = FileInterface.readFromFile(false);
                        Char delimiter = '-';
                        String[] subStrings = line.Split(delimiter);
                        lbList.Add(new LeaderBoard
                        {
                            lbPosition = subStrings[0],
                            lbName = subStrings[1],
                            lbTime = subStrings[2],
                            lbScore = subStrings[3]
                        }
                        );
                    }
                }
            }
        }
    }
}