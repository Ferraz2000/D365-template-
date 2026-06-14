using System;

namespace Template.Plugins.Common
{
    /// <summary>Pré-condições enxutas (Clean Code: falhe cedo, com mensagem clara).</summary>
    public static class Guard
    {
        public static void AgainstNull(object value, string name)
        {
            if (value is null)
                throw new ArgumentNullException(name);
        }

        public static void AgainstNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Não pode ser vazio.", name);
        }
    }
}
