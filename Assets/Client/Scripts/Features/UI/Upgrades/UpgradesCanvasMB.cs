using Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesCanvasMB : MonoBehaviour
{
    private GameState _gameState;
    [SerializeField] private UiUpgradeMB Attack;
    [SerializeField] private UiUpgradeMB AttackSpeed;
    [SerializeField] private UiUpgradeMB Health;
    [SerializeField] private UiUpgradeMB HealthRecovery;

    private UiUpgradeMB[] _allUpgrades;

    private void Start()
    {
        _gameState = GameState.Get();

        _allUpgrades = new UiUpgradeMB[] { Attack, AttackSpeed, Health, HealthRecovery };

        GameState.OnMoneyValueChanged += CheckCostValue;

        SetDefaultValues();
    }

    private void OnDestroy()
    {
        GameState.OnMoneyValueChanged -= CheckCostValue;
    }

    private void SetDefaultValues()
    {
        Attack.Level = 1;
        Attack.Cost = _gameState.UpgradesConfig.AttackCostPerLevel;
        Attack.CostText.text = Attack.Cost.ToString();
        Attack.Value = _gameState.PlayerConfig.StartDamage;
        Attack.ValueText.text = Attack.Value.ToString();

        AttackSpeed.Level = 1;
        AttackSpeed.Cost = _gameState.UpgradesConfig.AttackSpeedCostPerLevel;
        AttackSpeed.CostText.text = AttackSpeed.Cost.ToString();
        AttackSpeed.Value = _gameState.PlayerConfig.StartAttackSpeed;
        AttackSpeed.ValueText.text = AttackSpeed.Value.ToString();

        Health.Level = 1;
        Health.Cost = _gameState.UpgradesConfig.HealthCostPerLevel;
        Health.CostText.text = Health.Cost.ToString();
        Health.Value = _gameState.PlayerConfig.StartHealth;
        Health.ValueText.text = Health.Value.ToString();

        HealthRecovery.Level = 1;
        HealthRecovery.Cost = _gameState.UpgradesConfig.HealthRecoveryCostPerLevel;
        HealthRecovery.CostText.text = HealthRecovery.Cost.ToString();
        HealthRecovery.Value = _gameState.PlayerConfig.StartHealthRecovery;
        HealthRecovery.ValueText.text = HealthRecovery.Value.ToString();
    }

    private void CheckCostValue(int moneyCount)
    {
        foreach (var upgrade in _allUpgrades)
        {
            upgrade.EnhanceButton.interactable = upgrade.Cost <= moneyCount;
        }
    }

    public void UpgradeHealth()
    {
        Health.Level++;
        Health.LevelText.text = "Lv "+ Health.Level.ToString();
        Health.Cost += _gameState.UpgradesConfig.HealthCostPerLevel;
        Health.CostText.text = Health.Cost.ToString();
        Health.Value += _gameState.UpgradesConfig.HealthPerLevel;
        Health.ValueText.text = Health.Value.ToString();

        UpgradePlayer(UpgradeType.Health, _gameState.UpgradesConfig.HealthPerLevel, Health.Cost - _gameState.UpgradesConfig.HealthCostPerLevel);
    }

    public void UpgradeHealthRecovery()
    {
        HealthRecovery.Level++;
        HealthRecovery.LevelText.text = "Lv " + HealthRecovery.Level.ToString();
        HealthRecovery.Cost += _gameState.UpgradesConfig.HealthRecoveryCostPerLevel;
        HealthRecovery.CostText.text = HealthRecovery.Cost.ToString();
        HealthRecovery.Value += _gameState.UpgradesConfig.HealthRecoveryPerLevel;
        HealthRecovery.ValueText.text = HealthRecovery.Value.ToString();

        UpgradePlayer(UpgradeType.HealthRecovery, _gameState.UpgradesConfig.HealthRecoveryPerLevel, HealthRecovery.Cost - _gameState.UpgradesConfig.HealthRecoveryCostPerLevel);
    }

    public void UpgradeAttack()
    {
        Attack.Level++;
        Attack.LevelText.text = "Lv " + Attack.Level.ToString();
        Attack.Cost += _gameState.UpgradesConfig.AttackCostPerLevel;
        Attack.CostText.text = Attack.Cost.ToString();
        Attack.Value += _gameState.UpgradesConfig.AttackPerLevel;
        Attack.ValueText.text = Attack.Value.ToString();

        UpgradePlayer(UpgradeType.Attack, _gameState.UpgradesConfig.AttackPerLevel, Attack.Cost - _gameState.UpgradesConfig.AttackCostPerLevel);
    }

    public void UpgradeAttackSpeed()
    {
        AttackSpeed.Level++;
        AttackSpeed.LevelText.text = "Lv " + AttackSpeed.Level.ToString();
        AttackSpeed.Cost += _gameState.UpgradesConfig.AttackSpeedCostPerLevel;
        AttackSpeed.CostText.text = AttackSpeed.Cost.ToString();
        AttackSpeed.Value += _gameState.UpgradesConfig.AttackSpeedPerLevel;
        AttackSpeed.ValueText.text = AttackSpeed.Value.ToString();

        UpgradePlayer(UpgradeType.AttackSpeed, _gameState.UpgradesConfig.AttackSpeedPerLevel, Attack.Cost - _gameState.UpgradesConfig.AttackSpeedCostPerLevel);
    }

    private void UpgradePlayer(UpgradeType upgradeType, float value, int cost)
    {
        _gameState.MoneyValueChange(-cost);

        _gameState.UpgradePlayer(upgradeType, value);
    }
}
