﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Bindable.Linq.SampleApplication.RepositorySample
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="AccountsSample")]
	public partial class AccountsDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    #endregion
		
		public AccountsDataContext() : 
				base(global::Bindable.Linq.SampleApplication.Properties.Settings.Default.AccountsSampleConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public AccountsDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountsDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountsDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public AccountsDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Account> Accounts
		{
			get
			{
				return this.GetTable<Account>();
			}
		}
	}
	
	[Table(Name="dbo.Accounts")]
	public partial class Account
	{
		
		private System.Guid _AccountID;
		
		private string _AccountName;
		
		private bool _IsActive;
		
		public Account()
		{
		}
		
		[Column(Storage="_AccountID", DbType="UniqueIdentifier NOT NULL")]
		public System.Guid AccountID
		{
			get
			{
				return this._AccountID;
			}
			set
			{
				if ((this._AccountID != value))
				{
					this._AccountID = value;
				}
			}
		}
		
		[Column(Storage="_AccountName", DbType="NVarChar(100) NOT NULL", CanBeNull=false)]
		public string AccountName
		{
			get
			{
				return this._AccountName;
			}
			set
			{
				if ((this._AccountName != value))
				{
					this._AccountName = value;
				}
			}
		}
		
		[Column(Storage="_IsActive", DbType="Bit NOT NULL")]
		public bool IsActive
		{
			get
			{
				return this._IsActive;
			}
			set
			{
				if ((this._IsActive != value))
				{
					this._IsActive = value;
				}
			}
		}
	}
}
#pragma warning restore 1591