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
/// Creator: Ryan Cunneen
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
/// Date modified: 08-Apr-2017
/// Date created: 08-Apr-2017
/// </summary>
namespace SCaR_Arcade
{
    static class LeaderBoardInterface
    {
        // The top 20 of the players scores will be added, and shown.
        private const int MAXNUMBEROFLOCALSCORES = 20;

        // The top 100 of players scores around the world will be added, and shown.
        private const int MAXNUMBEROFONLINESCORES = 100;

        private static int localPosition = 0;
        // ----------------------------------------------------------------------------------------------------------------
        // Populates the Leader board with data of scores that are either from the local, or online text files.
        public static List<LeaderBoard> PopulateLeaderBoardData(bool isOnline)
        { 
            // A particular .txt file (local, or online) will be used determined by the boolean parameter.
            List<string> unsortedList = FileInterface.readFromScoreFile(isOnline);

            // This list will be sorted;
            List<LeaderBoard> unsortedLb = new List<LeaderBoard>();

            if (unsortedList != null)
            {
                unsortedLb = addLeaderboardObjects(unsortedList, isOnline);
            }
            // Return a sorted Leaderboard list. 
            return selectionSort(unsortedLb);
        }

        // ----------------------------------------------------------------------------------------------------------------
        // We will sort the entire list here, using the sorting algorithm selection.
        // Resource: http://cforbeginners.com/CSharp/SelectionSort.html helped us create the algorithm.
        // Returns a list of Leaderboard data sorted in ascending order, by the instance variable lbPosition.
        private static List<LeaderBoard> selectionSort(List<LeaderBoard> sortedList)
        {
            int position = 0;
            LeaderBoard temp;
            for (int x = 0; x < sortedList.Count - 1; x++)
            {
                position = x;
                for (int y = x + 1; y < sortedList.Count; y++)
                {
                    if (sortedList[y].lbPosition < sortedList[position].lbPosition)
                    {
                        position = y;
                    }
                }
                if (position != x)
                {
                    temp = sortedList[x];
                    sortedList[x] = sortedList[position];
                    sortedList[position] = temp;
                }
            }
            return sortedList;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // @param playersScore should be in the format Name-Score-Time
        // Will format the @parameter playersScore into the format Position-Name-Score-Time
        // Position - The position the player is in the list.
        // Name - Name of the player.
        // Score - The number of moves taken to win the game.
        // Time - How long it took to win the game. 
        public static void addNewScore(string playersScore)
        {
            // The boolean parameter will determine if we are using with the local file, or online file.
            int onlinePosition = determinePosition(true, playersScore);
           
            // Remove the current score at position localPosition;
            FileInterface.removeScoreAtPosition(false, localPosition);
            // Now we can add the new score.
            // This will be in the format Position-Name-Score-Time
            FileInterface.addScoreToFile(false, localPosition + "-" + playersScore);
          
            if (onlinePosition <= MAXNUMBEROFONLINESCORES)
            {
                // Remove the current score at position onlinePosition;
                FileInterface.removeScoreAtPosition(false, onlinePosition);

                // Now we can add the new score. 
                // This will be in the format Position-Name-Score-Time
                FileInterface.addScoreToFile(true, onlinePosition + "-" + playersScore);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        private static int determinePosition(bool isOnline, string playersScore)
        {
            // @param playersScore will be formatted as "name + "-" + score + "-" + diff + "-" + time";

            // Get the index of the first "-"
            int startIndex = playersScore.IndexOf("-");

            //Get the index of the last "-";
            int endIndex = playersScore.LastIndexOf("-");
            // Get the substring from @param playersScore, and convert it to an integer.
            int score = Convert.ToInt32(playersScore.Substring(startIndex, endIndex));

            // Get the substring from @param playersScore, and convert it to a double.
            double time = Convert.ToDouble(playersScore.Substring(endIndex, playersScore.Length));

            int position = 0;

            // A particular .txt file (local, or online) will be used determined by the boolean parameter.
            // We determine if the score is high enough to be added into the local file, if it is, it may be 
            // Able to be added into the online .txt file.
            // But first we get the local scores determine by the boolean paramater.
            List<string> scoreList = FileInterface.readFromScoreFile(isOnline);

            // At this point we are not concerned with sorting the list. 
            List<LeaderBoard> leaderBdList = addLeaderboardObjects(scoreList, isOnline);

            // We have set up all the variables, we can determine the position of the @param playersScore,
            // once we have done that we add it onto the @param playersScore, and submit it to be read into the .txt file.
            foreach (LeaderBoard lb in leaderBdList)
            {
                if (score > lb.lbScore)
                { 
                    position++;
                }
            }

            return position;  
        }

        // ----------------------------------------------------------------------------------------------------------------
        //
        private static List<LeaderBoard> addLeaderboardObjects(List<string> list, bool isOnline)
        {
            int count = 0;
            if (isOnline)
            {
                count = MAXNUMBEROFONLINESCORES;
            }
            else
            {
                count = MAXNUMBEROFLOCALSCORES;
            }
            // This list will be sorted;
            List<LeaderBoard> temp = new List<LeaderBoard>();
            for (int i = 0; i < count && i < list.Count; i++)
            {
                // Return the data (string) and index i;
                string line = list[i];

                Char delimiter = '-';
                String[] subStrings = line.Split(delimiter);

                temp.Add(new LeaderBoard
                {
                    lbPosition = Convert.ToInt32(subStrings[0]),
                    lbName = subStrings[1],
                    lbScore = Convert.ToInt32(subStrings[2]),
                    lbDiff = Convert.ToInt32(subStrings[3]),
                    lbTime = subStrings[4]
                });
            }
            return temp;
        }
        // ----------------------------------------------------------------------------------------------------------------
        //
        public static string formatLeaderBoardScore(string name, string score,int dif, string time)
        {
            return name + "-" + score + "-" + dif + "-"+ time;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // 
        public static bool checkForNewLocalHighScore(int score, int hours, int minutes, int seconds)
        {
            int currentHours = 0;
            int currentMinutes = 0;
            int currentSeconds = 0;
            bool newScore = false;
            // A particular .txt file (local, or online) will be used determined by the boolean parameter.
            // The method populateLeaderBoardData will have been sorted.
            List<LeaderBoard> listLBd = PopulateLeaderBoardData(false);
            for (int i = 0; i < listLBd.Count && !newScore; i++)
            {
                if (score <= listLBd[i].lbScore)
                {
                    if (hours == 0)
                    {
                        currentMinutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, false));
                        currentSeconds = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, true));
                        if (minutes < currentMinutes)
                        {
                            newScore = true;
                        }
                        else if (minutes == currentMinutes)
                        {
                            if (seconds < currentSeconds)
                            {
                                newScore = true;
                            }
                            else
                            {
                                newScore = false;
                                localPosition++;
                            }
                        }
                        else
                        {
                            newScore = false;
                            localPosition++;
                        }
                    }
                    else
                    {
                        currentHours = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, false));
                        currentMinutes = Convert.ToInt32(GlobalApp.extractValuesFromString(":", listLBd[i].lbTime, true));
                        currentSeconds = Convert.ToInt32(listLBd[i].lbTime.Substring(listLBd[i].lbTime.LastIndexOf(":"), 2));
                        if (hours < currentHours)
                        {
                            newScore = true;
                        }
                        else if (hours == currentHours)
                        {
                            if (minutes < currentMinutes)
                            {
                                newScore = true;
                            }
                            else
                            {
                                if (seconds < currentSeconds)
                                {
                                    newScore = true;
                                }
                                else
                                {
                                    newScore = false;
                                    localPosition++;
                                }
                            }
                        }
                        else
                        {
                            newScore = false;
                            localPosition++;
                        }
                    }
                }
            }
            return newScore;
        }
    }
}