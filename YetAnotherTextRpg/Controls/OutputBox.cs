using Cuit.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YetAnotherTextRpg.Controls
{
    class OutputBox : Label
    {
        private readonly Queue<string> _bufferQuene = new Queue<string>();

        public int MaxRows { get; set; } = -1;

        public OutputBox(int left, int top)
            : base(left, top)
        {
            IsMultiline = true;
        }

        public void AddOutput(string output)
        {
            AddLinesToBuffer(output);
            SyncBuffer();
        }

        public void SetOutput(string output)
        {
            _bufferQuene.Clear();
            AddOutput(output);
        }

        public void Clear()
        {
            _bufferQuene.Clear();
            SyncBuffer();
        }

        private void AddLinesToBuffer(string output)
        {
            output.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)
                .ToList()
                .ForEach(l =>
                {
                    _bufferQuene.Enqueue(l);

                    if (MaxRows > 0 && _bufferQuene.Count > MaxRows)
                    {
                        _bufferQuene.Dequeue();
                    }
                });
        }

        private void SyncBuffer()
        {
            Text = string.Join(Environment.NewLine, _bufferQuene.Reverse());
        }
    }
}
