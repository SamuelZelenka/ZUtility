using System;
using System.Text;

public static class StringEncryption
{
    public static string Encrypt(string data, string key)
    {
        uint[] formattedKey = FormatKey(key);

        if(data.Length % 2 != 0)
        {
            data += '\0';
        }
        
        byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes(data);

        string cipher = string.Empty;
        uint[] tempData = new uint[2];
        
        for(int i = 0; i < dataBytes.Length; i += 2)
        {
            tempData[0] = dataBytes[i];
            tempData[1] = dataBytes[i+1];
            Code(tempData, formattedKey);
            cipher += ConvertUIntToString(tempData[0]) + ConvertUIntToString(tempData[1]);
        }

        return cipher;
    }
    
    public static string Decrypt(string data, string key)
    {
        uint[] formattedKey = FormatKey(key);

        int x = 0;
        uint[] tempData = new uint[2];
        byte[] dataBytes = new byte[data.Length / 8 * 2];
        
        for(int i = 0; i < data.Length; i += 8)
        {
            tempData[0] = ConvertStringToUInt(data.Substring(i, 4));
            tempData[1] = ConvertStringToUInt(data.Substring(i+4, 4));
            
            Decode(tempData, formattedKey);
            
            dataBytes[x++] = (byte)tempData[0];
            dataBytes[x++] = (byte)tempData[1];
        }

        string decipheredString = ASCIIEncoding.ASCII.GetString(dataBytes, 0, dataBytes.Length);
        
        if (decipheredString[decipheredString.Length - 1] == '\0')
        {
            decipheredString = decipheredString.Substring(0, decipheredString.Length - 1);
        }

        return decipheredString;
    }
    
    private static void Code(uint[] value, uint[] key)
    {
        uint firstValue = value[0];
        uint secondValue = value[1];
        
        uint sum = 0;
        uint delta = 0x9e3779b9;
        uint n = 32;

        while (n-- > 0)
        {
            firstValue += (secondValue << 4 ^ secondValue >> 5) + secondValue ^ sum + key[sum & 3];
            sum += delta;
            secondValue += (firstValue << 4 ^ firstValue >> 5) + firstValue ^ sum + key[sum >> 11 & 3];
        }
        
        value[0] = firstValue;
        value[1] = secondValue;
    }
    
    private static void Decode(uint[] value, uint[] key)
    {
        uint firstValue = value[0];
        uint secondValue = value[1];
        
        uint sum;
        uint delta = 0x9e3779b9;
        uint n = 32;

        sum = delta << 5 ;

        while(n-- > 0)
        {
            secondValue -= (firstValue << 4 ^ firstValue >> 5) + firstValue ^ sum + key[sum >> 11 & 3];
            sum -= delta;
            firstValue -= (secondValue << 4 ^ secondValue >> 5) + secondValue ^ sum + key[sum & 3];
        }

        value[0] = firstValue;
        value[1] = secondValue;
    }
    
    private static uint[] FormatKey(string key)
    {
        if (key.Length == 0)
        {
            throw new ArgumentException("Key must be between 1 and 16 characters in length");  
        }

        key = key.PadRight(16,' ').Substring(0, 16);
        
        uint[] formattedKey = new uint[4];
        int j = 0;

        for (int i = 0; i < key.Length; i += 4)
        {
            formattedKey[j++] = ConvertStringToUInt(key.Substring(i, 4));
        }

        return formattedKey;
    }

    public static uint ConvertStringToUInt(string input)
    {
        uint output;
        
        output =  ((uint)input[0]);
        output += ((uint)input[1] << 8);
        output += ((uint)input[2] << 16);
        output += ((uint)input[3] << 24);
        
        return output;
    }
    
    public static string ConvertUIntToString(uint input)
    {
        StringBuilder output = new StringBuilder();
        output.Append((char)((input & 0xFF)));
        output.Append((char)((input >> 8) & 0xFF));
        output.Append((char)((input >> 16) & 0xFF));
        output.Append((char)((input >> 24) & 0xFF));
        return output.ToString();
    }
}
