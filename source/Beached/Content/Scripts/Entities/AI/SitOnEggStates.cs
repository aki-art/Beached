using Beached.Content.ModDb;
using Klei.AI;
using UnityEngine;

namespace Beached.Content.Scripts.Entities.AI
{

	public class SitOnEggStates : GameStateMachine<SitOnEggStates, SitOnEggStates.Instance, IStateMachineTarget, SitOnEggStates.Def>
	{
		private const float ANIM_OFFSET = 0.07f;

		public ApproachSubState<Approachable> moveToEgg;
		public State placeButtPre;
		public State lowerButt;
		public State lowerButtPst;
		public State sitting;
		public State sitPost;
		public State interrupted;
		public State behaviorComplete;

		public TargetParameter target;

		public override void InitializeStates(out BaseState default_state)
		{
			default_state = moveToEgg;

			root
				.Enter(SetMainTarget)
				.ToggleStatusItem("Eggsitting", "");

			moveToEgg
				.InitializeStates(masterTarget, target, placeButtPre);

			placeButtPre
				// TODO: play anim
				.GoTo(lowerButt)
				.Exit(ApplyEffect);

			lowerButt
				// TODO: play anim
				.Enter(smi => smi.kbac.Offset = Vector3.up)
				.Enter(TargetButtToEgg)
				.UpdateTransition(lowerButtPst, MoveButtToTarget, UpdateRate.RENDER_EVERY_TICK);

			lowerButtPst
				// TODO: play anim
				.GoTo(sitting);

			sitting
				.Enter(ApplyEffect)
				.Exit(ResetButtTarget)
				.ScheduleGoTo(smi => Random.Range(30f, 60f), sitPost)
				.Exit(RemoveEffect);

			sitPost
				.UpdateTransition(behaviorComplete, MoveButtToTarget, UpdateRate.RENDER_EVERY_TICK);

			behaviorComplete
				.BehaviourComplete(BTags.Creatures.wantsToSitOnEgg);
		}

		private bool MoveButtToTarget(Instance smi, float dt)
		{
			if (!smi.hasSetPosition)
				return false;

			var movement = Vector3.MoveTowards(smi.kbac.Offset, smi.relativePositionTarget, dt * smi.speed);
			smi.kbac.Offset = movement;

			Log.Debug("moving to position " + movement.magnitude);
			Log.Debug("moving to position " + dt * smi.speed);

			return Vector3.Distance(smi.relativePositionTarget, movement) < 0.01f;
		}

		private void ResetButtTarget(Instance smi)
		{
			smi.relativePositionTarget = Vector3.zero;
			smi.hasSetPosition = false;
		}

		private static readonly HashedString bammothEgg = "egg_icebelly_kanim";

		private void TargetButtToEgg(Instance smi)
		{
			smi.relativePositionTarget = Vector3.zero;

			var egg = target.Get(smi);
			if (egg == null)
				return;

			var debug = ModDebug.AddSimpleLineRenderer(egg.transform, Color.magenta, Color.green, 0.03f);
			var debug2 = ModDebug.AddSimpleLineRenderer(egg.transform, Color.red, Color.blue, 0.03f);

			if (egg.TryGetComponent(out KBatchedAnimController kbac))
			{
				// TODO: lookups
				var symbol = kbac.CurrentAnim.animFile.build.GetSymbol("egg01");

				if (kbac.currentAnim == bammothEgg)
				{
					symbol = kbac.CurrentAnim.animFile.build.GetSymbol("ib_egg01");
				}
				if (symbol == null)
				{
					foreach (var s in kbac.CurrentAnim.animFile.build.symbols)
					{
						if (HashCache.Get().Get(s.hash).StartsWith("egg"))
						{
							symbol = s;
							break;
						}
					}
				}

				if (symbol != null)
				{
					var data = KAnimBatchManager.Instance().GetBatchGroupData(kbac.batchGroupID);
					var instance = data.symbolFrameInstances[symbol.firstFrameIdx];
					var heightOfSprite = (instance.bboxMin - instance.bboxMax);
					var minT = Vector3.zero;
					var maxT = Vector3.zero;
					float y = egg.transform.position.y;

					var mesh = kbac.batch.group.mesh;
					Log.Debug("vertices length: " + mesh.vertices.Length);
					Log.Debug(mesh.vertices[0].ToString());

					var min = instance.bboxMin * kbac.animScale;
					var max = instance.bboxMax * kbac.animScale;

					var test = Vector3.zero;

					var frameIndex = kbac.CurrentAnim.firstFrameIdx;
					if (kbac.CurrentAnim.TryGetFrame(data.groupID, frameIndex, out var frame))
					{
						var elementIdx = frame.firstElementIdx;
						var frameElement = data.frameElements[elementIdx];
						var matrix = frameElement.transform;

						minT = (kbac.GetTransformMatrix() * frameElement.transform)
							.MultiplyPoint(Vector3.zero) - egg.transform.position;

						var height = max.y - min.y;
						minT = (kbac.GetTransformMatrix() * frameElement.transform)
							.MultiplyPoint(instance.bboxMin) - egg.transform.position;

						maxT = (kbac.GetTransformMatrix() * frameElement.transform)
							.MultiplyPoint(instance.bboxMax) - egg.transform.position;

						y = egg.transform.position.y - minT.y;
					}



					debug2.positionCount = 4;
					debug2.SetPositions([
						egg.transform.position + new Vector3(minT.x, minT.y),
						egg.transform.position + new Vector3(maxT.x, minT.y),
						egg.transform.position + new Vector3(maxT.x, maxT.y),
						egg.transform.position + new Vector3(minT.x, maxT.y),
											]);

					smi.relativePositionTarget = new Vector3(0, egg.transform.position.y - y);

					var debugPoint = (egg.transform.position + smi.relativePositionTarget) with { z = egg.transform.position.z };
					debug.positionCount = 4;
					debug.SetPositions([
						egg.transform.position,
						debugPoint,
						debugPoint with { x = debugPoint.x - 0.3f},
						debugPoint with { x = debugPoint.x + 0.3f},
						]);

					debug.loop = false;
					debug2.loop = true;

					debug.gameObject.SetActive(true);
					debug2.gameObject.SetActive(true);

				}
			}

			Log.Debug("set target to : " + smi.relativePositionTarget.ToString());
			smi.hasSetPosition = true;

		}

		private static void ApplyEffect(Instance smi)
		{
			var go = smi.sm.target.Get(smi);

			if (go == null)
				return;

			if (go.HasTag(GameTags.Egg))
				go.GetComponent<Effects>().Add(BEffects.KARACOO_HUG, true);
		}

		private static void RemoveEffect(Instance smi)
		{
			var go = smi.sm.target.Get(smi);

			if (go == null)
				return;

			if (go.TryGetComponent(out Effects effects))
				effects.Remove(BEffects.KARACOO_HUG);
		}

		private void SetMainTarget(Instance smi) => target.Set(smi.GetSMI<SittableEggsMonitor.Instance>().target, smi, false);


		public class Def : BaseDef
		{
		}

		public new class Instance : GameInstance
		{
			public Vector3 relativePositionTarget;
			public KBatchedAnimController kbac;
			public float speed = 1f;
			internal bool hasSetPosition;

			public Instance(Chore<Instance> chore, Def def) : base(chore, def)
			{
				kbac = chore.gameObject.GetComponent<KBatchedAnimController>();
				chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, BTags.Creatures.wantsToSitOnEgg);
			}
		}
	}
}
