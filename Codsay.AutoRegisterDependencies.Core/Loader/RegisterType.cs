using System;

namespace Codsay.AutoRegisterDependencies.Core.Loader
{
    public class RegisterType
    {
        public InjectableAttribute Attr { get; }

        public Type Implementation { get; }

        public RegisterType(InjectableAttribute attr, Type impl)
        {
            Attr = attr;
            Implementation = impl;
        }
    }
}
