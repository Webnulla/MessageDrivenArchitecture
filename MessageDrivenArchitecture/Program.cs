using System;

namespace MessageDrivenArchitecture
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var message = new Message();
            message.Notification();
        }
    }
}