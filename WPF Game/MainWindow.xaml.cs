using GameEngine;

namespace WPF_Game
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            new Media().AddMedia("", "");
            //new GameMaker().InitializeGame(this, 800, 600);
        }
    }
}