using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace GameEngine
{
    public class EnemyAI : PhysicalObject
    {
        
        #region Variables
        
        private List<Enemy> Enemies = new List<Enemy>();     
        
        #endregion

        public EnemyAI(Enemy enemy)
        {
            this.Enemies.Add(enemy);
        }

        #region Methods
        
        private void EnemyAI_Thread()
        {
         
            while (true)
            {
                
                
                Thread.Sleep(100);
            }
        }

        public void Start(ref GameRenderer render)
        {
            new Thread(EnemyAI_Thread).Start();
        }
        
        #endregion
        
    }
}