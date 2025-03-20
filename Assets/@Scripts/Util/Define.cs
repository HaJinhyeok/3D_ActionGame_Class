using UnityEngine;

public class Define
{
    #region Input
    public const string MouseX = "Mouse X";
    public const string MouseY = "Mouse Y";
    public const string MouseScroll = "Mouse ScrollWheel";
    public const string Horizontal = "Horizontal";
    public const string Vertical = "Vertical";
    #endregion

    #region Animation
    public readonly static int Speed = Animator.StringToHash("Speed");
    public readonly static int ComboCount = Animator.StringToHash("ComboCount");
    public readonly static int isNextCombo = Animator.StringToHash("isNextCombo");
    public readonly static int isAttacking = Animator.StringToHash("isAttacking");
    #endregion

    #region Layer
    public const string Player = "Player";
    public const string Enemy = "Enemy";
    public const string Obstacle = "Obstacle";
    #endregion
}
