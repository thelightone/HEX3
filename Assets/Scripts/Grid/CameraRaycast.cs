using Unity.VisualScripting;
using UnityEngine;


public enum TurnState
{
    SelectUnit,
    MoveOrAttack,
    ChooseSkillHex,
    ChooseSkillUnit,
    Pause
}

public class CameraRaycast : MonoBehaviour
{
    private Camera _mainCamera;
    private HexTile _targetHex;
    private UnitMoveController _targetUnit;
    private BeAim _activeAim;
    private Vector3 _mousePos;
    private Transform objectHit;

    public static TurnState turnState = TurnState.SelectUnit;

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    void Update()
    {
        RaycastHit hit;
        _mousePos = Input.mousePosition;
        var ray = _mainCamera.ScreenPointToRay(_mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            objectHit = hit.transform;

            switch (turnState)
            {
                case TurnState.SelectUnit:
                    SelectUnitHandler();
                    break;
                case TurnState.MoveOrAttack:
                    MoveOrAttackHandler();
                    break;
                case TurnState.ChooseSkillHex:
                    ChooseSkillHexHandler();
                    break;
                case TurnState.ChooseSkillUnit:
                    ChooseSkillUnitHandler();
                    break;
                case TurnState.Pause:
                    PauseHandler();
                    break;
            }
           
        }
    }

    private void SelectUnitHandler()
    {
        if (objectHit.GetComponentInParent<UnitMoveController>())
        {
            _targetUnit = objectHit.GetComponentInParent<UnitMoveController>();

            if (_targetUnit.player == BattleSystem.Instance.curPlayer && _targetUnit.actions > 0)
            {
                TileManager.Instance.LightUnit(_targetUnit);

                if (Input.GetMouseButtonUp(0))
                {
                    TileManager.Instance.ChooseUnit(_targetUnit);
                    turnState = TurnState.MoveOrAttack;
                }
            }
        }
        else
        {
            TileManager.Instance.DisLightUnit();
        }
    }

    private void MoveOrAttackHandler()
    {
        if (objectHit.GetComponentInParent<UnitMoveController>())
        {
            _targetUnit = objectHit.GetComponentInParent<UnitMoveController>();

            if (_targetUnit.player != BattleSystem.Instance.curPlayer && objectHit.GetComponent<AttackSector>())
            {
                _activeAim = _targetUnit.GetComponentInChildren<BeAim>();
                _activeAim.LightAim(objectHit.parent.gameObject);

                if (Input.GetMouseButtonUp(0))
                {
                    _activeAim.Attack(_activeAim, objectHit.parent.gameObject);
                }
            }
            else if (_targetUnit.player == BattleSystem.Instance.curPlayer && _targetUnit.actions > 0)
            {
                TileManager.Instance.LightUnit(_targetUnit);

                if (Input.GetMouseButtonUp(0))
                {
                    TileManager.Instance.ChooseUnit(_targetUnit);
                    turnState = TurnState.MoveOrAttack;
                }
            }
        }
        else
        {
           _activeAim?.DislightAim(1);

            if (objectHit.parent.GetComponent<HexTile>())
            {
                _targetHex = objectHit.parent.GetComponent<HexTile>();
                _targetHex.Highlight();

                if (Input.GetMouseButtonUp(0))
                {
                    _targetHex.Select();                    
                }
            }
        }
    }

    private void ChooseSkillHexHandler()
    {
        if (objectHit.parent.GetComponent<HexTile>())
        {
            _targetHex = objectHit.parent.GetComponent<HexTile>();
            TileManager.Instance.ChangeCursor(_targetHex, TileManager.CursorState.skill);

            if (Input.GetMouseButtonUp(0))
            {                
                TileManager.Instance.currentSkill.AddHex(_targetHex);
                TileManager.Instance.currentSkill.OnActivate();
                TileManager.Instance.DisSelect();

                TileManager.Instance.activeUnit.FinishMove();
                turnState = TurnState.SelectUnit;
            }
        }
    }

    private void ChooseSkillUnitHandler()
    {
        if (objectHit.GetComponentInParent<UnitMoveController>() && objectHit.GetComponent<AttackSector>())
        {
            _targetUnit = objectHit.GetComponentInParent<UnitMoveController>();

            if ((_targetUnit.player != BattleSystem.Instance.curPlayer && TileManager.Instance.currentSkill.aimPlayer == SkillParent.AimPlayer.enemy)
                || (_targetUnit.player == BattleSystem.Instance.curPlayer && TileManager.Instance.currentSkill.aimPlayer == SkillParent.AimPlayer.self)
                || (TileManager.Instance.currentSkill.aimPlayer == SkillParent.AimPlayer.both))
            {
                _activeAim = _targetUnit.GetComponentInChildren<BeAim>();
                _activeAim.LightSkill();

                if (Input.GetMouseButtonUp(0))
                {
                    TileManager.Instance.currentSkill.unitAims.Add(_targetUnit.fightController);
                    TileManager.Instance.currentSkill.OnActivate();
                    _activeAim.DislightAim(1);

                    TileManager.Instance.activeUnit.FinishMove();

                    turnState = TurnState.SelectUnit;
                }
            }       
        }
        else
        {
            _activeAim.DislightAim(1);
        }
    }

   private void PauseHandler()
    {

    }
}

