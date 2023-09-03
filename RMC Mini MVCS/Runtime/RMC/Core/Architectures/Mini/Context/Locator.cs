using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine;

namespace RMC.Core.Architectures.Mini.Context
{

	/// <summary>
	/// The Locator manages the storage, lookup,
	/// and retrieval of <see cref="IItem"/> objects
	/// of arbitrary type. 
	/// </summary>
	public class Locator<IItem>
		where IItem : class
	{
		public class AddItemCompletedUnityEvent : UnityEvent<IItem> {}

		//  Events ----------------------------------------
		public readonly AddItemCompletedUnityEvent OnAddItemCompleted = new AddItemCompletedUnityEvent();
		
		//  Fields ----------------------------------------
		private List<IItem> _items = new List<IItem>();
        		
		//  Methods ---------------------------------------
		public void AddItem<SubType> (SubType item) where SubType : class, IItem 
		{
			if (HasItem<SubType>())
			{
				// Allow MAX 0 or 1 instance of T
				throw new Exception("AddItem() failed. Item already added. Call HasItem<T>() first.");
			}

			_items.Add(item);
			OnAddItemCompleted.Invoke(item);
		}
		
		public bool HasItem<SubType>() 
			where SubType : class, IItem 
		{
			return GetItem<SubType>() != null;
		}
		
		public SubType GetItem<SubType>() 
			where SubType : class, IItem 
		{
			foreach(IItem item in _items)
			{
				if(item.GetType() == typeof(SubType))
				{
					return item as SubType;
				}
			}
			
			return null;
		}
		
		public IItem GetItem(Type type)
		{
			return _items.FirstOrDefault(item => item.GetType() == type);
		}

		public void RemoveItem(IItem item)
		{
			if (!HasItem<IItem>())
			{
				throw new Exception("RemoveItem() failed. Must call HasItem<T>() first.");
			}

			_items.Remove(item);
		}
	}
}