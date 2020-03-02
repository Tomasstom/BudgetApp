namespace BudgetApp.Common.Identity
{
    public interface ICurrentUserContext
    {
        string Id { get; }
        string UserName { get; }
    }
}