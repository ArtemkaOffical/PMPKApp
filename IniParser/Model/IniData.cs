using System;
using IniParser.Model.Configuration;
using IniParser.Model.Formatting;

namespace IniParser.Model
{
	/// <summary>
	///     Represents all data from an INI file
	/// </summary>
	// Token: 0x02000008 RID: 8
	public class IniData : ICloneable
	{
		/// <summary>
		///     Initializes an empty IniData instance.
		/// </summary>
		// Token: 0x06000037 RID: 55 RVA: 0x0000295C File Offset: 0x00000B5C
		public IniData() : this(new SectionDataCollection())
		{
		}

		/// <summary>
		///     Initializes a new IniData instance using a previous
		///     <see cref="T:IniParser.Model.SectionDataCollection" />.
		/// </summary>
		/// <param name="sdc">
		///     <see cref="T:IniParser.Model.SectionDataCollection" /> object containing the
		///     data with the sections of the file
		/// </param>
		// Token: 0x06000038 RID: 56 RVA: 0x00002969 File Offset: 0x00000B69
		public IniData(SectionDataCollection sdc)
		{
			this._sections = (SectionDataCollection)sdc.Clone();
			this.Global = new KeyDataCollection();
			this.SectionKeySeparator = '.';
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002995 File Offset: 0x00000B95
		public IniData(IniData ori) : this(ori.Sections)
		{
			this.Global = (KeyDataCollection)ori.Global.Clone();
			this.Configuration = ori.Configuration.Clone();
		}

		/// <summary>
		///     Configuration used to write an ini file with the proper
		///     delimiter characters and data.
		/// </summary>
		/// <remarks>
		///     If the <see cref="T:IniParser.Model.IniData" /> instance was created by a parser,
		///     this instance is a copy of the <see cref="T:IniParser.Model.Configuration.IniParserConfiguration" /> used
		///     by the parser (i.e. different objects instances)
		///     If this instance is created programatically without using a parser, this
		///     property returns an instance of <see cref="T:IniParser.Model.Configuration.IniParserConfiguration" />
		/// </remarks>
		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003A RID: 58 RVA: 0x000029CA File Offset: 0x00000BCA
		// (set) Token: 0x0600003B RID: 59 RVA: 0x000029E5 File Offset: 0x00000BE5
		public IniParserConfiguration Configuration
		{
			get
			{
				if (this._configuration == null)
				{
					this._configuration = new IniParserConfiguration();
				}
				return this._configuration;
			}
			set
			{
				this._configuration = value.Clone();
			}
		}

		/// <summary>
		/// 	Global sections. Contains key/value pairs which are not
		/// 	enclosed in any section (i.e. they are defined at the beginning 
		/// 	of the file, before any section.
		/// </summary>
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003C RID: 60 RVA: 0x000029F3 File Offset: 0x00000BF3
		// (set) Token: 0x0600003D RID: 61 RVA: 0x000029FB File Offset: 0x00000BFB
		public KeyDataCollection Global { get; protected set; }

		/// <summary>
		/// Gets the <see cref="T:IniParser.Model.KeyDataCollection" /> instance 
		/// with the specified section name.
		/// </summary>
		// Token: 0x1700000C RID: 12
		public KeyDataCollection this[string sectionName]
		{
			get
			{
				if (!this._sections.ContainsSection(sectionName))
				{
					if (!this.Configuration.AllowCreateSectionsOnFly)
					{
						return null;
					}
					this._sections.AddSection(sectionName);
				}
				return this._sections[sectionName];
			}
		}

		/// <summary>
		/// Gets or sets all the <see cref="T:IniParser.Model.SectionData" /> 
		/// for this IniData instance.
		/// </summary>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002A3E File Offset: 0x00000C3E
		// (set) Token: 0x06000040 RID: 64 RVA: 0x00002A46 File Offset: 0x00000C46
		public SectionDataCollection Sections
		{
			get
			{
				return this._sections;
			}
			set
			{
				this._sections = value;
			}
		}

		/// <summary>
		///     Used to mark the separation between the section name and the key name 
		///     when using <see cref="M:IniParser.Model.IniData.TryGetKey(System.String,System.String@)" />. 
		/// </summary>
		/// <remarks>
		///     Defaults to '.'.
		/// </remarks>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002A4F File Offset: 0x00000C4F
		// (set) Token: 0x06000042 RID: 66 RVA: 0x00002A57 File Offset: 0x00000C57
		public char SectionKeySeparator { get; set; }

		// Token: 0x06000043 RID: 67 RVA: 0x00002A60 File Offset: 0x00000C60
		public override string ToString()
		{
			return this.ToString(new DefaultIniDataFormatter(this.Configuration));
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002A73 File Offset: 0x00000C73
		public virtual string ToString(IIniDataFormatter formatter)
		{
			return formatter.IniDataToString(this);
		}

		/// <summary>
		///     Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		///     A new object that is a copy of this instance.
		/// </returns>
		// Token: 0x06000045 RID: 69 RVA: 0x00002A7C File Offset: 0x00000C7C
		public object Clone()
		{
			return new IniData(this);
		}

		/// <summary>
		///     Deletes all comments in all sections and key values
		/// </summary>
		// Token: 0x06000046 RID: 70 RVA: 0x00002A84 File Offset: 0x00000C84
		public void ClearAllComments()
		{
			this.Global.ClearComments();
			foreach (SectionData sectionData in this.Sections)
			{
				sectionData.ClearComments();
			}
		}

		/// <summary>
		///     Merges the other iniData into this one by overwriting existing values.
		///     Comments get appended.
		/// </summary>
		/// <param name="toMergeIniData">
		///     IniData instance to merge into this. 
		///     If it is null this operation does nothing.
		/// </param>
		// Token: 0x06000047 RID: 71 RVA: 0x00002ADC File Offset: 0x00000CDC
		public void Merge(IniData toMergeIniData)
		{
			if (toMergeIniData == null)
			{
				return;
			}
			this.Global.Merge(toMergeIniData.Global);
			this.Sections.Merge(toMergeIniData.Sections);
		}

		/// <summary>
		///     Attempts to retrieve a key, using a single string combining section and 
		///     key name.
		/// </summary>
		/// <param name="key">
		///     The section and key name to retrieve, separated by <see cref="!:IniParserConfiguration.SectionKeySeparator" />.
		///
		///     If key contains no separator, it is treated as a key in the <see cref="P:IniParser.Model.IniData.Global" /> section.
		///
		///     Key may contain no more than one separator character.
		/// </param>
		/// <param name="value">
		///     If true is returned, is set to the value retrieved.  Otherwise, is set
		///     to an empty string.
		/// </param>
		/// <returns>
		///     True if key was found, otherwise false.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		///     key contained multiple separators.
		/// </exception>
		// Token: 0x06000048 RID: 72 RVA: 0x00002B04 File Offset: 0x00000D04
		public bool TryGetKey(string key, out string value)
		{
			value = string.Empty;
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			string[] array = key.Split(new char[]
			{
				this.SectionKeySeparator
			});
			int num = array.Length - 1;
			if (num > 1)
			{
				throw new ArgumentException("key contains multiple separators", "key");
			}
			if (num == 0)
			{
				if (!this.Global.ContainsKey(key))
				{
					return false;
				}
				value = this.Global[key];
				return true;
			}
			else
			{
				string text = array[0];
				key = array[1];
				if (!this._sections.ContainsSection(text))
				{
					return false;
				}
				KeyDataCollection keyDataCollection = this._sections[text];
				if (!keyDataCollection.ContainsKey(key))
				{
					return false;
				}
				value = keyDataCollection[key];
				return true;
			}
		}

		/// <summary>
		///     Retrieves a key using a single input string combining section and key name.
		/// </summary>
		/// <param name="key">
		///     The section and key name to retrieve, separated by <see cref="!:IniParserConfiguration.SectionKeySeparator" />.
		///
		///     If key contains no separator, it is treated as a key in the <see cref="P:IniParser.Model.IniData.Global" /> section.
		///
		///     Key may contain no more than one separator character.
		/// </param>
		/// <returns>
		///     The key's value if it was found, otherwise null.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		///     key contained multiple separators.
		/// </exception>
		// Token: 0x06000049 RID: 73 RVA: 0x00002BB0 File Offset: 0x00000DB0
		public string GetKey(string key)
		{
			string result;
			if (!this.TryGetKey(key, out result))
			{
				return null;
			}
			return result;
		}

		/// <summary>
		///     Merge the sections into this by overwriting this sections.
		/// </summary>
		// Token: 0x0600004A RID: 74 RVA: 0x00002BCB File Offset: 0x00000DCB
		private void MergeSection(SectionData otherSection)
		{
			if (!this.Sections.ContainsSection(otherSection.SectionName))
			{
				this.Sections.AddSection(otherSection.SectionName);
			}
			this.Sections.GetSectionData(otherSection.SectionName).Merge(otherSection);
		}

		/// <summary>
		///     Merges the given global values into this globals by overwriting existing values.
		/// </summary>
		// Token: 0x0600004B RID: 75 RVA: 0x00002C0C File Offset: 0x00000E0C
		private void MergeGlobal(KeyDataCollection globals)
		{
			foreach (KeyData keyData in globals)
			{
				this.Global[keyData.KeyName] = keyData.Value;
			}
		}

		/// <summary>
		///     Represents all sections from an INI file
		/// </summary>
		// Token: 0x0400000A RID: 10
		private SectionDataCollection _sections;

		/// <summary>
		///     See property <see cref="P:IniParser.Model.IniData.Configuration" /> for more information. 
		/// </summary>
		// Token: 0x0400000D RID: 13
		private IniParserConfiguration _configuration;
	}
}
