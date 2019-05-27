using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour {

    public TextAsset JSON;

	void Awake ()
    {
        JsonConvert.DefaultSettings = () => new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new TileSerializer(), new InteractableSerializer(), new AbilitySerializer() }
        };

        Current.core = this;
        Current.game = new Game();
        Current.party = new Party();
        Current.windowStack = new WindowStack();
        Current.game.Level = Level.InstantiateLevel(JSON.text);
    }
	
	void Update ()
    {
        Find.WindowStack.WindowsUpdate();
        WindowStackManager.Update();
        ScreenUtility.ApplyScale();

        if(Input.GetKeyDown(KeyCode.P))
        {
            Objective i = new Objective("Test 1", "desc");
            i.AddSubObjective(new SubObjective_KillAll("Allo", "Pinto", true));
            i.AddSubObjective(new SubObjective_ProtectAll("Gato", "A l'eau", true));
            Find.Game.Objective = i;
        }
    }

    void OnGUI()
    {
        Find.WindowStack.WindowsOnGUI();
    }
}
