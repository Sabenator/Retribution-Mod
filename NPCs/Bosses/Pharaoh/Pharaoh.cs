using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Retribution.Buffs;
using Retribution.Items.Weapons.Ranger;
using Retribution.Items.Bags;

namespace Retribution.NPCs.Bosses.Pharaoh
{
    [AutoloadBossHead]
    public class Pharaoh : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Buried Pharaoh");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;

            if (RetributionWorld.nightmareMode == false)
            {
                npc.lifeMax = 9050;
            }
            if (RetributionWorld.nightmareMode == true)
            {
                npc.lifeMax = 11000;
            }

            if (RetributionWorld.nightmareMode == false)
            {
                npc.damage = 50;
            }

            if (RetributionWorld.nightmareMode == true)
            {
                npc.damage = 60;
            }

            npc.defense = 21;
            npc.knockBackResist = 0f;
            npc.width = 52;
            npc.height = 180;
            npc.value = Item.buyPrice(0, 4, 0, 0);
            npc.npcSlots = 12f;
            npc.boss = true;
            npc.alpha = 0;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            NPCVuls.toWater = true;
            NPCVuls.fireImmune = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/inferis");
            bossBag = ModContent.ItemType<Tesca_Bag>();
        }

        private double counting;

        public int frameIdle = 0;

        private int frame;

        bool enraged = false;
        bool moving = true;
        bool canSpike = false;

        private int spikeTimer;

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 180);
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void AI()
        {
            Player player = Main.player[base.npc.target];

            #region Behaviour
            npc.TargetClosest(true);
            Vector2 targetPosition = Main.player[npc.target].position;

            if (moving == true)
            {
                if (targetPosition.X < npc.position.X)
                {
                    npc.velocity.X = -0.8f;
                }
                if (targetPosition.X > npc.position.X)
                {
                    npc.velocity.X = 0.8f;
                }
                if (targetPosition.Y < npc.position.Y)
                {
                    npc.velocity.Y = -0.8f;
                }
                if (targetPosition.Y > npc.position.Y)
                {
                    npc.velocity.Y = 0.8f;
                }
            }
            else if (moving == false)
            {
                npc.velocity.X = 0;
                npc.velocity.Y = 0;
            }
            
            #endregion

            #region Sand Spike
            spikeTimer++;

            if (spikeTimer > 498 && Main.rand.NextFloat() < .25f)
            {
                canSpike = true;
            }
            else if (spikeTimer > 501 && canSpike == false)
            {
                spikeTimer = 0;
            }

            if (canSpike == true)
            {
                if (spikeTimer == 500 || spikeTimer == 560 || spikeTimer == 620 || spikeTimer == 680 || spikeTimer == 740)
                {
                    moving = false;

                    Projectile.NewProjectile(player.Center.X, player.Center.Y + 650f, 0f, 0f, ModContent.ProjectileType<SandSpikeWarn>(), 0, 0f, Main.myPlayer, 0f, 0f);

                    int choice = Main.rand.Next(0, 1);
                    if (choice == 0)
                    {
                        Main.PlaySound(29, npc.Center, 61);
                    }
                    else if (choice == 1)
                    {
                        Main.PlaySound(29, npc.Center, 62);
                    }
                }

                if (spikeTimer > 740)
                {
                    spikeTimer = 0;
                    canSpike = false;
                    moving = true;
                }
            }
            #endregion
        }

        public override void FindFrame(int frameHeight)
        {
            counting += 1.0;
            if (counting < 5.0)
            {
                npc.frame.Y = 0;
            }
            else if (counting < 10.0)
            {
                npc.frame.Y = frameHeight;
            }
            else if (counting < 15.0)
            {
                npc.frame.Y = frameHeight * 2;
            }
            else if (counting < 20.0)
            {
                npc.frame.Y = frameHeight * 3;
            }
            else if (counting < 25.0)
            {
                npc.frame.Y = frameHeight * 4;
            }
            else if (counting < 30.0)
            {
                npc.frame.Y = frameHeight * 5;
            }
            else if (counting < 35.0)
            {
                npc.frame.Y = frameHeight * 6;
            }
            else if (counting < 40.0)
            {
                npc.frame.Y = frameHeight * 7;
            }
            else
            {
                counting = 0.0;
            }
        }

        public override void NPCLoot()
        {
            Main.NewText("The essence of the Tundra has been released...", 111, 199, 214, true);

            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Tesca1"), 1f);
            Gore.NewGore(npc.position, -npc.velocity, mod.GetGoreSlot("Gores/Tesca0"), 1f);

            if (Main.expertMode)
            {
                npc.DropBossBags();
            }

            Main.PlaySound(SoundID.NPCKilled, npc.Center, 33);

            RetributionWorld.downedTesca = true;
            if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.WorldData);
            }
        }
    }
}