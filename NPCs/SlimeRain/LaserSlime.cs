using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution;


namespace Retribution.NPCs.SlimeRain
{
    public class LaserSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Laser Slime");
            Main.npcFrameCount[npc.type] = 2;

        }

        public override void SetDefaults()
        {
            npc.width = 56;
            npc.height = 80;
            npc.damage = 16;
            npc.defense = 4;
            npc.lifeMax = 35;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.aiStyle = 1;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return Main.slimeRain ? 0.5f : 0f;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax += (int)(npc.lifeMax * 0.579f * bossLifeScale);
            npc.damage += (int)(npc.damage * 0.6f);
        }

        private double counting;
        private bool stateAttack = false;
        private int phaseCount = 0;
        private int bulletTimer;

        private bool moving;

        public override void AI()
        {
            Player player = Main.player[npc.target];

            npc.TargetClosest(true);
            Vector2 targetPosition = Main.player[npc.target].position;

            if (player.Center.X > npc.Center.X)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }

            #region Attack
            bulletTimer++;
            npc.TargetClosest(true);
            npc.netUpdate = true;
            Vector2 vector = Main.player[npc.target].Center + new Vector2(npc.Center.X, npc.Center.Y);
            Vector2 vector2 = npc.Center + new Vector2(npc.Center.X, npc.Center.Y);
            float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
            if (bulletTimer >= 180 && Main.rand.NextFloat() < .50f)
            {
                Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Cos((double)num) * 2.0 * -1.0), (float)(Math.Sin((double)num) * 2.0 * -1.0), ProjectileID.EyeLaser, 8, 0f, 0, 0f, 8f);


                bulletTimer = 0;
            }
            else
            {
                bulletTimer = 0;
            }
            #endregion
        }

        public override void FindFrame(int frameHeight)
        {
            counting += 0.5;
            if (counting < 6.0)
            {
                npc.frame.Y = 0;
            }
            else if (counting < 12.0)
            {
                npc.frame.Y = frameHeight;
            }
            else
            {
                counting = 0.0;
            }
        }
    }
}