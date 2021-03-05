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

            [Test]
        public void checkSetTo()
		{
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Player"));
            Player player = gameObject.GetComponent<Player>();
            player.SetToLobby();
            Assert.AreEqual(player.currentState, GameManager.GameStates.LOBBY);
            player.SetToGame();
            Assert.AreEqual(player.currentState, GameManager.GameStates.GAME);
        }

        


    }
}
