using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Data;

public static class SerializationHelper
{
	/// <summary>
	/// Serializes the object.
	/// </summary>
	/// <param name="serializeThisObject">The serialize this object.</param>
	/// <returns>the serializeThisObject serialized to xml.</returns>
	public static string Serialize(object serializeThisObject)
	{
		using (StringWriter wr = new StringWriterWithEncoding(Encoding.UTF8))
		{
			XmlSerializer xwr = new XmlSerializer(serializeThisObject.GetType());
			xwr.Serialize(wr, serializeThisObject);
			xwr = null;
			return wr.ToString().Replace("encoding=\"utf-8\"?>", "?>");
		}
	}

	/// <summary>
	/// Deserializes the specified serialized object.
	/// </summary>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="serializedObject">The serialized object.</param>
	/// <param name="deserializeToType">Type of the deserialize to.</param>
	/// <returns>new instance of the Tresulttype</returns>
	public static TResult Deserialize<TResult>(string serializedObject, Type deserializeToType)
	{
		object deserialized = null;
		if (!string.IsNullOrEmpty(serializedObject))
		{
			using (StringReader rd = new StringReader(serializedObject))
			{

				XmlSerializer xwr = new XmlSerializer(deserializeToType);
				deserialized = xwr.Deserialize(rd);
			}
		}
		return (TResult)deserialized;
	}
}

/// <summary>
/// Compensates for the fact that you can not modify the encoding in a normal stringwriter
/// </summary>
class StringWriterWithEncoding : StringWriter
{
	/// <summary>
	/// 
	/// </summary>
	private Encoding _Encoding;

	/// <summary>
	/// Initializes a new instance of the <see cref="StringWriterWithEncoding"/> class.
	/// </summary>
	/// <param name="encoding">The encoding.</param>
	public StringWriterWithEncoding(Encoding encoding)
		: base()
	{
		_Encoding = encoding;
	}

	/// <summary>
	/// Gets the <see cref="T:System.Text.Encoding"/> in which the output is written.
	/// </summary>
	/// <value></value>
	/// <returns>The Encoding in which the output is written.</returns>
	public override Encoding Encoding
	{
		get { return _Encoding; }
	}
}
