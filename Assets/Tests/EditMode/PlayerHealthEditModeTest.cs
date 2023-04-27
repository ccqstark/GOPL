using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerHealthEditModeTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void PlayerHealthEditModeTestSimplePasses()
    {
        PlayerHealthTest player = new PlayerHealthTest();
        player.CurrentHealth = player.MaxHealth;
        // 测试伤害方法
        player.TakeDamage(65);
        Assert.AreEqual(35, player.CurrentHealth);
        // 测试回血方法
        player.AddHealth(15);
        Assert.AreEqual(50, player.CurrentHealth);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayerHealthEditModeTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
        Assert.IsEmpty("");
    }
}
