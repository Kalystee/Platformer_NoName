using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlegroundUI : MonoBehaviour
{
    public static BattlegroundUI singleton { get; private set; }

    private Level level { get { return Level.singleton; } }
    private BattlegroundDisplayer bgDisplayer { get { return BattlegroundDisplayer.singleton; } }
    private BattlegroundController bgController { get { return BattlegroundController.singleton; } }

    public Transform battleCharacterListVignetteTransform;
    public Transform battleCharacterPlayingVignetteTransform;
    public Transform battleCharacterPlayingAbilitiesBarTransform;

    public TMP_Text healthText;
    public TMP_Text paText;

    private Dictionary<BattleCharacter, GameObject> battleCharacterListVignetteDictionnary = new Dictionary<BattleCharacter, GameObject>();
    private GameObject vignettePrefab;
    private GameObject slotPrefab;

    private Dictionary<AbilityBase, Clickable> abilitiesButtons;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.LogError("Une instance de BattlegroundUI existe déja !");
            Destroy(this);
            return;
        }

        singleton = this;

        abilitiesButtons = new Dictionary<AbilityBase, Clickable>();

        vignettePrefab = Resources.Load<GameObject>("Prefabs/UI/Character_Vignette");
        slotPrefab = Resources.Load<GameObject>("Prefabs/UI/AbilityButton");

        battleCharacterListVignetteDictionnary = new Dictionary<BattleCharacter, GameObject>();

        level.RegisterOnDeployement(OnDeployement);
        level.RegisterOnGettingPlayingCharacter(OnGettingPlayingCharacter);
    }
    
    private void OnDeployement(BattleCharacter bc)
    {
        if (!battleCharacterListVignetteDictionnary.ContainsKey(bc))
        {
            GameObject vignette = Instantiate(vignettePrefab);
            vignette.transform.Find("Character_OutlineTeamColor").GetComponent<Image>().color = bc.teamTag.color;
            battleCharacterListVignetteDictionnary.Add(bc, vignette);

            //On réarange la liste
            battleCharacterListVignetteTransform.DetachChildren();
            Transform first = battleCharacterListVignetteDictionnary[level.battleCharacterList.First()].transform;
            first.SetParent(battleCharacterPlayingVignetteTransform, false);
            first.localScale = Vector3.one;
            
            bc.RegisterOnDeathAction(delegate ()
            {
                Destroy(vignette);
                OnKillingCharacter(bc);
            });
            bc.RegisterOnMoveAction(delegate (Int2 pos)
            {
                UpdateCharacter(bc);
            });
            bc.RegisterOnAbilityUseAction(delegate ()
            {
                UpdateCharacter(bc);
            });

            foreach (BattleCharacter bc_ in level.battleCharacterList.Skip(1).ToArray())
            {
                Transform other = battleCharacterListVignetteDictionnary[bc_].transform;
                other.SetParent(battleCharacterListVignetteTransform, false);
                other.localScale = Vector3.one;
            }

            OnGettingPlayingCharacter(bc);
        }
        else
        {
            Debug.Log("Le BattleCharacter a déjà une vignette associée");
        }
    }

    public void OnKillingCharacter(BattleCharacter bc)
    {
        if (battleCharacterListVignetteDictionnary.ContainsKey(bc))
        {
            Destroy(battleCharacterListVignetteDictionnary[bc]);
            battleCharacterListVignetteDictionnary.Remove(bc);
        }
    }

    public void UpdateCharacter(BattleCharacter bc)
    {
        paText.text = bc.useablePA.ToString();
        healthText.text = bc.remainingHealth.ToString();

        foreach (AbilityBase abi in abilitiesButtons.Keys)
        {
            Clickable click = abilitiesButtons[abi];
            click.interactable = bc.CanUseAbility(abi);
        }
    }

    private void OnGettingPlayingCharacter(BattleCharacter bc)
    {
        battleCharacterPlayingVignetteTransform.GetChild(0).SetParent(battleCharacterListVignetteTransform, false);
        battleCharacterListVignetteDictionnary[bc].transform.SetParent(battleCharacterPlayingVignetteTransform, false);

        foreach(Transform child in battleCharacterPlayingAbilitiesBarTransform)
        {
            Destroy(child.gameObject);
        }

        abilitiesButtons = new Dictionary<AbilityBase, Clickable>();

        if (bc.teamTag == TeamTag.Ally)
        {
            foreach (AbilityBase abi in bc.character.useableAbilities)
            {
                GameObject slot = Instantiate(slotPrefab);
                slot.GetComponentInChildren<Image>().sprite = abi.Sprite;
                slot.transform.SetParent(battleCharacterPlayingAbilitiesBarTransform, false);
                Clickable clickable = slot.GetComponent<Clickable>();
                clickable.AddLeftClick(delegate()
                {
                    bgController.SelectAbility(abi);
                });
                abilitiesButtons.Add(abi, clickable);
            }
        }

        UpdateCharacter(bc);
    }
}
