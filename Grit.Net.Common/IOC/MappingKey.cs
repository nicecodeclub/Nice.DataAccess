using System;

namespace Grit.Net.Common.IOC
{
    internal class MappingKey
    {
        /// <summary>
        /// 依赖类型
        /// </summary>
        public Type Type { get; protected set; }
        /// <summary>
        /// 实例名
        /// </summary>
        public string InstanceName { get; protected set; }


        /// <summary>
        /// 创建新的实例
        /// </summary>
        /// <param name="type">依赖类型</param>
        /// <param name="instanceName">实例名</param>
        /// <exception cref="ArgumentNullException">type</exception>
        public MappingKey(Type type, string instanceName)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Type = type;
            InstanceName = instanceName;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int multiplier = 31;
                int hash = GetType().GetHashCode();

                hash = hash * multiplier + Type.GetHashCode();
                hash = hash * multiplier + (InstanceName == null ? 0 : InstanceName.GetHashCode());

                return hash;
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            MappingKey compareTo = obj as MappingKey;

            if (ReferenceEquals(this, compareTo))
                return true;

            if (compareTo == null)
                return false;

            return Type.Equals(compareTo.Type) &&
                string.Equals(InstanceName, compareTo.InstanceName, StringComparison.InvariantCultureIgnoreCase);
        }
        public override string ToString()
        {
            const string format = "{0} ({1}) - hash code: {2}";

            return string.Format(format, this.InstanceName ?? "[null]",
                this.Type.FullName,
                this.GetHashCode()
            );
        }

        public string ToTraceString()
        {
            const string format = "Instance Name: {0} ({1})";

            return string.Format(format, this.InstanceName ?? "[null]",
                this.Type.FullName
            );
        }
    }
}
