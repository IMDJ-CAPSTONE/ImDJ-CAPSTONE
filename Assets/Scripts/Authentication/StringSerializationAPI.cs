/*! @file       : 	StringSerializationAPI.cs
*   @author     : 	Ivan Granic, Jason Kassies, Div Dankahara, Mike Hilts
*   @date       : 	2021-03-01
*   @brief      : 	This file can Serialize and Deserialize data based on type
*/

using FullSerializer;
using System;

/*! <summary>
*  This file can Serialize and Deserialize data based on type
*  </summary>
*/
public static class StringSerializationAPI
{
    private static readonly fsSerializer Serializer = new fsSerializer();

    /*! <summary>
     *  This function attempts to Serializes an object into JSON
     *  </summary>
     *  <param name="type">the type of the object passed in</param>
     *  <param name="value">the actual data passed as an object</param>
     *  <returns>string data : the data passed in serialized into JSON</returns>
     */
    public static string Serialize(Type type, object value)
    {
        // serialize the data
        fsData data;
        Serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

        // emit the data via JSON
        return fsJsonPrinter.CompressedJson(data);
    }

    /*! <summary>
     *  This function attemps to deserialize the JSON string into the Type given
     *  </summary>
     *  <param name="type">the desired type to be deserialized into</param>
     *  <param name="serializedState">the JSON string holding the data to be deserialized</param>
     *  <returns>object deserialized : the passed in data deserialized into the desired type returned as an Object</returns>
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
