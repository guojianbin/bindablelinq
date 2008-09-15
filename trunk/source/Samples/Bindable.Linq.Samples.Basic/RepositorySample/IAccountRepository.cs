using Bindable.Linq.Interfaces;

namespace Bindable.Linq.SampleApplication.RepositorySample
{
    public interface IAccountRepository
    {
        IBindableCollection<Account> AllAccounts { get; }
        IBindableCollection<Account> ActiveAccounts { get; }
        IBindableCollection<Account> InactiveAccounts { get; }
        void CreateAccount(Account account);
    }
}