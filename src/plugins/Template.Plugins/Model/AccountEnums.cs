namespace Template.Plugins.Model
{
    /// <summary>
    /// OptionSet <c>accountcategorycode</c>. Os valores devem bater com os do Dataverse.
    /// (Em projeto real, gere com pac modelbuilder; aqui são exemplos.)
    /// </summary>
    public enum AccountCategory
    {
        PreferredCustomer = 1,
        Standard = 2
    }

    /// <summary>statecode da account.</summary>
    public enum AccountState
    {
        Active = 0,
        Inactive = 1
    }
}
