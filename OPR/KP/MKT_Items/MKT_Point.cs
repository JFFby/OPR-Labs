using System;
using OPR.lb1;
using OPR.SSGA2.Interfaces;

namespace OPR.KP.MKT_Items
{
    public sealed class MKT_Point : SquarePoint, IValue
    {
        private static int id;

        private Func<float, float, float> _fn;
        private float? value;

        public MKT_Point(float x, float y, Func<float, float, float> _fn) : base(x, y)
        {
            this._fn = _fn;
            Id = ++id;
        }

        public MKT_Point(float x, float y, MKT_Point shlpFrPoint) : this(x, y, shlpFrPoint._fn)
        {
            ShlpFrom = shlpFrPoint;
        }
        
        public int Id { get; set; }

        public MKT_Point ShlpFrom { get; }

        public bool IsSHLP { get { return ShlpFrom != null; } }

        public bool HasValue { get { return value.HasValue; } }

        public void SetFunction(Func<float, float, float> fn)
        {
            this._fn = fn;
        }

        public float Value
        {
            get
            {
                if (_fn == null)
                {
                    throw new ArgumentNullException("fn");
                }

                if (!value.HasValue)
                {
                    value = _fn(x, y);
                }

                return value.Value;
            }
        }
    }
}
