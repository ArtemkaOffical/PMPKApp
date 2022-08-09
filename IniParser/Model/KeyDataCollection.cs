using System;
using System.Collections;
using System.Collections.Generic;

namespace IniParser.Model
{
	/// <summary>
	///     Represents a collection of Keydata.
	/// </summary>
	// Token: 0x0200000A RID: 10
	public class KeyDataCollection : ICloneable, IEnumerable<KeyData>, IEnumerable
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.KeyDataCollection" /> class.
		/// </summary>
		// Token: 0x06000055 RID: 85 RVA: 0x00002D1A File Offset: 0x00000F1A
		public KeyDataCollection() : this(EqualityComparer<string>.Default)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.KeyDataCollection" /> class with a given
		///     search comparer
		/// </summary>
		/// <param name="searchComparer">
		///     Search comparer used to find the key by name in the collection
		/// </param>
		// Token: 0x06000056 RID: 86 RVA: 0x00002D27 File Offset: 0x00000F27
		public KeyDataCollection(IEqualityComparer<string> searchComparer)
		{
			this._searchComparer = searchComparer;
			this._keyData = new Dictionary<string, KeyData>(this._searchComparer);
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.KeyDataCollection" /> class
		///     from a previous instance of <see cref="T:IniParser.Model.KeyDataCollection" />.
		/// </summary>
		/// <remarks>
		///     Data from the original KeyDataCollection instance is deeply copied
		/// </remarks>
		/// <param name="ori">
		///     The instance of the <see cref="T:IniParser.Model.KeyDataCollection" /> class 
		///     used to create the new instance.
		/// </param>
		// Token: 0x06000057 RID: 87 RVA: 0x00002D48 File Offset: 0x00000F48
		public KeyDataCollection(KeyDataCollection ori, IEqualityComparer<string> searchComparer) : this(searchComparer)
		{
			foreach (KeyData keyData in ori)
			{
				if (this._keyData.ContainsKey(keyData.KeyName))
				{
					this._keyData[keyData.KeyName] = (KeyData)keyData.Clone();
				}
				else
				{
					this._keyData.Add(keyData.KeyName, (KeyData)keyData.Clone());
				}
			}
		}

		/// <summary>
		///     Gets or sets the value of a concrete key.
		/// </summary>
		/// <remarks>
		///     If we try to assign the value of a key which doesn't exists,
		///     a new key is added with the name and the value is assigned to it.
		/// </remarks>
		/// <param name="keyName">
		///     Name of the key
		/// </param>
		/// <returns>
		///     The string with key's value or null if the key was not found.
		/// </returns>
		// Token: 0x17000012 RID: 18
		public string this[string keyName]
		{
			get
			{
				if (this._keyData.ContainsKey(keyName))
				{
					return this._keyData[keyName].Value;
				}
				return null;
			}
			set
			{
				if (!this._keyData.ContainsKey(keyName))
				{
					this.AddKey(keyName);
				}
				this._keyData[keyName].Value = value;
			}
		}

		/// <summary>
		///     Return the number of keys in the collection
		/// </summary>
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600005A RID: 90 RVA: 0x00002E2D File Offset: 0x0000102D
		public int Count
		{
			get
			{
				return this._keyData.Count;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E3A File Offset: 0x0000103A
		public bool AddKey(string keyName)
		{
			if (!this._keyData.ContainsKey(keyName))
			{
				this._keyData.Add(keyName, new KeyData(keyName));
				return true;
			}
			return false;
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E5F File Offset: 0x0000105F
		[Obsolete("Pottentially buggy method! Use AddKey(KeyData keyData) instead (See comments in code for an explanation of the bug)")]
		public bool AddKey(string keyName, KeyData keyData)
		{
			if (this.AddKey(keyName))
			{
				this._keyData[keyName] = keyData;
				return true;
			}
			return false;
		}

		/// <summary>
		///     Adds a new key to the collection
		/// </summary>
		/// <param name="keyData">
		///     KeyData instance.
		/// </param>
		/// <returns>
		///     <c>true</c> if the key was added  <c>false</c> if a key with the same name already exist 
		///     in the collection
		/// </returns>
		// Token: 0x0600005D RID: 93 RVA: 0x00002E7A File Offset: 0x0000107A
		public bool AddKey(KeyData keyData)
		{
			if (this.AddKey(keyData.KeyName))
			{
				this._keyData[keyData.KeyName] = keyData;
				return true;
			}
			return false;
		}

		/// <summary>
		///     Adds a new key with the specified name and value to the collection
		/// </summary>
		/// <param name="keyName">
		///     Name of the new key to be added.
		/// </param>
		/// <param name="keyValue">
		///     Value associated to the key.
		/// </param>
		/// <returns>
		///     <c>true</c> if the key was added  <c>false</c> if a key with the same name already exist 
		///     in the collection.
		/// </returns>
		// Token: 0x0600005E RID: 94 RVA: 0x00002E9F File Offset: 0x0000109F
		public bool AddKey(string keyName, string keyValue)
		{
			if (this.AddKey(keyName))
			{
				this._keyData[keyName].Value = keyValue;
				return true;
			}
			return false;
		}

		/// <summary>
		///     Clears all comments of this section
		/// </summary>
		// Token: 0x0600005F RID: 95 RVA: 0x00002EC0 File Offset: 0x000010C0
		public void ClearComments()
		{
			foreach (KeyData keyData in this)
			{
				keyData.Comments.Clear();
			}
		}

		/// <summary>
		/// Gets if a specifyed key name exists in the collection.
		/// </summary>
		/// <param name="keyName">Key name to search</param>
		/// <returns><c>true</c> if a key with the specified name exists in the collectoin
		/// <c>false</c> otherwise</returns>
		// Token: 0x06000060 RID: 96 RVA: 0x00002F0C File Offset: 0x0000110C
		public bool ContainsKey(string keyName)
		{
			return this._keyData.ContainsKey(keyName);
		}

		/// <summary>
		/// Retrieves the data for a specified key given its name
		/// </summary>
		/// <param name="keyName">Name of the key to retrieve.</param>
		/// <returns>
		/// A <see cref="T:IniParser.Model.KeyData" /> instance holding
		/// the key information or <c>null</c> if the key wasn't found.
		/// </returns>
		// Token: 0x06000061 RID: 97 RVA: 0x00002F1A File Offset: 0x0000111A
		public KeyData GetKeyData(string keyName)
		{
			if (this._keyData.ContainsKey(keyName))
			{
				return this._keyData[keyName];
			}
			return null;
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00002F38 File Offset: 0x00001138
		public void Merge(KeyDataCollection keyDataToMerge)
		{
			foreach (KeyData keyData in keyDataToMerge)
			{
				this.AddKey(keyData.KeyName);
				this.GetKeyData(keyData.KeyName).Comments.AddRange(keyData.Comments);
				this[keyData.KeyName] = keyData.Value;
			}
		}

		/// <summary>
		/// 	Deletes all keys in this collection.
		/// </summary>
		// Token: 0x06000063 RID: 99 RVA: 0x00002FB4 File Offset: 0x000011B4
		public void RemoveAllKeys()
		{
			this._keyData.Clear();
		}

		/// <summary>
		/// Deletes a previously existing key, including its associated data.
		/// </summary>
		/// <param name="keyName">The key to be removed.</param>
		/// <returns>
		/// <c>true</c> if a key with the specified name was removed 
		/// <c>false</c> otherwise.
		/// </returns>
		// Token: 0x06000064 RID: 100 RVA: 0x00002FC1 File Offset: 0x000011C1
		public bool RemoveKey(string keyName)
		{
			return this._keyData.Remove(keyName);
		}

		/// <summary>
		/// Sets the key data associated to a specified key.
		/// </summary>
		/// <param name="data">The new <see cref="T:IniParser.Model.KeyData" /> for the key.</param>
		// Token: 0x06000065 RID: 101 RVA: 0x00002FCF File Offset: 0x000011CF
		public void SetKeyData(KeyData data)
		{
			if (data == null)
			{
				return;
			}
			if (this._keyData.ContainsKey(data.KeyName))
			{
				this.RemoveKey(data.KeyName);
			}
			this.AddKey(data);
		}

		/// <summary>
		/// Allows iteration througt the collection.
		/// </summary>
		/// <returns>A strong-typed IEnumerator </returns>
		// Token: 0x06000066 RID: 102 RVA: 0x00002FFD File Offset: 0x000011FD
		public IEnumerator<KeyData> GetEnumerator()
		{
			foreach (string key in this._keyData.Keys)
			{
				yield return this._keyData[key];
			}
			Dictionary<string, KeyData>.KeyCollection.Enumerator enumerator = default(Dictionary<string, KeyData>.KeyCollection.Enumerator);
			yield break;
		
		}

		/// <summary>
		/// Implementation needed
		/// </summary>
		/// <returns>A weak-typed IEnumerator.</returns>
		// Token: 0x06000067 RID: 103 RVA: 0x0000300C File Offset: 0x0000120C
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this._keyData.GetEnumerator();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		// Token: 0x06000068 RID: 104 RVA: 0x0000301E File Offset: 0x0000121E
		public object Clone()
		{
			return new KeyDataCollection(this, this._searchComparer);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x0000302C File Offset: 0x0000122C
		internal KeyData GetLast()
		{
			KeyData result = null;
			if (this._keyData.Keys.Count <= 0)
			{
				return result;
			}
			foreach (string key in this._keyData.Keys)
			{
				result = this._keyData[key];
			}
			return result;
		}

		// Token: 0x04000011 RID: 17
		private IEqualityComparer<string> _searchComparer;

		/// <summary>
		/// Collection of KeyData for a given section
		/// </summary>
		// Token: 0x04000012 RID: 18
		private readonly Dictionary<string, KeyData> _keyData;
	}
}
