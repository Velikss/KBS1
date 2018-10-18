using System;
using System.CodeDom;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameEngine;
using WPF_Game.Game;

namespace RunchUnitTests
{ 

    [TestClass]
    public class ScoreboardTest
    {
        [TestMethod]
        public void TestLoadScoreBoard()
        {
            ScoreController.LoadScoreBoard();
            foreach (var Score in ScoreController.Scores)
            {
                Console.WriteLine(Score);
            }
        }

        [TestMethod]
        public void TestScoreBoardSave()
        {
            ScoreController.SaveScore(new Level("Test level"), 9);
        }
        
    }

    [TestClass]
    public class LevelTest
    {
        [TestMethod]
        public void TestLevelLoading()
        {
            Level.LoadLevels(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        public void TestOneLevelLoad()
        {
            Level.Load(AppDomain.CurrentDomain.BaseDirectory + "Levels/level1.lvl");
        }

    }
}
