using Cuit.Control;
using Cuit.Screen;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using YetAnotherTextRpg.Controls;
using YetAnotherTextRpg.Models;
using YetAnotherTextRpg.Managers;
using YetAnotherTextRpg.Helpers;
using YetAnotherTextRpg.Game;

namespace YetAnotherTextRpg.Forms
{
    public class EncounterForm : SingleFocusControlFormScreen
    {
        enum BattleActions
        {
            Attack,
            Defend,
            Run,
            Continue
        }

        enum EncounterState
        {
            Default,
            RunAway, 
            PlayerWon,
            PlayerDied
        }

        private Listbox<BattleActions> _battleActionListBox;
        private OutputBox _outputBox;
        private Image _playerImage;
        private Image _enemyImage;
        private Progressbar _playerHealth;
        private Progressbar _enemyHealth;

        private readonly Encounter _encounter;
        private Enemy _currentEnemy;
        private bool _enemyIsStunned = false;
        private EncounterState _encounterState;

        public EncounterForm(Encounter encounter)
        {
            _encounter = encounter;
        }

        public override void InstantiateComponents()
        {
            base.InstantiateComponents();

            _battleActionListBox = new Listbox<BattleActions>(5, 1);
            _battleActionListBox.Width = 15;
            _battleActionListBox.Height = 6;
            RegisterControl(_battleActionListBox);

            _outputBox = new OutputBox(24, 2);
            _outputBox.Width = Application.Width - 25;
            _outputBox.MaxRows = 6;
            RegisterControl(_outputBox);

            _playerHealth = new Progressbar(6, 9);
            _playerHealth.Width = 40;
            RegisterControl(_playerHealth);

            _playerImage = new Image(6, 12);
            RegisterControl(_playerImage);

            _enemyHealth = new Progressbar(55, 9);
            _enemyHealth.Width = 40;
            RegisterControl(_enemyHealth);

            _enemyImage = new Image(55, 12);
            RegisterControl(_enemyImage);

            _battleActionListBox.SelectionChanged += _battleActionListBox_SelectionChanged;

            _playerImage.SetImageFromFile("Resources/Encounters/Images/player.asc");

            _playerHealth.Maximum = GameManager.Instance.State.MaxHealth;
            _playerHealth.Value = GameManager.Instance.State.Health;
        }

        public override void OnGotFocus()
        {
            base.OnGotFocus();

            _outputBox.AddOutput(_encounter.InitialText);
            NextEnemy();

            _battleActionListBox.Items.Add(BattleActions.Attack);
            _battleActionListBox.Items.Add(BattleActions.Defend);

            if (_encounter.CanRun)
                _battleActionListBox.Items.Add(BattleActions.Run);
        }

        private void _battleActionListBox_SelectionChanged(object sender, BattleActions action)
        {
            _battleActionListBox.ClearSelection();

            bool playerIsDefending = false;
            var playerCombatProfile = GameManager.Instance.State.GetPlayerCombatProfile();

            //TODO: Take equipped items into consideration

            switch (action)
            {
                case BattleActions.Attack:
                    var result = CombatHelper.ResolveAttack(playerCombatProfile, _currentEnemy);
                    switch (result.Result)
                    {
                        case AttackResultCode.Missed:
                            _outputBox.AddOutput($"Player attack missed enemy {_currentEnemy.Name}");
                            break;
                        case AttackResultCode.ArmorBlocked:
                            _outputBox.AddOutput($"Enemy {_currentEnemy.Name}'s armor blocked the attack");
                            break;
                        case AttackResultCode.DidDamage:
                            _outputBox.AddOutput($"Player did {result.DamageDone} damage to {_currentEnemy.Name}");

                            _currentEnemy.Health -= result.DamageDone;
                            SetHealth(_enemyHealth, Math.Max(0, _currentEnemy.Health), false);
                            break;
                    }

                    break;
                case BattleActions.Defend:
                    playerIsDefending = true;
                    playerCombatProfile.Defense = Convert.ToInt32(playerCombatProfile.Defense * 1.3);
                    playerCombatProfile.Armor = Convert.ToInt32(playerCombatProfile.Armor * 1.2);

                    _outputBox.AddOutput("Player is defending");

                    break;
                case BattleActions.Run:
                    if (!string.IsNullOrEmpty(_encounter.RunConditional))
                    {
                        var conditionalResult = EmbeddedFunctionsHelper.Conditional(_encounter.RunConditional);

                        if (!string.IsNullOrEmpty(conditionalResult.Output))
                        {
                            _outputBox.AddOutput(conditionalResult.Output);
                        }    

                        if (conditionalResult.Success)
                        {
                            TriggerContinue(EncounterState.RunAway);
                        }
                    }
                    else
                    {
                        TriggerContinue(EncounterState.RunAway);
                    }

                    return;

                case BattleActions.Continue:
                    switch (_encounterState)
                    {
                        case EncounterState.Default:
                            break;
                        case EncounterState.RunAway:
                            EncounterManager.Instance.Exit();
                            break;
                        case EncounterState.PlayerWon:
                            EncounterManager.Instance.PlayerWon(_encounter);
                            break;
                        case EncounterState.PlayerDied:
                            EncounterManager.Instance.PlayerDied(_encounter);
                            break;
                    } 

                    return;
            }

            if (_currentEnemy.Health > 0)
            {
                if (!_enemyIsStunned)
                {
                    var result = CombatHelper.ResolveAttack(_currentEnemy, playerCombatProfile);
                    switch (result.Result)
                    {
                        case AttackResultCode.Missed:
                            _outputBox.AddOutput($"{_currentEnemy.Name}'s attack missed player");
                            break;
                        case AttackResultCode.ArmorBlocked:
                            _outputBox.AddOutput($"Player's armor blocked the attack");
                            break;
                        case AttackResultCode.DidDamage:
                            _outputBox.AddOutput($"{_currentEnemy.Name} did {result.DamageDone} damage to player");

                            GameManager.Instance.State.Health -= result.DamageDone;
                            SetHealth(_playerHealth, GameManager.Instance.State.Health, false);
                            break;
                    }
                }
                {
                    _enemyIsStunned = false;
                    _outputBox.AddOutput($"{_currentEnemy.Name}'s was temporarily stunned");
                }


                if (playerIsDefending && DiceHelper.RollD6() >= 5)
                {
                    _enemyIsStunned = true;
                    _outputBox.AddOutput($"{_currentEnemy.Name}'s is stunned");
                }
            }
            else
            {
                _outputBox.AddOutput($"{_currentEnemy.Name} died!");

                if (_encounter.Enemies.Any())
                {
                    NextEnemy();
                    _outputBox.AddOutput($"New enemy {_currentEnemy.Name} appeared!");
                }
                else
                {
                    _outputBox.AddOutput($"Victory! Player successfully defeated encounter!");
                    TriggerContinue(EncounterState.PlayerWon);
                }
            }

            if (GameManager.Instance.State.Health <= 0)
            {
                GameManager.Instance.State.Health = 0;
                SetHealth(_playerHealth, GameManager.Instance.State.Health, false);

                _outputBox.AddOutput("Player died :'(");
                TriggerContinue(EncounterState.PlayerDied);
            }
        }

        private void TriggerContinue(EncounterState encounterState)
        {
            _encounterState = encounterState;

            _battleActionListBox.Items.Clear();
            _battleActionListBox.Items.Add(BattleActions.Continue);
        }

        private void SetHealth(Progressbar progressbar, int health, bool isMax)
        {
            if (isMax)
                progressbar.Maximum = health;

            progressbar.Value = health;
        }

        private void NextEnemy()
        {
            if (!_encounter.Enemies.Any())
                return;

            _currentEnemy = _encounter.Enemies.First();
            _encounter.Enemies.RemoveAt(0);

            SetHealth(_enemyHealth, _currentEnemy.Health, true);
            _enemyImage.SetImageFromFile(_currentEnemy.ImagePath);
        }
    }
}
