using System.Reflection;
using System.Text;

namespace SharedLibrary.Network
{
    public class ClassSerializer : ByteBuffer
    {
        public byte[] Serialize(object obj)
        {
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                object value = prop.GetValue(obj, null);

                Console.WriteLine(prop.PropertyType.Name);

                if (value != null && prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    // Recursivo em caso de class value.
                    Write(Serialize(value));
                }
                else
                {
                    // Processa outros tipos de dados
                    if (value is string)
                    {
                        Write((string)value);
                    }
                    else if (value is int)
                    {
                        Write((int)value);
                    }
                    else if (value is byte)
                    {
                        Write((byte)value);
                    }
                    else if (value is string[])
                    {
                        string[] stringArray = (string[])value;

                        if (stringArray.Length > 0)
                        {
                            foreach (string stringValue in stringArray)
                            {
                                Write(stringValue);
                            }
                        }
                    }
                    else if (value is byte[])
                    {
                        byte[] byteArray = (byte[])value;

                        if (byteArray.Length > 0)
                        {
                            Write(byteArray);
                        }
                    }
                    else if (value is int[])
                    {
                        // Serialize int array
                        int[] intArray = (int[])value;
                        foreach (int intValue in intArray)
                        {
                            Write(intValue);
                        }
                    }
                    else if (value is Enum)
                    {
                        // Assume que a enumeração é representada como int
                        int intValue = (int)value;
                        Write(intValue);
                    }
                    else if (value is List<string>)
                    {
                        // Serialize List<byte>
                        List<string> stringList = (List<string>)value;

                        if (stringList.Count > 0)
                        {
                            foreach (string stringValue in stringList)
                            {
                                Write(stringValue);
                            }
                        }
                    }
                    else if (value is List<byte>)
                    {
                        // Serialize List<byte>
                        List<byte> byteList = (List<byte>)value;
                        
                        if (byteList.Count > 0)
                        {
                            Write(byteList.ToArray());
                        }
                    }
                    else if (value is List<int>)
                    {
                        // Serialize List<byte>
                        List<int> byteList = (List<int>)value;

                        if (byteList.Count > 0)
                        {
                            foreach (int intValue in byteList)
                            {
                                Write(intValue);
                            }
                        }
                    }
                }
            }
            return buffer.ToArray();
        }

        public T Deserialize<T>(byte[] data) where T : new()
        {
            Clear();  // Limpar o buffer antes de desserializar
            buffer.AddRange(data);

            T obj = new T();
            foreach (PropertyInfo prop in typeof(T).GetProperties())
            {
                object value = null;

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                {
                    // Desserialize object (recursive)
                    value = Deserialize<Object>(buffer.ToArray());
                }
                else
                {
                    // Handle other data types
                    if (prop.PropertyType == typeof(string))
                    {
                        value = ReadString();
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        value = ReadInt32();
                    }
                    else if (prop.PropertyType == typeof(byte))
                    {
                        value = ReadByte();
                    }
                    else if (prop.PropertyType == typeof(string[]))
                    {
                        int arrayLength = ReadInt32();
                        string[] stringArray = new string[arrayLength];
                        for (int i = 0; i < arrayLength; i++)
                        {
                            stringArray[i] = ReadString();
                        }
                        value = stringArray;
                    }
                    else if (prop.PropertyType == typeof(byte[]))
                    {
                        int arrayLength = ReadInt32();
                        value = ReadBytes(arrayLength);
                    }
                    else if (prop.PropertyType == typeof(int[]))
                    {
                        int arrayLength = ReadInt32();
                        int[] intArray = new int[arrayLength];
                        for (int i = 0; i < arrayLength; i++)
                        {
                            intArray[i] = ReadInt32();
                        }
                        value = intArray;
                    }
                    else if (prop.PropertyType == typeof(List<string>))
                    {
                        int listCount = ReadInt32();
                        List<string> stringList = new List<string>();
                        for (int i = 0; i < listCount; i++)
                        {
                            stringList.Add(ReadString());
                        }
                        value = stringList;
                    }
                    else if (prop.PropertyType == typeof(List<byte>))
                    {
                        int listCount = ReadInt32();
                        List<byte> byteList = new List<byte>(ReadBytes(listCount));
                        value = byteList;
                    }
                    else if (prop.PropertyType == typeof(List<int>))
                    {
                        int listCount = ReadInt32();
                        List<int> intList = new List<int>();
                        for (int i = 0; i < listCount; i++)
                        {
                            intList.Add(ReadInt32());
                        }
                        value = intList;
                    }
                }

                prop.SetValue(obj, value);
            }

            return obj;
        }
    }
}
