using System;
using NUnit.Framework;
using Rhino.Mocks;

namespace Bindable.Linq.Documentation.Tests.Behaviour
{
	/// <summary>
	/// Abstract base for behavioural context
	/// </summary>
	public abstract class BehaviourBase
	{
		private MockRepository mocks;

		public IDisposable Record
		{
			get { return Mocks.Record(); }
		}

		public IDisposable PlayBack
		{
			get { return Mocks.Playback(); }
		}

		public MockRepository Mocks
		{
			get { return mocks; }
		}

		[SetUp]
		public void Setup()
		{
			mocks = new MockRepository();
			establish_context();
		}

		[TearDown]
		public void TearDown()
		{
			after_each_spec();
		}

		protected virtual void after_each_spec()
		{
		}

		protected abstract void establish_context();

		public T Dependancy<T>() where T : class
		{
			return Mocks.DynamicMock<T>();
		}
	}
}