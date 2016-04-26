﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TailBlazer.Domain.FileHandling
{
    public class StartFromLineProvider : ILineProvider
    {
        private ILineProvider source;
        private long startPosition;

        public StartFromLineProvider(ILineProvider source, long startPosition)
        {
            this.source = source;
            this.startPosition = startPosition;
        }

        public int Count
        {
            get
            {
                return source.Count;
            }
        }

        public IEnumerable<Line> ReadLines(ScrollRequest scroll)
        {
            var offset = startPosition + scroll.Position;

            var startFromScroll = new ScrollRequest(scroll.Mode, scroll.PageSize, offset);
            return source
                .ReadLines(startFromScroll)
                .Select(RepositionLine)
                ;              
        }

        private Line RepositionLine(Line line, int index)
        {
            return new Line(
                lineInfo: new LineInfo(index + 1, index, line.LineInfo.Start - startPosition, line.LineInfo.End - startPosition, line.LineInfo.EndOfTail),
                text: line.Text,
                timestamp: line.Timestamp
                );            
        }
    }
}
