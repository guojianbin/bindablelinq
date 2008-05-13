//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Collections.ObjectModel;

//namespace Bindable.Linq.SampleApplication.RepositorySample
//{
//    public class AccountRepository : IAccountRepository
//    {
//        private InstanceRegistry<Account> _accountRegistry;
//        private IBindableCollection<Account> _allAccountsCache;
//        private IBindableCollection<Account> _activeAccountsCache;
//        private IBindableCollection<Account> _inactiveAccountsCache;

//        public AccountRepository()
//        {
//            _accountRegistry = new InstanceRegistry<Account>();
//        }

//        public IBindableCollection<Account> AllAccounts
//        {
//            get
//            {
//                if (_allAccountsCache == null)
//                {
//                    _allAccountsCache = _accountRegistry;
//                }

//                AccountsDataContext databaseDataContext = new AccountsDataContext();
//                return _allAccountsCache
//                    .WithOnDemand(
//                        _accountRegistry,
//                        databaseDataContext.Accounts
//                        );
//            }
//        }

//        public IBindableCollection<Account> ActiveAccounts
//        {
//            get
//            {
//                if (_activeAccountsCache == null)
//                {
//                    _activeAccountsCache = _accountRegistry.Where(c => c.IsActive == true);
//                }

//                AccountsDataContext databaseDataContext = new AccountsDataContext();
//                return _activeAccountsCache
//                    .WithOnDemand(
//                        _accountRegistry,
//                        databaseDataContext.Accounts.Where(c => c.IsActive == true)
//                        );
//            }
//        }

//        public IBindableCollection<Account> InactiveAccounts
//        {
//            get
//            {
//                if (_inactiveAccountsCache == null)
//                {
//                    _inactiveAccountsCache = _accountRegistry.Where(c => c.IsActive == false);
//                }

//                AccountsDataContext databaseDataContext = new AccountsDataContext();
//                return _inactiveAccountsCache
//                    .WithOnDemand(
//                        _accountRegistry,
//                        databaseDataContext.Accounts.Where(c => c.IsActive == false)
//                        );
//            }
//        }

//        public void CreateAccount(Account account)
//        {
//            AccountsDataContext context = new AccountsDataContext();
//            context.Accounts.InsertOnSubmit(account);
//            context.SubmitChanges();
//        }
//    }
//}
