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
					npc.lifeMax = 2800;
					npc.damage = 68;
				}
			}
		}

		private int kingShootTimer;

		public override void AI(NPC npc)
        {
			#region King Slime
			if (RetributionWorld.nightmareMode == true)
			{
				if (npc.type == NPCID.KingSlime)
				{
					kingShootTimer++;

					if (kingShootTimer > 120)
					{
						Main.PlaySound(SoundID.Item17, (int)npc.position.X, (int)npc.position.Y);
						npc.TargetClosest(true);
						Vector2 vector = Main.player[npc.target].Center + new Vector2(npc.Center.X, npc.Center.Y);
						Vector2 vector2 = npc.Center + new Vector2(npc.Center.X, npc.Center.Y);
						npc.netUpdate = true;

						Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 45, 1f, 0f);
						float num = (float)Math.Atan2((double)(vector2.Y - vector.Y), (double)(vector2.X - vector.X));
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y, (float)(Math.Cos((double)num) * 10.0 * -1.0), (float)(Math.Sin((double)num) * 10.0 * -1.0), ModContent.ProjectileType<SlimeBolt>(), 8, 0f, 0, 0f, 0f);
						kingShootTimer = 0;
					}
				}
			}
			#endregion
		}
	}
}
