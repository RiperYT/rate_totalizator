using CurrencyRateBattle.WebAPI.Abstractions;

namespace CurrencyRateBattle.WebAPI.Common;

public class RewardSharingLogic : IRewardSharingLogic
{
    private const decimal MaxLossPercent = 0.95m;
    private const decimal MinWinPercent = 0.05m;

    public RewardSharingLogic() { }

    public Stack<decimal> CalculateRewardsWithNoLosers(decimal totalReward, int numberOfRewards, decimal rate)
    {
        var startReward = rate * MaxLossPercent;
        return MainLogic(totalReward, numberOfRewards, startReward);
    }

    public Stack<decimal> CalculateRewards(decimal totalReward, int numberOfRewards, decimal rate)
    {
        var startReward = rate + (rate * MinWinPercent);
        return MainLogic(totalReward, numberOfRewards, startReward);
    }

    private Stack<decimal> MainLogic(decimal totalReward, int numberOfRewards, decimal startReward)
    {
        var rewardsStack = new Stack<decimal>();

        if (numberOfRewards == 1)
        {
            rewardsStack.Push(totalReward);
            return rewardsStack;
        }

        decimal d = ((totalReward * 2 / numberOfRewards) - (2 * startReward)) / (numberOfRewards - 1);
        rewardsStack.Push(startReward);

        for (var i = 0; i < numberOfRewards - 1; i++)
        {
            startReward += d;
            rewardsStack.Push(startReward);
        }

        return rewardsStack;
    }
}
