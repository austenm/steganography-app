using System;

namespace P1_1
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] bmpBytes = new byte[] {
                0x42, 0x4D, 0x4C, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x0C, 0x00,
                0x00, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00,
                0x18, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF,
                0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00,
                0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF,
                0xFF, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF,
                0xFF, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0x00,
                0x00, 0x00
            };
            byte[] noHeader = new byte[48];
            int index = Array.IndexOf(bmpBytes, (byte)0x18);
            Array.Copy(bmpBytes, index + 2, noHeader, 0, 48);

            byte[] header = new byte[] {
                0x42, 0x4D, 0x4C, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x0C, 0x00,
                0x00, 0x00, 0x04, 0x00, 0x04, 0x00, 0x01, 0x00,
                0x18, 0x00
            };

            byte[] body = new byte[48];

            string[] hideThis = Environment.GetCommandLineArgs();
            int i = 0;
            foreach (string word in hideThis[1].Split(" ")) 
            {
                Int16 iByte = Convert.ToInt16(word, 16);
                Int16 andByte = 3;

                Int16 b1 = (byte)(iByte & andByte);
                iByte = (short)(iByte >> 2);

                Int16 b2 = (byte)(iByte & andByte);
                iByte = (short)(iByte >> 2);

                Int16 b3 = (byte)(iByte & andByte);
                iByte = (short)(iByte >> 2);

                Int16 b4 = (byte)(iByte & andByte);

                Int16 x1 = (byte)(b4 ^ noHeader[i]);
                Int16 x2 = (byte)(b3 ^ noHeader[i+1]);
                Int16 x3 = (byte)(b2 ^ noHeader[i+2]);
                Int16 x4 = (byte)(b1 ^ noHeader[i+3]);
                
                body[i] = (byte)x1;
                body[i+1] = (byte)x2;
                body[i+2] = (byte)x3;
                body[i+3] = (byte)x4;

                i += 4;
            }

            byte[] hm = new byte[header.Length + body.Length];
            System.Buffer.BlockCopy(header, 0, hm, 0, header.Length);
            System.Buffer.BlockCopy(body, 0, hm, header.Length, body.Length);
            
            Console.WriteLine(BitConverter.ToString(hm).Replace("-", " "));
        }
    }
}
