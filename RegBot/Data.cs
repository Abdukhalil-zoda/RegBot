﻿namespace RegBot
{

    public class Data
    {
        public class Dict
        {
            public Guid Id;
            public string text;
            public DateTime created;
        }

        List<Dict> data = new List<Dict>();
        public void Add(Dict dict)
        {
            data.Add(dict);
            Clear();
        }
        public Dict GetDict(Guid id)
        {
            return data.FirstOrDefault(p => p.Id == id);
        }
        public void Del(Dict id)
        {
            Clear();
            data.Remove(id);
        }

        void Clear()
        {
            int length = data.Count;

            for (int i = 0; i < length; i++)
            {
                if (DateTime.Now - data[i].created > TimeSpan.FromMinutes(10))
                {
                    length--;
                    data.Remove(data[i]);
                }
            }
        }
    }
}
