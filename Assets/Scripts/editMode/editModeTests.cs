using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Photon.Pun;

using Photon.Realtime;

namespace Tests
{
    public class editModeTests
    {
        private byte maxPlayersPerRoom = 8;
        // A Test behaves as an ordinary method
        [Test]
        public void hitDetectedTest()
        {

            
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
              Player player = gameObject.GetComponent<Player>();
          
            float startHealth = player.getHealth();
            player.hitDetected(1, player);

            Assert.AreEqual(player.getHealth(), startHealth - 1);
            // Use the Assert class to test conditions

              
            
        }

         
         

        // A Test behaves as an ordinary method
        [Test]
        public void RespawDespawn()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();

            Assert.AreEqual(player.canShoot, true);
            player.Despawn();
            Assert.AreEqual(player.canShoot, false);
            Assert.AreEqual(player.canMove,false);
            player.Respawn();
            Assert.AreEqual(player.canShoot, true);
            Assert.AreEqual(player.canMove, true);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CheckDeath()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();

            int startDeath = player.GetDeaths();
            player.hitDetected(10, player);
            Assert.AreEqual(player.canMove, false);
        }

            // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
            // `yield return null;` to skip a frame.
            [UnityTest]
        public IEnumerator editModeTestsWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
