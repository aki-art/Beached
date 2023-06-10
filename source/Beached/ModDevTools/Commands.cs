using System;
using System.Collections.Generic;
using UnityEngine;

namespace Beached.ModDevTools
{
	public class Commands
	{
		public Dictionary<string, Command> items;
		private string lastCommand;

		public void Init()
		{
			items = new()
			{
				{ "help", new Command(Help, "lists all commands", 0, 0) },
				{ "id", new Command(LogId, "logs id of selected object", 0, 0) },
				{ "position", new Command(Position, "", 2, 3) },
				{ "removegerms", new Command(RemoveGerms, "removes germs on the selected object", 0, 0) },
				{ "repeat", new Command(Repeat, "<number> repeat last commands number times", 1, 1) },
				{ "setgerms", new Command(SetGerms, "<germid> <amount> sets germs on the selected object", 2, 2) },
				{ "setmass", new Command(SetMass, "<mass> sets mass of selected object", 1, 1) },
				{ "settemperature", new Command(SetTemperature, "<celsius> sets temperature of selected object", 1, 1) },
				{ "spawn", new Command(Spawn, "<id> <amount?> spawns a prefabId at selected cell", 1, 2) },
				{ "tint", new Command(Tint, "<hex> or <r> <g> <b> sets tint color of selected object.", 1, 3) },
			};
		}

		private string RemoveGerms(string[] arg)
		{
			var go = SelectTool.Instance.selected;

			if (go == null)
				return "nothing is selected";

			if (go.TryGetComponent(out PrimaryElement element))
				element.ModifyDiseaseCount(-element.diseaseCount, "beached console remove germs");

			return null;
		}

		private string SetGerms(string[] arg)
		{
			var go = SelectTool.Instance.selected;

			if (go == null)
				return "nothing is selected";

			if (go.TryGetComponent(out PrimaryElement element))
			{
				var disease = Db.Get().Diseases.GetIndex(arg[1]);
				if (disease == byte.MaxValue)
					return "not a valid germ";

				if (!int.TryParse(arg[2], out int count))
					return "not a valid count";

				element.ModifyDiseaseCount(-element.diseaseCount, "remove germs");
				element.AddDisease(disease, count, "beached debug console");
			}

			return null;
		}

		private string SetMass(string[] arg)
		{
			var go = SelectTool.Instance.selected;

			if (go == null)
				return "nothing is selected";

			if (go.TryGetComponent(out PrimaryElement element)
				&& float.TryParse(arg[1], out var mass))
				element.Mass = mass;

			return null;
		}

		private string SetTemperature(string[] arg)
		{
			var go = SelectTool.Instance.selected;

			if (go == null)
				return "nothing is selected";

			if (go.TryGetComponent(out PrimaryElement element)
				&& float.TryParse(arg[1], out var temp))
				element.Temperature = MiscUtil.CelsiusToKelvin(temp);

			return null;
		}

		private string Tint(string[] args)
		{
			if (SelectTool.Instance.selected == null)
				return "nothing is selected";

			if (SelectTool.Instance.selected.TryGetComponent(out KBatchedAnimController kbac))
			{
				// hex
				if (args.Length == 2)
				{
					var color = Util.ColorFromHex(args[1]);
					kbac.TintColour = color;
					return null;
				}
				else if (args.Length == 4)
				{
					if (byte.TryParse(args[1], out var r)
						&& byte.TryParse(args[2], out var g)
						&& byte.TryParse(args[3], out var b))
					{
						kbac.TintColour = new Color32(r, g, b, 1);
						return null;
					}
					else if (float.TryParse(args[1], out var rf)
						&& float.TryParse(args[2], out var gf)
						&& float.TryParse(args[3], out var bf))
					{
						kbac.TintColour = new Color(rf, gf, bf);
						return null;
					}
				}

				return "not a color";
			}
			else
				return "not a tintable object";
		}

		private string Position(string[] arg)
		{
			var transform = SelectTool.Instance.selected.transform;
			float x = transform.position.x, y = transform.position.y, z = transform.position.z;

			if (arg.Length > 1)
			{
				if (arg[1] == "~")
					x = transform.position.x;
				else if (arg[1].StartsWith("+"))
				{
					if (float.TryParse(arg[1].Substring(1), out float xo))
						x += xo;
				}
				else if (arg[1].StartsWith("-"))
				{
					if (float.TryParse(arg[1].Substring(1), out float xo))
						x -= xo;
				}
				else
					float.TryParse(arg[1], out x);
			}

			if (arg.Length > 2)
				if (arg[2] == "~")
					y = transform.position.y;
				else if (arg[2].StartsWith("+"))
				{
					if (float.TryParse(arg[2].Substring(1), out float yo))
						y += yo;
				}
				else if (arg[2].StartsWith("-"))
				{
					if (float.TryParse(arg[2].Substring(1), out float yo))
						y -= yo;
				}
				else
					float.TryParse(arg[2], out y);
			if (arg.Length > 3)
				if (arg[3] == "~")
					z = transform.position.z;
				else if (arg[3].StartsWith("+"))
				{
					if (float.TryParse(arg[3].Substring(1), out float zo))
						z += zo;
				}
				else if (arg[3].StartsWith("-"))
				{
					if (float.TryParse(arg[3].Substring(1), out float zo))
						z -= zo;
				}
				else
					float.TryParse(arg[3], out z);

			transform.position = new UnityEngine.Vector3(x, y, z);
			return null;
		}

		private string LogId(string[] args)
		{
			if (SelectTool.Instance.selected == null)
				return "Nothing is selected";

			return SelectTool.Instance.selected.PrefabID().ToString();
		}

		private string Repeat(string[] args)
		{
			if (int.TryParse(args[1], out int num))
			{
				for (int i = 0; i < num; i++)
				{
					Process(lastCommand);
				}

				return null;
			}

			return "not a number";
		}

		private string Help(string[] arg)
		{
			foreach (var item in items)
			{

				ConsoleDevTool.Log($"{item.Key} {item.Value.description}");
			}

			return null;
		}

		private static string Spawn(string[] args)
		{
			var cell = SelectTool.Instance.GetSelectedCell();
			var num = args.Length > 2 && int.TryParse(args[2], out int n) ? n : 1;

			if (!Grid.IsValidCell(cell))
				return "Not a valid placement. Select a cell to spawn at.";

			for (int i = 0; i < num; i++)
			{
				var go = FUtility.Utils.Spawn(args[1], Grid.CellToPos(cell));
				if (go == null)
					return "Could not spawn object";
			}

			ConsoleDevTool.Log($"Spawned {num} {args[1]}.");
			return null;
		}

		public string Process(string commandStr)
		{
			if (commandStr.IsNullOrWhiteSpace())
				return "Empty Command";

			var split = commandStr.Split(' ');

			if (split[0] != "repeat")
				lastCommand = commandStr;

			if (items.TryGetValue(split[0], out var command))
				return command.Process(split);

			return "failed to parse command";
		}

		public class Command
		{
			public Func<string[], string> processFn;
			public int minArgs, maxArgs;
			public string description;

			public Command(Func<string[], string> processFn, string description, int minArgs = -1, int maxArgs = -1)
			{
				this.processFn = processFn;
				this.minArgs = minArgs;
				this.maxArgs = maxArgs;
				this.description = description;
			}

			public string Process(string[] args)
			{
				if ((minArgs != -1 && args.Length - 1 < minArgs)
					|| maxArgs != -1 && args.Length - 1 > maxArgs)
					return "Error: incorrect number of arguments";

				return processFn?.Invoke(args);
			}
		}
	}
}
