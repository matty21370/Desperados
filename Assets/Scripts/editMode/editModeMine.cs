using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class editModeMine
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestMine()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Mine"));
            Mine mine = gameObject.GetComponent<Mine>();
            Assert.AreEqual(mine.getDamage(), 2);
        }

        [Test]
        public void TestBulletSpawn()
        {
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Bullet"));
            Bullet bull = gameObject.GetComponent<Bullet>();
            Assert.AreEqual(bull.getDamage(), 1);
        }

        [Test]
        public void TestBulletIncrease()
		{
            GameObject gameObject = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Bullet"));
            Bullet bull = gameObject.GetComponent<Bullet>();
            bull.damageIncrease();
            Assert.AreEqual(bull.getDamage(), 2);

        }

    }
}
