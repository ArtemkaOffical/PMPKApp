using System;
using System.Collections.Generic;

namespace IniParser.Model
{
	/// <summary>
	///     Information associated to a section in a INI File
	///     Includes both the value and the comments associated to the key.
	/// </summary>
	// Token: 0x0200000B RID: 11
	public class SectionData : ICloneable
	{
		// Token: 0x0600006A RID: 106 RVA: 0x000030A4 File Offset: 0x000012A4
		public SectionData(string sectionName) : this(sectionName, EqualityComparer<string>.Default)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.SectionData" /> class.
		/// </summary>
		// Token: 0x0600006B RID: 107 RVA: 0x000030B4 File Offset: 0x000012B4
		public SectionData(string sectionName, IEqualityComparer<string> searchComparer)
		{
			this._trailingComments = new List<string>();
			base..ctor();
			this._searchComparer = searchComparer;
			if (string.IsNullOrEmpty(sectionName))
			{
				throw new ArgumentException("section name can not be empty");
			}
			this._leadingComments = new List<string>();
			this._keyDataCollection = new KeyDataCollection(this._searchComparer);
			this.SectionName = sectionName;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.SectionData" /> class
		///     from a previous instance of <see cref="T:IniParser.Model.SectionData" />.
		/// </summary>
		/// <remarks>
		///     Data is deeply copied
		/// </remarks>
		/// <param name="ori">
		///     The instance of the <see cref="T:IniParser.Model.SectionData" /> class 
		///     used to create the new instance.
		/// </param>
		/// <param name="searchComparer">
		///     Search comparer.
		/// </param>
		// Token: 0x0600006C RID: 108 RVA: 0x00003110 File Offset: 0x00001310
		public SectionData(SectionData ori, IEqualityComparer<string> searchComparer = null)
		{
			this._trailingComments = new List<string>();
		
			this.SectionName = ori.SectionName;
			this._searchComparer = searchComparer;
			this._leadingComments = new List<string>(ori._leadingComments);
			this._keyDataCollection = new KeyDataCollection(ori._keyDataCollection, searchComparer ?? ori._searchComparer);
		}

		/// <summary>
		///     Deletes all comments in this section and key/value pairs
		/// </summary>
		// Token: 0x0600006D RID: 109 RVA: 0x0000316E File Offset: 0x0000136E
		public void ClearComments()
		{
			this.LeadingComments.Clear();
			this.TrailingComments.Clear();
			this.Keys.ClearComments();
		}

		/// <summary>
		/// Deletes all the key-value pairs in this section.
		/// </summary>
		// Token: 0x0600006E RID: 110 RVA: 0x00003191 File Offset: 0x00001391
		public void ClearKeyData()
		{
			this.Keys.RemoveAllKeys();
		}

		/// <summary>
		///     Merges otherSection into this, adding new keys if they don't exists
		///     or overwriting values if the key already exists.
		/// Comments get appended.
		/// </summary>
		/// <remarks>
		///     Comments are also merged but they are always added, not overwritten.
		/// </remarks>
		/// <param name="toMergeSection"></param>
		// Token: 0x0600006F RID: 111 RVA: 0x000031A0 File Offset: 0x000013A0
		public void Merge(SectionData toMergeSection)
		{
			foreach (string item in toMergeSection.LeadingComments)
			{
				this.LeadingComments.Add(item);
			}
			this.Keys.Merge(toMergeSection.Keys);
			foreach (string item2 in toMergeSection.TrailingComments)
			{
				this.TrailingComments.Add(item2);
			}
		}

		/// <summary>
		///     Gets or sets the name of the section.
		/// </summary>
		/// <value>
		///     The name of the section
		/// </value>
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003250 File Offset: 0x00001450
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003258 File Offset: 0x00001458
		public string SectionName
		{
			get
			{
				return this._sectionName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this._sectionName = value;
				}
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003269 File Offset: 0x00001469
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003271 File Offset: 0x00001471
		[Obsolete("Do not use this property, use property Comments instead")]
		public List<string> LeadingComments
		{
			get
			{
				return this._leadingComments;
			}
			internal set
			{
				this._leadingComments = new List<string>(value);
			}
		}

		/// <summary>
		///     Gets or sets the comment list associated to this section.
		/// </summary>
		/// <value>
		///     A list of strings.
		/// </value>
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003269 File Offset: 0x00001469
		public List<string> Comments
		{
			get
			{
				return this._leadingComments;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000075 RID: 117 RVA: 0x0000327F File Offset: 0x0000147F
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00003287 File Offset: 0x00001487
		[Obsolete("Do not use this property, use property Comments instead")]
		public List<string> TrailingComments
		{
			get
			{
				return this._trailingComments;
			}
			internal set
			{
				this._trailingComments = new List<string>(value);
			}
		}

		/// <summary>
		///     Gets or sets the keys associated to this section.
		/// </summary>
		/// <value>
		///     A collection of KeyData objects.
		/// </value>
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000077 RID: 119 RVA: 0x00003295 File Offset: 0x00001495
		// (set) Token: 0x06000078 RID: 120 RVA: 0x0000329D File Offset: 0x0000149D
		public KeyDataCollection Keys
		{
			get
			{
				return this._keyDataCollection;
			}
			set
			{
				this._keyDataCollection = value;
			}
		}

		/// <summary>
		///     Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		///     A new object that is a copy of this instance.
		/// </returns>
		// Token: 0x06000079 RID: 121 RVA: 0x000032A6 File Offset: 0x000014A6
		public object Clone()
		{
			return new SectionData(this, null);
		}

		// Token: 0x04000013 RID: 19
		private IEqualityComparer<string> _searchComparer;

		// Token: 0x04000014 RID: 20
		private List<string> _leadingComments;

		// Token: 0x04000015 RID: 21
		private List<string> _trailingComments;

		// Token: 0x04000016 RID: 22
		private KeyDataCollection _keyDataCollection;

		// Token: 0x04000017 RID: 23
		private string _sectionName;
	}
}
