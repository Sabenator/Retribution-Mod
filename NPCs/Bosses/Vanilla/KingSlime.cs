using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Retribution.Buffs;
using Terraria.ID;

namespace Retribution.NPCs.Bosses.Vanilla
{
	public class KingSlime : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		public override void SetDefaults(NPC npc)
		{
			if (RetributionWorld.nightmareMode == true)
			{
				if (npc.type == NPCID.KingSlime)
				{
					//random number that is approximately 3990 life
					npc.lifeMax = (int)(npc.lifeMax * 1.4f);
					//in vanilla, expert scaling is * 1.8 damage
					npc.damage = 60;
				}
			}
		}

		private int kingShootTimer = 0;
		private int rainTimer = 0;
		private int c = 0;

		public override void AI(NPC npc)
        {
			#region King Slime
			if (RetributionWorld.nightmareMode == true)
			{
				if (npc.type == NPCID.KingSlime)
				{
					npc.TargetClosest(true);
					Player player = Main.player[npc.target];
					kingShootTimer++;
					//does rain effect
					if (rainTimer > 0  && npc.velocity.Y == 0) {
						rainTimer = 0;
						c++;
						if (c % 2 == 0)
						{
							Rain(npc, player);
						}
						//crappy dust effect
						for (int d = 0; d < 40; d++)
						{
							Dust.NewDust(npc.position, npc.width, npc.height, DustID.t_Slime, 0f, 0f, 150, default(Color), 1.5f);
						}
					}
					//Phase 1
					if (npc.life > npc.lifeMax * 0.7f)
					{
						if (kingShootTimer > 120 && Main.rand.NextBool())
						{
							Shoot(npc, player);
							kingShootTimer = 0;
						}
					}
					//Phase 2
					else if (npc.life < npc.lifeMax * 0.7f && npc.life > npc.lifeMax * 0.3f)
					{
						if (Main.rand.NextBool() && kingShootTimer > 90)
						{
							if (Main.rand.NextBool())
							{
								if (Main.rand.NextBool())
								{
									Shoot(npc, player);
								}
							}
							else {
								Fan(npc, player);
							}
							kingShootTimer = 0;
						}
					}
					//Phase 3
					else if(npc.life <= npc.lifeMax * 0.3f){
						if (Main.rand.NextBool() && kingShootTimer > 60)
						{
							Fan(npc, player);
							kingShootTimer = 0;
						}
					}
					if (npc.velocity.Y > 0.5f || npc.velocity.Y < -0.5f) {
						rainTimer = 1;
					}
				}
			}
			
#endregion
		}
		//projectile stuff. I used the original sounds.
		public void Rain(NPC npc, Player player) {
			Main.PlaySound(SoundID.Item17, (int)npc.position.X, (int)npc.position.Y);
			Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
			npc.netUpdate = true;
			AIMethods.Rain(16, ModContent.ProjectileType<SlimeBolt>(), 12f, 2f, 10, player.Center, 0);
			kingShootTimer = 0;
		}
		public void Shoot(NPC npc, Player player) {
			Main.PlaySound(SoundID.Item17, (int)npc.position.X, (int)npc.position.Y);
			npc.netUpdate = true;
			Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
			AIMethods.ShootAt(player, npc.Center, ModContent.ProjectileType<SlimeBolt>(), 12f, 2f, 10, 5);
		}
		public void Fan(NPC npc, Player player) {
			Main.PlaySound(SoundID.Item17, (int)npc.position.X, (int)npc.position.Y);
			Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
			npc.netUpdate = true;
			AIMethods.ShootFan(12, ProjectileID.SpikedSlimeSpike, 12f, 4f, 10, new Vector2(npc.Center.X, npc.Center.Y - 100), 120, npc.Center);
		}
	}
}
