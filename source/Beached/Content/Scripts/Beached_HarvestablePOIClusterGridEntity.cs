﻿using System.Collections.Generic;
using UnityEngine;

namespace Beached.Content.Scripts
{
	public class Beached_HarvestablePOIClusterGridEntity : HarvestablePOIClusterGridEntity
	{
		[SerializeField] public string animFile;

		public override List<AnimConfig> AnimConfigs =>
		[
			new AnimConfig
			{
				animFile = Assets.GetAnim(animFile),
				initialAnim = m_Anim
			}
		];
	}
}
