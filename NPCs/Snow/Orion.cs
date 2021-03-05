using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Retribution.Buffs;
using Microsoft.Xna.Framework;
using System;
using Retribution.NPCs.Bosses.Tesca;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace Retribution.NPCs.Snow
{
	public class Orion : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion");
			Main.npcFrameCount[npc.type] = 4;
		}

		public override void SetDefaults()
		{
			npc.width = 56;
			npc.height = 106;
			npc.damage = 8;
			npc.defense = 3;
			npc.lifeMax = 80;
			npc.HitSound = SoundID.NPCHit41;
			npc.DeathSound = SoundID.NPCDeath7;
			npc.value = 60f;
			npc.knockBackResist = 0.5f;
			npc.noGravity = true;
			npc.aiStyle = 22;
		}

		private double counting;
		private int frame;

		private int shootTimer;

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneSnow ? .1f : 0f;
		}

        public override void AI()
        {
			Player player = Main.player[npc.target];
			npc.spriteDirection = npc.direction;
			npc.rotation = npc.velocity.X * 0.05f;
			if (npc.Distance(Main.player[npc.target].Center) <= 800f && Main.rand.Next(100) == 0)
			{
				Main.PlaySound(SoundID.Item28, npc.Center);
				float Speed = 14f;
				Vector2 vector8 = new Vector2(npc.position.X + (float)(npc.width / 2), npc.position.Y + (float)(npc.height / 2));
				int damage = 34;
				int type = ModContent.ProjectileType<TescanHome>();
				float rotation = (float)Math.Atan2((double)(vector8.Y - (player.position.Y + (float)player.height * 0.5f)), (double)(vector8.X - (player.position.X + (float)player.width * 0.5f)));
				int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)(Math.Cos((double)rotation) * (double)Speed * -1.0), (float)(Math.Sin((double)rotation) * (double)Speed * -1.0), type, damage, 0f, 0, 0f, 0f);
				Main.projectile[num54].netUpdate = true;
			}
		}

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
			target.AddBuff(ModContent.BuffType<TerrariasFrost>(), 120);
        }

        public override void FindFrame(int frameHeight)
		{
			counting += 1.0;
			if (counting < 6.0)
			{
				npc.frame.Y = 0;
			}
			else if (counting < 12.0)
			{
				npc.frame.Y = frameHeight;
			}
			else if (counting < 18.0)
			{
				npc.frame.Y = frameHeight * 2;
			}
			else if (counting < 24.0)
			{
				npc.frame.Y = frameHeight * 3;
			}
			else
			{
				counting = 0.0;
			}
		}

		public override void NPCLoot()
		{
			for (int d = 0; d < 20; d++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, 185, 0f, 0f, 150);
			}
		}
	}
}
