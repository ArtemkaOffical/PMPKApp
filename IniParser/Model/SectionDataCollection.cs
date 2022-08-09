using System;
using System.Collections;
using System.Collections.Generic;

namespace IniParser.Model
{
	/// <summary>
	/// <para>Represents a collection of SectionData.</para>
	/// </summary>
	// Token: 0x0200000C RID: 12
	public class SectionDataCollection : ICloneable, IEnumerable<SectionData>, IEnumerable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:IniParser.Model.SectionDataCollection" /> class.
		/// </summary>
		// Token: 0x0600007A RID: 122 RVA: 0x000032AF File Offset: 0x000014AF
		public SectionDataCollection() : this(EqualityComparer<string>.Default)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IniParser.Model.SectionDataCollection" /> class.
		/// </summary>
		/// <param name="searchComparer">
		///     StringComparer used when accessing section names
		/// </param>
		// Token: 0x0600007B RID: 123 RVA: 0x000032BC File Offset: 0x000014BC
		public SectionDataCollection(IEqualityComparer<string> searchComparer)
		{
			this._searchComparer = searchComparer;
			this._sectionData = new Dictionary<string, SectionData>(this._searchComparer);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:IniParser.Model.SectionDataCollection" /> class
		/// from a previous instance of <see cref="T:IniParser.Model.SectionDataCollection" />.
		/// </summary>
		/// <remarks>
		/// Data is deeply copied
		/// </remarks>
		/// <param name="ori">
		/// The instance of the <see cref="T:IniParser.Model.SectionDataCollection" /> class 
		/// used to create the new instance.</param>
		// Token: 0x0600007C RID: 124 RVA: 0x000032DC File Offset: 0x000014DC
		public SectionDataCollection(SectionDataCollection ori, IEqualityComparer<string> searchComparer)
		{
			this._searchComparer = (searchComparer ?? EqualityComparer<string>.Default);
			this._sectionData = new Dictionary<string, SectionData>(this._searchComparer);
			foreach (SectionData sectionData in ori)
			{
				this._sectionData.Add(sectionData.SectionName, (SectionData)sectionData.Clone());
			}
		}

		/// <summary>
		/// Returns the number of SectionData elements in the collection
		/// </summary>
		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00003360 File Offset: 0x00001560
		public int Count
		{
			get
			{
				return this._sectionData.Count;
			}
		}

		/// <summary>
		/// Gets the key data associated to a specified section name.
		/// </summary>
		/// <value>An instance of as <see cref="T:IniParser.Model.KeyDataCollection" /> class 
		/// holding the key data from the current parsed INI data, or a <c>null</c>
		/// value if the section doesn't exist.</value>
		// Token: 0x1700001A RID: 26
		public KeyDataCollection this[string sectionName]
		{
			get
			{
				if (this._sectionData.ContainsKey(sectionName))
				{
					return this._sectionData[sectionName].Keys;
				}
				return null;
			}
		}

		/// <summary>
		/// Creates a new section with empty data.
		/// </summary>
		/// <remarks>
		/// <para>If a section with the same name exists, this operation has no effect.</para>
		/// </remarks>
		/// <param name="keyName">Name of the section to be created</param>
		/// <return><c>true</c> if the a new section with the specified name was added,
		/// <c>false</c> otherwise</return>
		/// <exception cref="T:System.ArgumentException">If the section name is not valid.</exception>
		// Token: 0x0600007F RID: 127 RVA: 0x00003390 File Offset: 0x00001590
		public bool AddSection(string keyName)
		{
			if (!this.ContainsSection(keyName))
			{
				this._sectionData.Add(keyName, new SectionData(keyName, this._searchComparer));
				return true;
			}
			return false;
		}

		/// <summary>
		///     Adds a new SectionData instance to the collection
		/// </summary>
		/// <param name="data">Data.</param>
		// Token: 0x06000080 RID: 128 RVA: 0x000033B8 File Offset: 0x000015B8
		public void Add(SectionData data)
		{
			if (this.ContainsSection(data.SectionName))
			{
				this.SetSectionData(data.SectionName, new SectionData(data, this._searchComparer));
				return;
			}
			this._sectionData.Add(data.SectionName, new SectionData(data, this._searchComparer));
		}

		/// <summary>
		/// Removes all entries from this collection
		/// </summary>
		// Token: 0x06000081 RID: 129 RVA: 0x00003409 File Offset: 0x00001609
		public void Clear()
		{
			this._sectionData.Clear();
		}

		/// <summary>
		/// Gets if a section with a specified name exists in the collection.
		/// </summary>
		/// <param name="keyName">Name of the section to search</param>
		/// <returns>
		/// <c>true</c> if a section with the specified name exists in the
		///  collection <c>false</c> otherwise
		/// </returns>
		// Token: 0x06000082 RID: 130 RVA: 0x00003416 File Offset: 0x00001616
		public bool ContainsSection(string keyName)
		{
			return this._sectionData.ContainsKey(keyName);
		}

		/// <summary>
		/// Returns the section data from a specify section given its name.
		/// </summary>
		/// <param name="sectionName">Name of the section.</param>
		/// <returns>
		/// An instance of a <see cref="T:IniParser.Model.SectionData" /> class 
		/// holding the section data for the currently INI data
		/// </returns>
		// Token: 0x06000083 RID: 131 RVA: 0x00003424 File Offset: 0x00001624
		public SectionData GetSectionData(string sectionName)
		{
			if (this._sectionData.ContainsKey(sectionName))
			{
				return this._sectionData[sectionName];
			}
			return null;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003444 File Offset: 0x00001644
		public void Merge(SectionDataCollection sectionsToMerge)
		{
			foreach (SectionData sectionData in sectionsToMerge)
			{
				if (this.GetSectionData(sectionData.SectionName) == null)
				{
					this.AddSection(sectionData.SectionName);
				}
				this[sectionData.SectionName].Merge(sectionData.Keys);
			}
		}

		/// <summary>
		/// Sets the section data for given a section name.
		/// </summary>
		/// <param name="sectionName"></param>
		/// <param name="data">The new <see cref="T:IniParser.Model.SectionData" />instance.</param>
		// Token: 0x06000085 RID: 133 RVA: 0x000034B8 File Offset: 0x000016B8
		public void SetSectionData(string sectionName, SectionData data)
		{
			if (data != null)
			{
				this._sectionData[sectionName] = data;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="keyName"></param>
		/// <return><c>true</c> if the section with the specified name was removed, 
		/// <c>false</c> otherwise</return>
		// Token: 0x06000086 RID: 134 RVA: 0x000034CA File Offset: 0x000016CA
		public bool RemoveSection(string keyName)
		{
			return this._sectionData.Remove(keyName);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		// Token: 0x06000087 RID: 135 RVA: 0x000034D8 File Offset: 0x000016D8
		public IEnumerator<SectionData> GetEnumerator()
		{
			foreach (string key in this._sectionData.Keys)
			{
				yield return this._sectionData[key];
			}
			Dictionary<string, SectionData>.KeyCollection.Enumerator enumerator = default(Dictionary<string, SectionData>.KeyCollection.Enumerator);
			yield break;
			
		}

		/// <summary>
		/// Returns an enumerator that iterates through a collection.
		/// </summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		// Token: 0x06000088 RID: 136 RVA: 0x000034E7 File Offset: 0x000016E7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		// Token: 0x06000089 RID: 137 RVA: 0x000034EF File Offset: 0x000016EF
		public object Clone()
		{
			return new SectionDataCollection(this, this._searchComparer);
		}

		// Token: 0x04000018 RID: 24
		private IEqualityComparer<string> _searchComparer;

		/// <summary>
		/// Data associated to this section
		/// </summary>
		// Token: 0x04000019 RID: 25
		private readonly Dictionary<string, SectionData> _sectionData;
	}
}
