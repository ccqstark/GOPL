using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerHealthPlayerModeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerHealthPlayerModeTestSimplePasses()
    {

    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerHealthPlayerModeTestWithEnumeratorPasses()
    {
        var playerTest = new GameObject("PlayerHealthTest");
        playerTest.AddComponent<PlayerHealthController>();
        var playerHealthController = playerTest.GetComponent<PlayerHealthController>();
        yield return new WaitForSeconds(0.1f);
        playerHealthController.TakeDamage(25);
        Assert.AreEqual(75f, playerHealthController.CurrentHealth);
        yield return new WaitForSeconds(playerHealthController.BeginRecoveryTime + 1f);
        Assert.Greater(playerHealthController.CurrentHealth, 75f);
    }
}
