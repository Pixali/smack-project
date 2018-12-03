using System;
namespace Player
{
    public interface ISkill : IStatSource
    {
        bool CanCast(PlayerStats player);
    }
}
