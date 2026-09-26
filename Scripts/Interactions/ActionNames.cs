using System;

public readonly struct ActionNames : IEquatable<ActionNames>
{
    public static ActionNames Movement => new ActionNames("Movement");
    public static ActionNames PrimaryInteract => new ActionNames("PrimaryInteract");
    public static ActionNames SecondaryInteract => new ActionNames("SecondaryInteract");

    public string Value { get; }

    private ActionNames(string value)
    {
        Value = value;
    }

    public override string ToString() => Value;

    #region Equals Code

    public bool Equals(ActionNames other) => Value == other.Value;
    
    public override bool Equals(object obj) => obj is ActionNames other && Equals(other);
    
    public override int GetHashCode() => Value.GetHashCode();

    public static bool operator ==(ActionNames left, ActionNames right) => left.Equals(right);
    public static bool operator !=(ActionNames left, ActionNames right) => !left.Equals(right);

    #endregion
}