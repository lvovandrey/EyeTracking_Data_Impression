using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeLineControlLibrary
{
    internal class VirtualizationBuffer
    {
        TimeLineEx TimeLine;
        VirtualizationBuffer OldBuffer;
        public VirtualizationBuffer(TimeLineEx timeLine, double bufferStockRatio, VirtualizationBuffer oldBuffer)
        {
            TimeLine = timeLine;
            BufferStockRatio = bufferStockRatio;
            OldBuffer = oldBuffer;
            //Interval = new TimeInterval() { Begin = TimeLine.TimeBeginViewport, End = TimeLine.TimeEndViewport };
        }

        public TimeInterval Interval;
        public List<TimeInterval> UnusedIntervals = new List<TimeInterval>();
        public List<TimeInterval> NewIntervals = new List<TimeInterval>();
        public double BufferStockRatio;

        public void ChangeSizeVirtualizerBuffer()
        {
            Interval.Begin = TimeLine.TimeBeginViewport - TimeSpan.FromSeconds(TimeLine.TimeIntervalViewport.TotalSeconds * BufferStockRatio);
            if (Interval.Begin < TimeSpan.Zero) Interval.Begin = TimeSpan.Zero;

            Interval.End = TimeLine.TimeEndViewport + TimeSpan.FromSeconds(TimeLine.TimeIntervalViewport.TotalSeconds * BufferStockRatio);
            if (Interval.End > TimeLine.FullTime) Interval.End = TimeLine.FullTime;
        }

        public void FindUnusedIntervals() 
        {
            if (OldBuffer == null) return;
            if(OldBuffer.Interval.Begin<Interval.Begin)
            {
                TimeInterval ti = new TimeInterval()
                {
                    Begin = OldBuffer.Interval.Begin,
                    End = Interval.Begin
                };
                UnusedIntervals.Add(ti);
            }
            if (OldBuffer.Interval.End > Interval.End)
            {
                TimeInterval ti = new TimeInterval()
                {
                    Begin = Interval.End,
                    End = OldBuffer.Interval.End
                };
                UnusedIntervals.Add(ti);
            }
        }

        public void FindNewIntervals()
        {
            if (OldBuffer == null) return;
            if (OldBuffer.Interval.Begin >= Interval.Begin)
            {
                TimeInterval ti = new TimeInterval()
                {
                    Begin = Interval.Begin,
                    End = OldBuffer.Interval.Begin
                };
                NewIntervals.Add(ti);
            }
            if (OldBuffer.Interval.End <= Interval.End)
            {
                TimeInterval ti = new TimeInterval()
                {
                    Begin = OldBuffer.Interval.End,
                    End = Interval.End
                };
                NewIntervals.Add(ti);
            }
        }




    }


}
