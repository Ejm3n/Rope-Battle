using BG.UI.Main;
using UnityEngine;
public class UIGridPanel : Panel
{
    [SerializeField] private UIButtonText addButton;
    [SerializeField] private UIButton startButton;
    [SerializeField] private UIMergePopup mergePopup;
    [SerializeField] private UIRenderSpace renderSpace;
    private GridArea area;
    private GridMover mover;
    private RopeHolder ropeHolder;
    private ControllerStateMachine controller;
    private Character toSpawn;
    private int childCount = 1;
    private ulong moneyForCharacter;

    public UIButton AddButton => addButton;
    public UIButton StartButton => startButton;

    private void Awake()
    {
        LevelManager.Default.OnLevelLoaded += OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad += OnLevelPreLoad;
    }
    protected override void Start()
    {
        addButton.AddListener(Spawn);
        startButton.AddListener(OnStartButtonClick);
        mergePopup.Hide(false);
        mergePopup.OnHide += renderSpace.Stop;
        toSpawn = CharacterHolder.Default[0];
        base.Start();
    }
    private void OnDestroy()
    {
        LevelManager.Default.OnLevelLoaded -= OnLevelLoaded;
        LevelManager.Default.OnLevelPreLoad -= OnLevelPreLoad;
        mergePopup.OnHide -= renderSpace.Stop;
    }
    //public override void ShowPanel(bool animate = true)
    //{
    //    base.ShowPanel(animate);
    //    string boss;
    //    if (PartyManager.Default.GetLastBossSpecifer(out boss))
    //    {
    //        Character character;
    //        if (CharacterHolder.Default.TryFindCharcterBySpecifier(boss,out character))
    //        {
    //            renderSpace.RenderCharacter(character);
    //            mergePopup.SetInfo(character);
    //            mergePopup.Show();
    //        }
    //    }
    //}
    //public override void HidePanel()
    //{
    //    base.HidePanel();
    //    renderSpace.Stop();
    //}
    private void OnLevelLoaded(LevelMaster levelMaster)
    {
        area = levelMaster.GetArea(0);
        mover = levelMaster.Mover;
        ropeHolder = levelMaster.RopeHolder;
        controller = levelMaster.ControllerMachine;
        area.OnAdd += OnAddArea;
        mover.OnSelect += OnSlelect;
        mover.OnMoveComplete += OnMove;
        mover.OnMergeComplete += OnMerge;
        UpdateBuyCharacter();
        UpdateButtons();
        //if (!_hideOnStart)
        //    levelMaster.GameStart();
    }
    private void OnLevelPreLoad(LevelMaster levelMaster)
    {
        if (LevelManager.Default.HasCurrent)
        {
            area.OnAdd -= OnAddArea;
            mover.OnMoveComplete -= OnMove;
            mover.OnSelect -= OnSlelect;
            mover.OnMergeComplete -= OnMerge;
            UpdateButtons();
        }

    }


    [ContextMenu("Spawn")]
    private void Spawn()
    {
        if (MoneyService.Default.GetMoney() >= moneyForCharacter && !mover.InWork)
        {
            //area.SpawnCharacter(CharacterHolder.Default[0]);
            //Character toSpawn = CharacterHolder.Default[0];
            //GridArea.Cell cell;
            //if(area.GetWeakestCell(out cell))
            //{
            //    toSpawn = CharacterHolder.Default.GetActiveCharacter();
            //    var week = cell.character;
            //    if (week.Power < toSpawn.Power)
            //        toSpawn = week;
            //}

            area.SpawnCharacter(toSpawn);
            MoneyService.Default.SpendMoney(moneyForCharacter);
            PartyManager.Default.IncreaseBuyCount(childCount);
            PartyManager.Default.SaveParty(area);
            addButton.Click();
            SaveManager.AddBattlersBought(1);
            UpdateBuyCharacter();
            UpdateButtons();
            //startButton.Interactable = true;
        }
    }
    private void OnStartButtonClick()
    {
        //if (Time.time < startClickTime + GameData.Default.timeWhenEndTouchToClick &&
        //        startButton.Interactable &&
        //         !mover.InMove && 
        //         !mover.InMerge &&
        //         LevelManager.Default.CurrentLevel.State == LevelMaster.LevelState.process)
        //{
        if (startButton.Interactable &&
         !mover.InMove &&
         !mover.InMerge &&
         LevelManager.Default.CurrentLevel.State == LevelMaster.LevelState.process)
        {
            PartyManager.Default.SaveParty(area);
            startButton.Interactable = false;
            addButton.Interactable = false;
            controller.Current.ForceFinish();
            UIManager.Default.CurentState = UIState.Rope;
        }
    }
    private void UpdateButtons()
    {
        addButton.Interactable = CheckAddButtonInteractable();
        addButton.UpdateText(moneyForCharacter.ToString());
        startButton.Interactable = ropeHolder.CheckEnoughCharactersOnRopes();


    }
    private bool CheckAddButtonInteractable()
    {
        return
            area.HasEmptyCells() &&
            MoneyService.Default.GetMoney() + MoneyService.Default.GetBank() >= moneyForCharacter;
    }
    private void UpdateBuyCharacter()
    {
        GridArea.Cell cell;
        if (area.GetWeakestCell(out cell))
        {
            var inh = CharacterHolder.Default.GetActive();
            var week = cell.character;
            if (week.Power < inh.character.Power)
            {
                toSpawn = week;
            }
            else
            {
                toSpawn = inh.character;
                //moneyForCharacter *= inh.priceMultiplayer;
            }
        }
        else
        {
            toSpawn = CharacterHolder.Default[0];
        }
        childCount = CharacterHolder.Default.GetChildCount(toSpawn);
        moneyForCharacter = CharacterHolder.Default.GetMoneyForCharacter(childCount);

    }
    //private int MoneyForCharacter()
    //{
    //    return CharacterHolder.Default.GetMoneyForCharacter();


    //}
    private void OnAddArea(Character character)
    {
        UpdateButtons();
    }
    private void OnMove(Character character, GridArea.Cell cell)
    {
        UpdateButtons();
    }
    private void OnSlelect(Character character, GridArea.Cell cell)
    {
        startButton.Interactable = false;
        addButton.Interactable = false;
    }
    private void OnMerge(Character character, GridArea.Cell cell)
    {

        if (!PartyManager.Default.CheckPopup(character))
        {
            renderSpace.RenderCharacter(character);
            mergePopup.SetInfo(character);
            mergePopup.Show();
            PartyManager.Default.SetPopup(character);
        }
        UpdateBuyCharacter();
        UpdateButtons();

    }
}
