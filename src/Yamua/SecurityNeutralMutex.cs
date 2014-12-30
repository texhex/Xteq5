using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Security.AccessControl;
using System.Security.Principal;

namespace xsharq.common
{
    public static class SecurityNeutralMutex
    {
        //Original Code: http://rdn-consulting.com/blog/2007/08/20/kernel-object-namespace-and-vista/
        public static Mutex Create(string Name)
        {
            bool bTrash;
            return Create(Name, out bTrash);
        }

        public static Mutex Create(string Name, out bool MutexWasCreated)
        {
            //Always use global scop
            string name = @"Global\" + Name;

            MutexSecurity sec = new MutexSecurity();

            MutexAccessRule secRule = new MutexAccessRule(
                                         new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                                         MutexRights.FullControl, AccessControlType.Allow);

            sec.AddAccessRule(secRule);

            bool mutexWasCreated;
            Mutex m = new Mutex(false, name, out mutexWasCreated, sec);

            MutexWasCreated = mutexWasCreated;
            return m;
        }
    }
}
