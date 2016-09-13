﻿using System.Reflection;

namespace IDI.Studio.ODP.FastReflection
{
    public class FieldAccessorFactory : IFastReflectionFactory<FieldInfo, IFieldAccessor>
    {
        public IFieldAccessor Create(FieldInfo key)
        {
            return new FieldAccessor(key);
        }

        #region IFastReflectionFactory<FieldInfo,IFieldAccessor> Members

        IFieldAccessor IFastReflectionFactory<FieldInfo, IFieldAccessor>.Create(FieldInfo key)
        {
            return this.Create(key);
        }

        #endregion
    }
}
