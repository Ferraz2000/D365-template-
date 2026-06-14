using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Template.Plugins.Common;
using Template.Plugins.Repositories;
using Xunit;

namespace Template.Plugins.Tests
{
    /// <summary>
    /// Testes de arquitetura: garantem a regra de dependência e as convenções no nível da API
    /// (base, campos, parâmetros, retornos). Sem dependência externa — usa reflection.
    /// (Limite: não inspeciona corpo de método; para isso usaríamos NetArchTest/Mono.Cecil.)
    /// </summary>
    public class ArquiteturaTests
    {
        private static readonly Assembly Assembly = typeof(PluginBase).Assembly;

        private static IEnumerable<Type> TiposNo(string ns) =>
            Assembly.GetTypes().Where(t =>
                t.Namespace != null &&
                (t.Namespace == ns || t.Namespace.StartsWith(ns + ".")) &&
                !t.Name.StartsWith("<"));

        [Fact]
        public void Plugins_herdam_PluginBase_sao_sealed_e_terminam_em_Plugin()
        {
            var plugins = TiposNo("Template.Plugins.Plugins").Where(t => t.IsClass && !t.IsAbstract).ToList();
            Assert.NotEmpty(plugins);
            foreach (var p in plugins)
            {
                Assert.True(typeof(PluginBase).IsAssignableFrom(p), $"{p.Name} deve herdar PluginBase");
                Assert.True(p.IsSealed, $"{p.Name} deve ser sealed");
                Assert.EndsWith("Plugin", p.Name);
            }
        }

        [Fact]
        public void Repositorios_concretos_herdam_RepositoryBase()
        {
            var repos = TiposNo("Template.Plugins.Repositories")
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repositorio")).ToList();
            Assert.NotEmpty(repos);
            foreach (var r in repos)
                Assert.True(typeof(RepositoryBase).IsAssignableFrom(r), $"{r.Name} deve herdar RepositoryBase");
        }

        [Fact]
        public void Model_nao_depende_de_Services_Repositories_Plugins()
            => SemDependenciaPara("Template.Plugins.Model",
                "Template.Plugins.Services", "Template.Plugins.Repositories", "Template.Plugins.Plugins");

        [Fact]
        public void Repositories_nao_dependem_de_Plugins_nem_Services()
            => SemDependenciaPara("Template.Plugins.Repositories",
                "Template.Plugins.Plugins", "Template.Plugins.Services");

        [Fact]
        public void Services_nao_dependem_de_Plugins()
            => SemDependenciaPara("Template.Plugins.Services", "Template.Plugins.Plugins");

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
