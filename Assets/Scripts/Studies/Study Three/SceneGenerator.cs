using UnityEditor;
using UnityEngine;

namespace Jake.Studies.Four
{
	public class SceneGenerator : EditorWindow
	{
		[InitializeOnLoadMethod]
		static void OnLoad()
		{
			EditorApplication.update += LoadPrefsAtFirstUpdate;
		}

		public static void LoadPrefs()
		{
			Menu.SetChecked("Jake/Study Three Conditions/Fire", EditorPrefs.GetBool("fire", false));
			Menu.SetChecked("Jake/Study Three Conditions/Save Girl", EditorPrefs.GetBool("girl", false));
			Menu.SetChecked("Jake/Study Three Conditions/Save Adults", EditorPrefs.GetBool("adults", false));
			Menu.SetChecked("Jake/Study Three Conditions/No Influence", EditorPrefs.GetBool("noinfluence", false));
			Menu.SetChecked("Jake/Study Three Conditions/Swap Elevators", EditorPrefs.GetBool("swapelevators", false));
			Menu.SetChecked("Jake/Study Three Conditions/Swap Hands", EditorPrefs.GetBool("swaphands", false));
		}

		static void LoadPrefsAtFirstUpdate()
		{
			LoadPrefs();

			EditorApplication.update -= LoadPrefs;
		}

		[MenuItem("Jake/Study Three Conditions/Fire", priority = -100)]
		static void SetFire()
		{
			var fire = EditorPrefs.GetBool("fire", false);
			EditorPrefs.SetBool("fire", !fire);

			Menu.SetChecked("Jake/Study Three Conditions/Fire", !fire);

			SetCondition();
		}

		[MenuItem("Jake/Study Three Conditions/Save Girl", priority = -80)]
		static void SaveGirl()
		{
			var girl = EditorPrefs.GetBool("girl", false);
			if (girl == false)
			{
				EditorPrefs.SetBool("girl", true);
				EditorPrefs.SetBool("adults", false);
				EditorPrefs.SetBool("noinfluence", false);
			}

			Menu.SetChecked("Jake/Study Three Conditions/Save Girl", true);
			Menu.SetChecked("Jake/Study Three Conditions/Save Adults", false);
			Menu.SetChecked("Jake/Study Three Conditions/No Influence", false);

			SetCondition();
		}

		[MenuItem("Jake/Study Three Conditions/Save Adults", priority = -80)]
		static void SaveAdults()
		{
			var adults = EditorPrefs.GetBool("adults", false);
			if (adults == false)
			{
				EditorPrefs.SetBool("girl", false);
				EditorPrefs.SetBool("adults", true);
				EditorPrefs.SetBool("noinfluence", false);
			}

			Menu.SetChecked("Jake/Study Three Conditions/Save Girl", false);
			Menu.SetChecked("Jake/Study Three Conditions/Save Adults", true);
			Menu.SetChecked("Jake/Study Three Conditions/No Influence", false);

			SetCondition();
		}

		[MenuItem("Jake/Study Three Conditions/No Influence", priority = -80)]
		static void NoInfluence()
		{
			var noInfluence = EditorPrefs.GetBool("noinfluence", false);
			if (noInfluence == false)
			{
				EditorPrefs.SetBool("girl", false);
				EditorPrefs.SetBool("adults", false);
				EditorPrefs.SetBool("noinfluence", true);
			}

			Menu.SetChecked("Jake/Study Three Conditions/Save Girl", false);
			Menu.SetChecked("Jake/Study Three Conditions/Save Adults", false);
			Menu.SetChecked("Jake/Study Three Conditions/No Influence", true);

			SetCondition();
		}

		[MenuItem("Jake/Study Three Conditions/Swap Elevators", priority = -60)]
		static void SwapElevators()
		{
			var swapElevators = EditorPrefs.GetBool("swapelevators", false);
			EditorPrefs.SetBool("swapelevators", !swapElevators);

			Menu.SetChecked("Jake/Study Three Conditions/Swap Elevators", !swapElevators);

			SetCondition();
		}

		[MenuItem("Jake/Study Three Conditions/Swap Hands", priority = -40)]
		static void SwapHands()
		{
			var swapHands = EditorPrefs.GetBool("swaphands", false);
			EditorPrefs.SetBool("swaphands", !swapHands);

			Menu.SetChecked("Jake/Study Three Conditions/Swap Hands", !swapHands);

			SetCondition();
		}

		private static void SetCondition()
		{
			var fire = EditorPrefs.GetBool("fire", false);
			var girl = EditorPrefs.GetBool("girl", false);
			var adults = EditorPrefs.GetBool("adults", false);

			var influenceType = girl ? InfluenceType.Girl : (adults ? InfluenceType.Adults : InfluenceType.None);

			SetCondition(fire, influenceType, EditorPrefs.GetBool("swapelevators", false), EditorPrefs.GetBool("swaphands", false));
		}

		private static void SetCondition(bool fire, InfluenceType influenceType, bool swapElevators, bool swapHands)
		{
			var data = GameObject.Find("Condition Data").GetComponent<Jake.Studies.Four.ConditionData>();
			if (fire)
			{
				if (influenceType == InfluenceType.None)
				{
					data.timelineManager.SetTimeline(3, data.fireNoGroup);
				}
				else
				{
					data.timelineManager.SetTimeline(3, data.fireGroup);
				}
			}
			else
			{
				if (influenceType == InfluenceType.None)
				{
					data.timelineManager.SetTimeline(3, data.noFireNoGroup);
				}
				else
				{
					data.timelineManager.SetTimeline(3, data.noFireGroup);
				}
			}

			switch (influenceType)
			{
				case InfluenceType.Adults:
					data.alex.groups[14].animations[2] = data.alexSaveGroup;
					data.sophie.groups[14].animations[2] = data.sophieSaveGroup;
					data.steve.groups[14].animations[2] = data.steveSaveGroup;
					break;
				case InfluenceType.Girl:
					data.alex.groups[14].animations[2] = data.alexSaveGirl;
					data.sophie.groups[14].animations[2] = data.sophieSaveGirl;
					data.steve.groups[14].animations[2] = data.steveSaveGirl;
					break;
			}

			if (swapElevators)
			{
				data.adult1.target = data.leftElevatorTargets.Find("Top Left");
				data.adult2.target = data.leftElevatorTargets.Find("Top Right");
				data.adult3.target = data.leftElevatorTargets.Find("Middle");
				data.adult4.target = data.leftElevatorTargets.Find("Bottom Left");
				data.adult5.target = data.leftElevatorTargets.Find("Bottom Right");

				data.girl.target = data.rightElevatorTargets.Find("Middle");
			}
			else
			{
				data.adult1.target = data.rightElevatorTargets.Find("Top Left");
				data.adult2.target = data.rightElevatorTargets.Find("Top Right");
				data.adult3.target = data.rightElevatorTargets.Find("Middle");
				data.adult4.target = data.rightElevatorTargets.Find("Bottom Left");
				data.adult5.target = data.rightElevatorTargets.Find("Bottom Right");

				data.girl.target = data.leftElevatorTargets.Find("Middle");
			}

			data.study.useRightHand = !swapHands;
		}
	}
}
