/*  FILE          : 	StringSerializationAPI.cs
*   PROJECT       : 	PROG3221 - Capstone
*   PROGRAMMER    : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   FIRST VERSION : 	2021-04-05
*   DESCRIPTION   : 	This file can Serialize and Deserialize data based on type
*/

using FullSerializer;
using System;

public static class StringSerializationAPI
{
    private static readonly fsSerializer Serializer = new fsSerializer();

    /*  Function	:	Serialize()
    *
    *	Description	:	This function attempts to Serializes an object into JSON
    *
    *	Parameters	:	Type type    : the type of the object passed in
    *	                object value : the actual data passed as an object
    *
    *	Returns		:	string data  : the data passed in serialized into JSON
    */
    public static string Serialize(Type type, object value)
    {
        // serialize the data
        fsData data;
        Serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

        // emit the data via JSON
        return fsJsonPrinter.CompressedJson(data);
    }

    /*  Function	:	Deserialize()
    *
    *	Description	:	This function attemps to deserialize the JSON string into the Type given
    *
    *	Parameters	:	Type type              : the desired type to be deserialized into
    *	                string serializedState : the JSON string holding the data to be deserialized
    *
    *	Returns		:	object deserialized : the passed in data deserialized into the desired type returned as Object
    */
    public static object Deserialize(Type type, string serializedState)
    {
        // step 1: parse the JSON data
        fsData data = fsJsonParser.Parse(serializedState);

        // step 2: deserialize the data
        object deserialized = null;
        Serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

        return deserialized;
    }
}