using System;
using System.Collections.Generic;
using System.IO;
using GameEngine;

namespace WPF_Game.Game
{
    public static class ScoreController
    {
        public static List<Score> Scores = new List<Score>();

        public static void LoadScoreBoard()
        {
            foreach (var Score in File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Levels/scores.scoreboard"))
                Scores.Add(new Score(Score.Split(':')[0], Convert.ToInt32(Score.Split(':')[1]), Score.Split(':')[2]));
        }

        public static void SaveScore(Level l, int Score)
        {
            Scores.Add(new Score(l.Name, Score, DateTime.Now.ToString("MM/dd/yyyy hh:mm")));
            var sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "Levels/scores.scoreboard")
                {AutoFlush = true};
            foreach (var score in Scores)
                sw.WriteLine(score.LevelName + ":" + score.score + ":" + score.Date);
            sw.Close();
            Console.WriteLine("Scores Saved");
        }

        public class Score
        {
            public string Date;

            public string LevelName;

            public int score;

            public Score(string LevelName, int score, string Date)
            {
                this.LevelName = LevelName;
                this.score = score;
                this.Date = Date;
            }
        }
    }
}