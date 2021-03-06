using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Photon.Pun;

using Photon.Realtime;

namespace Tests
{/// <summary>
///   created by Andrew Viney
/// these test focus on the player class ensuring it works as expected.
/// as they are unable to test graphical features and network features please comment out lines of code \
/// marked remove for testing inorder to allow tests to pass with out errors
/// </summary>
    public class editModeTests
    {
        //private byte maxPlayersPerRoom = 8;
        /**
         * Test checks that a player will reduce health when they are hit
         */
        [Test]
        public void hitDetectedTest()
        {   
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
              Player player = gameObject.GetComponent<Player>();
          
            float startHealth = player.getHealth();
            player.hitDetected(1, player);

            Assert.AreEqual(player.getHealth(), startHealth - 1);           
           }

         
         

        /**
         * test checks that the player will be able to be despawned and unable to move and shoot
         * and that once the player respawns they will  be able to shoot once again
         * 
         */
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

        /**
         * test checks that the player will die when they run out of health
         * 
         */
        [Test]
        public void CheckDeath()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();

            int startDeath = player.GetDeaths();
            player.hitDetected(10, player);
            Assert.AreEqual(player.canMove, false);
        }

        /**
         * test checks that the player will lose currency when they make a purchase
         * 
         */
        [Test]
        public void checkPurchase()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();
            int firstMoney=player.getCurrency();
            int money = 2;
            player.purchaseMade(money);
            Assert.AreEqual(firstMoney - money, player.getCurrency());


        }

        /**
         * checks that the player will be awarded a kill when they destory an oponent
         * 
         */
        [Test]
        public void checkKill()
        {
         
           
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();
            Player playerTwo = gameObject.GetComponent<Player>();
            int count = playerTwo.GetKills();
            playerTwo.NetworkAddKill();
            Assert.AreEqual(playerTwo.GetKills(),count+1);

        }
        /**
         * tests that the player can move betweeen lobby and game modes
         * 
         */
            [Test]
        public void checkSetToGame()
		{
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();
            player.SetToLobby();
            Assert.AreEqual(player.currentState, GameManager.GameStates.LOBBY);
            player.SetToGame();
            Assert.AreEqual(player.currentState, GameManager.GameStates.GAME);
        }
        /**
         * test that upgrades are assigned to the player
         */
        [Test]
        public void checkUpgrade()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();
            float testFloat = player.getCoolDown();
            player.cooldownUpgrade();
            Assert.AreEqual(player.getCoolDown() * 2, testFloat);
        }


    }
}
