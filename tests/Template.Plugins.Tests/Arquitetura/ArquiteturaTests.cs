using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Template.Plugins.Common;
using Xunit;

namespace Template.Plugins.Tests
{
    /// <summary>
    /// Testes de arquitetura para o desenho em **vertical slice** (Screaming).
    /// Regras checadas no nível da API (base, campos, parâmetros, retornos) via reflection,
    /// sem dependência externa. (Limite: não inspeciona corpo de método.)
    /// </summary>
    public class ArquiteturaTests
    {
        private static readonly Assembly Assembly = typeof(PluginBase).Assembly;

        private static IEnumerable<Type> Todos =>
            Assembly.GetTypes().Where(t => !t.Name.StartsWith("<"));

        private static IEnumerable<Type> TiposNo(string ns) =>
            Todos.Where(t => t.Namespace != null && (t.Namespace == ns || t.Namespace.StartsWith(ns + ".")));

        [Fact]
        public void Plugins_sao_sealed_herdam_PluginBase_e_terminam_em_Plugin()
        {
            var plugins = Todos.Where(t => t.IsClass && !t.IsAbstract && typeof(PluginBase).IsAssignableFrom(t)).ToList();
            Assert.NotEmpty(plugins);
            foreach (var p in plugins)
            {
                Assert.True(p.IsSealed, $"{p.Name} deve ser sealed");
                Assert.EndsWith("Plugin", p.Name);
            }
        }

        [Fact]
        public void Repositorios_herdam_RepositoryBase()
        {
            var repos = Todos.Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repositorio")).ToList();
            Assert.NotEmpty(repos);
            foreach (var r in repos)
                Assert.True(typeof(RepositoryBase).IsAssignableFrom(r), $"{r.Name} deve herdar RepositoryBase");
        }

        [Fact]
        public void Common_nao_depende_de_features()
            => SemDependenciaPara("Template.Plugins.Common",
                "Template.Plugins.Contas", "Template.Plugins.Contatos", "Template.Plugins.Integracao");

        [Fact]
        public void Integracao_nao_depende_de_features()
            => SemDependenciaPara("Template.Plugins.Integracao",
                "Template.Plugins.Contas", "Template.Plugins.Contatos");

        [Fact]
        public void Feature_Contatos_nao_depende_de_Contas()
            => SemDependenciaPara("Template.Plugins.Contatos", "Template.Plugins.Contas");

        [Fact]
        public void Plugins_sao_pontos_de_entrada_ninguem_os_referencia()
        {
            foreach (var t in Todos)
                foreach (var dep in Referenciados(t))
                    Assert.False(
                        dep != t && typeof(PluginBase).IsAssignableFrom(dep) && dep.Name.EndsWith("Plugin"),
                        $"{t.FullName} não pode referenciar o plugin {dep.Name} (plugins são pontos de entrada)");
        }

        private static void SemDependenciaPara(string nsOrigem, params string[] nsProibidos)
        {
            foreach (var tipo in TiposNo(nsOrigem))
                foreach (var dep in Referenciados(tipo))
                    if (dep.Namespace != null)
                        foreach (var proibido in nsProibidos)
                            Assert.False(
                                dep.Namespace == proibido || dep.Namespace.StartsWith(proibido + "."),
                                $"{tipo.FullName} não pode depender de {dep.FullName}");
        }

        private static IEnumerable<Type> Referenciados(Type t)
        {
            const BindingFlags F = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            var set = new List<Type>();
            if (t.BaseType != null) set.Add(t.BaseType);
            set.AddRange(t.GetInterfaces());
            set.AddRange(t.GetFields(F).Select(f => f.FieldType));
            set.AddRange(t.GetProperties(F).Select(p => p.PropertyType));
            foreach (var c in t.GetConstructors(F)) set.AddRange(c.GetParameters().Select(p => p.ParameterType));
            foreach (var m in t.GetMethods(F))
            {
                set.Add(m.ReturnType);
                set.AddRange(m.GetParameters().Select(p => p.ParameterType));
            }
            return set.SelectMany(Desembrulhar).Where(x => x != null).Distinct();
        }

        private static IEnumerable<Type> Desembrulhar(Type t)
        {
            if (t == null) yield break;
            if (t.IsArray) { foreach (var x in Desembrulhar(t.GetElementType())) yield return x; yield break; }
            yield return t;
            if (t.IsGenericType)
                foreach (var arg in t.GetGenericArguments())
                    foreach (var x in Desembrulhar(arg))
                        yield return x;
        }
    }
}
