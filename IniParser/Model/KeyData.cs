using System;
using System.Collections.Generic;

namespace IniParser.Model
{
	/// <summary>
	///     Information associated to a key from an INI file.
	///     Includes both the value and the comments associated to the key.
	/// </summary>
	// Token: 0x02000009 RID: 9
	public class KeyData : ICloneable
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.KeyData" /> class.
		/// </summary>
		// Token: 0x0600004C RID: 76 RVA: 0x00002C64 File Offset: 0x00000E64
		public KeyData(string keyName)
		{
			if (string.IsNullOrEmpty(keyName))
			{
				throw new ArgumentException("key name can not be empty");
			}
			this._comments = new List<string>();
			this._value = string.Empty;
			this._keyName = keyName;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="T:IniParser.Model.KeyData" /> class
		///     from a previous instance of <see cref="T:IniParser.Model.KeyData" />.
		/// </summary>
		/// <remarks>
		///     Data is deeply copied
		/// </remarks>
		/// <param name="ori">
		///     The instance of the <see cref="T:IniParser.Model.KeyData" /> class 
		///     used to create the new instance.
		/// </param>
		// Token: 0x0600004D RID: 77 RVA: 0x00002C9C File Offset: 0x00000E9C
		public KeyData(KeyData ori)
		{
			this._value = ori._value;
			this._keyName = ori._keyName;
			this._comments = new List<string>(ori._comments);
		}

		/// <summary>
		/// Gets or sets the comment list associated to this key.
		/// </summary>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004E RID: 78 RVA: 0x00002CCD File Offset: 0x00000ECD
		// (set) Token: 0x0600004F RID: 79 RVA: 0x00002CD5 File Offset: 0x00000ED5
		public List<string> Comments
		{
			get
			{
				return this._comments;
			}
			set
			{
				this._comments = new List<string>(value);
			}
		}

		/// <summary>
		///     Gets or sets the value associated to this key.
		/// </summary>
		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000050 RID: 80 RVA: 0x00002CE3 File Offset: 0x00000EE3
		// (set) Token: 0x06000051 RID: 81 RVA: 0x00002CEB File Offset: 0x00000EEB
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		/// <summary>
		///     Gets or sets the name of the key.
		/// </summary>
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000052 RID: 82 RVA: 0x00002CF4 File Offset: 0x00000EF4
		// (set) Token: 0x06000053 RID: 83 RVA: 0x00002CFC File Offset: 0x00000EFC
		public string KeyName
		{
			get
			{
				return this._keyName;
			}
			set
			{
				if (value != string.Empty)
				{
					this._keyName = value;
				}
			}
		}

		/// <summary>
		///     Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		///     A new object that is a copy of this instance.
		/// </returns>
		// Token: 0x06000054 RID: 84 RVA: 0x00002D12 File Offset: 0x00000F12
		public object Clone()
		{
			return new KeyData(this);
		}

		// Token: 0x0400000E RID: 14
		private List<string> _comments;

		// Token: 0x0400000F RID: 15
		private string _value;

		// Token: 0x04000010 RID: 16
		private string _keyName;
	}
}
