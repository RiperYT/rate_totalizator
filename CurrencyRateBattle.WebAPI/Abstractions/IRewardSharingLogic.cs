namespace CurrencyRateBattle.WebAPI.Abstractions;

public interface IRewardSharingLogic
{
    Stack<decimal> CalculateRewardsWithNoLosers(decimal totalReward, int numberOfRewards, decimal rate);

    Stack<decimal> CalculateRewards(decimal totalReward, int numberOfRewards, decimal rate);
}
