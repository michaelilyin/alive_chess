using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class MessagesPool
    {
        private Queue<String> messages;

        public MessagesPool()
        {
            messages = new Queue<string>();
        }

        public bool HasMessages
        {
            get
            {
                return messages.Count > 0;
            }
        }

        public void AddMessage(String message)
        {
            lock (messages)
                messages.Enqueue(message);
        }

        public String GetMessage()
        {
            String res = "";
            if (messages.Count > 0)
                lock (messages)
                    res = messages.Dequeue();
            return res;
        }

        public String[] GetMessages(int count)
        {
            int size = count > messages.Count ? messages.Count : count;
            if (size > 0)
            {
                String[] res = new String[size];
                lock (messages)
                    for (int i = 0; i < size; i++)
                        res[i] = messages.Dequeue();
                return res;
            }
            return null;
                
        }
    }
}
