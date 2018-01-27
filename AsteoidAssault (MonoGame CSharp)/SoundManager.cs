using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

public static class SoundManager
{
    private static List<SoundEffect> explosions =
        new List<SoundEffect>();
    private static int explosionCount = 4;

    private static SoundEffect playerShot;
    private static SoundEffect enemyShot;

    private static Random rand = new Random();

    public static void Initialize(ContentManager content)
    {
        playerShot = content.Load<SoundEffect>(@"Sounds\Shot1");
        enemyShot = content.Load<SoundEffect>(@"Sounds\Shot2");

        for (int x = 1; x <= explosionCount; x++)
        {
            explosions.Add(
                content.Load<SoundEffect>(@"Sounds\Explosion" + x.ToString()));
        }
    }

    public static void PlayerExplosion()
    {
        explosions[rand.Next(0, explosionCount)].Play();
    }

    public static void PlayPlayerShot()
    {
        playerShot.Play();
    }

    public static void PlayEnemyShot()
    {
        enemyShot.Play();
    }
}