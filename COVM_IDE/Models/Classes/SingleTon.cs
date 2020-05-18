using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE
{
    public abstract class Singleton<ClassType> where ClassType : new()
    {
        static Singleton()
        {
        }

        private static readonly ClassType instance = new ClassType();

        public static ClassType Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
