using GameEngine;

namespace WPF_Game
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            new GameMaker().InitializeGame(this, 800, 600);
        }
    }
}