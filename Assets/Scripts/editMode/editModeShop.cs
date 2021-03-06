using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{   /// <summary>
    ///Created BY Andrew Viney
    ///test to check the shop is working correctly.
    /// </summary>

    public class editModeShop
    {
        /**
         * test to check that the shop can be acessed 
         * 
         */
        [Test]
        public void testEnabled()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Shop"));
            Shop shop = gameObject.GetComponent<Shop>();
            shop.setEnabled();
            Assert.AreEqual(shop.shopEnabled,true);
        }

        [Test]
        public void testPurchase()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Shop"));
            ButtonHandler button = gameObject.GetComponent<ButtonHandler>();
            button.SetText("speed");
            Assert.AreEqual(true, button.getSpeedTextp());
            button.Reset();
            Assert.AreEqual(false, button.getSpeedTextp());
        }

     }
}
