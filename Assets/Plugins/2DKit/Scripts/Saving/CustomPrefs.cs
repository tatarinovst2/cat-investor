/*
	PreviewLabs.PlayerPrefs
	April 1, 2014 version

	Public Domain
	
	To the extent possible under law, PreviewLabs has waived all copyright and related or neighboring rights to this document. This work is published from: Belgium.
	
	http://www.previewlabs.com
	
*/

/*
	This is based on PreviewLabs.PlayerPrefs

	Changes:
	Removed encryption
	Removed long-type support
	Added Vector support
	Made savefiles more readable for the user
	Custom filenames
	Multiple instances (For example: one for in-game values, one for settings)
*/

using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Kit.Saving
{
	public class CustomPrefs
	{
		private readonly Hashtable _playerPrefsHashtable = new Hashtable();
		private bool _hashTableChanged = false;
		private string _serializedOutput = "";
		private string _serializedInput = "";
		public const string PARAMETERS_SEPERATOR = "\n";
		public const string KEY_VALUE_SEPERATOR = ":";
		private string[] _seperators = new string[] { PARAMETERS_SEPERATOR, KEY_VALUE_SEPERATOR };
		public string fileName = Application.persistentDataPath + "/DefaultSave.txt";

		public CustomPrefs(string pathEnding)
		{
			fileName = Application.persistentDataPath + pathEnding;

			StreamReader fileReader = null;

			if (File.Exists(fileName))
			{
				fileReader = new StreamReader(fileName);
				_serializedInput = fileReader.ReadToEnd();
			}

			if (!string.IsNullOrEmpty(_serializedInput))
			{
				Deserialize();
			}

			if (fileReader != null)
			{
				fileReader.Close();
			}
		}

		public bool HasKey(string key)
		{
			return _playerPrefsHashtable.ContainsKey(key);
		}

		public void SetString(string key, string value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public void SetInt(string key, int value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public void SetFloat(string key, float value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public void SetBool(string key, bool value)
		{
			if (!_playerPrefsHashtable.ContainsKey(key))
			{
				_playerPrefsHashtable.Add(key, value);
			}
			else
			{
				_playerPrefsHashtable[key] = value;
			}

			_hashTableChanged = true;
		}

		public string GetString(string key)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return _playerPrefsHashtable[key].ToString();
			}

			return "";
		}

		public string GetString(string key, string defaultValue)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return _playerPrefsHashtable[key].ToString();
			}
			else
			{
				_playerPrefsHashtable.Add(key, defaultValue);
				_hashTableChanged = true;
				return defaultValue;
			}
		}

		public int GetInt(string key)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (int)_playerPrefsHashtable[key];
			}

			return 0;
		}

		public int GetInt(string key, int defaultValue)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (int)_playerPrefsHashtable[key];
			}
			else
			{
				_playerPrefsHashtable.Add(key, defaultValue);
				_hashTableChanged = true;
				return defaultValue;
			}
		}

		public float GetFloat(string key)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (float)_playerPrefsHashtable[key];
			}

			return 0.0f;
		}

		public float GetFloat(string key, float defaultValue)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				return (float)_playerPrefsHashtable[key];
			}
			else
			{
				_playerPrefsHashtable.Add(key, defaultValue);
				_hashTableChanged = true;
				return defaultValue;
			}
		}

		public bool GetBool(string key)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				if (_playerPrefsHashtable[key].GetType() == typeof(bool))
				{
					return (bool)_playerPrefsHashtable[key];
				}
				else if (_playerPrefsHashtable[key].GetType() == typeof(int))
				{
					return ((int)_playerPrefsHashtable[key] != 0);
				}
			}

			return false;
		}

		public bool GetBool(string key, bool defaultValue)
		{
			if (_playerPrefsHashtable.ContainsKey(key))
			{
				if (_playerPrefsHashtable[key].GetType() == typeof(bool))
				{
					return (bool)_playerPrefsHashtable[key];
				}
				else if (_playerPrefsHashtable[key].GetType() == typeof(int))
				{
					return ((int)_playerPrefsHashtable[key] != 0);
				}

				return defaultValue;
			}
			else
			{
				_playerPrefsHashtable.Add(key, defaultValue);
				_hashTableChanged = true;
				return defaultValue;
			}
		}

		// My code

		public void SetVector2(string key, Vector2 value)
		{
			SetFloat(key + "X", value.x);
			SetFloat(key + "Y", value.y);
		}

		public Vector2 GetVector2(string key)
		{
			float x = GetFloat(key + "X");
			float y = GetFloat(key + "Y");

			return new Vector2(x, y);
		}

		public void SetVector3(string key, Vector3 value)
		{
			SetFloat(key + "X", value.x);
			SetFloat(key + "Y", value.y);
			SetFloat(key + "Z", value.z);
		}

		public Vector3 GetVector3(string key)
		{
			float x = GetFloat(key + "X");
			float y = GetFloat(key + "Y");
			float z = GetFloat(key + "Z");

			return new Vector3(x, y, z);
		}

		//

		public void DeleteKey(string key)
		{
			_playerPrefsHashtable.Remove(key);
		}

		public void DeleteAll()
		{
			_playerPrefsHashtable.Clear();
		}

		public void Flush()
		{
			if (_hashTableChanged)
			{
				Serialize();

				string output = (_serializedOutput);

				string folder = new DirectoryInfo(Path.GetDirectoryName(fileName)).FullName;

				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				StreamWriter fileWriter = File.CreateText(fileName);

				if (fileWriter == null)
				{
					Debug.LogWarning("PlayerPrefs::Flush() opening file for writing failed: " + fileName);
					return;
				}

				fileWriter.Write(output);

				fileWriter.Close();

				_serializedOutput = "";
			}
		}

		private void Serialize()
		{
			IDictionaryEnumerator myEnumerator = _playerPrefsHashtable.GetEnumerator();
			StringBuilder sb = new StringBuilder();
			bool firstString = true;

			while (myEnumerator.MoveNext())
			{
				if (!firstString)
				{
					sb.Append(PARAMETERS_SEPERATOR);
				}

				sb.Append(EscapeNonSeperators(myEnumerator.Key.ToString(), _seperators));
				sb.Append(" ");
				sb.Append(KEY_VALUE_SEPERATOR);
				sb.Append(" ");
				sb.Append(EscapeNonSeperators(myEnumerator.Value.ToString(), _seperators));
				sb.Append(" ");
				sb.Append(KEY_VALUE_SEPERATOR);
				sb.Append(" ");
				sb.Append(myEnumerator.Value.GetType());
				firstString = false;
			}

			_serializedOutput = sb.ToString();
		}

		private void Deserialize()
		{
			string[] parameters = _serializedInput.Split(new string[] { PARAMETERS_SEPERATOR }, StringSplitOptions.RemoveEmptyEntries);

			foreach (string parameter in parameters)
			{
				string[] parameterContent = parameter.Split(new string[] { " " + KEY_VALUE_SEPERATOR + " " }, StringSplitOptions.None);

				_playerPrefsHashtable.Add(DeEscapeNonSeperators(parameterContent[0], _seperators), GetTypeValue(parameterContent[2], DeEscapeNonSeperators(parameterContent[1], _seperators)));

				if (parameterContent.Length > 3)
				{
					Debug.LogWarning("PlayerPrefs::Deserialize() parameterContent has " + parameterContent.Length + " elements");
				}
			}
		}

		public string EscapeNonSeperators(string inputToEscape, string[] seperators)
		{
			inputToEscape = inputToEscape.Replace("\\", "\\\\");

			for (int i = 0; i < seperators.Length; ++i)
			{
				inputToEscape = inputToEscape.Replace(seperators[i], "\\" + seperators[i]);
			}

			return inputToEscape;
		}

		public string DeEscapeNonSeperators(string inputToDeEscape, string[] seperators)
		{
			for (int i = 0; i < seperators.Length; ++i)
			{
				inputToDeEscape = inputToDeEscape.Replace("\\" + seperators[i], seperators[i]);
			}

			inputToDeEscape = inputToDeEscape.Replace("\\\\", "\\");

			return inputToDeEscape;
		}

		// 

		private object GetTypeValue(string typeName, string value)
		{
			if (typeName == "System.String")
			{
				return (object)value.ToString();
			}
			if (typeName == "System.Int32")
			{
				return Convert.ToInt32(value);
			}
			if (typeName == "System.Boolean")
			{
				return Convert.ToBoolean(value);
			}
			if (typeName == "System.Single")
			{ //float
				return Convert.ToSingle(value);
			}
			if (typeName == "System.Int64")
			{ //long
				return Convert.ToInt64(value);
			}
			else
			{
				Debug.LogError("Unsupported type: " + typeName);
			}

			return null;
		}
	}
}